using CITER.Data;
using CITER.Models;
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
    public partial class ActivityPage : ContentPage
    {
        ActivityRepository actRepository = new ActivityRepository();
        UserAnsRepository userAnsRep = new UserAnsRepository();
        public ActivityPage()
        {
            InitializeComponent();
            BindingContext = this;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            DisplayAct();
        }
        private async void ActivityClicked(object sender, EventArgs e)
        {
            Grid ap = (Grid)sender;
            var item = (Activity)ap.BindingContext;
            Activity act = new Activity() 
            {
                ActivitiesID = item.ActivitiesID,
                Title = item.Title,
                Score = item.Score,
                Percentage = item.Percentage,
                Time = item.Time,
                Date = item.Date
            };
            await Navigation.PushAsync(new QuizResult(act));
        }
        private async void DisplayAct()
        {
            collectionViewActivities.ItemsSource = await actRepository.List();
        }

        private async void DeleteClicked(object sender, EventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            Grid grid = (Grid)btn.Parent;
            var value = (Activity)grid.BindingContext;
            //int actID = value.ActivitiesID;
            var result = await DisplayAlert("Delete", "Are you sure you want to delete this ? ", "Yes", "No");
            if (result)
            {
                await actRepository.Delete(value.ActivitiesID);
                await userAnsRep.Delete(value.ActivitiesID);
                DisplayAct();
            }
        }

    }
}