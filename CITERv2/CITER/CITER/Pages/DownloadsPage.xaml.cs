using CITER.Data;
using CITER.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CITER.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DownloadsPage : ContentPage
    {
        TopicsRepository repository = new TopicsRepository();
        public bool btnPress;
        public DownloadsPage()
        {
            InitializeComponent();
            BindingContext = this;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            DisplayTopics();
            
            btnPress = true;
        }
        private async void DisplayTopics()
        {
            collectionViewDownloads.ItemsSource = await repository.ListTopics();
        }
        private async void TopicClicked(object sender, EventArgs e)
        {
            if (btnPress)
            {
                Grid tp = (Grid)sender;
                var item = (Downloads)tp.BindingContext;
                Topics topic = new Topics()
                {
                    Title = item.Title,
                    Desc = item.Desc
                };
                btnPress = false;
                await Navigation.PushAsync(new TopicPage(topic));
            }
        }

        private async void DeleteClicked(object sender, EventArgs e)
        {
            
            ImageButton btn = (ImageButton)sender;
            Grid grid = (Grid)btn.Parent;
            var value = (Downloads)grid.BindingContext;
            var fp = Path.Combine(Android.App.Application.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryDownloads).AbsolutePath, value.Title + ".pdf");
            var result = await DisplayAlert("Delete", "Are you sure you want to delete this ? ", "Yes", "No");
            if (result)
            {
                await repository.DeleteTopic(value.Title);
                DisplayTopics();
                File.Delete(fp);
            }
        }

    }
}