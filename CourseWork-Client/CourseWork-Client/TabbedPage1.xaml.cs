using CourseWork_Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using ConsoleAppForStudentsApp.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConsoleAppForStudentsApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedPage1 : TabbedPage
    {
        public static string status = "None";
        public static Website Site { get; private set; }
        public IUser User { get; }
        public TabbedPage1()
        {
            switch (status)
            {   case "None":
                    DisplayAlert("Ошибка!", "Что то пошло не так", "OK");
                    break;
                case "teacher":
                    User = new Teacher();
                    break;
                case "student":
                    User = new Student();
                    break;
                case "admin":
                    User = new Admin();
                    break; }

            bool result = false;
            User.LoadMyData(out result);
            if (!result)
                throw new Exception("WTF");

            InitializeComponent();
            BindingContext = this;
        }
        private async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            await Xamarin.Essentials.SecureStorage.SetAsync("logined", "false");
            await Navigation.PushAsync(new MainPage());
        }
    }
}