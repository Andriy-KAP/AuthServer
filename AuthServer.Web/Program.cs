using AuthServer.Core.Core;
using AuthServer.Core.Services;
using AuthServer.Domain.Model;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

var builder = WebApplication.CreateBuilder(args);

//Register services
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IEmailService, EmailService>();

//Add database context
builder.Services.AddDbContext<AuthServerContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Authentication
builder.Services.AddAuthorization();
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(
        options=>
        {
            options.User.RequireUniqueEmail = true;
        }
    ).AddEntityFrameworkStores<AuthServerContext>()
    .AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromSeconds(30);

    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDebied";
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();