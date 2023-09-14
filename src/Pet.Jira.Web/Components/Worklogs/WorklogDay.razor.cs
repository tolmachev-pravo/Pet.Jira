﻿using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Pet.Jira.Application.Worklogs.Commands;
using Pet.Jira.Application.Worklogs.Dto;
using Pet.Jira.Domain.Models.Worklogs;
using Pet.Jira.Web.Shared;
using System;
using System.Threading.Tasks;

namespace Pet.Jira.Web.Components.Worklogs
{
    public partial class WorklogDay : ComponentBase
    {
        [Parameter] public WorkingDay Entity { get; set; }

        [Inject] private IMediator Mediator { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }

        [CascadingParameter] public ErrorHandler ErrorHandler { get; set; }

        public Color Color => Entity.IsWeekend ? Color.Error : Color.Default;

        private async Task AddWorklogAsync(WorkingDayWorklog entity)
        {
            try
            {
                var actualEntityClone = entity.Clone(WorklogType.Actual);
                await Mediator.Send(new AddWorklog.Command(AddedWorklogDto.Create(actualEntityClone)));
                Entity.AddWorklog(actualEntityClone);
                Snackbar.Add(
                    $"Worklog {entity.Issue.Key} added successfully!",
                    Severity.Success,
                    config => { config.ActionColor = Color.Success; });
                StateHasChanged();
            }
            catch (Exception e)
            {
                ErrorHandler.ProcessError(e);
            }
        }

        private async Task AddCustomWorklogAsync(WorkingDayWorklog entity)
        {
            try
            {
                await Mediator.Send(new AddWorklog.Command(AddedWorklogDto.Create(entity)));
                Entity.AddWorklog(entity);
                Snackbar.Add(
                    $"Worklog {entity.Issue.Key} added successfully!",
                    Severity.Success,
                    config => { config.ActionColor = Color.Success; });
                StateHasChanged();
            }
            catch (Exception e)
            {
                ErrorHandler.ProcessError(e);
            }
        }
    }
}
