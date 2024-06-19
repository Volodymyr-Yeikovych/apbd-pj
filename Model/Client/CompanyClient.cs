using Microsoft.IdentityModel.Tokens;
using s28201_Project.Dto;

namespace s28201_Project.Model;

public class CompanyClient : Client
{
    public string Name { get; set; }
    public string KrsNum { get; init; }
    
    public virtual List<Discount> Discounts { get; set; }
    public virtual List<CompanyContract> Contracts { get; set; }
    
    public CompanyClient()
    {
    }

    public CompanyClient(CompanyClientDto dto)
    {
        Address = dto.Address;
        Email = dto.Email;
        PhoneNum = dto.PhoneNum;
        Name = dto.Name;
        KrsNum = dto.KrsNum;
    }

    public void Set(CompanyClientDto dto)
    {
        Address = dto.Address;
        Email = dto.Email;
        PhoneNum = dto.PhoneNum;
        Name = dto.Name;
    }
}