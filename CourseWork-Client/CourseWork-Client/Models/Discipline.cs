namespace ConsoleAppForStudentsApp.Models
{
    public class Discipline
    {
        public string id { get; set; }
        public string name { get; set; }
        public override string ToString()
        {
            return name;
        }
    }
}
