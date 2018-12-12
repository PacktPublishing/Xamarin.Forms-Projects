using System.ComponentModel;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using MeTracker.Controls;
using MeTracker.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace MeTracker.Droid.Renderers
{
    public class CustomMapRenderer : MapRenderer
    {
        private GoogleMap map;

        public CustomMapRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                Control.GetMapAsync(this);
            }
        }

        protected override void OnMapReady(GoogleMap map)
        {
            this.map = map;

            base.OnMapReady(map);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == CustomMap.PointsProperty.PropertyName)
            {
                var element = (CustomMap)Element;

                foreach (var point in element.Points)
                {
                    var options = new CircleOptions();
                    options.InvokeStrokeWidth(0);
                    options.InvokeFillColor(point.Heat.ToAndroid());
                    options.InvokeRadius(200);
                    options.InvokeCenter(new LatLng(point.Location.Latitude, point.Location.Longitude));

                    map.AddCircle(options);
                }
            }
        }
    }
}