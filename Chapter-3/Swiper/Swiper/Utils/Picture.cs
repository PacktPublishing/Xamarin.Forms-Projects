using System;
using System.Collections.Generic;
using System.Text;

namespace Swiper.Utils
{
    public class Picture
    {
        public Uri Uri { get; set; }
        public string Description { get; set; }

        public Picture()
        {
            Uri = new Uri($"https://picsum.photos/400/400/?random&ts={DateTime.Now.Ticks}");

            var generator = new DescriptionGenerator();
            Description = generator.Generate();
        }
    }
}
