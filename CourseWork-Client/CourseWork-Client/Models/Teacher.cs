using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace ConsoleAppForStudentsApp.Models
{
    public class Teacher : IUser
    {
        public string status { get; set; }
        public string id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string fathername { get; set; }
        public List<string> groups { get; set; }
        public List<Discipline> Disciplines { get; set; }
        public string LoadMyData(out bool result)
        {
            result = false;

            string message = string.Empty;

            var task1 = Website.client.GetAsync(Website.Url + "/api/my-data");
            task1.Wait();
            var http_result = task1.Result;

            var task2 = http_result.Content.ReadAsStringAsync();
            task2.Wait();

            if (http_result.StatusCode == HttpStatusCode.OK)
                result = true;
            else
                result = false;

            Teacher teacher = JsonConvert.DeserializeObject<Teacher>(task2.Result);

            status = teacher.status;
            id = teacher.id;
            firstname = teacher.firstname;
            lastname = teacher.lastname;
            fathername = teacher.fathername;
            groups = teacher.groups;

            return message;
        }
        public string LoadDisciplines(out bool result)
        {
            result = false;

            string message = string.Empty;

            var task1 = Website.client.GetAsync(Website.Url + "/api/get-disciplines");
            task1.Wait();
            var http_result = task1.Result;

            var task2 = http_result.Content.ReadAsStringAsync();
            task2.Wait();

            if (http_result.StatusCode == HttpStatusCode.OK)
                result = true;
            else
                result = false;

            Teacher teacher = JsonConvert.DeserializeObject<Teacher>(task2.Result);

            Disciplines = teacher.Disciplines;

            return message;
        }
    }
}
