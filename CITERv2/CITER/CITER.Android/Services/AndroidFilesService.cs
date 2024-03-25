using Android.Content;
using Android.Widget;
using AndroidX.Core.Content;
using CITER.Interfaces;
//using PdfFiles.Models.Events;
using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using Xamarin.Forms;

[assembly: Dependency(typeof(CITER.Droid.Services.AndroidFilesService))]
namespace CITER.Droid.Services
{
    public class AndroidFilesService : IFilesService
    {
        public string StorageDirectory => Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath;
        
        public void OpenFile(string path)
        {
            
            string extension = Android.Webkit.MimeTypeMap.GetFileExtensionFromUrl(Android.Net.Uri.FromFile(new Java.IO.File(path)).ToString());
            string mimeType = Android.Webkit.MimeTypeMap.Singleton.GetMimeTypeFromExtension(extension);
            
            Context context = Android.App.Application.Context;
            Android.Net.Uri uri = FileProvider.GetUriForFile(context, "com.CITER.citer.provider", new Java.IO.File(path));
            
            Intent intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(uri, mimeType);
            intent.AddFlags(ActivityFlags.NewTask);
            intent.AddFlags(ActivityFlags.GrantReadUriPermission);
            Intent chooserInten = Intent.CreateChooser(intent, "Open with:");
            chooserInten.AddFlags(ActivityFlags.NewTask);

            try
            {
                context.StartActivity(intent);
            }
            catch (Exception exp)
            {
                Toast.MakeText(context, "No application available to View PDF", ToastLength.Short).Show();
            }
        }

    }
}