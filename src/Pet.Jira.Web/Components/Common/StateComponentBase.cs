﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Pet.Jira.Web.Shared;

namespace Pet.Jira.Web.Components
{
    public class StateComponentBase: ComponentBase
    {
        [CascadingParameter] public ErrorHandler ErrorHandler { get; set; }

        public async Task InvokeStateActionAsync(Func<Task> action, IStateComponentModel model)
        {
            try
            {
                model.StateTo(ComponentModelState.InProgress);
                await action();
            }
            catch (Exception e)
            {
                ErrorHandler.ProcessError(e);
            }
            finally
            {
                model.StateTo(ComponentModelState.Success);
            }
        }
    }
}
