﻿using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pet.Jira.Application.Common.Behaviors
{
	public class ValidationBehavior<TRequest, TResponse> :
		IPipelineBehavior<TRequest, TResponse>
		where TRequest : IRequest<TResponse>
	{
		private readonly IEnumerable<IValidator<TRequest>> _validators;

		public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
		{
			_validators = validators;
		}

		public async Task<TResponse> Handle(
			TRequest request,
			RequestHandlerDelegate<TResponse> next,
			CancellationToken cancellationToken)
		{
			if (!_validators.Any())
				return await next();


			var context = new ValidationContext<TRequest>(request);
			var validationResults =
				await Task.WhenAll(_validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));

			var failures = validationResults
				.SelectMany(result => result.Errors)
				.Where(failure => failure != null)
				.ToList();

			if (failures.Any())
				throw new ValidationException(failures);

			return await next();
		}
	}
}
