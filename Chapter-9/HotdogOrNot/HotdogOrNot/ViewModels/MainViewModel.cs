using HotdogOrNot.Models;
using HotdogOrNot.Views;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace HotdogOrNot.ViewModels
{
    public class MainViewModel : ViewModel
    {
        private IClassifier classifier;
        private byte[] bytes;

        public MainViewModel(IClassifier classifier)
        {
            this.classifier = classifier;
        }

        private void HandlePhoto(MediaFile photo)
        {
            if (photo == null)
            {
                return;
            }

            var stream = photo.GetStream();
            bytes = ReadFully(stream);

            classifier.ClassificationCompleted += Classifier_ClassificationCompleted;
            classifier.Classify(bytes);
        }

        private byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream memoryStream = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    memoryStream.Write(buffer, 0, read);
                }

                return memoryStream.ToArray();
            }
        }

        void Classifier_ClassificationCompleted(object sender, ClassificationEventArgs e)
        {
            classifier.ClassificationCompleted -= Classifier_ClassificationCompleted;

            Result result = null;

            if (e.Classifications.Any())
            {
                var classificationResult = e.Classifications.OrderByDescending(x => x.Value).First();

                result = new Result()
                {
                    IsHotdog = classificationResult.Key == "hotdog",
                    Confidence = classificationResult.Value,
                    PhotoBytes = bytes
                };
            }
            else
            {
                result = new Result()
                {
                    IsHotdog = false,
                    Confidence = 1.0f,
                    PhotoBytes = bytes
                };
            }

            var view = Resolver.Resolve<ResultView>();
            ((ResultViewModel)view.BindingContext).Initialize(result);

            Navigation.PushAsync(view);
        }

        public ICommand TakePhoto => new Command(async () =>
        {
            var photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
            {
                DefaultCamera = CameraDevice.Rear
            });

            HandlePhoto(photo);
        });

        public ICommand PickPhoto => new Command(async () =>
        {
            var photo = await CrossMedia.Current.PickPhotoAsync();

            HandlePhoto(photo);
        });
    }
}
