using System.Xml.Serialization;
using System.Collections.Generic;

namespace OctopusDeploy.CCTray.Models
{
    [XmlRoot("Projects")]
    public class Projects
    {
        [XmlElement("Project")]
        public List<Project> ListOfProjects { get; set; }

        public Projects()
        {
            ListOfProjects = new List<Project>();
        }
    }
}