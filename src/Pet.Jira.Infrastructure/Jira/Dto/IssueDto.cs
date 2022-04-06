﻿using Pet.Jira.Domain.Models.Issues;

namespace Pet.Jira.Infrastructure.Jira.Dto
{
    public class IssueDto
    {
        public string Key { get; set; }
        public string Summary { get; set; }
        public string Link { get; set; }

        public Issue Adapt()
        {
            return new Issue
            {
                Key = Key,
                Link = Link,
                Summary = Summary
            };
        }

        public static IssueDto Create(Atlassian.Jira.Issue issue, IJiraLinkGenerator linkGenerator = null)
        {
            return new IssueDto
            {
                Key = issue.Key.Value,
                Summary = issue.Summary,
                Link = linkGenerator?.Generate(issue.Key.Value)
            };
        }
    }
}
