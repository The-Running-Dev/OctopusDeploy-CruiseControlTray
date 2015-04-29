using System.Linq;
using System.Diagnostics;
using System.Configuration;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OctopusDeploy.CCTray.Tests
{
    [TestClass]
    public class CCTrayTests
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _serverUrl = ConfigurationManager.AppSettings["ServerUrl"];
            _apiKey = ConfigurationManager.AppSettings["ApiKey"];
            _projectName = ConfigurationManager.AppSettings["ProjectName"];

            _cc = new CCTray(_serverUrl, _apiKey);
        }

        [TestMethod]
        public void Should_Get_Projects()
        {
            var p = _cc.GetProject(_projectName);

            Debug.WriteLine("Id: {0}, Name: {1}", p.Id, p.Name);

            Assert.IsTrue(!string.IsNullOrEmpty(p.Id));
        }

        [TestMethod]
        public void Should_Get_Environments()
        {
            var environments = _cc.GetEnvironments().ToList();

            foreach (var e in environments)
            {
                Debug.WriteLine("Id: {0}, Name: {1}", e.Id, e.Name);
            }

            Assert.IsTrue(environments.Any());
        }

        [TestMethod]
        public void Should_Get_Project_Tasks()
        {
            var tasks = _cc.GetProjectTasks(_projectName).ToList();

            foreach (var t in tasks)
            {
                Debug.WriteLine("Id: {0}, Name: {1}, Description: {2}", t.Id, t.Name, t.Description);
            }

            Assert.IsTrue(tasks.Any());
        }

        [TestMethod]
        public void Should_Get_All_Project_Deployments()
        {
            var deployments = _cc.GetProjectDeploymentsByProjectName(_projectName).ToList();

            foreach (var d in deployments)
            {
                Debug.WriteLine("Id: {0}, Name: {1}, Release Id: {2}", d.Id, d.Name, d.ReleaseId);
            }

            Assert.IsTrue(deployments.Any());
        }

        [TestMethod]
        public void Should_Get_Last_Project_Deployment()
        {
            var deployments = _cc.GetProjectLastDeploymentsByProjectName(_projectName).ToList();

            foreach (var d in deployments)
            {
                Debug.WriteLine("Id: {0}, Name: {1}, Release Id: {2}", d.Id, d.Name, d.ReleaseId);
            }

            Assert.IsTrue(deployments.Any());
        }

        [TestMethod]
        public void Should_Get_Dashboard()
        {
            var dashboard = _cc.GetDashboard(_projectName);

            foreach (var e in dashboard.Environments)
            {
                Debug.WriteLine(e.Name);
            }

            foreach (var i in dashboard.Items.OrderByDescending(x => x.QueueTime))
            {
                var environment = dashboard.Environments.FirstOrDefault(x => x.Id == i.EnvironmentId);
                var environmentName = environment != null ? environment.Name : string.Empty;

                Debug.WriteLine("Deploying {0} v{1} to {2}", _projectName, i.ReleaseVersion, environmentName);
            }

            Assert.IsTrue(dashboard.Environments.Any());
            Assert.IsTrue(dashboard.Items.Any());
        }

        [TestMethod]
        public void Should_Get_Project_Status_As_CC_Tray_Status()
        {
            var statusXml = _cc.GetProjectStatusAsCCTrayStatus(_projectName);

            Debug.WriteLine(statusXml);

            Assert.IsTrue(!string.IsNullOrEmpty(statusXml));
        }

        private static CCTray _cc;
        private static string _serverUrl;
        private static string _apiKey;
        private static string _projectName;
    }
}