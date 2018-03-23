using System;
using System.Diagnostics;
using Android.Content;
using TODODemo.Data.Models;
using TODODemo.DependecyServices;
using TODODemo.Droid.DependencyServices;

[assembly: Xamarin.Forms.Dependency(typeof(IShareFileImp))]
namespace TODODemo.Droid.DependencyServices
{
    public class IShareFileImp: IShareFile
    {
        public void ShareLocalFile(TodoItem item)
        {
            try
            {
                var localFilePath = item.Image;
                var date = String.Format("{0:dddd, MMMM d, yyyy}", item.LastModified);
                var subject = $"Task from {date} with status {item.Status}";
                var body = item.Content;

                if (string.IsNullOrWhiteSpace(localFilePath))
                {
                    Debug.WriteLine("ShareLocalFile Warning: localFilePath null or empty");
                    return;
                }

                if (!localFilePath.StartsWith("file://"))
                    localFilePath = string.Format("file://{0}", localFilePath);

                var fileUri = Android.Net.Uri.Parse(localFilePath);

                var intent = new Intent();
                intent.SetFlags(ActivityFlags.ClearTop);
                intent.SetFlags(ActivityFlags.NewTask);
                intent.SetAction(Intent.ActionSend);
                intent.SetType("*/*");
                intent.PutExtra(Intent.ExtraStream, fileUri);
                intent.PutExtra(Intent.ExtraSubject, subject);
                intent.PutExtra(Intent.ExtraText, body);
                intent.AddFlags(ActivityFlags.GrantReadUriPermission);

                var chooserIntent = Intent.CreateChooser(intent, "Share");
                chooserIntent.SetFlags(ActivityFlags.ClearTop);
                chooserIntent.SetFlags(ActivityFlags.NewTask);
                Android.App.Application.Context.StartActivity(chooserIntent);
            }
            catch (Exception ex)
            {
                if (ex != null && !string.IsNullOrWhiteSpace(ex.Message))
                {
                    Debug.WriteLine("ShareLocalFile Exception: {0}", ex);
                }
            }
        }
    }
}
