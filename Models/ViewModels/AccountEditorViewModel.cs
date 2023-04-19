using System.ComponentModel.DataAnnotations;

namespace TapInMotion.Models.ViewModels;

public class AccountEditorViewModel
{
    public AccountEditorViewModel() { }

    public AccountEditorViewModel(Student? student, Administrator? admin)
    {
        if (student != null)
        {
            System.Console.WriteLine("Student: " + student.Name);
            new AccountEditorViewModel(student);
            return;
        }
        else if (admin != null)
        {
            System.Console.WriteLine("Admin: " + admin.AdministratorID);
            new AccountEditorViewModel(admin);
            return;
        }
        else
        {
            new AccountEditorViewModel();
        }
    }

    public AccountEditorViewModel(Student student)
    {
        UserName = student.User?.UserName ?? "";
        StudentID = student.StudentID;
        Name = student.Name;
        AccountType = AccountType.Student;
    }

    public AccountEditorViewModel(Administrator admin)
    {
        UserName = admin.User?.UserName ?? "";
        AdministratorID = admin.AdministratorID;
        Name = admin.Name;
        AccountType = AccountType.Administrator;
    }

    public AccountEditorViewModel(AppUser user)
    {
        UserName = user.UserName!;
        StudentID = user.Student?.StudentID;
        Student = user.Student;
        AdministratorID = user.Administrator?.AdministratorID;
        Administrator = user.Administrator;
        SchoolID =
            user.Student != null
                ? user.Student.SchoolID
                : (user.Administrator != null ? user.Administrator.SchoolID : null);
        Name = user.Student?.Name ?? user.Administrator?.Name;
    }

    public string UserName { get; set; } = "";
    public string? Name { get; set; }

    public AccountType AccountType { get; set; }
    public int? SchoolID { get; set; }
    public int? StudentID { get; set; }
    public virtual Student? Student { get; set; }
    public int? AdministratorID { get; set; }
    public virtual Administrator? Administrator { get; set; }

    public string? ReturnUrl { get; set; } = "/";
}
