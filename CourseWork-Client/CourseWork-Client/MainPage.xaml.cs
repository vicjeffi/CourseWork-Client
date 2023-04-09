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
using System.Net.NetworkInformation;
using ConsoleAppForStudentsApp.Models;
using System.Reflection;

namespace CourseWork_Client
{
    public partial class MainPage : ContentPage
    {
        public static Website Site { get; set; }
        public ICommand OpenUrlAction => new Command<string>(async(url) => await Launcher.OpenAsync(url));
        public MainPage()
        {
            Site = new Website();
            InitializeComponent();
            BindingContext = this;
        }
        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            loading_indicator.IsRunning = true;
            await Task.Delay(1);

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

                //await DisplayAlert("Залогинился!", message, "OK");

                bool result;
                TabbedPage1.status = Site.GetStatus(out result);

                if (!result)
                    await DisplayAlert("Ошибка!", TabbedPage1.status, "OK");

                if (result)
                {
                    await Navigation.PushAsync(new TabbedPage1());
                    Navigation.RemovePage(this);
                }
            }
            else
            {
                loading_indicator.IsRunning = false;
                await DisplayAlert("Ошибка!", message, "OK");
            }
        }
        private async void OnServerStatusClicked(object sender, EventArgs e)
        {
            bool result = false;
            string status;
            
            long ping = Site.ServerStatus(out result);
            status = result == true ? "ОК" : "ДЕД";
            status = ping <= 5000 ? "ПАЙТОН РОТ КРУТИЛ" : status;
            status = ping <= 1000 ? "ПОЧТИ ДЕД" : status;
            status = ping <= 500 ? "ДОЛГИЙ" : status;
            status = ping <= 250 ? "Нормас" : status;
            status = ping <= 150 ? "БЫСТРО" : status;

            await DisplayAlert("Статус сервера: " + status, ping.ToString() + " Секунд", "OK");
        }
        protected override void OnAppearing()
        {
            BindingContext = this;
        }
        
    }
    public class Website
    {
        public static HttpClient client;
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

        private static string noHTTPS_url = @"vicjeffi.pythonanywhere.com";
        public static string NoHttps_Url { get { return noHTTPS_url; } }

        public string GetStatus(out bool result)
        {
            string message = string.Empty;

            var task1 = client.GetAsync(url);
            task1.Wait();
            var http_result = task1.Result;

            var task2 = http_result.Content.ReadAsStringAsync();
            task2.Wait();

            var json = JObject.Parse(task2.Result);

            message = json.GetValue("status").ToString();

            if (http_result.StatusCode == HttpStatusCode.OK)
                result = true;
            else
                result = false;

            return message;
        }

        public string GetId(out bool result)
        {
            string message = string.Empty;

            var task1 = client.GetAsync(url);
            task1.Wait();
            var http_result = task1.Result;

            var task2 = http_result.Content.ReadAsStringAsync();
            task2.Wait();

            var json = JObject.Parse(task2.Result);

            message = json.GetValue("id").ToString();

            if (http_result.StatusCode == HttpStatusCode.Created)
                result = true;
            else
                result = false;

            return message;
        }
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

        public string GetGroupInfo(string index, out bool result)
        {
            string message = string.Empty;

            var task1 = client.GetAsync(url + $"api/get-group?group_index=" + index);
            task1.Wait();
            var http_result = task1.Result;

            var task2 = http_result.Content.ReadAsStringAsync();
            task2.Wait();

            message += task2.Result;

            if (http_result.StatusCode == HttpStatusCode.Created)
                result = true;
            else
                result = false;

            return message;
        }

        public long ServerStatus(out bool result)
        {
            result = false;
            var ping = new System.Net.NetworkInformation.Ping();

            var _result = ping.Send(noHTTPS_url);

            if (_result.Status == System.Net.NetworkInformation.IPStatus.Success)
            {
                result = true;
                return _result.RoundtripTime;
            }
            return 0;
        }
        public string PostAttendance(out bool result, string student_id, string discipline, DateTime time)
        {
            string _time = time.ToString("dd'-'MM'-'yyyy'-'HH'-'mm'-'ss");

            string message = string.Empty;

            var task1 = client.GetAsync(url + $"add-attendance?student-id={student_id}&discipline-id={discipline}&time={_time}");
            task1.Wait();
            var http_result = task1.Result;

            var task2 = http_result.Content.ReadAsStringAsync();
            task2.Wait();

            message += task2.Result;

            if (http_result.StatusCode == HttpStatusCode.Created)
                result = true;
            else
                result = false;

            return message;
        }
    }
}
