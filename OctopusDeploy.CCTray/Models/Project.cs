using System;
using System.Xml.Serialization;

namespace OctopusDeploy.CCTray.Models
{
    public class Project
    {
        [XmlIgnore]
        public string Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("activity")]
        public Activity Activity { get; set; }

        [XmlAttribute("lastBuildStatus")]
        public LastBuildStatus LastBuildStatus { get; set; }

        [XmlAttribute("lastBuildLabel")]
        public string LastBuildLabel { get; set; }

        [XmlAttribute("lastBuildTime")]
        public DateTime LastBuildTime { get; set; }

        [XmlAttribute("webUrl")]
        public string WebUrl { get; set; }
    }
}