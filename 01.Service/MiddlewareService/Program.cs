using MiddlewareService.Application.Services;
using MiddlewareService.Domain.Interfaces;
using MiddlewareService.Infrastructure.HttpClients;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddHttpClient<IAuthenticationService, AuthenticationService>();
builder.Services.AddHttpClient<ISearchService, SearchService>();
builder.Services.AddHttpClient<IUnnaxService, UnnaxService>();

builder.Services.AddScoped<ApiService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
