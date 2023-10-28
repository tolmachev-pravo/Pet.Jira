﻿using Pet.Jira.Application.Worklogs.Dto;
using Pet.Jira.Domain.Models.Worklogs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pet.Jira.Application.Common.Extensions
{
	internal static class WorklogEnumerableExtensions
	{
		public static IEnumerable<IWorklog> SplitByDays(
			this IEnumerable<IWorklog> worklogs,
			DateTime firstDate,
			DateTime lastDate)
		{
			var day = lastDate.Date;
			while (day >= firstDate.Date)
			{
				var startOfDay = day;
				var endOfDay = day.AddDays(1).AddMilliseconds(-1);

				var dateWorklogs = worklogs
					.Where(worklog => worklog.CompleteDate > startOfDay
									  && worklog.StartDate < endOfDay)
					.ToList();

				foreach (var dateWorklog in dateWorklogs)
				{
					var estimatedStartDate = dateWorklog.StartDate > startOfDay
						? dateWorklog.StartDate
						: startOfDay;
					var estimatedEndDate = dateWorklog.CompleteDate < endOfDay
						? dateWorklog.CompleteDate
						: endOfDay;

					yield return new WorklogDto
					{
						Issue = dateWorklog.Issue,
						StartDate = estimatedStartDate,
						CompleteDate = estimatedEndDate,
						Source = dateWorklog.Source
					};
				}
				day = day.AddDays(-1);
			}
		}
	}
}
