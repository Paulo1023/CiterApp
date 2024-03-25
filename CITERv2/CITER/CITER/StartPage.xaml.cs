using CITER.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CITER
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        public StartPage()
        {
            InitializeComponent();
        }

        private void ContinueBtnClickedAsync(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new MainPage());
            Preferences.Set("StartupShow", "False");
            App.Current.MainPage = new NavigationPage(new MainPage());
        }
    }
}