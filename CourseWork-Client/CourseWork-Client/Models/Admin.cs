﻿using Newtonsoft.Json;
using System.Net;

namespace ConsoleAppForStudentsApp.Models
{
    public class Admin : IUser
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

            Admin student = JsonConvert.DeserializeObject<Admin>(task2.Result);

            status = student.status;
            id = student.id;
            firstname = student.firstname;
            lastname = student.lastname;
            fathername = student.fathername;

            return message;
        }
    }
}
