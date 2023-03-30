using ConsoleAppForStudentsApp;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CourseWork_Client
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var task0 = GetIsStayLogined();
            task0.Wait();

            if (task0.Result)
            {
                var task1 = SecureStorage.GetAsync("username");
                task1.Wait();
                string saved_username = task1.Result;

                var task2 = SecureStorage.GetAsync("password");
                task2.Wait();
                string saved_password = task2.Result;

                bool loginedOnServer;
                var task3 = new Website().TryLogin(saved_username, saved_password, out loginedOnServer);
                if (loginedOnServer)
                {
                    MainPage = new NavigationPage(new TabbedPage1());
                    return;
                }
            }

            MainPage = new NavigationPage(new MainPage());
        }
        public static async Task<bool> GetIsStayLogined()
        {
            string logined = await Xamarin.Essentials.SecureStorage.GetAsync("logined");
            bool parsed;
            if (Boolean.TryParse(logined, out parsed))
            {
                return parsed;
            }
            return false;
        }

        protected override void OnStart()
        {
            // Handle when your app starts  
        }
        protected override void OnSleep()
        {
            // Handle when your app sleeps  
        }
        protected override void OnResume()
        {
            // Handle when your app resumes  
        }
    }
}
