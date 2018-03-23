using System;
using System.Diagnostics;
using System.IO;
using Foundation;
using TODODemo.Data.Models;
using TODODemo.DependecyServices;
using TODODemo.iOS.DependencyServices;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(IShareFileImp))]
namespace TODODemo.iOS.DependencyServices
{
    public class IShareFileImp: IShareFile
    {
        public void ShareLocalFile(TodoItem item)
        {
            try
            {
                var localFilePath = item.Image;
                var date = String.Format("{0:dddd, MMMM d, yyyy}", item.LastModified);
                var subject = $"Task from {date} with status {item.Status} ";
                var body = item.Content;


                if (string.IsNullOrWhiteSpace(localFilePath))
                {
                    Debug.WriteLine("ShareLocalFile Warning: localFilePath null or empty");
                    return;
                }

                var rootController = UIApplication.SharedApplication.KeyWindow.RootViewController;
                var sharedItems = new System.Collections.Generic.List<NSObject>();


                var fileName = Path.GetFileName(localFilePath);

                ///File
                var fileUrl = NSUrl.FromFilename(localFilePath);
                sharedItems.Add(fileUrl);

                //Text
                var messageNSStr = new NSString(subject);
                sharedItems.Add(messageNSStr);

                //Text
                var messageBodyNSStr = new NSString(body);
                sharedItems.Add(messageBodyNSStr);

                UIActivity[] applicationActivities = null;
                var activityViewController = new UIActivityViewController(sharedItems.ToArray(), applicationActivities);

                // Subject
                if (!string.IsNullOrWhiteSpace("Share Task"))
                    activityViewController.SetValueForKey(NSObject.FromObject("Share Task"), new NSString("subject"));

                if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
                    activityViewController.PopoverPresentationController.SourceView = rootController.View;

                rootController.PresentViewController(activityViewController, true, null);
            }
            catch (Exception ex)
            {
                if (ex != null && !string.IsNullOrWhiteSpace(ex.Message))
                {
                    Debug.WriteLine("ShareLocalFile Exception: {0}", ex.Message);
                }
            }
        }
    }
}
