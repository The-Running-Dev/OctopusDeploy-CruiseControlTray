using System;
using Octopus.Client.Model;

namespace OctopusDeploy.CCTray.Models
{
    public class DeploymentTask
    {
        public TaskResource TaskResource { get; set; }

        public string ProjectId { get; set; }

        public string ProjectName { get; set; }

        public string EnvironmentId { get; set; }

        public string EnvironmentName { get; set; }
        
        public string ReleaseId { get; set; }

        public string DeploymentId { get; set; }
        
        public string ReleaseVersion { get; set; }
    }
}