namespace ProjektKunde.Models;

public class Customer
{
    public  int Id { get; set; }
    public  string Company { get; set; }
    public required string Adress { get; set; }
    
    public virtual ICollection<Project> Projects { get; set; }
    
    public string FullCompany => $"{Company} {Adress}";
}