using CITER.Models;
using Plugin.CloudFirestore;
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
    public partial class SortPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        List<CertProvider> certProv = new List<CertProvider>();
        public string provider { get; set; }
        ReviewerPage reviewerPage;
        public SortPopup(ReviewerPage rp)
        {
            InitializeComponent();
            reviewerPage = rp;
            GetCertProv();
            BindingContext = this;
            
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            
        }

        private async void GetCertProv()
        {
            var Query = await CrossCloudFirestore.Current
                                                .Instance
                                                .Collection("Reviewers")
                                                .GetAsync();
            var cert = Query.ToObjects<CertProvider>();
            foreach (var data in cert)
            {
                CertProvider cProv = new CertProvider();
                cProv.Title = data.Title;
                certProv.Add(cProv);
            }
             
            foreach (CertProvider data in certProv)
            {
                Button btn = new Button()
                {
                    Text = data.Title,
                    TextColor = Color.Maroon,
                    BackgroundColor = Color.White,

                    CornerRadius = 15,
                    BorderColor = Color.Maroon,
                    BorderWidth = 1.5

                };
                btn.Clicked += (s, e) =>
                {
                    
                    provider = data.Title;
                    OnPropertyChanged(nameof(provider));
                    this.filter.IsVisible = true;

                };
                this.sortCertProv.Children.Add(btn);
            }
        }

        private void RemoveFilter(object sender, EventArgs e)
        {
            provider = "";
            OnPropertyChanged(nameof(provider));
            this.filter.IsVisible = false;

        }

        private void CancelBtn(object sender, EventArgs e)
        {
            BackBtn();
        }

        private void ApplyBtn(object sender, EventArgs e)
        {
            reviewerPage.Filtered(provider);
            BackBtn();
        }

        protected override bool OnBackButtonPressed()
        {
            BackBtn();
            return true;
        }
        private async void BackBtn()
        {
            reviewerPage.sb.Text = "";
            reviewerPage.Filter(provider);
            await PopupNavigation.Instance.PopAsync();
        }

        


    }
}