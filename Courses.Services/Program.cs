using Courses.Services.Controllers;
using Courses.Services.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var server = builder.Configuration["DbServer"] ?? "mssql-service";
var user = builder.Configuration["DbUser"] ?? "SA";
var password = builder.Configuration["Password"] ?? "2Secure*Password2";
var database = builder.Configuration["Database"] ?? "SchoolMultiContDb";


// Add Db context as a service to our application
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer($"Server={server};Initial Catalog={database};User ID={user};Password={password}"));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapCourseEndpoints();

app.Run();
