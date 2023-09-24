using AuthServer.Core.Core;
using AuthServer.Core.Services;
using AuthServer.Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddDbContext<AuthServerContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(
        options=>
        {
            options.SignIn.RequireConfirmedAccount = false;
        }
    ).AddEntityFrameworkStores<AuthServerContext>()
    .AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly= true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

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