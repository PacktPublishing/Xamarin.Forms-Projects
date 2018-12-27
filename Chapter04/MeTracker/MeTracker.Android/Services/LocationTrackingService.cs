using Android.App;
using Android.App.Job;
using Android.Content;
using MeTracker.Services;

namespace MeTracker.Droid.Services
{
    public class LocationTrackingService : ILocationTrackingService
    {
        public void StartTracking()
        {
            var javaClass = Java.Lang.Class.FromType(typeof(LocationJobService));
            var componentName = new ComponentName(Application.Context, javaClass);
            var jobBuilder = new JobInfo.Builder(1, componentName);

            jobBuilder.SetOverrideDeadline(1000);
            jobBuilder.SetPersisted(true);
            jobBuilder.SetRequiresDeviceIdle(false);
            jobBuilder.SetRequiresBatteryNotLow(true);

            var jobInfo = jobBuilder.Build();

            var jobScheduler = (JobScheduler)Application.Context.GetSystemService(Context.JobSchedulerService);
            jobScheduler.Schedule(jobInfo);
        }
    }
}