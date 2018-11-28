using System;
using System.Collections.Generic;
using System.Text;

namespace MeTracker.Models
{
    public class Point
    {
        public Location Location { get; set; }
        public int Count { get; set; } = 1;
        public Xamarin.Forms.Color Heat { get; set; }
    }
}
