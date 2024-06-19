using Microsoft.EntityFrameworkCore;
using s28201_Project.Context;
using s28201_Project.Model;

namespace s28201_Project.Service;

public class LicenseService(ApiContext context)
{
    public async Task<SoftwareLicense?> GetLicenseByIdAsync(long id)
    {
        return await context.SoftwareLicenses.FirstOrDefaultAsync(c => c.SoftwareLicenseId == id);
    }
}