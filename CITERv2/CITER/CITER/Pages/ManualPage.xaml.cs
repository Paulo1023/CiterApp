using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CITER.Models;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CITER.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManualPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        public ManualPage()
        {
            InitializeComponent();
            

            List<ManualImage> manualImage = new List<ManualImage>()
            { 
                new ManualImage(){ imgSource="@drawable/page1" },
                new ManualImage(){ imgSource="@drawable/page2" },
                new ManualImage(){ imgSource="@drawable/page3" },
                new ManualImage(){ imgSource="@drawable/page4" },
                new ManualImage(){ imgSource="@drawable/page5" }
            };

            carouselView.ItemsSource = manualImage;
            //manualImage.ItemsSource = manualImage;
        }

        private async void CloseClicked(object sender, EventArgs e)
        {
            
            if (checkBox.IsChecked)
            {
                Preferences.Set("ManualShow", "False");
            }
            await PopupNavigation.Instance.PopAsync();
        }
        private void nextBtn_Clicked(object sender, EventArgs e)
        {

        }

        private void prevBtn_Clicked(object sender, EventArgs e)
        {

        }
    }
}