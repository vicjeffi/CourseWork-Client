using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppForStudentsApp.Models
{
    public interface IPeople
    {
        
        void setName(String name);

        void setFatherName(String fatherName);

        void setSurname(String surname);

        void setBirthday(DateTime birthday);

        void setAddress(String address);

        void setPhoneNumber(String phoneNumber);
    }
}
