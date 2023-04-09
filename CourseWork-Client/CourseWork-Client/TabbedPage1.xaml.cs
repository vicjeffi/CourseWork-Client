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

namespace ConsoleAppForStudentsApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedPage1 : TabbedPage
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
        public IUser User { get; private set; }
        public string StatusColor { get; private set; }
        public string StatusBackColor { get; private set; }
        private List<Group> Groups = new List<Group>();
        public TabbedPage1()
        {
            if (Site == null)
                Site = new Website();

            again:
            switch (status)
            { case "None":
                    bool _result;
                    TabbedPage1.status = Site.GetStatus(out _result);
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
                    break; }

            bool result = false;
            User.LoadMyData(out result);
            if (!result)
                throw new Exception("Сайт мертвый...");

            InitializeComponent();

            //toolBar_status.Text = status;

            BindingContext = this;
            switch (status)
            {
                case "teacher":
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

                    mainCollection_cv.SelectionChanged += mainCollection_cv_OnSelection;
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
                            else throw new Exception("Group");
                        }
                    }
                    groupPicker.ItemsSource = Groups;
                    disciplinePicker.ItemsSource = t.Disciplines;
                    statusContent.Children.Add(groupPicker);
                    statusContent.Children.Add(disciplinePicker);
                    break;
                case "student":
                    addAttendance_button.IsVisible = false;
                    clearSelection_button.IsVisible = false;
                    profile_status_label.Text = "Вы Ученик";
                    mainCollection_cv.IsVisible = false;
                    break;
                case "admin":
                    profile_status_label.Text = "Вы Адиминиcтрация";
                    mainCollection_cv.IsVisible = false;
                    break;
            }
        }
        private void disciplinePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            picker.Title = "Выбранная группа - " + (picker.SelectedItem as Discipline).ToString();
            SelectedDiscipline = (picker.SelectedItem as Discipline);

            if (mainCollection_cv.SelectedItems.Count >= 1 && SelectedDiscipline != null)
                addAttendance_button.IsEnabled = true;
        }
        private void groupPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            picker.Title = "Выбранная группа - " + (picker.SelectedItem as Group).ToString();
            SelectedGroup = (picker.SelectedItem as Group);

            if (mainCollection_cv != null)
            {
                mainCollection_cv.ItemsSource = null;
                mainCollection_cv.ItemsSource = SelectedGroup.Student;
            }
        }
        private void mainCollection_cv_OnSelection(object sender, EventArgs e)
        {
            if(mainCollection_cv.SelectedItems.Count >= 1 && SelectedDiscipline != null)
                addAttendance_button.IsEnabled = true;
            else
            {
                addAttendance_button.IsEnabled = false;
            }
        }

        private async void toolbar_OnLogoutButtonClicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Подтверждение", "Вы точно хотите выйти из профиля?", "Да", "Нет");
            if (answer)
            {
                await Xamarin.Essentials.SecureStorage.SetAsync("logined", "false");
                await Navigation.PushAsync(new MainPage());
                Navigation.RemovePage(this);
            }
        }
        private void clearSelection_button_OnClearSelection(object sender, EventArgs e)
        {
            mainCollection_cv.SelectedItems.Clear();
            addAttendance_button.IsEnabled = false;
        }
        private async void addAttendance_button_OnAddAttendance(object sender, EventArgs e)
        {
            string students = "";
            foreach (Student s in mainCollection_cv.SelectedItems)
                students += s.ToString() + "\n";
            bool answer = await DisplayAlert("Вы точно хотите отметить отсутствие этих учеников?", students + "\n" + "По дисциплине: " + SelectedDiscipline.ToString(), "Да", "Нет");
            if(answer)
            {
                foreach(Student student in mainCollection_cv.SelectedItems)
                {
                    bool result;
                    var message = Site.PostAttendance(out result, student.id, SelectedDiscipline.id, DateTime.Now);
                    if (!result)
                        await DisplayAlert("Ошибка!", message, "Ок");
                }
            }
            mainCollection_cv.SelectedItems.Clear();
            addAttendance_button.IsEnabled = false;
        }
    }
}