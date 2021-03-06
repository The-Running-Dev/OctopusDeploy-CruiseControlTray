﻿using System.Linq;
using System.Collections.Generic;

using Octopus.Client.Model;

using OctopusDeploy.CCTray.Models;

namespace OctopusDeploy.CCTray.Extensions
{
    public static class DashboardResourceExtensions
    {
        public static List<Project> ToCCTrayProjects(this DashboardResource dashboard, List<DeploymentTask> previousDeploymentTasks)
        {
            var projects = new List<Project>();

            // Get the project and it's name from the first project in the dashboard
            var project = dashboard.Projects.FirstOrDefault();
            var projectName = (project != null ? project.Name : string.Empty);

            foreach (var item in dashboard.Items)
            {
                // Get the environment and its name for the current item
                var environment = dashboard.Environments.FirstOrDefault(x => x.Id == item.EnvironmentId);
                var environmentName = (environment != null ? environment.Name : string.Empty);

                // Get the previous deployment task for the item's environment
                var previousDeploymentTask = previousDeploymentTasks.FirstOrDefault(x => x.EnvironmentId == item.EnvironmentId);

                projects.Add(item.ToProject(item.ToDeploymentTask(projectName, environmentName), previousDeploymentTask));
            }

            return projects;
        }
    }
}