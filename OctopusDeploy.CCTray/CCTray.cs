using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System.Collections.Generic;

using Octopus.Client;
using Octopus.Client.Model;

using OctopusDeploy.CCTray.Models;
using OctopusDeploy.CCTray.Extensions;

namespace OctopusDeploy.CCTray
{
    public class CCTray
    {
        public CCTray(string serverUrl, string apiKey)
        {
            _repository = new OctopusRepository(new OctopusServerEndpoint(serverUrl, apiKey));
        }

        public ProjectResource GetProject(string projectName)
        {
            return _repository.Projects.FindByName(projectName);
        }

        public IEnumerable<ReferenceDataItem> GetEnvironments()
        {
            return _repository.Environments.GetAll();
        }

        public IEnumerable<TaskResource> GetProjectTasks(string projectName)
        {
            var environments = GetEnvironments();

            var tasks = _repository.Tasks.FindMany(x => x.Description.Contains(projectName));

            return environments.Select(e => tasks.FirstOrDefault(x => x.Description.Contains(e.Name))).Where(task => task != null).ToList();
        }

        public DashboardResource GetDashboard(string projectName)
        {
            var project = GetProject(projectName);

            return _repository.Dashboards.GetDynamicDashboard(new[] { project.Id }, null);
        }

        public IEnumerable<DeploymentResource> GetProjectDeploymentsByProjectName(string projectName)
        {
            var project = GetProject(projectName);

            return GetProjectDeploymentsByProjectId(project.Id);
        }

        public IEnumerable<DeploymentResource> GetProjectDeploymentsByProjectId(string projectId)
        {
            return _repository.Deployments.FindMany(x => x.ProjectId == projectId);
        }

        public IEnumerable<DeploymentResource> GetProjectLastDeploymentsByProjectName(string projectName)
        {
            var project = GetProject(projectName);

            return GetProjectLastDeploymentsByProjectId(project.Id);
        }

        public IEnumerable<DeploymentResource> GetProjectLastDeploymentsByProjectId(string projectId)
        {
            // Get all the environments
            var environments = GetEnvironments();

            // Get all the deployments
            var deployments = GetProjectDeploymentsByProjectId(projectId);

            // Filter them by deployments that are not currently executing
            var completedDeployments = (
                from deployment in deployments
                let task = _repository.Tasks.Get(deployment.TaskId)
                where task.State != Octopus.Platform.Model.TaskState.Executing
                select deployment).ToList();

            // Return only the first deployment for each environment
            return environments.Select(e => completedDeployments.FirstOrDefault(x => x.EnvironmentId == e.Id)).Where(task => task != null).ToList();
        }

        public string GetProjectStatusAsCCTrayStatus(string projectName)
        {
            string xmlEncodedList;

            var dashboard = GetDashboard(projectName);
            var deployments = GetProjectLastDeploymentsByProjectName(projectName);

            var previousDeploymentTasks = (from deployment in deployments
                                           let task = _repository.Tasks.Get(deployment.TaskId)
                                           let release = _repository.Releases.Get(deployment.ReleaseId)
                                           let environment = dashboard.Environments.FirstOrDefault(x => x.Id == deployment.EnvironmentId)
                                           let environmentName = environment != null ? environment.Name : string.Empty
                                           select new DeploymentTask()
                                           {
                                               TaskResource = task,
                                               EnvironmentId = deployment.EnvironmentId,
                                               EnvironmentName = environmentName,
                                               ReleaseId = deployment.ReleaseId,
                                               ReleaseVersion = release.Version
                                           }).ToList();

            var ccTrayStatus = dashboard.ToProjects(previousDeploymentTasks);

            // Create our own namespaces for the output
            var ns = new XmlSerializerNamespaces();

            // Add an empty namespace and empty value
            ns.Add("", "");

            using (var writer = new StringWriter())
            {
                new XmlSerializer(ccTrayStatus.GetType()).Serialize(writer, ccTrayStatus, ns);

                xmlEncodedList = writer.GetStringBuilder().ToString();
            }

            return xmlEncodedList;
        }

        private readonly OctopusRepository _repository;
    }
}
