using Microsoft.AspNetCore.Identity;

namespace TapInMotion.Models
{
    public enum AccountType
    {
        Student,
        Administrator,
        TapAdmin
    }

    public class AppUser : IdentityUser<Guid>
    {
        public AccountType AccountType { get; set; }

        // public int? StudentID { get; set; }
        public virtual Student? Student { get; set; }

        // public int? AdministratorID { get; set; }
        public virtual Administrator? Administrator { get; set; }

        override public string ToString()
        {
            return Student != null ? Student.ToString() : "AppUser";
        }
    }
}
