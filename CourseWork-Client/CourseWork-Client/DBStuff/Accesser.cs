using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppForStudentsApp
{
    internal class Accesser
    {
        //private string _username;
        //private string _password;
        private Accesser(string username, string password)
        {

        }

        public bool GetAccess()
        {
            return true;
        }
    }
}
