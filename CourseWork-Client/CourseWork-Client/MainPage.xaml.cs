using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CourseWork_Client
{
    public partial class MainPage : ContentPage
    {
        Action FailueToLogin;
        public MainPage()
        {
            InitializeComponent();
            FailueToLogin += () => DisplayAlert("Error", Password_Entry.Text, "OK");
        }

        protected override void OnAppearing()
        {
            BindingContext = this;
            base.OnAppearing();
        }
        private void OnSignUpButtonClicked(object sender, EventArgs e)
        {

        }
        private void OnLoginButtonClicked(object sender, EventArgs e)
        {
            FailueToLogin();
        }
    }
}
