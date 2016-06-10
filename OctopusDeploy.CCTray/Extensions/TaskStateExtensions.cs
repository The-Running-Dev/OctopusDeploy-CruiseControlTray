using OctopusDeploy.CCTray.Models;

namespace OctopusDeploy.CCTray.Extensions
{
    public static class TaskStateExtensions
    {
        public static Activity ToActivity(this TaskState taskState)
        {
            if (taskState == TaskState.Success || taskState == TaskState.Failed)
            {
                return Activity.Sleeping;
            }

            if (taskState == TaskState.Queued || taskState == TaskState.Executing)
            {
                return Activity.Building;
            }

            return Activity.CheckingModifications;
        }

        public static LastBuildStatus ToLastBuildStatus(this TaskState taskState)
        {
            if (taskState == TaskState.Success || taskState == TaskState.Executing)
            {
                return LastBuildStatus.Success;
            }

            if (taskState == TaskState.Failed || taskState == TaskState.TimedOut)
            {
                return LastBuildStatus.Failure;
            }

            return LastBuildStatus.Unknown;
        }
    }
}