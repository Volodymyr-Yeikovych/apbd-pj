using Microsoft.AspNetCore.Authentication;
using s28201_Project.Context;
using s28201_Project.Middlewares;
using s28201_Project.Service;
using s28201_Project.Service.Installment;
using s28201_Project.Service.Revenue;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddXmlSerializerFormatters();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<NbpCurrencyService>();
builder.Services.AddDbContext<ApiContext>();

builder.Services.AddScoped<ICurrencyService, NbpCurrencyService>();
builder.Services.AddScoped<IndividualClientService>();
builder.Services.AddScoped<CompanyClientService>();
builder.Services.AddScoped<IndividualContractService>();
builder.Services.AddScoped<CompanyContractService>();
builder.Services.AddScoped<LicenseService>();
builder.Services.AddScoped<IndividualInstallmentService>();
builder.Services.AddScoped<CompanyInstallmentService>();
builder.Services.AddScoped<RevenueService>();

builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("BasicAuthentication", null);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("0"));
    options.AddPolicy("UserOrAdmin", policy => policy.RequireAssertion(context =>
        context.User.IsInRole("0") || context.User.IsInRole("1")));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
