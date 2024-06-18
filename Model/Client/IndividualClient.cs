using Microsoft.IdentityModel.Tokens;

namespace s28201_Project.Model;

public class IndividualClient : Client
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Pesel {
        get => Pesel;
        set
        {
            if (Pesel.IsNullOrEmpty())
            {
                Pesel = value;
            }
        }
    }
    public bool IsDeleted { get; set; }
    
    public virtual List<Discount> Discounts { get; set; }
}