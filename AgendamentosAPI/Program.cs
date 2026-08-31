using AgendamentosAPI.Adapters.Infrastructure;
using AgendamentosAPI.Adapters.Infrastructure.ExternalServices;
using AgendamentosAPI.Adapters.Infrastructure.Repositories;
using AgendamentosAPI.Domain.Ports;
using AgendamentosAPI.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();

// "options" passa a string de conexão, senha e tudo mais que eu definir no program 
// que essa conexão deva ter
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    /*
        devo informar:
            - o banco que estou usando passando um .Use[Nome do banco ou depência que
            adiciona ele]
            - acessar o doc de configurações e inserir o caminho até a string de conexão
    */
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IServiceProviderRepository, ServiceProviderRepository>();
builder.Services.AddScoped<IServiceProviderService, ServiceProviderService>();
// Registra o Provider de Token e o Adapter do Calendar
builder.Services.AddScoped<IGoogleTokenProvider, GoogleServiceAccountTokenProvider>();
builder.Services.AddScoped<ICalendarIntegrationPort, GoogleCalendarAdapter>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Permite receber string via controller para Enums (não ficar recebendo só numeros)
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseExceptionHandler();
app.MapControllers();


app.Run();
