using System.Globalization;
using HotelManagement;
using HotelManagement.Models;
using HotelManagement.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";
    });

// Add services to the container.
builder.Services.AddControllersWithViews().AddNewtonsoftJson();


builder.Services.AddControllers(options =>
{
    options.InputFormatters.Insert(0, MyJPIF.GetJsonPatchInputFormatter());
});


// Read appsettings.json
builder.Configuration.AddJsonFile("appsettings.json");

// Register ApiSettings
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

// Register services
builder.Services.AddApiServices();

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddDistributedMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Configure the CultureInfo for the entire application
var supportedCultures = new[] { new CultureInfo("vi-VN") };
supportedCultures[0].NumberFormat.CurrencySymbol = "₫";
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("vi-VN"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();