using System.ComponentModel.DataAnnotations;

namespace TapInMotion.Models;

public class Administrator
{
    public int AdministratorID { get; set; }
    public int SchoolID { get; set; }
    public virtual School? School { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public Guid? UserID {get;set;}
    public virtual AppUser? User {get;set;}

    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; } = DateTime.Now;
}
