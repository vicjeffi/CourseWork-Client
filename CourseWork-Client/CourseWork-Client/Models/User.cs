namespace ConsoleAppForStudentsApp.Models
{
    public interface IUser
    {
        string id { get; set; }
        string firstname { get; set; }
        string lastname { get; set; }
        string fathername { get; set; }
        string LoadMyData(out bool result);
    }
}
