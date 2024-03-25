using CITER.Data;
using CITER.Models;
using Rg.Plugins.Popup.Extensions;
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
    public partial class ReviewAnswer : ContentPage
    {
        UserAnsRepository userAns = new UserAnsRepository();
        List<Exam> questionList = new List<Exam>();
        Exam exam = new Exam();
        int actID; 
        public ReviewAnswer(int id)
        {
            InitializeComponent();
            actID = id;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing(); 
            DisplayQuestionnaires();
        }

        private async void DisplayQuestionnaires()
        {
            questionList = await userAns.CheckActID(actID);
            
            foreach (Exam data in questionList)
            {
                StackLayout stacklayout = new StackLayout
                {
                    BackgroundColor = Color.WhiteSmoke,
                    Margin = 10,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand

                };
                
                
                stacklayout.Children.Add(
                    new StackLayout()
                    {
                        BackgroundColor = Color.WhiteSmoke,
                        Padding = new Thickness(20,20,20,0),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        Orientation = StackOrientation.Horizontal,
                        
                        Children = {
                            new Label()
                            {
                                Text = "Question " + data.QuestionnaireID,
                                FontSize = 14,
                                TextColor = Color.Black,
                                HorizontalOptions = LayoutOptions.StartAndExpand
                            },
                            new Label()
                            {
                                Text = "Section " + data.Section,
                                FontSize = 14,
                                TextColor = Color.Black,
                                HorizontalOptions = LayoutOptions.EndAndExpand
                            }
                        }
                    }
                );
                stacklayout.Children.Add(
                    new Label()
                    { 
                        Text = data.Question,
                        FontSize = 18,
                        TextColor = Color.Black,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        Padding = new Thickness( 20,0,20,10 )
                    }
                );
                if (!string.IsNullOrEmpty(data.qImage))
                { 


                    var p = Path.Combine(Android.App.Application.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryDownloads).AbsolutePath, data.qImage);
                    Grid grid = new Grid()
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand

                    };
                    Image questImage = new Image()
                    {
                        Source = p,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Aspect = Aspect.AspectFit,
                        Margin = 0
                    };
                    ImageButton imgBtn = new ImageButton()
                    {
                        Source = "@drawable/zoomin",
                        CornerRadius = 25,
                        WidthRequest = 32,
                        HeightRequest = 32,
                        HorizontalOptions = LayoutOptions.EndAndExpand,
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        BackgroundColor = Color.FromHex("#00FBC1BC")
                    };
                    
                    imgBtn.Clicked += async (s, e) =>
                    {
                        await Navigation.PushPopupAsync(new ViewImage(p));
                    };
                    grid.Children.Add(questImage);
                    grid.Children.Add(imgBtn);

                    stacklayout.Children.Add(grid);


                }
                int count = 0;
                string uA = "";
                string cA = "";
                string[] arrChoices = data.choices.Split('|');
                string[] multiChoices = {"A.","B.","C.","D.","E.","F.","G.","H.","I.","J.","K.","L.","M.","N.","O.","P.","Q.","R.","S.","T.","U.","V.","W.","X.","Y.","Z."};
                foreach (var value in arrChoices)
                {
                    uA = data.UserAnswer;
                    cA = data.CorrectAnswer;
                    var path = Path.Combine(Android.App.Application.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryDownloads).AbsolutePath, value);
                    
                    if (File.Exists(path))
                    {
                        if (value == data.UserAnswer)
                        {
                            uA = multiChoices[count];
                        }
                        if (value == data.CorrectAnswer)
                        {
                            cA = multiChoices[count];
                        }
                        Grid grid = new Grid()
                        {
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            BackgroundColor = Color.WhiteSmoke

                        };
                        Image img = new Image()
                        {
                            Source = path,
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            Aspect = Aspect.AspectFit,
                            Margin = 0
                        };
                        ImageButton imgBtn = new ImageButton()
                        {
                            Source = "@drawable/zoomin",
                            CornerRadius = 25,
                            WidthRequest = 32,
                            HeightRequest = 32,
                            HorizontalOptions = LayoutOptions.EndAndExpand,
                            VerticalOptions = LayoutOptions.StartAndExpand,
                            BackgroundColor = Color.FromHex("#00FBC1BC")
                        };

                        imgBtn.Clicked += async (s, e) =>
                        {
                            await Navigation.PushPopupAsync(new ViewImage(path));
                        };
                        grid.Children.Add(img);
                        grid.Children.Add(imgBtn);
                        StackLayout sLayout = new StackLayout()
                        {
                            
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            
                            Children = {
                                    new Label()
                                    {

                                        Text = multiChoices[count],
                                        FontSize = 18,
                                        TextColor = Color.Black,
                                        HorizontalOptions = LayoutOptions.StartAndExpand,
                                        VerticalOptions = LayoutOptions.StartAndExpand,
                                        Padding = new Thickness( 15,0,5,5 )
                                    },
                                    grid
                                }
                        };
                        stacklayout.Children.Add(
                            new Frame()
                            {
                                BackgroundColor = Color.WhiteSmoke,
                                CornerRadius = 10,
                                Padding = 10,
                                BorderColor = Color.Black,
                                Content = sLayout
                            }
                        );
                         
                        
                    }
                    else
                    {
                        stacklayout.Children.Add(
                            new Frame()
                            {
                                BackgroundColor = Color.WhiteSmoke,
                                BorderColor = Color.Black,
                                CornerRadius = 10,
                                Padding = 10,
                                Content = new Label()
                                {
                                        
                                    Text = multiChoices[count] + " " + value,
                                    FontSize = 18,
                                    TextColor = Color.Black,
                                    HorizontalOptions = LayoutOptions.StartAndExpand,
                                    VerticalOptions = LayoutOptions.StartAndExpand,
                                    Padding = new Thickness(15, 0, 15, 5)
                                }
                            }
                        );
                    }
                    
                    
                    count++;
                }
                stacklayout.Children.Add(
                        new Label()
                        {
                            Text = "User Answer",
                            FontSize = 14,
                            TextColor = Color.Black,
                            HorizontalOptions = LayoutOptions.StartAndExpand
                        }

                    ); 
                stacklayout.Children.Add(
                    new Frame()
                    {
                        BackgroundColor = Color.WhiteSmoke,
                        CornerRadius = 10,
                        Padding = 10, 
                        Background = (uA == cA) ? Color.FromHex("#28a745") : Color.Red,
                        Content = new Label()
                        { 
                            Text = uA,
                            FontSize = 18,
                            TextColor = Color.White,
                            HorizontalOptions = LayoutOptions.StartAndExpand,
                            VerticalOptions = LayoutOptions.StartAndExpand,
                            Padding = new Thickness(15, 0, 15, 5)
                        }
                    }
                );
                stacklayout.Children.Add(
                    new Label()
                    {
                        Text = "Correct Answer",
                        FontSize = 14,
                        TextColor = Color.Black,
                        HorizontalOptions = LayoutOptions.StartAndExpand
                    }

                );
                stacklayout.Children.Add(
                    new Frame()
                    {
                        BackgroundColor = Color.WhiteSmoke,
                        CornerRadius = 10,
                        Padding = 10,
                        Background = Color.FromHex("#007bff"),
                        Content = new Label()
                        { 
                            Text = cA,
                            FontSize = 18,
                            TextColor = Color.White,
                            HorizontalOptions = LayoutOptions.StartAndExpand,
                            VerticalOptions = LayoutOptions.StartAndExpand,
                            Padding = new Thickness(15, 0, 15, 5)
                        }
                    }
                );
                questionnairesCont.Children.Add(stacklayout);
            }
        }
        private async void BackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}