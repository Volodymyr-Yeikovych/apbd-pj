using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using s28201_Project.Context;
using s28201_Project.Model.Employee;

namespace s28201_Project.Middlewares;

public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public BasicAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
            return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));

        try
        {
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            if (authHeader.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase))
            {
                var credentials = Encoding.UTF8
                    .GetString(Convert
                        .FromBase64String(authHeader.Parameter))
                    .Split(':');
                var username = credentials[0];
                var password = credentials[1];
                
                if (IsAuthenticatedAndAuthorized(username, password, Context, out Role role))
                {
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, username),
                        new Claim(ClaimTypes.Role, role.ToIndexString())
                    };
                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);

                    return Task.FromResult(AuthenticateResult.Success(ticket));
                }

                return Task.FromResult(AuthenticateResult.Fail("Invalid Credentials"));
            }

            return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Scheme"));
        }
        catch
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
        }
    }

    private bool IsAuthenticatedAndAuthorized(string username, string password, HttpContext context, out Role role)
    {
        using var scope = context.RequestServices.CreateScope();

        var apiContext = scope.ServiceProvider.GetRequiredService<ApiContext>();

        var emp = apiContext.Employees
            .FirstOrDefault(e => e.Login == username && e.Password == password);

        if (emp == null)
        {
            role = Role.NoRole;
            return false;
        }

        role = emp.Role;
        return true;
    }
}