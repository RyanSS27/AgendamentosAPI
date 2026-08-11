using AgendamentosAPI.Adapters.Infrastructure;
using AgendamentosAPI.Adapters.Infrastructure.Repositories;
using AgendamentosAPI.Adapters.Infrastructure.Repositories.Ports;
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
builder.Services.AddControllers();
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
