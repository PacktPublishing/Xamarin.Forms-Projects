using HotdogOrNot.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotdogOrNot.ViewModels
{
    public class ResultViewModel : ViewModel
    {
        private string title;
        public string Title
        {
            get => title;
            set => Set(ref title, value);
        }

        private string description;
        public string Description
        {
            get => description;
            set => Set(ref description, value);
        }

        private byte[] photoBytes;
        public byte[] PhotoBytes
        {
            get => photoBytes;
            set => Set(ref photoBytes, value);
        }

        public void Initialize(Result result)
        {
            PhotoBytes = result.PhotoBytes;

            if (result.IsHotdog && result.Confidence > 0.9)
            {
                Title = "Hotdog";
                Description = "This is for sure a hotdog";
            }
            else if (result.IsHotdog)
            {
                Title = "Maybe";
                Description = "This is maybe a hotdog";
            }
            else
            {
                Title = "Not a hotdog";
                Description = "This is not a hotdog";
            }
        }
    }
}
