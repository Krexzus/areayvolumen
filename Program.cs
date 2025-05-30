using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { 
        Title = "Calculadora de Área y Volumen API", 
        Version = "v1",
        Description = "API para calcular áreas y volúmenes de diferentes figuras geométricas"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Configurar la URL
var urls = new[] { "http://localhost:8080" };
app.Urls.Clear();
app.Urls.Add(urls[0]);

Console.WriteLine($"La aplicación está corriendo en: {urls[0]}/swagger");
app.Run(); 