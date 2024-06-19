using Microsoft.IdentityModel.Tokens;
using s28201_Project.Dto;

namespace s28201_Project.Model;

public class IndividualClient : Client
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Pesel { get; init; }

    public bool IsDeleted { get; set; }
    
    public virtual List<Discount> Discounts { get; set; }
    public virtual List<IndividualContract> Contracts { get; set; }

    public IndividualClient()
    {
    }

    public IndividualClient(IndividualClientDto dto)
    {
        Address = dto.Address;
        Email = dto.Email;
        PhoneNum = dto.PhoneNum;
        FirstName = dto.FirstName;
        LastName = dto.LastName;
        Pesel = dto.Pesel;
    }

    public void Set(IndividualClientDto dto)
    {
        Address = dto.Address;
        Email = dto.Email;
        PhoneNum = dto.PhoneNum;
        FirstName = dto.FirstName;
        LastName = dto.LastName;
    }
}