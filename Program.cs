using Microsoft.EntityFrameworkCore;
using s28201_Project.Context;
using s28201_Project.Service;
using s28201_Project.Service.Installment;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddXmlSerializerFormatters();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApiContext>();
builder.Services.AddScoped<IndividualClientService>();
builder.Services.AddScoped<CompanyClientService>();
builder.Services.AddScoped<CompanyContractService>();
builder.Services.AddScoped<LicenseService>();
builder.Services.AddScoped<CompanyInstallmentService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
