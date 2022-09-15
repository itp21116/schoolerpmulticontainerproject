using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolERP.MVC.UI;
using SchoolERP.MVC.UI.Data;
using SchoolERP.MVC.UI.Models;

var builder = WebApplication.CreateBuilder(args);
var startup = new Startup(builder.Configuration);

var server = builder.Configuration["DbServer"] ?? "mssql-service";
var user = builder.Configuration["DbUser"] ?? "SA"; // Warning do not use the SA account
var password = builder.Configuration["Password"] ?? "2Secure*Password2";
var database = builder.Configuration["Database"] ?? "SchoolMultiContDb";

// Add Db context as a service to our application
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer($"Server={server};Initial Catalog={database};User ID={user};Password={password}"));


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
     .AddDefaultTokenProviders()
    .AddDefaultUI();
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
    //db.Database.Migrate();
}

startup.ConfigureServices(builder.Services, builder.Services.BuildServiceProvider());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    }
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
