namespace s28201_Project.Model;

public abstract class Client
{
    public long ClientId { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string PhoneNum { get; set; }
}