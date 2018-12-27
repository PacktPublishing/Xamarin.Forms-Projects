using System.ComponentModel;
using System.Linq;
using MapKit;
using MeTracker.Controls;
using MeTracker.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace MeTracker.iOS.Renderers
{
    public class CustomMapRenderer : MapRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == CustomMap.PointsProperty.PropertyName)
            {
                var mapView = (MKMapView)Control;
                var customMap = (CustomMap)Element;

                mapView.OverlayRenderer = (map, overlay) =>
                {
                    var circle = overlay as MKCircle;

                    if (circle != null)
                    {
                        var point = customMap.Points.Single(x => x.Location.Latitude == circle.Coordinate.Latitude && x.Location.Longitude == circle.Coordinate.Longitude);

                        var circleRenderer = new MKCircleRenderer(circle)
                        {
                            FillColor = point.Heat.ToUIColor(),
                            Alpha = 1.0f
                        };

                        return circleRenderer;
                    }

                    return null;
                };

                foreach (var point in customMap.Points)
                {
                    var overlay = MKCircle.Circle(new CoreLocation.CLLocationCoordinate2D(point.Location.Latitude, point.Location.Longitude), 100);

                    mapView.AddOverlay(overlay);
                }
            }
        }
    }
}