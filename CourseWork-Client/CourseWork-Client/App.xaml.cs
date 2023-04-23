using ConsoleAppForStudentsApp;
using OneSignalSDK.Xamarin;
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

            var taskLogined = Client.GetIsStayLogined();
            taskLogined.Wait();

            if(!taskLogined.Result)
                MainPage = new NavigationPage(new LoginPage());

            if (taskLogined.Result)
            {
                var taskGetUsername = SecureStorage.GetAsync("username");
                taskGetUsername.Wait();
                string saved_username = taskGetUsername.Result;

                var taskGetPassword = SecureStorage.GetAsync("password");
                taskGetPassword.Wait();
                string saved_password = taskGetPassword.Result;

                bool isLoginedOnServer;
                var taskTryToLoginOnServer = Website.Basic.TryLogin(saved_username, saved_password, out isLoginedOnServer);
                if (isLoginedOnServer)
                {
                    MainPage = new NavigationPage(new MainPage());
                    return;
                }
            }
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
    public class Client
    {
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
    }
}
