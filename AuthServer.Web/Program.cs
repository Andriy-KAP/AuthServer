using AuthServer.Core.Core;
using AuthServer.Core.Services;
using AuthServer.Domain.Model;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

//Register services
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IEmailService, EmailService>(_=> new EmailService(
    builder.Configuration.GetSection("EmailFrom").Value,
    builder.Configuration.GetSection("AppPassword").Value
));

//Add database context
builder.Services.AddDbContext<AuthServerContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Authentication
builder.Services.AddAuthorization();
builder.Services.AddAuthentication()
    .AddFacebook(options =>
    {
        options.AppId = builder.Configuration["Authentication:Facebook:AppId"];
        options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
        options.CallbackPath = "/api/Account/ExternalLoginCallback?returnUrl=&remoteError=";
    });
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(
        options=>
        {
            options.User.RequireUniqueEmail = false;
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
builder.Services.AddSwaggerGen();
var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();