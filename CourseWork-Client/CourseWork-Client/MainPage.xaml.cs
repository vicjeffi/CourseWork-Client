using ConsoleAppForStudentsApp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace CourseWork_Client
{
    
    public partial class MainPage : ContentPage
    {
        public static Website Site { get; private set; }
        public ICommand OpenUrlAction => new Command<string>(async(url) => await Launcher.OpenAsync(url));
        public MainPage()
        {
            Site = new Website();
            InitializeComponent();
            BindingContext = this;
        }
        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            if (savelogin_checkBox.IsChecked)
            {
                if(username_Entry.Text != "" || password_Entry.Text != "")
                    await Xamarin.Essentials.SecureStorage.SetAsync("logined", "true");
                else
                    await Xamarin.Essentials.SecureStorage.SetAsync("logined", "false");
            }
            string username = username_Entry.Text;
            string password = password_Entry.Text;

            // запрос на логирование на сервере

            bool logined;
            var message = Site.TryLogin(username, password, out logined);

            if (logined)
            {
                Application.Current.Properties["username"] = username;
                Application.Current.Properties["password"] = password;

                bool isStayLogined = false;
                Boolean.TryParse(await Xamarin.Essentials.SecureStorage.GetAsync("logined"), out isStayLogined);
                if (isStayLogined)
                {
                    await Xamarin.Essentials.SecureStorage.SetAsync("username", username_Entry.Text);
                    await Xamarin.Essentials.SecureStorage.SetAsync("password", password_Entry.Text);
                }

                await DisplayAlert("Залогинился!", message, "OK");

                await Navigation.PushAsync(new TabbedPage1());
                Navigation.RemovePage(this);
            }
            else
            {
                await DisplayAlert("Ошибка!", message, "OK");
            }
        }
        private async void OnSingUpButtonClicked(object sender, EventArgs e)
        {
            // пока так, но потом отдельная страница
            await Browser.OpenAsync(Website.Url);
        }
        protected override void OnAppearing()
        {
            BindingContext = this;
        }
        
    }
    public class Website
    {
        private HttpClient client;
        public Website()
        {
            if(client == null)
            {
                client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
            }
        }
        private static string url = @"https://vicjeffi.pythonanywhere.com/";
        public static string Url { get { return url; } }
        public string TryRegistration(string username, string password, out bool result, ContentPage myStartPage)
        {
            string message = "";
            // логика отправки запроса на регу
            if (true)
            {
                // логика
                result = true;

                message = "Успешно!";
            }
            else
            {
                #pragma warning disable CS0162
                result = false;

                message = "Писдосики, ошибочка";
                #pragma warning restore CS0162
            }
            // вернуть ответ
            return message;
        }

        //Доделать
        public string TryLogin(string username, string password, out bool result)
        {
            string message = string.Empty;

            var task1 = client.GetAsync(url + $"login?username={username}&password={password}");
            task1.Wait();
            var http_result = task1.Result;

            var task2 = http_result.Content.ReadAsStringAsync();
            task2.Wait();

            var json = JObject.Parse(task2.Result);

            message = json.GetValue("message").ToString();

            if (http_result.StatusCode == HttpStatusCode.Created)
                result = true;
            else
                result = false;

            return message;
        }

    }
}
