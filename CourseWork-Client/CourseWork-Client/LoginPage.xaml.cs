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
    public partial class LoginPage : ContentPage
    {
        public static Website Site { get; set; }
        public LoginPage()
        {
            Site = Website.Basic;

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
                MainPage.status = Site.GetStatus(out result);

                if (!result)
                    await DisplayAlert("Ошибка!", MainPage.status, "OK");

                if (result)
                {
                    await Navigation.PushAsync(new MainPage());
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
            status = ping <= 5000 ? "ПАЙТОН КРУТОЙ ЯЗЫК" : status;
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
}
