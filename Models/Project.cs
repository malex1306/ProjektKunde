namespace ProjektKunde.Models;

public class Project
{
    public  int Id { get; set; }
    public  string title { get; set; }
    public  string description { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
}