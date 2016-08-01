using System.Linq;
using System.Diagnostics;
using System.Configuration;
using NUnit.Framework;

namespace OctopusDeploy.CCTray.Tests
{
    [TestFixture]
    public class CCTrayTests
    {
        [SetUp]
        public static void ClassInitialize(TestContext context)
        {
            _serverUrl = ConfigurationManager.AppSettings["ServerUrl"];
            _apiKey = ConfigurationManager.AppSettings["ApiKey"];
            _projects = ConfigurationManager.AppSettings["Projects"];

            _cc = new CCTray(_serverUrl, _apiKey);
        }

        [Test]
        public void Should_Get_Project()
        {
            var project = _projects.Contains(";") ? _projects.Split(';')[0] : _projects;
            var p = _cc.GetProject(project);

            Debug.WriteLine("Id: {0}, Name: {1}", p.Id, p.Name);

            Assert.IsTrue(!string.IsNullOrEmpty(p.Id));
        }

        [Test]
        public void Should_Get_Projects()
        {
            var projectList = _cc.GetProjects(_projects);

            foreach (var p in projectList)
            {
                Debug.WriteLine("Id: {0}, Name: {1}", p.Id, p.Name);

                Assert.IsTrue(!string.IsNullOrEmpty(p.Id));
            }
        }

        [Test]
        public void Should_Get_Environments()
        {
            var environments = _cc.GetEnvironments().ToList();

            foreach (var e in environments)
            {
                Debug.WriteLine("Id: {0}, Name: {1}", e.Id, e.Name);
            }

            Assert.IsTrue(environments.Any());
        }

        [Test]
        public void Should_Get_Project_Tasks()
        {
            var project = _projects.Contains(";") ? _projects.Split(';')[0] : _projects;
            var tasks = _cc.GetProjectTasks(project).ToList();

            foreach (var t in tasks)
            {
                Debug.WriteLine("Id: {0}, Name: {1}, Description: {2}", t.Id, t.Name, t.Description);
            }

            Assert.IsTrue(tasks.Any());
        }

        [Test]
        public void Should_Get_All_Project_Deployments()
        {
            var project = _projects.Contains(";") ? _projects.Split(';')[0] : _projects;
            var deployments = _cc.GetProjectDeploymentsByProjectName(project).ToList();

            foreach (var d in deployments)
            {
                Debug.WriteLine("Id: {0}, Name: {1}, Release Id: {2}", d.Id, d.Name, d.ReleaseId);
            }

            Assert.IsTrue(deployments.Any());
        }

        [Test]
        public void Should_Get_Last_Project_Deployment()
        {
            var project = _projects.Contains(";") ? _projects.Split(';')[0] : _projects;
            var deployments = _cc.GetProjectLastDeploymentsByProjectName(project).ToList();

            foreach (var d in deployments)
            {
                Debug.WriteLine("Id: {0}, Name: {1}, Release Id: {2}", d.Id, d.Name, d.ReleaseId);
            }

            Assert.IsTrue(deployments.Any());
        }

        [Test]
        public void Should_Get_Dashboard()
        {
            var project = _projects.Contains(";") ? _projects.Split(';')[0] : _projects;
            var dashboard = _cc.GetDashboard(project);

            foreach (var e in dashboard.Environments)
            {
                Debug.WriteLine(e.Name);
            }

            foreach (var i in dashboard.Items.OrderByDescending(x => x.QueueTime))
            {
                var environment = dashboard.Environments.FirstOrDefault(x => x.Id == i.EnvironmentId);
                var environmentName = environment != null ? environment.Name : string.Empty;

                Debug.WriteLine("Deploying {0} v{1} to {2}", _projects, i.ReleaseVersion, environmentName);
            }

            Assert.IsTrue(dashboard.Environments.Any());
            Assert.IsTrue(dashboard.Items.Any());
        }

        [Test]
        public void Should_Get_Project_Status_As_CC_Tray_Status()
        {
            var statusXml = _cc.GetProjectStatusAsCCTrayStatus(_projects);

            Debug.WriteLine(statusXml);

            Assert.IsTrue(!string.IsNullOrEmpty(statusXml));
        }

        private static CCTray _cc;
        private static string _serverUrl;
        private static string _apiKey;
        private static string _projects;
    }
}