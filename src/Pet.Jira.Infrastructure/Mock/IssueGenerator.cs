﻿using Pet.Jira.Domain.Models.Issues;
using System;

namespace Pet.Jira.Infrastructure.Mock
{
	internal static class IssueGenerator
	{
		public static Issue Create(string key = null)
		{
			key ??= $"CASEM-{new Random().Next(1000, 10000)}";
			var summary = TextGenerator.Create();
			return new Issue
			{
				Key = key,
				Summary = summary,
				Link = "http://localhost"
			};
		}
	}
}
