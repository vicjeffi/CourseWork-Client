using System.Text.RegularExpressions;
using System;
using System.Collections;

namespace ConsoleAppForStudentsApp.Models
{
    public class Group
    {
        private IPeople _curator;

        private Student[] _students;

        private int _studentCount;

        private String _groupName;

        private String _groupSpecialization;

        private int _courseNumber;

        public IPeople Curator {
            get
            {
                return _curator;
            }
            private set
            {
                _curator = value;
            }
        }
        public Student[] Students {
            get
            {
                return _students;
            }
            private set
            {
                _students = value;
            }
        }
        public int StudentCount {
            get
            {
                return _studentCount;
            }
            private set
            {
                _studentCount = value;
            }
        }
        public String GroupName
        {
            get
            {
                return _groupName;
            }
            private set
            {
                _groupName = value;
            }
        }
        public String GroupSpecialization
        {
            get
            {
                return _groupSpecialization;
            }
            private set
            {
                _groupSpecialization = value;
            }
        }
        public int CourseNumber
        {
            get
            {
                return _courseNumber;
            }
            private set
            {
                _courseNumber = value;
            }
        }

        public void addStudent(Student new_student)
        {
            Student[] prev_students = this.Students;
            Students = new Student[StudentCount + 1];

            if (prev_students != null)
            {
                for (int i = 0; i < prev_students.Length; i++)
                {
                    Students[i] = prev_students[i];
                }
            }

            Students[StudentCount] = new_student;

            StudentCount++;

            sortStudentsBySurname();
        }

        public Student getStudentByIndex(int index)
        {
            if (index < 0 || index > Students.Length - 1)
                return null;//must be exception;
            else
                return Students[index];
        }

        public void deleteStudentByIndex(int index)
        {
            if (index < 0 || index > Students.Length - 1)
                return;//must be exception;

            Student[] prev_students = this.Students;
            Students = new Student[StudentCount - 1];
            int shift = 0;

            for (int i = 0; i < prev_students.Length; i++)
            {
                if (i == index)
                {
                    shift++;
                    continue;
                }

                Students[i - shift] = prev_students[i];
            }

            StudentCount--;

            sortStudentsBySurname();
        }

        public void setStudentCount(int studentCount)
        {
            this.StudentCount = studentCount;
        }

        public int getStudnetCount()
        {
            return StudentCount;
        }

        public void setGroupName(String groupName)
        {
            this.GroupName = groupName;
        }

        public String getGroupName()
        {
            return GroupName;
        }

        public void setGroupSpecialization(String groupSpecialization)
        {
            this.GroupSpecialization = groupSpecialization;
        }

        public String getGroupSpecialization()
        {
            return GroupSpecialization;
        }

        public void setCourseNumber(int courseNumber)
        {
            this.CourseNumber = courseNumber;
        }

        public int getCourseNumber()
        {
            return CourseNumber;
        }

        public void showGroup()
        {
            Console.WriteLine("Group name: " + GroupName);
            Console.WriteLine("Group spesialization: " + GroupSpecialization);
            Console.WriteLine("Course number: " + CourseNumber.ToString());
            Console.WriteLine("Student count: " + StudentCount.ToString());
            Console.WriteLine("----------------------------------------");

            for (int i = 0; i < Students.Length; i++)
            {
                Console.WriteLine((i + 1).ToString() + ". " + Students[i].getSurname() + " " + Students[i].getName() +
                                    " " + Students[i].getFatherName() + ", " + Students[i].calculateAvarageMark().ToString());
            }

        }

        public Group()
        {
            fillGroupAttributesWithDefaultValues();
            addStudentAutomaticaly(10);
        }

        public Group(int studentCount)
        {
            fillGroupAttributesWithDefaultValues();
            addStudentAutomaticaly(studentCount);
        }

        public Group(Student[] students)
        {
            fillGroupAttributesWithDefaultValues();

            foreach (var item in students)
            {
                addStudent(item);
            }
        }

        public Group(String groupName, String groupSpecialization, int courseNumber)
        {
            setGroupName(groupName);
            setGroupSpecialization(groupSpecialization);
            setCourseNumber(courseNumber);
        }

        public void deleteStudentWithLowestMark()
        {
            if (StudentCount == 0)
            {
                return;
            }

            int index = 0;
            int lowestMark = 1000;

            for (int i = 0; i < Students.Length; i++)
            {
                if (Students[i].calculateAvarageMark() < lowestMark)
                {
                    lowestMark = Students[i].calculateAvarageMark();
                    index = i;
                }
            }

            deleteStudentByIndex(index);
        }

        public void setStudentData(int studentIndex, String name, String fatherName, String surname)
        {
            Student currentStudent = getStudentByIndex(studentIndex);
            currentStudent.setName(name);
            currentStudent.setFatherName(fatherName);
            currentStudent.setSurname(surname);

            sortStudentsBySurname();
        }

        public void moveStudentToAnotherGroup(int studentIndex, Group newGroup)
        {
            newGroup.addStudent(getStudentByIndex(studentIndex));
            deleteStudentByIndex(studentIndex);
        }

        private void fillGroupAttributesWithDefaultValues()
        {
            setGroupName("Unknown");
            setGroupSpecialization("Unknown");
            setCourseNumber(0);
        }

        private void addStudentAutomaticaly(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Student newStudent = new Student("Unknown" + i.ToString(), "Unknown" + i.ToString(), "Unknown" + i.ToString());
                newStudent.fillCourseWorkMarkArrayWithMark(i + 1);
                newStudent.fillCreditMarkArrayWithMark(i + 1);
                newStudent.fillExamMarkArrayWithMark(i + 1);
                addStudent(newStudent);
            }
        }

        private void sortStudentsBySurname()
        {
            IComparer comparer = new SurnameCompare();
            Array.Sort(Students, comparer);
        }
    }
    public class SurnameCompare : IComparer
    {
        public int Compare(object a, object b)
        {
            Student studentA = (Student)a;
            Student studentB = (Student)b;
            return studentA.getSurname().CompareTo(studentB.getSurname());
        }
    }
}