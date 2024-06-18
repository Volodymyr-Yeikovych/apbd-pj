using Microsoft.IdentityModel.Tokens;

namespace s28201_Project.Model;

public class CompanyClient : Client
{
    public string Name { get; set; }
    public string KrsNum
    {
        get => KrsNum;
        set
        {
            if (KrsNum.IsNullOrEmpty())
            {
                KrsNum = value;
            }
        }
    }
    
    public virtual List<Discount> Discounts { get; set; }
}