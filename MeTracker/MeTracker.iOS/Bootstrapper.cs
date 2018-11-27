using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Foundation;
using MeTracker.iOS.Services;
using MeTracker.Services;
using UIKit;

namespace MeTracker.iOS
{
    public class Bootstrapper : MeTracker.Bootstrapper
    {
        public static void Init()
        {
            var instance = new Bootstrapper();
        }

        protected override void Initialize()
        {
            base.Initialize();

            ContainerBuilder.RegisterType<LocationTrackingService>().As<ILocationTrackingService>().SingleInstance();
        }
    }
}