using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using ImageCircle.Forms.Plugin.iOS;
using TODODemo.Data;
using TODODemo.Data.Managers;
using TODODemo.iOS.DependencyServices;
using UIKit;

namespace TODODemo.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            //Initialize the SQLite Manager
            ConnectionManager.Instance.Initialize(new ISQLiteManagerImp());

            global::Xamarin.Forms.Forms.Init();

            ImageCircleRenderer.Init();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
