﻿using MediatR;
using Pet.Jira.Application.Worklogs.Dto;
using System.Threading;
using System.Threading.Tasks;

namespace Pet.Jira.Application.Worklogs.Commands
{
    public class AddWorklog
    {
        public class Command : IRequest<Model>
        {
            public Command(AddedWorklogDto worklog)
            {
                _worklog = worklog;
            }

            private readonly AddedWorklogDto _worklog;
            public AddedWorklogDto Worklog => _worklog;
        }

        public class Model
        {
            public AddedWorklogDto Worklog { get; set; }
        }

        public class Handler : IRequestHandler<Command, Model>
        {
            private readonly IWorklogRepository _worklogRepository;

            public Handler(IWorklogRepository worklogRepository)
            {
                _worklogRepository = worklogRepository;
            }

            public async Task<Model> Handle(
                Command request,
                CancellationToken cancellationToken)
            {
                await _worklogRepository.AddAsync(request.Worklog);
                return new Model
                {
                    Worklog = request.Worklog
                };
            }
        }
    }
}
