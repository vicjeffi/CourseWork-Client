using CourseWork_Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ConsoleAppForStudentsApp.Models
{
    public class Group
    {
        public string id { get; set; }
        public string group { get; set; }
        public string course { get; set; }
        public string number { get; set; }
        public string Name { get {return group + "-" + course + number; } }
        private List<Student> _users = new List<Student>();
        public List<Student> Student { get { return _users; } set { _users = value; } }
        public string LoadStudents(out bool result)
        {
            result = false;

            string message = string.Empty;

            var task1 = Website.client.GetAsync(Website.Url + "/api/get-students-by-group?group-id=" + id);
            task1.Wait();
            var http_result = task1.Result;

            var task2 = http_result.Content.ReadAsStringAsync();
            task2.Wait();

            if (http_result.StatusCode == HttpStatusCode.OK)
                result = true;
            else
                result = false;

            Group group = JsonConvert.DeserializeObject<Group>(task2.Result);

            this.Student = group.Student;

            return message;
        }

        public override string ToString()
        {
            return group + "-" + course + number;
        }
    }
}
