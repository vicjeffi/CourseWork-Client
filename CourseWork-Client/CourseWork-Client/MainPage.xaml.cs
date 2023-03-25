using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CourseWork_Client
{
    
    public partial class MainPage : ContentPage
    {
        public readonly string website = @"http://VicJeffi.pythonanywhere.com";
        public string Website
        {
            get
            {
                return website;
            }
        }
        public ICommand NoAccountAction => new Command<string>(async(url) => await Launcher.OpenAsync(url));
        public MainPage()
        {
            InitializeComponent();
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

        }
    }
}
