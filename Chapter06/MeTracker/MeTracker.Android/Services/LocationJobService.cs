using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.App.Job;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MeTracker.Repositories;

namespace MeTracker.Droid.Services
{
    [Service(Name = "MeTracker.Droid.Services.LocationJobService",
          Permission = "android.permission.BIND_JOB_SERVICE")]
    public class LocationJobService : JobService, ILocationListener
    {
        private ILocationRepository locationRepository;
        private static LocationManager locationManager;

        public LocationJobService()
        {
            locationRepository = Resolver.Resolve<ILocationRepository>();
        }

        public void OnLocationChanged(Location location)
        {
            var newLocation = new Models.Location(location.Latitude, location.Longitude);
            locationRepository.Save(newLocation);
        }

        public void OnProviderDisabled(string provider)
        {

        }

        public void OnProviderEnabled(string provider)
        {

        }

        public override bool OnStartJob(JobParameters @params)
        {
            locationManager = (LocationManager)ApplicationContext.GetSystemService(Context.LocationService);
            locationManager.RequestLocationUpdates(LocationManager.GpsProvider, 1000L, 0.1f, this);
            
            return true;
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {

        }

        public override bool OnStopJob(JobParameters @params)
        {
            return true;
        }
    }
}