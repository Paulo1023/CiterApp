using CITER.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CITER
{
    public partial class MainPage : TabbedPage
    {

        public MainPage()
        {
            InitializeComponent();
            
            CurrentPage = Children[1];
        }
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert("Exit", "Are you sure you want to exit?", "Yes", "No");

                if (result)
                {
                    System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
                }
            });
            return true;
        }


    }

}
