using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using WhackABox.Droid.Renderers;
using WhackABox;
using WhackABox.Controls;
using WhackABox.Droid;
using Urho.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ARView), typeof(ARViewRenderer))]

namespace WhackABox.Droid.Renderers
{
    public class ARViewRenderer : ViewRenderer<ARView, Android.Views.View>
    {
        private UrhoSurfacePlaceholder surface;

        public ARViewRenderer(Context context) : base(context)
        {
            MessagingCenter.Subscribe<MainActivity>(this, "OnResume", async (sender) =>
            {
                await Initialize();
            });
        }

        protected async override void OnElementChanged(ElementChangedEventArgs<ARView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                await Initialize();
            }
        }

        private async Task Initialize()
        {
            if (ContextCompat.CheckSelfPermission(Context, Manifest.Permission.Camera) != Permission.Granted)
            {
                ActivityCompat.RequestPermissions(Context as Activity, new[] { Manifest.Permission.Camera }, 42);
                return;
            }

            if (surface != null)
                return;

            surface = UrhoSurface.CreateSurface(Context as Activity);
            SetNativeControl(surface);

            await surface.Show<Game>();
        }
    }
}