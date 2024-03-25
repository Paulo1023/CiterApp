using CITER.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CITER.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        //TopicsRepository repository = new TopicsRepository();
        //string ms = "MICROSOFT";
        //string pn = "PHILNITS";
        //string ct = "COMPTIA";
        public AboutPage()
        {
            InitializeComponent();

            var navBar = App.Current.MainPage as NavigationPage;
            navBar.BarBackgroundColor = Color.WhiteSmoke;
        }
        //protected override async void OnAppearing()
        //{
        //    base.OnAppearing();
        //    //MicrosoftList.ItemsSource = await repository.Filter(ms.ToLower());
        //    //PhilnitsList.ItemsSource = await repository.Filter(pn.ToLower());
        //    //ComptiaList.ItemsSource = await repository.Filter(ct.ToLower());
        //}
        private async void BackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}