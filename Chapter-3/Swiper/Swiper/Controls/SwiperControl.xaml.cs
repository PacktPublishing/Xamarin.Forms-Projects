using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Swiper.Utils;

namespace Swiper.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SwiperControl : ContentView
	{
        private readonly double _initialRotation;
        private static readonly Random _random = new Random();
        private double _screenWidth = -1;
        private const double DeadZone = 0.4d;
        private const double DecisionThreshold = 0.4d;

        public event EventHandler OnLike;
        public event EventHandler OnDeny;

        public SwiperControl ()
		{
			InitializeComponent ();

            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += OnPanUpdated;
            this.GestureRecognizers.Add(panGesture);

            _initialRotation = _random.Next(-10, 10);
            photo.RotateTo(_initialRotation, 100, Easing.SinOut);

            var picture = new Picture();
            descriptionLabel.Text = picture.Description;
            image.Source = new UriImageSource() { Uri = picture.Uri };

            loadingLabel.SetBinding(IsVisibleProperty, "IsLoading");
            loadingLabel.BindingContext = image;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (Application.Current.MainPage == null)
            {
                return;
            }

            _screenWidth = Application.Current.MainPage.Width;
        }

        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    PanStarted();
                    break;

                case GestureStatus.Running:
                    PanRunning(e);
                    break;

                case GestureStatus.Completed:
                    PanCompleted();
                    break;
            }
        }

        private void PanStarted()
        {
            photo.ScaleTo(1.1, 100);
        }

        private void PanRunning(PanUpdatedEventArgs e)
        {
            photo.TranslationX = e.TotalX;
            photo.TranslationY = e.TotalY;
            photo.Rotation = _initialRotation + (photo.TranslationX / 25);

            CalculatePanState(e.TotalX);
        }

        private void PanCompleted()
        {
            if (CheckForExitCriteria())
            {
                Exit();
            }

            likeStackLayout.Opacity = 0;
            denyStackLayout.Opacity = 0;

            photo.TranslateTo(0, 0, 250, Easing.SpringOut);
            photo.RotateTo(_initialRotation, 250, Easing.SpringOut);
            photo.ScaleTo(1, 250);
        }

        private void CalculatePanState(double panX)
        {
            var halfScreenWidth = _screenWidth / 2;
            var deadZoneEnd = DeadZone * halfScreenWidth;

            if (Math.Abs(panX) < deadZoneEnd)
            {
                return;
            }

            var passedDeadzone = panX < 0 ? panX + deadZoneEnd : panX - deadZoneEnd;
            var decisionZoneEnd = DecisionThreshold * halfScreenWidth;
            var opacity = passedDeadzone / decisionZoneEnd;

            opacity = Clamp(opacity, -1, 1);

            likeStackLayout.Opacity = opacity;
            denyStackLayout.Opacity = -opacity;
        }


        private void Exit()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var direction = photo.TranslationX < 0 ? -1 : 1;

                if (direction > 0)
                {
                    OnLike?.Invoke(this, new EventArgs());
                }

                if (direction < 0)
                {
                    OnDeny?.Invoke(this, new EventArgs());
                }


                await photo.TranslateTo(photo.TranslationX + (_screenWidth * direction),
                                        photo.TranslationY, 200, Easing.CubicIn);
                var parent = Parent as Layout<View>;
                parent?.Children.Remove(this);
            });
        }

        private bool CheckForExitCriteria()
        {
            var halfScreenWidth = _screenWidth / 2;
            var decisionBreakpoint = DeadZone * halfScreenWidth;
            return (Math.Abs(photo.TranslationX) > decisionBreakpoint);
        }

        private static double Clamp(double value, double min, double max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }
    }
}