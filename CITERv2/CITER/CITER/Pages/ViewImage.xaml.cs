using Rg.Plugins.Popup.Services;
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
    public partial class ViewImage : Rg.Plugins.Popup.Pages.PopupPage
    {
        public string img { get; set; }
        public ViewImage(string source)
        {
            InitializeComponent();

            img = source;

            BindingContext = this;
        }
        private void CloseClicked(object sender, EventArgs e)
        {
            BackBtn();
        }

        protected override bool OnBackButtonPressed()
        {
            BackBtn();
            return true;
        }

        private async void BackBtn()
        {
            await PopupNavigation.Instance.PopAsync();
        }

    }
}