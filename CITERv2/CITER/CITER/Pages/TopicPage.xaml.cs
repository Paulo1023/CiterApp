using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Android.App;
using Android.Content;
using Android.Widget;
using CITER.Models;
using CITER.Pages;
using Firebase.Storage;
using Prism.Navigation;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Plugin.CloudFirestore;
using System.Collections.Generic;
using CITER.Data;
using CITER.Interfaces;

namespace CITER
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TopicPage : ContentPage
    {
        private readonly IFilesService filesService;
        FirebaseStorage firebaseStorage = new FirebaseStorage("citerapp-1e2e7.appspot.com");
        QuestionnairesRepository questRepository = new QuestionnairesRepository();

        Topics st = new Topics();
        Quiz quiz = new Quiz();

        public string title { get; set; }
        public string desc { get; set; }
        string provider;


        IEnumerable<Questionnaire> Questionnaires;
        public TopicPage(Topics tp)
        {
            InitializeComponent();

            title = tp.Title;
            desc = tp.Desc;
            provider = tp.Provider;
            filesService = DependencyService.Get<IFilesService>();

            BindingContext = this;

        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            PopupLoading.IsVisible = false;

            moduleBtn.IsEnabled = false;
            var fp = Path.Combine(Android.App.Application.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryDownloads).AbsolutePath, title + ".pdf");
            if (File.Exists(fp))
            {
                downloadBtn.IsEnabled = false;
                downloadBtn.Animation = "done.json";
                downloadBtn.RepeatCount = 0;
                moduleBtn.IsEnabled = true;
            }
            else
            {
                downloadBtn.Animation = "downloading.json";
                downloadBtn.StopAnimation();
            }


        }
        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();
        }

        private async void BackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }





        private void OpenReviewerClicked(object sender, EventArgs e)
        {

            string pathToNewFolder = Path.Combine(filesService.StorageDirectory, Android.OS.Environment.DirectoryDownloads);
            string pathToNewFile = Path.Combine(pathToNewFolder, title + ".pdf");
            filesService.OpenFile(pathToNewFile);

        }

        private async void DownloadClicked(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    downloadBtn.PlayAnimation();
                    downloadBtn.RepeatCount = 20;
                    var url = await firebaseStorage
                            .Child("ReviewerFile")
                            .Child(title + ".pdf")
                            .GetDownloadUrlAsync();
                    var filePath = Path.Combine(Android.App.Application.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryDownloads).AbsolutePath, title + ".pdf");

                    var httpClient = new HttpClient();
                    var pdfBytes = await httpClient.GetByteArrayAsync(url + "alt=media");
                    File.WriteAllBytes(filePath, pdfBytes);
                    downloadBtn.Animation = "done.json";
                    downloadBtn.PlayAnimation();
                    downloadBtn.RepeatCount = 0;
                    downloadBtn.IsEnabled = false;
                    moduleBtn.IsEnabled = true;
                    SaveTopic();
                    await DisplayAlert("Complete", "Download Complete", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "OK");
                    downloadBtn.StopAnimation();
                }
            }
            else
            {
                await DisplayAlert("Error", "Connection Error", "OK");
            }
        }



        private async void StartMockTestClicked(object sender, EventArgs e)
        {
                try
                {
                    animationView.PlayAnimation();
                    PopupLoading.IsVisible = true;
                    await questRepository.DelQuestionnaires();
                    var current = Connectivity.NetworkAccess;
                    if (current == NetworkAccess.Internet)
                    {
                        RetrieveQuestions();
                        await Navigation.PushAsync(new QuizPage(title));
                        animationView.StopAnimation();
                        PopupLoading.IsVisible = false;
                    }
                    else
                    {
                        await DisplayAlert("Connection Error", "No intenet connection", "OK");
                        animationView.StopAnimation();
                        PopupLoading.IsVisible = false;
                    }

                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "OK");
                }
          
            
            


        }

        private async void RetrieveQuestions()
        {
            int qid = 1;

            try
            {
                var Query = await CrossCloudFirestore.Current
                                                   .Instance
                                                   .CollectionGroup("Quiz")
                                                   .WhereEqualsTo("topic", title)
                                                   .GetAsync();


                Questionnaires = Query.ToObjects<Questionnaire>().Shuffle();
                foreach (Questionnaire data in Questionnaires)
                {
                    quiz.topic = data.topic;
                    quiz.questionID = qid;
                    quiz.section = data.section;
                    quiz.question = data.question;
                    quiz.qImage = data.qImage;
                    quiz.choices = string.Join("|", data.choices.Shuffle());
                    quiz.userAnswer = "";
                    quiz.correctAnswer = data.correctAnswer;

                   

                    qid++;

                    await questRepository.SaveQuestionaires(quiz);
                }
            }
            catch (Exception e)
            {
                await DisplayAlert("Server Error ffff", e.Message, "OK");
                await Navigation.PopAsync();
            }
            


        }

        private void SaveTopic()
        {
            int topicId;
            TopicsRepository topicsRepository = new TopicsRepository();
            var count = topicsRepository.CountTopic();

            if (count.Equals(0))
            {
                topicId = 1;
            }
            else 
            {
                topicId = count.Result+1;
            }
            Downloads topicData = new Downloads() { 
                TopicID = topicId,
                Title = title,
                Desc = desc,
                Provider = provider 
            };

            topicsRepository.SaveTopic(topicData);
             
        }
        
    }
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.Shuffle(new Random());
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (rng == null) throw new ArgumentNullException(nameof(rng));

            return source.ShuffleIterator(rng);
        }

        private static IEnumerable<T> ShuffleIterator<T>(
            this IEnumerable<T> source, Random rng)
        {
            var buffer = source.ToList();
            for (int i = 0; i < buffer.Count; i++)
            {
                int j = rng.Next(i, buffer.Count);
                yield return buffer[j];

                buffer[j] = buffer[i];
            }
        }
    }
}