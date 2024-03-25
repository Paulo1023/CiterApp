
using CITER.Data;
using CITER.Models;
using CITER.Pages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CITER
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();



            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
            if (Settings.FirstRun)
            {

                Preferences.Set("ManualShow", "True");
                Preferences.Set("StartupShow", "True");

                using (Stream stream =
                assembly.GetManifestResourceStream("CITER.Database.citerdb.db"))
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        stream.CopyTo(memoryStream);

                        File.WriteAllBytes(dbPath.path, memoryStream.ToArray());
                    }
                }

                Settings.FirstRun = false;
            }
            //else
            //{
            //    MainPage = new NavigationPage(new MainPage());
            //}
            var startupShow = Preferences.Get("StartupShow", "True");
            if (startupShow == "True")
            {
                MainPage = new NavigationPage(new StartPage());
            }
            else
            {
                MainPage = new NavigationPage(new MainPage());
            }

        }
        
        public static class Settings
        { 
            public static bool FirstRun
            {
                get => Preferences.Get(nameof(FirstRun), true);
                set => Preferences.Set(nameof(FirstRun), value);
            }
            
        }
        public static class dbPath
        {
            public static string path { get; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "citerdb.db");
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
