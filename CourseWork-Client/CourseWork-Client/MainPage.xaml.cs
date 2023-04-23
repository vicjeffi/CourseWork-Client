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
using Newtonsoft.Json;
using OneSignalSDK.Xamarin;
using System.Threading;
using Xamarin.Essentials;

namespace ConsoleAppForStudentsApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : TabbedPage
    {
        public static string status = "None";
        public static Website Site { get; private set; }
        private Group _selectedGroup;
        public Group SelectedGroup
        {
            get
            {
                return _selectedGroup;
            }
            set
            {
                if (_selectedGroup != value)
                {
                    _selectedGroup = value;
                }
            }
        }
        private Discipline _selectedDiscipline;
        public Discipline SelectedDiscipline
        {
            get
            {
                return _selectedDiscipline;
            }
            set
            {
                if (_selectedDiscipline != value)
                {
                    _selectedDiscipline = value;
                }
            }
        }
        private List<Attendance> _studentAttendance = new List<Attendance>();
        public List<Attendance> StudentAttendance
        {
            get
            {
                return _studentAttendance;
            }
            set
            {
                if (_studentAttendance != value)
                {
                    _studentAttendance = value;
                }
            }
        }
        public IUser User { get; private set; }
        public string StatusColor { get; private set; }
        public string StatusBackColor { get; private set; }
        private List<Group> Groups = new List<Group>();
        public MainPage()
        {
            if (Site == null)
                Site = Website.Basic;

            LoadUserStatus();

            bool result = false;
            User.LoadMyData(out result);

            InitializePush(User.id);
            InitializeComponent();

            BindingContext = this;
            switch (status)
            {
                case "teacher":
                    addAttendance_button.IsVisible = true;
                    clearSelection_button.IsVisible = true;
                    studentsCollection_cv.IsVisible = true;
                    statusContent.IsVisible = true;
                    profile_status_label.Text = "Вы Учитель";

                    var groupPicker = new Picker()
                    {
                        Title = "Группа: "
                    };
                    var disciplinePicker = new Picker()
                    {
                        Title = "Дисциплина: "
                    };
                    statusName.Text = "Добавить отсутствие ученика";

                    studentsCollection_cv.SelectionChanged += mainCollection_cv_OnSelection;
                    groupPicker.SelectedIndexChanged += groupPicker_SelectedIndexChanged;
                    disciplinePicker.SelectedIndexChanged += disciplinePicker_SelectedIndexChanged;

                    Teacher t = User as Teacher;
                    t.LoadDisciplines(out result);
                    if (t != null)
                    {
                        foreach (var group in t.groups)
                        {
                            var message = Site.GetGroupInfo(group, out result);
                            Group _group = JsonConvert.DeserializeObject<Group>(message);
                            _group.LoadStudents(out result);
                            if (_group != null)
                                Groups.Add(_group);
                        }
                    }
                    groupPicker.ItemsSource = Groups;
                    disciplinePicker.ItemsSource = t.Disciplines;
                    statusContent.Children.Add(groupPicker);
                    statusContent.Children.Add(disciplinePicker);
                    break;
                case "student":
                    editAttendance_button.IsVisible = true;
                    attendancesCollection_cv.IsVisible = true;
                    attendancesCollection_cv.SelectionChanged += attendancesCollection_cv_OnSelection;
                    profile_status_label.Text = "Вы Ученик";
                    statusName.Text = "Ваши отсутствия:";

                    Student student = User as Student;

                    student.LoadMyUnCheckedAttendances();
                    StudentAttendance = student.Attendances;

                    if (StudentAttendance.Count > 0)
                        attendancesCollection_cv.SetBinding(ItemsView.ItemsSourceProperty, "StudentAttendance");
                    else
                    {
                        statusContent.IsVisible = true;
                        statusContent.Children.Add(new Label() { HorizontalTextAlignment = TextAlignment.Center, Text = "У вас нет прогулов. Продолжайте в том же духе!", FontSize = 20f });
                        editAttendance_button.Text = "Отмечать то нечего...";
                    }
                    this.ToolbarItems.Add(new ToolbarItem("Обновить данные", null, async () => await ReloadPage()));
                    break;
                case "admin":
                    profile_status_label.Text = "Вы Адиминиcтрация";
                    break;
            }
        }
        private void disciplinePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            picker.Title = "Выбранная группа - " + (picker.SelectedItem as Discipline).ToString();
            SelectedDiscipline = (picker.SelectedItem as Discipline);

            if (studentsCollection_cv.SelectedItems.Count >= 1 && SelectedDiscipline != null)
                addAttendance_button.IsEnabled = true;
        }
        private void groupPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            picker.Title = "Выбранная группа - " + (picker.SelectedItem as Group).ToString();
            SelectedGroup = (picker.SelectedItem as Group);

            if (studentsCollection_cv != null)
            {
                studentsCollection_cv.ItemsSource = null;
                studentsCollection_cv.ItemsSource = SelectedGroup.Student;
            }
        }
        private void mainCollection_cv_OnSelection(object sender, EventArgs e)
        {
            if(studentsCollection_cv.SelectedItems.Count >= 1 && SelectedDiscipline != null)
                addAttendance_button.IsEnabled = true;
            else
            {
                addAttendance_button.IsEnabled = false;
            }
        }
        private void attendancesCollection_cv_OnSelection(object sender, EventArgs e)
        {
            if (attendancesCollection_cv.SelectedItems.Count >= 1)
                editAttendance_button.IsEnabled = true;
            else
            {
                editAttendance_button.IsEnabled = false;
            }
        }

        private async void toolbar_OnLogoutButtonClicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Подтверждение", "Вы точно хотите выйти из профиля?", "Да", "Нет");
            if (answer)
            {
                await OneSignal.Default.RemoveExternalUserId();
                await Xamarin.Essentials.SecureStorage.SetAsync("logined", "false");
                await Navigation.PushAsync(new LoginPage());
                Navigation.RemovePage(this);
            }
        }
        private void clearSelection_button_OnClearSelection(object sender, EventArgs e)
        {
            studentsCollection_cv.SelectedItems.Clear();
            addAttendance_button.IsEnabled = false;
        }
        private async void addAttendance_button_OnAddAttendance(object sender, EventArgs e)
        {
            string students = "";
            foreach (Student s in studentsCollection_cv.SelectedItems)
                students += s.ToString() + "\n";
            bool answer = await DisplayAlert("Вы точно хотите отметить отсутствие этих учеников?", students + "\n" + "По дисциплине: " + SelectedDiscipline.ToString(), "Да", "Нет");
            if(answer)
            {
                foreach(Student student in studentsCollection_cv.SelectedItems)
                {
                    bool result;
                    var message = Site.PostAttendance(out result, student.id, SelectedDiscipline.id, DateTime.Now);
                    if (!result)
                        await DisplayAlert("Ошибка!", message, "Ок");
                }
            }
            studentsCollection_cv.SelectedItems.Clear();
            addAttendance_button.IsEnabled = false;
        }
        private async void editAttendance_button_OnEditAttendance(object sender, EventArgs e)
        {
            string message = "";
            int count = 0;

            foreach (Attendance attendance in attendancesCollection_cv.SelectedItems.ToList())
            {
                var page = new EditAttendancePage(attendance);
                var waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
                page.Disappearing += (sender2, e2) =>
                {
                    waitHandle.Set();
                };
                await Navigation.PushAsync(page);
                await Task.Run(() => waitHandle.WaitOne());
                if (page.IsEditComlited)
                {
                    message += attendance.ToString() + "\n";
                    var isRemovedCv = attendancesCollection_cv.SelectedItems.Remove(attendance);
                    count += 1;
                }
                if(!page.IsEditComlited)
                    Navigation.RemovePage(page);
            }
            if(count > 0)
                await DisplayAlert("Вы отметили посещаемость:", message, "Ок");

            await ReloadPage();
        }
        private void LoadUserStatus()
        {
            again:
            switch (status)
            {
                case "None":
                    bool _result;
                    MainPage.status = Site.GetStatus(out _result);
                    if (!_result)
                        throw new Exception("Сайт мертвый...");
                    goto again;
                case "teacher":
                    StatusColor = "#7e57c2";
                    StatusBackColor = "#9575cd";
                    User = new Teacher();
                    break;
                case "student":
                    StatusColor = "#00bfa5";
                    StatusBackColor = "#1de9b6";
                    User = new Student();
                    break;
                case "admin":
                    StatusColor = "#f4511e";
                    StatusBackColor = "#e64a19";
                    User = new Admin();
                    break;
            }
        }
        private void InitializePush(string external_id)
        {
            OneSignal.Default.SetExternalUserId(external_id);
            OneSignal.Default.Initialize("7a5269eb-0cb5-458e-a2ca-2faae45351bf");
            OneSignal.Default.NotificationWillShow += Default_NotificationWillShow;
        }
        private OneSignalSDK.Xamarin.Core.Notification Default_NotificationWillShow(OneSignalSDK.Xamarin.Core.Notification notification)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                ReloadPage(); // я не чешу как я до этого додумался...
            });
            return notification;
        }

        private async Task ReloadPage()
        {
            await Navigation.PushAsync(new MainPage());
            Navigation.RemovePage(this);
        }
    }
}