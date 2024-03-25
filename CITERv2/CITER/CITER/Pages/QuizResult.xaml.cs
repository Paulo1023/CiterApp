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
    public partial class QuizResult : ContentPage
    {
        List<Exam> exam = new List<Exam>();
        UserAnsRepository userAnsRepository = new UserAnsRepository();
        Exam examResult = new Exam();
        int actID;
        public string topicTitle { get; set; }
        public string Score { get; set; }
        public string Percentage { get; set; }
        public string Time { get; set; }
        public string Date { get; set; }
        public string Items { get; set; }
        public QuizResult(Activity act)
        {
            InitializeComponent();
            actID = act.ActivitiesID;
            DisplayData(act);
            BindingContext = this;
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
        private void BtnReturnClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new MainPage());
        }

        private async void BtnReviewAnsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ReviewAnswer(actID));
        }
        private async void DisplayData(Activity act)
        {
            
            topicTitle = act.Title;
            Score = act.Score;
            Percentage = act.Percentage;
            Time = act.Time;
            Date = act.Date;
            var i = await userAnsRepository.examCount(actID);
            Items = i.ToString();

            exam = await userAnsRepository.CheckActID(actID);
            var sections = exam.Select(x => x.Section).Distinct().ToList();
            foreach (var data in sections)
            {
                int sectItems = exam.Where(x => x.Section == data).Count();
                int sectAnswered = exam.Where(x => x.UserAnswer != "" && x.Section == data).Count();
                int sectCorrect = exam.Where(x => x.UserAnswer == x.CorrectAnswer && x.Section == data).Count();
                int sectWrong = exam.Where(x => (x.UserAnswer != x.CorrectAnswer || x.UserAnswer == null) && x.Section == data).Count();
                
                var result = exam.Where(x => x.Section == data).ToList();
                resultCont.Children.Add(
                    new Label()
                    {
                        Text = "Section " + data,
                        FontSize = 14,
                        TextColor = Color.Black,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        Margin = new Thickness(25,0,25,0),
                        FontAttributes = FontAttributes.Bold
                    }
                );
                Grid grid = new Grid()
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    RowDefinitions = {
                            new RowDefinition(),
                            new RowDefinition()
                    },
                    ColumnDefinitions = {
                            new ColumnDefinition(),
                            new ColumnDefinition(),
                            new ColumnDefinition(),
                            new ColumnDefinition()
                    },
                    Margin = new Thickness(25,0,25,25),
                    Padding = new Thickness(5)
                };
                Label sectionItems = new Label() 
                {
                    Text = "Items",
                    FontSize = 14,
                    TextColor = Color.Black,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };
                Label sectionAnswered = new Label() 
                {
                    Text = "Answered",
                    FontSize = 14,
                    TextColor = Color.Black,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };
                Label sectionCorrect = new Label() 
                {
                    Text = "Correct",
                    FontSize = 14,
                    TextColor = Color.Black,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };
                Label sectionWrong = new Label() 
                {
                    Text = "Incorrect",
                    FontSize = 14,
                    TextColor = Color.Black,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };
                Label sectionItems1 = new Label()
                {
                    Text = sectItems.ToString(),
                    FontSize = 14,
                    TextColor = Color.Black,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };
                Label sectionAnswered1 = new Label()
                {
                    Text = sectAnswered.ToString(),
                    FontSize = 14,
                    TextColor = Color.Black,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };
                Label sectionCorrect1 = new Label()
                {
                    Text = sectCorrect.ToString(),
                    FontSize = 14,
                    TextColor = Color.Black,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };
                Label sectionWrong1 = new Label()
                {
                    Text = sectWrong.ToString(),
                    FontSize = 14,
                    TextColor = Color.Black,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };

                resultCont.Children.Add(grid);
                grid.Children.Add(sectionItems);
                grid.Children.Add(sectionAnswered,1,0);
                grid.Children.Add(sectionCorrect,2,0);
                grid.Children.Add(sectionWrong,3,0);
                grid.Children.Add(sectionItems1,0,1);
                grid.Children.Add(sectionAnswered1,1,1);
                grid.Children.Add(sectionCorrect1,2,1);
                grid.Children.Add(sectionWrong1,3,1);

                //grid.Children.Add()
                //resultCont.Children.Add(
                //    new Grid()
                //    { 
                //        HorizontalOptions = LayoutOptions.FillAndExpand,
                //        RowDefinitions = { 
                //            new RowDefinition(),
                //            new RowDefinition()
                //        },
                //        ColumnDefinitions = { 
                //            new ColumnDefinition(),
                //            new ColumnDefinition(),
                //            new ColumnDefinition(),
                //            new ColumnDefinition()
                //        },
                //        Children = {
                //            new Label()
                //            {
                //                Text = "Items",
                //                Grid.SetRow =
                //            },
                //            new Label()
                //            {
                //                Text = "Answered"
                //            },
                //            new Label()
                //            {
                //                Text = "Correct"
                //            },
                //            new Label()
                //            { 
                //                Text = "Wrong"
                //            }
                //        }
                //    }
                    
                //);
            }
        }
    }
}