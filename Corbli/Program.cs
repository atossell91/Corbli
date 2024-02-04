using MySqlConnector;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Corbli.Models;
using Corbli.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IUserStore<ApplicationUser>, UserStore>();
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication().AddCookie("Cookies");

builder.Services.AddAuthorization(options =>
{
//    options.AddPolicy("IsLoggedIn", policy => policy.RequireClaim("UserType"));
});

var MySqlBuilder = new MySqlConnectionStringBuilder
{
    Server = "localhost",
    UserID = System.Environment.GetEnvironmentVariable("SQLUSR"),
    Password = System.Environment.GetEnvironmentVariable("SQLPASS"),
    Database = "corbli"
};

builder.Services.AddScoped<MySqlDataSource>(
    _ => new MySqlDataSource(MySqlBuilder.ConnectionString));


builder.Services.AddDefaultIdentity<ApplicationUser>().AddDefaultTokenProviders();

builder.Services.AddScoped<UserDbService>();
builder.Services.AddScoped<PasswordHasher<LoginModel>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{

    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });

    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
