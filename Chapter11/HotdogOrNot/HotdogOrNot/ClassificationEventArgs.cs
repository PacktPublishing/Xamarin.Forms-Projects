using System;
using System.Collections.Generic;

namespace HotdogOrNot
{
    public class ClassificationEventArgs : EventArgs
    {
        public Dictionary<string, float> Classifications { get; private set; }

        public ClassificationEventArgs(Dictionary<string, float> classifications)
        {
            Classifications = classifications;
        }
    }
}
