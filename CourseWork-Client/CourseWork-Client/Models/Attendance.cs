namespace ConsoleAppForStudentsApp.Models
{
    public class Attendance
    {
        public string id { get; set; }
        public string student_id { get; set; }
        public Discipline Discipline { get; set; }
        public string time { get; set; }
        public string reason { get; set; }
        public bool @checked { get; set; }

        public override string ToString()
        {
            return Discipline.name + " за " + time;
        }
    }
}
