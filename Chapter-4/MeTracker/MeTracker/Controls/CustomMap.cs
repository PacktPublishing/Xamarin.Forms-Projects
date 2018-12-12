using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MeTracker.Controls
{
    public class CustomMap : Map
    {
        public static BindableProperty PointsProperty = BindableProperty.Create(nameof(Points), typeof(List<Models.Point>), typeof(CustomMap), new List<Models.Point>());

        public List<Models.Point> Points
        {
            get => GetValue(PointsProperty) as List<Models.Point>;
            set => SetValue(PointsProperty, value);
        }
    }
}
