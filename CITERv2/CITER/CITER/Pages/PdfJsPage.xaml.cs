using Firebase.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CITER.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PdfJsPage : ContentPage
    {

        public PdfJsPage()
        {
            InitializeComponent();
            //Browser();
            //string link = "https://www.firebasestorage.googleapis.com/v0/b/citerapp-1e2e7.appspot.com/o/ReviewerFile%2F101.pdf?alt=media";
            //webView.Source = "https://www.firebasestorage.googleapis.com/v0/b/citerapp-1e2e7.appspot.com/o/ReviewerFile%2F101.pdf?alt=media&token=4296e504-7eea-44b6-9911-66be34ab5852";
            var uri = "https://firebasestorage.googleapis.com/v0/b/citerapp-1e2e7.appspot.com/o/ReviewerFile%2F101.pdf?alt=media";// + "?alt=media&token=";
            //webView.se;
            Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            //SetSupportMultipleWindows(false)
        }


        //void webOnNavigating(object sender, WebNavigatingEventArgs e)
        //{
        //    progress.IsVisible = true;

        //}

        //void webOnEndNavigating(object sender, WebNavigatedEventArgs e)
        //{
        //    progress.IsVisible = false; 24,26, 28, 30
        //}
    }
}