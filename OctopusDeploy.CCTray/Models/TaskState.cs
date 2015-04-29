namespace OctopusDeploy.CCTray.Models
{
    public enum TaskState
    {
        Queued = 1,

        Executing = 2,

        Failed = 3,

        Canceled = 4,

        TimedOut = 5,

        Success = 6,

        Cancelling = 8,
    }
}