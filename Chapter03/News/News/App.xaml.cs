﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace News
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Bootstrapper.Initialize();

            MainPage = Resolver.Resolve<MainShell>();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
