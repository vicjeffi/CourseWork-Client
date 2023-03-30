using CourseWork_Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConsoleAppForStudentsApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedPage1 : TabbedPage
    {
        public static Website Site { get; private set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public TabbedPage1()
        {
            Username = Application.Current.Properties["username"].ToString();
            Password = Application.Current.Properties["password"].ToString();

            InitializeComponent();
            BindingContext = this;
        }
        private async void OnSingUpButtonClicked(object sender, EventArgs e)
        {
            await Xamarin.Essentials.SecureStorage.SetAsync("logined", "false");
            await Navigation.PushAsync(new MainPage());
        }
    }
}