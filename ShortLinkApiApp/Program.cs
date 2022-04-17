using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ShortLinksApiApp.Data.Context;
using ShortLinksApiApp.Data.Repositories;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ShortLinkAPI",
        Description = "An ASP.NET Core Web API for creating short link",
        Contact = new OpenApiContact
        {
            Name = "My resume",
            Url = new Uri("https://drive.google.com/file/d/1giw0hQcaDzv3XR3v31qwiNXUbdc7sgPM/view?usp=sharing")
        },
        License = new OpenApiLicense
        {
            Name = "Telegram",
            Url = new Uri("https://t.me/pitkoro")
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
});
builder.Services.AddTransient<IShortLinkRepository, ShortLinkRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
