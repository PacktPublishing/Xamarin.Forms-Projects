using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreLocation;
using Foundation;
using MeTracker.Repositories;
using MeTracker.Services;
using UIKit;

namespace MeTracker.iOS.Services
{
    public class LocationTrackingService : ILocationTrackingService
    {
        private ILocationRepository locationRepository;
        private CLLocationManager locationManager;

        public LocationTrackingService(ILocationRepository locationRepository)
        {
            this.locationRepository = locationRepository;
        }

        public void StartTracking()
        {
            locationManager = new CLLocationManager
            {
                PausesLocationUpdatesAutomatically = false,
                AllowsBackgroundLocationUpdates = true
            };

            locationManager.AuthorizationChanged += (s, args) =>
            {
                if (args.Status == CLAuthorizationStatus.Authorized)
                {
                    locationManager.DesiredAccuracy = CLLocation.AccurracyBestForNavigation;
                    locationManager.LocationsUpdated += async (object sender, CLLocationsUpdatedEventArgs e) =>
                    {
                        var lastLocation = e.Locations.Last();
                        var newLocation = new Models.Location(lastLocation.Coordinate.Latitude, lastLocation.Coordinate.Longitude);

                        await locationRepository.Save(newLocation);
                    };

                    locationManager.StartUpdatingLocation();
                }
            };

            locationManager.RequestAlwaysAuthorization();
        }
    }
}