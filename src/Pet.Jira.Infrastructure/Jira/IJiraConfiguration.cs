﻿namespace Pet.Jira.Infrastructure.Jira
{
    public interface IJiraConfiguration
    {
        string Url { get; set; }
        string[] CachedIssues { get; set; }
    }
}