using System;
using System.Collections.Generic;
using System.Text;

namespace HotdogOrNot.Models
{
    public class Result
    {
        public bool IsHotdog { get; set; }
        public float Confidence { get; set; }
        public byte[] PhotoBytes { get; set; }
    }
}
