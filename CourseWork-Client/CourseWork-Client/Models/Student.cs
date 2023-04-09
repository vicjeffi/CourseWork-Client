using CourseWork_Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ConsoleAppForStudentsApp.Models
{
    public class Student: IUser
    {
        public string status { get; set; }
        public string id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string fathername { get; set; }
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

            Student student = JsonConvert.DeserializeObject<Student>(task2.Result);

            this.status = student.status;
            this.id = student.id;
            this.firstname = student.firstname;
            this.lastname = student.lastname;
            this.fathername = student.fathername;

            return message;
        }
        public override string ToString()
        {
            return lastname + " " + firstname + " " + fathername;
        }
    }
}
