using Microsoft.IdentityModel.Tokens;

namespace s28201_Project.Dto;

public class IndividualClientDto : ClientDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Pesel { get; init; }
}