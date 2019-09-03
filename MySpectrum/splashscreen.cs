using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MySpectrum
{
    [Activity(MainLauncher = true, NoHistory = true, Theme = "@style/Theme.Splash")]
    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var intent = new Intent(this, typeof(MainActivity));
            //System.Threading.Thread.Sleep(3000);
            StartActivity(intent);
        }
    }
}