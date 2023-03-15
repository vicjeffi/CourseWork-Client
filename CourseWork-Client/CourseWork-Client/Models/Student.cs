using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ConsoleAppForStudentsApp.Models;

namespace ConsoleAppForStudentsApp.Models
{
    public class Student : IPeople
    {
        /* Добавить свойсва! 
         * для этого:
         * 
         * 1) Переименовать во всем файле поля. Пример: _name в Name
         * 2) Добавить свойства копированием обьявления того же поля
         * 3) переименовать после добавления свойства поле в изначальное название
         * 
         * примечания:
         * set сделать приватным!
         * добавить интерфейс IStudent со всеми свойствами оценок и методами их получения / управления
         * в интерфейсе так же обозначить private set на свойствах!
         * 
         * добавить галочку на том что сделал - ✓
         */

        private String _name;

        private String _fatherName;

        private String _surname;

        private DateTime _birthday;

        private String _address;

        private String _phoneNumber;

        private static int _courseWorkCount = 5;

        private static int _creditCount = 5;

        private static int _examCount = 5;

        private int[] _courseWorkMarks = new int[_courseWorkCount];

        private int[] _creditMarks = new int[_creditCount];

        private int[] _examMarks = new int[_examCount];

        public Student(String name, String fatherName, String surname,
                        DateTime birthday, String address, String phoneNumber)
        {
            setName(name);
            setFatherName(fatherName);
            setSurname(surname);
            setBirthday(birthday);
            setAddress(address);
            setPhoneNumber(phoneNumber);

            fillDefaultMarkValues();
        }

        public Student() : this("Unknown", "Unknown", "Unknown", new DateTime(1900, 1, 1), "", "") { }

        public Student(String name, String fatherName, String surname) :
            this(name, fatherName, surname, new DateTime(1900, 1, 1), "", "")
        { }

        public void setName(String name)
        {
            this._name = name;
        }

        public void setFatherName(String fatherName)
        {
            this._fatherName = fatherName;
        }

        public void setSurname(String surname)
        {
            this._surname = surname;
        }

        public void setBirthday(DateTime birthday)
        {
            this._birthday = birthday;
        }

        public void setAddress(String address)
        {
            this._address = address;
        }

        public void setPhoneNumber(String phoneNumber)
        {
            this._phoneNumber = phoneNumber;
        }

        public void setCourseWorkMark(int mark, int index)
        {
            if ((index > _courseWorkCount - 1) || (index < 0))
            {
                return;//an exception should be here
            }

            _courseWorkMarks[index] = mark;
        }

        public void setCreditkMark(int mark, int index)
        {
            if ((index > _creditCount - 1) || (index < 0))
            {
                return;//an exception should be here
            }

            _creditMarks[index] = mark;
        }

        public void setExamMark(int mark, int index)
        {
            if ((index > _examCount - 1) || (index < 0))
            {
                return;//an exception should be here
            }

            _examMarks[index] = mark;
        }

        public String getName()
        {
            return _name;
        }

        public String getFatherName()
        {
            return _fatherName;
        }

        public String getSurname()
        {
            return _surname;
        }

        public DateTime getBirthday()
        {
            return _birthday;
        }

        public String getAddress()
        {
            return _address;
        }

        public int getCourseWorkMark(int index)
        {
            if ((index > _courseWorkCount - 1) || (index < 0))
            {
                return 0;//an exception should be here
            }

            return _courseWorkMarks[index];
        }

        public int getCreditMark(int index)
        {
            if ((index > _examCount - 1) || (index < 0))
            {
                return 0;//an exception should be here
            }

            return _examMarks[index];
        }

        public int getExamMark(int index)
        {
            if ((index > _examCount - 1) || (index < 0))
            {
                return 0;//an exception should be here
            }

            return _examMarks[index];
        }

        public void showStudentInfo()
        {
            Console.WriteLine("Name: " + _name);
            Console.WriteLine("Father name: " + _fatherName);
            Console.WriteLine("Surname: " + _surname);
            Console.WriteLine("Birthday: " + _birthday.ToString());
            Console.WriteLine("Address: " + _address);
            Console.WriteLine("Phone number: " + _phoneNumber);

            showMarks();
        }

        public int calculateAvarageMark()
        {
            int summMark = 0;
            int divider = 0;

            calculateCreditMarkSumm(ref summMark, ref divider);
            calculateCourseWorkMarkSumm(ref summMark, ref divider);
            calculateExamMarkSumm(ref summMark, ref divider);

            if (divider == 0)
                return 0;
            else
                return summMark / divider;
        }

        public void fillCreditMarkArrayWithMark(int mark)
        {
            for (int i = 0; i < _creditMarks.Length; i++)
            {
                setCreditkMark(mark, i);
            }
        }

        public void fillCourseWorkMarkArrayWithMark(int mark)
        {
            for (int i = 0; i < _courseWorkMarks.Length; i++)
            {
                setCourseWorkMark(mark, i);
            }
        }

        public void fillExamMarkArrayWithMark(int mark)
        {
            for (int i = 0; i < _examMarks.Length; i++)
            {
                setExamMark(mark, i);
            }
        }

        private void calculateCreditMarkSumm(ref int summMark, ref int divider)
        {
            divider++;

            for (int i = 0; i < _creditMarks.Length; i++)
            {
                summMark += _creditMarks[i];
            }

        }

        private void calculateCourseWorkMarkSumm(ref int summMark, ref int divider)
        {
            divider++;

            for (int i = 0; i < _courseWorkMarks.Length; i++)
            {
                summMark += _courseWorkMarks[i];
            }
        }

        private void calculateExamMarkSumm(ref int summMark, ref int divider)
        {
            divider++;

            for (int i = 0; i < _examMarks.Length; i++)
            {
                summMark += _examMarks[i];
            }
        }

        private void showMarks()
        {
            showCourseWorkMarks();
            showCreditkMarks();
            showExamMarks();
        }

        private void showCourseWorkMarks()
        {
            Console.Write("Couse work marks: ");

            for (int i = 0; i < _courseWorkMarks.Length; i++)
            {
                Console.Write("{0, 4}", _courseWorkMarks[i]);
            }

            Console.WriteLine();
        }

        private void showCreditkMarks()
        {
            Console.Write("Credit marks: ");

            for (int i = 0; i < _creditMarks.Length; i++)
            {
                Console.Write("{0, 4}", _creditMarks[i]);
            }

            Console.WriteLine();

        }

        private void showExamMarks()
        {
            Console.Write("Exam marks: ");

            for (int i = 0; i < _examMarks.Length; i++)
            {
                Console.Write("{0, 4}", _examMarks[i]);
            }

            Console.WriteLine();

        }

        private void fillDefaultMarkValues()
        {

            fillArrayWithZero(_courseWorkMarks);
            fillArrayWithZero(_creditMarks);
            fillArrayWithZero(_examMarks);
        }

        private void fillArrayWithZero(int[] markArray)
        {

            for (int i = 0; i < markArray.Length; i++)
                markArray[i] = 0;
        }
    }
}
