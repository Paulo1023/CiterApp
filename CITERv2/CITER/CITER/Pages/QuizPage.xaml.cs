using CITER.Data;
using CITER.Models;
using Firebase.Storage;
using Plugin.CloudFirestore;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CITER.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuizPage : ContentPage
    {
        List<Quiz> questionList = new List<Quiz>();
        Quiz quiz = new Quiz();
        ActivityRepository actRepository = new ActivityRepository();
        QuestionnairesRepository questRepository = new QuestionnairesRepository();
        UserAnsRepository userAnswer = new UserAnsRepository();
        Exam examAns = new Exam();
        FirebaseStorage firebaseStorage = new FirebaseStorage("citerapp-1e2e7.appspot.com");
        public int TimerHours { get; set; } = 2;
        public int TimerMinutes { get; set; } = 00;
        public int TimerSeconds { get; set; } = 00;
        int actID;
        string title;
        bool stopTimer;
        string today = "";
        string tConsumed;
        double score = 0;
        double items = 0;
        double percentage;
        public string Ans { get; set; } = " ";
        public int questNum { get; set; } = 1;

        public QuizPage(string title)
        {
            InitializeComponent();
            BindingContext = this;
            this.title = title;
            today = GetNetworkTime();
            RandomNum();

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();


            prevBtn.IsEnabled = false;
            submitBtn.IsEnabled = false;
            nextBtn.IsEnabled = false;
            qNum.Text = questNum.ToString();

            int numTry = 0;
            do
            {
                questionList = await questRepository.ListQuestionnaires();
                numTry++;
                if (numTry >= 1000)
                {
                    await Navigation.PopAsync();
                    await DisplayAlert("Server Error", "There was a problem with the server. Please contact the admin", "OK");
                }
            } while (questionList.Count < 60); //while less than 60



            DisplayQuestions(questNum);



            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                int h = TimerHours;
                if (stopTimer)
                {
                    return false;
                }
                if (TimerHours == 0 && TimerMinutes == 0 && TimerSeconds == 0)
                {
                    animationView.PlayAnimation();
                    PopupLoading.IsVisible = true;
                    tConsumed = "02:00:00";
                    SaveActivity();
                    return false;
                }
                if (TimerSeconds == 0 && TimerMinutes == 0 && TimerHours > 0)
                {
                    TimerMinutes = 60;
                    TimerHours--;
                }
                if (TimerSeconds == 0 && TimerMinutes >= 0)
                {
                    TimerMinutes--;
                    TimerSeconds = 60;
                }
                TimerSeconds--;
                val1.Text = TimerHours.ToString();
                val2.Text = TimerMinutes.ToString();
                val3.Text = TimerSeconds.ToString();
                tConsumed = (1 - TimerHours) + ":" + (59 - TimerMinutes) + ":" + (60 - TimerSeconds);
                return true;
            });
        }

        
        private async void DisplayQuestions(int id)
        {
            prevBtn.IsEnabled = false;
            submitBtn.IsEnabled = false;
            nextBtn.IsEnabled = false;


            questionList = await questRepository.ListQuestionnaires();

            

            foreach (Quiz data in questionList)
            {
                if (data.questionID == id)
                {
                    quiz.topic = data.topic;
                    quiz.questionID = data.questionID;
                    quiz.question = data.question;
                    quiz.qImage = data.qImage;
                    quiz.choices = data.choices;
                    quiz.correctAnswer = data.correctAnswer;
                    quiz.userAnswer = data.userAnswer;
                }
            }


            questionCont.Children.Add(
                new Label()
                {
                    Text = Convert.ToString(quiz.questionID) + ". " + quiz.question,
                    FontSize = 18,
                    TextColor = Color.Black
                }
            );
            if ((!string.IsNullOrEmpty(quiz.qImage)) && (quiz.qImage.Contains("png") || quiz.qImage.Contains("jpeg") || quiz.qImage.Contains("jpg")))
            {
                try
                { 
                    var ImageUrl = await firebaseStorage
                                   .Child("QuizImage")
                                   .Child(quiz.qImage)
                                   .GetDownloadUrlAsync();
                    Grid grid = new Grid()
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand

                    };
                    Image questImage = new Image()
                    {
                        Source = ImageUrl,
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
                        await Navigation.PushPopupAsync(new ViewImage(ImageUrl));
                    };

                    grid.Children.Add(questImage);
                    grid.Children.Add(imgBtn);

                    questionCont.Children.Add(grid);
                }
                catch (Exception exc)
                {
                    await DisplayAlert("Server Error", exc.Message, "OK");
                    await Navigation.PopAsync();
                }



            }
            string[] arrChoices = quiz.choices.Split('|');

            int count = 0;
            string[] multiChoices = { "A.", "B.", "C.", "D.", "E.", "F.", "G.", "H.", "I.", "J.", "K.", "L.", "M.", "N.", "O.", "P.", "Q.", "R.", "S.", "T.", "U.", "V.", "W.", "X.", "Y.", "Z." };
            foreach (var data in arrChoices)
            {

                RadioButton radButton = new RadioButton();


                radButton.VerticalOptions = LayoutOptions.FillAndExpand;
                radButton.HorizontalOptions = LayoutOptions.FillAndExpand;
                radButton.GroupName = "choices";
                radButton.CheckedChanged += async (s, e) => {
                    await questRepository.Update(data, quiz.questionID);
                };

                if (data == quiz.userAnswer)
                {
                    radButton.IsChecked = true;
                }

                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += async (s, e) =>
                {
                    radButton.IsChecked = true;
                    await questRepository.Update(data, quiz.questionID);

                };


                if (data.ToLower().Contains(".png") || data.ToLower().Contains(".jpeg") || data.ToLower().Contains(".jpg"))
                {
                    try 
                    {
                        
                        var choicesImageUrl = await firebaseStorage
                                    .Child("QuizImage")
                                    .Child(data)
                                    .GetDownloadUrlAsync();
                        Grid grid = new Grid()
                        {
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand

                        };
                        Image img = new Image()
                        {
                            Source = choicesImageUrl,
                            Aspect = Aspect.AspectFill,
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            Margin = 0
                        };
                        ImageButton btn = new ImageButton()
                        {
                            Source = "@drawable/zoomin",
                            CornerRadius = 25,
                            WidthRequest = 32,
                            HeightRequest = 32,
                            HorizontalOptions = LayoutOptions.EndAndExpand,
                            VerticalOptions = LayoutOptions.StartAndExpand,
                            BackgroundColor = Color.FromHex("#00FBC1BC")
                        };

                        btn.Clicked += async (sender, e) =>
                        {
                            await Navigation.PushPopupAsync(new ViewImage(choicesImageUrl));
                        };


                        grid.Children.Add(img);
                        grid.Children.Add(btn);


                        radButton.Content = multiChoices[count];
                        StackLayout sLayout = new StackLayout()
                        {

                            BackgroundColor = Color.WhiteSmoke,
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            Padding = 10,
                            Children = {
                        radButton,
                        grid
                        }
                        };
                        questionCont.Children.Add(
                            new Frame()
                            {
                                BackgroundColor = Color.WhiteSmoke,
                                CornerRadius = 10,
                                Padding = 10,
                                BorderColor = Color.Black,
                                Content = sLayout
                            }
                        );
                        sLayout.GestureRecognizers.Add(tapGestureRecognizer);
                        
                            
                        
                        
                    }
                    catch (Exception e)
                    {
                        radButton.Content = multiChoices[count] + " " + data;

                        Frame frame = new Frame()
                        {
                            BackgroundColor = Color.WhiteSmoke,
                            CornerRadius = 10,
                            Padding = 10,
                            BorderColor = Color.Black,
                            Content = radButton
                        };

                        questionCont.Children.Add(frame);
                        frame.GestureRecognizers.Add(tapGestureRecognizer);
                    }
                    
                }
                else
                {
                    radButton.Content = multiChoices[count] + " " + data;

                    Frame frame = new Frame()
                    {
                        BackgroundColor = Color.WhiteSmoke,
                        CornerRadius = 10,
                        Padding = 10,
                        BorderColor = Color.Black,
                        Content = radButton
                    };

                    questionCont.Children.Add(frame);
                    frame.GestureRecognizers.Add(tapGestureRecognizer);
                }

                


            count++;



            }


            prevBtn.IsEnabled = true;
            submitBtn.IsEnabled = true;
            nextBtn.IsEnabled = true;
            if (questNum == 1)
            {
                prevBtn.IsEnabled = false;
            }
            else { prevBtn.IsEnabled = true; }
            if (questNum == 60)
            {
                nextBtn.IsEnabled = false;
            }
            else { nextBtn.IsEnabled = true; }
        }


        private void BackClicked(object sender, EventArgs e)
        {
            BackAlertAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            BackAlertAsync();
            return true;
        }
        private async void BtnSubmitClicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert("Submit Answer", "Are you sure you want to submit all your answer? ", "Yes", "No");
            if (result)
            {
                animationView.PlayAnimation();
                PopupLoading.IsVisible = true;
                stopTimer = true;
                SaveActivity();
            }
        }
        private void NextBtnClicked(object sender, EventArgs e)
        {
            
            questionCont.Children.Clear();
            questNum++;
            qNum.Text = questNum.ToString();
            DisplayQuestions(questNum);
        }
        private void PrevBtnClicked(object sender, EventArgs e)
        {
            questionCont.Children.Clear();
            questNum--;
            qNum.Text = questNum.ToString();
            DisplayQuestions(questNum);
        }
       


       

        private bool BackAlertAsync()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {

                var result = await DisplayAlert("Exit", "The current progress that you have won't be saved, Are you sure you want to exit? ", "Yes", "No");

                if (result)
                {
                    await Navigation.PopAsync();
                }
            });
            return true;
        }



        private async void RandomNum()
        {
            int count;
            int randID;
            do
            {
                Random rand = new Random();
                randID = rand.Next();
                var questions = await userAnswer.CheckActID(randID);
                count = questions.Count;
            } while (count >= 1);
            actID = randID;
        }
        public static string GetNetworkTime()
        {
            const string ntpServer = "time.upd.edu.ph";
            var ntpData = new byte[48];
            ntpData[0] = 0x1B; //LeapIndicator = 0 (no warning), VersionNum = 3 (IPv4 only), Mode = 3 (Client Mode)

            var addresses = Dns.GetHostEntry(ntpServer).AddressList;
            var ipEndPoint = new IPEndPoint(addresses[0], 123);
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            socket.Connect(ipEndPoint);
            socket.Send(ntpData);
            socket.Receive(ntpData);
            socket.Close();

            ulong intPart = (ulong)ntpData[40] << 24 | (ulong)ntpData[41] << 16 | (ulong)ntpData[42] << 8 | (ulong)ntpData[43];
            ulong fractPart = (ulong)ntpData[44] << 24 | (ulong)ntpData[45] << 16 | (ulong)ntpData[46] << 8 | (ulong)ntpData[47];
            var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
            var networkDateTime = (new DateTime(1900, 1, 1)).AddMilliseconds((long)milliseconds);
            return networkDateTime.AddHours(8).ToString("dd/MM/yyyy h:mm tt"); //PH local time +08:00
        }

        private async void SaveActivity()
        { 
            var questions = await questRepository.ListQuestionnaires();
            foreach (var data in questions)
            {
                examAns.QuestionnaireID = data.questionID;
                examAns.ActivityID = actID;
                examAns.Topic = data.topic;
                examAns.Section = data.section;
                examAns.Question = data.question;
                examAns.choices = data.choices;
                examAns.qImage = data.qImage;
                examAns.UserAnswer = data.userAnswer;
                examAns.CorrectAnswer = data.correctAnswer;

                if ((!string.IsNullOrEmpty(data.qImage)) && (data.qImage.Contains(".png") || data.qImage.Contains(".jpeg") || data.qImage.Contains(".jpg")))
                { 
                    try 
                    {
                        var url = await firebaseStorage
                                   .Child("QuizImage")
                                   .Child(data.qImage)
                                   .GetDownloadUrlAsync();
                        var fPath = Path.Combine(Android.App.Application.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryDownloads).AbsolutePath, data.qImage);
                        var httpClient = new HttpClient();
                        var pdfBytes = await httpClient.GetByteArrayAsync(url+ "alt=media");
                        File.WriteAllBytes(fPath, pdfBytes);
                    }
                    catch (Exception ex)
                    {
                        //await DisplayAlert("Server Error", ex.Message, "OK");
                        //await Navigation.PopAsync();
                    }
                }

                string[] arrChoices = data.choices.Split('|');
                foreach (var val in arrChoices)
                {
                    if (val.ToLower().Contains(".png") || val.ToLower().Contains(".jpeg") || val.ToLower().Contains(".jpg"))
                    {
                        try
                        {
                            var url = await firebaseStorage
                                        .Child("QuizImage")
                                        .Child(val)
                                        .GetDownloadUrlAsync();
                            var fPath = Path.Combine(Android.App.Application.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryDownloads).AbsolutePath, val);
                            var httpClient = new HttpClient();
                            var pdfBytes = await httpClient.GetByteArrayAsync(url + "alt=media");
                            File.WriteAllBytes(fPath, pdfBytes);
                        }
                        catch (Exception ex)
                        {
                            //await DisplayAlert("Server Error", ex.Message, "OK");
                            //await Navigation.PopAsync();
                        }
                    }
                }

                await userAnswer.Save(examAns);
            }


            var i = await userAnswer.CheckActID(actID);
            items = i.Count;
            var s = await userAnswer.CheckCorrect(actID);
            score = s.Count;

            percentage = score * 100 / 60;
            Activity act = new Activity()
            {
                ActivitiesID = actID,
                Title = title,
                Score = score.ToString(),
                Percentage = Math.Round(percentage, 2).ToString(),
                Time = tConsumed,
                Date = today
            };
            await actRepository.Save(act);
            await Navigation.PushAsync(new QuizResult(act));
        }

    }
    
}