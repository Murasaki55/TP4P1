
using Microsoft.EntityFrameworkCore;
using TP4P1.Models.DataManager;
using TP4P1.Models.EntityFramework;
using TP4P1.Models.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FilmRatingsDBContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("FilmRatingsDBContext")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IDataRepository<Utilisateur>, UtilisateurManager>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
