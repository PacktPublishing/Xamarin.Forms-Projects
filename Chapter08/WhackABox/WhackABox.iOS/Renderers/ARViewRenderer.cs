using System.Threading.Tasks;

using Urho.iOS;
using WhackABox.Controls;
using WhackABox.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ARView), typeof(ARViewRenderer))]

namespace WhackABox.iOS.Renderers
{
    public class ARViewRenderer : ViewRenderer<ARView, UrhoSurface>
    {
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
            var surface = new UrhoSurface();
            SetNativeControl(surface);
            await surface.Show<Game>();
        }
    }
}