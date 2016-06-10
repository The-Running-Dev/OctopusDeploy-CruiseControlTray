using System;
using System.Web.UI;
using System.Configuration;

using OctopusDeploy.CCTray;

namespace WebStatus
{
    public partial class DefaultPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            _serverUrl = ConfigurationManager.AppSettings["ServerUrl"];
            _apiKey = ConfigurationManager.AppSettings["ApiKey"];
            _projects = ConfigurationManager.AppSettings["Projects"];

            _cc = new CCTray(_serverUrl, _apiKey);

            Response.AddHeader("Content-Type", "application/xml");
            Response.Write(_cc.GetProjectStatusAsCCTrayStatus(_projects));
        }

        private CCTray _cc;

        private string _serverUrl;
        private string _apiKey;
        private string _projects;
    }
}