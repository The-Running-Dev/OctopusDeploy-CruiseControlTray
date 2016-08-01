using System;
using Octopus.Client.Model;
using OctopusDeploy.CCTray.Models;
using TaskState = Octopus.Client.Model.TaskState;

namespace OctopusDeploy.CCTray.Extensions
{
    public static class DashboardItemResourceExtensions
    {
        public static DeploymentTask ToDeploymentTask(this DashboardItemResource dashboardItemResource, string projectName, string environmentName)
        {
            return new DeploymentTask()
            {
                TaskResource = new TaskResource()
                {
                    Id = dashboardItemResource.TaskId,
                    QueueTime = dashboardItemResource.QueueTime,
                    CompletedTime = dashboardItemResource.CompletedTime,
                    State = dashboardItemResource.State,
                    HasPendingInterruptions = dashboardItemResource.HasPendingInterruptions,
                    HasWarningsOrErrors = dashboardItemResource.HasWarningsOrErrors,
                    ErrorMessage = dashboardItemResource.ErrorMessage,
                    Duration = dashboardItemResource.Duration,
                },
                ProjectId = dashboardItemResource.Id,
                ProjectName = projectName,
                EnvironmentId = dashboardItemResource.EnvironmentId,
                EnvironmentName = environmentName,
                ReleaseId = dashboardItemResource.ReleaseId,
                DeploymentId = dashboardItemResource.DeploymentId,
                ReleaseVersion = dashboardItemResource.ReleaseVersion,
            };
        }

        public static Project ToProject(this DashboardItemResource dashboardItem, DeploymentTask currentDeploymentTask, DeploymentTask previousDeploymentTask)
        {
            return new Project()
            {
                Id = dashboardItem.ProjectId,
                Name = $"Deploying {currentDeploymentTask.ProjectName} to {currentDeploymentTask.EnvironmentName}",
                Activity = ((TaskState)dashboardItem.State).ToActivity(),
                LastBuildStatus =  ((TaskState)previousDeploymentTask.TaskResource.State).ToLastBuildStatus(),
                LastBuildLabel = previousDeploymentTask.ReleaseVersion,
                LastBuildTime = previousDeploymentTask.TaskResource.CompletedTime?.DateTime ?? DateTime.Now,
                WebUrl = dashboardItem.Links["Task"]
            };
        }
    }
}