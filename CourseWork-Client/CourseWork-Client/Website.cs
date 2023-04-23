using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Text;
using ConsoleAppForStudentsApp.Models;

namespace ConsoleAppForStudentsApp
{
    public class Website
    {
        public static HttpClient client;
        private static Website _basic;
        public Website()
        {
            if (client == null)
            {
                client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
            }
        }
        public static Website Basic
        {
            get
            {
                if (_basic == null)
                {
                    _basic = new Website();
                    return _basic;
                }
                return _basic;
            }
        }
        private static string _url = @"https://vicjeffi.pythonanywhere.com/";
        public static string Url { get { return _url; } }

        private static string _noHttps_url = @"vicjeffi.pythonanywhere.com";
        public static string NoHttps_Url { get { return _noHttps_url; } }
        public string GetStatus(out bool result)
        {
            string message = string.Empty;

            var task1 = client.GetAsync(Url);
            task1.Wait();
            var http_result = task1.Result;

            var task2 = http_result.Content.ReadAsStringAsync();
            task2.Wait();

            var json = JObject.Parse(task2.Result);

            message = json.GetValue("status").ToString();

            if (http_result.StatusCode == HttpStatusCode.OK)
                result = true;
            else
                result = false;

            return message;
        }

        public string GetId(out bool result)
        {
            string message = string.Empty;

            var task1 = client.GetAsync(Url);
            task1.Wait();
            var http_result = task1.Result;

            var task2 = http_result.Content.ReadAsStringAsync();
            task2.Wait();

            var json = JObject.Parse(task2.Result);

            message = json.GetValue("id").ToString();

            if (http_result.StatusCode == HttpStatusCode.Created)
                result = true;
            else
                result = false;

            return message;
        }
        public string TryLogin(string username, string password, out bool result)
        {
            string message = string.Empty;

            var task1 = client.GetAsync(Url + $"login?username={username}&password={password}");
            task1.Wait();
            var http_result = task1.Result;

            var task2 = http_result.Content.ReadAsStringAsync();
            task2.Wait();

            var json = JObject.Parse(task2.Result);

            message = json.GetValue("message").ToString();

            if (http_result.StatusCode == HttpStatusCode.Created)
                result = true;
            else
                result = false;

            return message;
        }

        public string GetGroupInfo(string index, out bool result)
        {
            string message = string.Empty;

            var task1 = client.GetAsync(Url + $"api/get-group?group_index=" + index);
            task1.Wait();
            var http_result = task1.Result;

            var task2 = http_result.Content.ReadAsStringAsync();
            task2.Wait();

            message += task2.Result;

            if (http_result.StatusCode == HttpStatusCode.Created)
                result = true;
            else
                result = false;

            return message;
        }

        public long ServerStatus(out bool result)
        {
            result = false;
            var ping = new System.Net.NetworkInformation.Ping();

            var _result = ping.Send(NoHttps_Url);

            if (_result.Status == System.Net.NetworkInformation.IPStatus.Success)
            {
                result = true;
                return _result.RoundtripTime;
            }
            return 0;
        }
        public string PostAttendance(out bool result, string student_id, string discipline, DateTime time)
        {
            string _time = time.ToString("dd'-'MM'-'yyyy'-'HH'-'mm'-'ss");

            string message = string.Empty;

            var task1 = client.GetAsync(Url + $"add-attendance?student-id={student_id}&discipline-id={discipline}&time={_time}");
            task1.Wait();
            var http_result = task1.Result;

            var task2 = http_result.Content.ReadAsStringAsync();
            task2.Wait();

            message += task2.Result;

            if (http_result.StatusCode == HttpStatusCode.Created)
                result = true;
            else
                result = false;

            return message;
        }
        public string EditAttendance(out bool result, string attendance_id, string reason)
        {
            var task1 = client.GetAsync(Url + $"/upload/attendance-reason?attendance-id={attendance_id}&reason={reason}");
            task1.Wait();
            var http_result = task1.Result;

            var task2 = http_result.Content.ReadAsStringAsync();
            task2.Wait();

            string message = task2.Result;

            if (http_result.StatusCode == HttpStatusCode.Created)
                result = true;
            else
                result = false;

            return message;
        }
    }
}
