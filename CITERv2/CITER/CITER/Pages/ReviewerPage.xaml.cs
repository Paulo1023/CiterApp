 
using CITER.Models;
using Plugin.CloudFirestore;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic; 
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CITER.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReviewerPage : ContentPage
    {
        IEnumerable<Topics> Topics;
        public string cert { get; set; }
        public bool btnPress;
        public SearchBar sb;

        public ReviewerPage()
        {
            InitializeComponent();
            
            DisplayTopics();
            sb = searchBar;
            filter.IsVisible = false;

            DisplayManual();

            BindingContext = this;
        }

        private async void DisplayManual()
        {
            var showManual = Preferences.Get("ManualShow", "True");
            if (showManual == "True")
            {
                await Navigation.PushPopupAsync(new ManualPage());
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            btnPress = true;
            
        }

        
        private async void DisplayTopics()
        {
            searchBar.Text = string.Empty;
            try
            {
                var Query = await CrossCloudFirestore.Current
                                               .Instance
                                               .CollectionGroup("Topics")
                                               .GetAsync();
                Topics = Query.ToObjects<Topics>();
                collectionView.ItemsSource = Topics;
            }
            catch (Exception exce)
            {
                await DisplayAlert("Server Error", exce.Message, "OK");
            }
            
        }
        public void Filter(string provider)
        {
            if (!string.IsNullOrEmpty(provider))
            {
                collectionView.ItemsSource = Topics.Where(x => x.Provider == provider);
            }
            else
            {
                collectionView.ItemsSource = Topics;
            }
        }
        private async void AboutClicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new ManualPage());
        }

        public void Filtered(string prov)
        {
            filter.IsVisible = true;
            cert = prov;
            OnPropertyChanged(nameof(cert));
        }

        private void RemoveFilter(object sender, EventArgs e)
        {
            filter.IsVisible = false;
            collectionView.ItemsSource = Topics;
        }

        private async void TopicClicked(object sender, EventArgs e)
        {
            if (btnPress)
            { 
                Grid tp = (Grid)sender;
                var item = (Topics)tp.BindingContext;
                Topics topic = new Topics()
                {
                    Title = item.Title,
                    Desc = item.Desc,
                    Provider = item.Provider
                };
                btnPress = false;
                await Navigation.PushAsync(new TopicPage(topic));
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var search = e.NewTextValue;
            if (!string.IsNullOrEmpty(search))
            {
                collectionView.ItemsSource = Topics.ToArray().Where(val => val.Title.ToLower().Contains(search.ToLower()));
            }
            else 
            {
                collectionView.ItemsSource = Topics;
            }
            
        }

        private void SortClicked(object sender, EventArgs e)
        {
            Navigation.PushPopupAsync(new SortPopup(this));
        }
        

        private void RefreshView_Refreshing(object sender, EventArgs e)
        {
            DisplayTopics();
            filter.IsVisible = false;
            myRefreshView.IsRefreshing = false;
        }
    }
}