using ConsoleAppForStudentsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConsoleAppForStudentsApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditAttendancePage : ContentPage
    {
        public bool IsEditComlited { get; private set; }
        private static Attendance _attendance;
        private static Attendance CurrentAttendance { get { return _attendance; } set {_attendance = value; } }
        public EditAttendancePage(Attendance attendance)
        {
            InitializeComponent();
            this.Title = attendance.ToString();
            CurrentAttendance = attendance;
            reason_Entry.Completed += Reason_Entry_Completed;
            editAttendance_button.Clicked += EditAttendance_button_Clicked;
            IsEditComlited = false;
        }

        private void EditAttendance_button_Clicked(object sender, EventArgs e)
        {
            bool result;
            var message = Website.Basic.EditAttendance(out result, CurrentAttendance.id, reason_Entry.Text);
            IsEditComlited = true;
            Navigation.RemovePage(this);
        }

        private async void Reason_Entry_Completed(object sender, EventArgs e)
        {
            if (reason_Entry.Text != "" && reason_Entry.Text != "" && !reason_Entry.Text.Contains(':'))
            {
                editAttendance_button.IsEnabled = true;
            }
            else
            {
                editAttendance_button.IsEnabled = false;
                await this.DisplayToastAsync("Описание не может быть пустым или содержать символ ':'", 4000);
            }
        }
    }
}