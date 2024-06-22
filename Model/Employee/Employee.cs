using System.ComponentModel.DataAnnotations.Schema;

namespace s28201_Project.Model.Employee;

public class Employee
{
    public long EmployeeId { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
}