using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using clsBacklog.Data;
using clsBacklog.Models;
using wppBacklog.Handlers.Interfaces;
using wppBacklog.Handlers;
using wppBacklog.Models;
using clsBacklog.Interfaces;
using clsBacklog.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<AppConfigModel>(builder.Configuration.GetSection("AppSettings"));
var identityDbConnectionString = builder.Configuration.GetConnectionString("IdentityDbConnection") ?? throw new InvalidOperationException("Connection string 'IdentityContextConnection' not found.");
var applicationDbConnectionString = builder.Configuration.GetConnectionString("ApplicationDbConnection") ?? throw new InvalidOperationException("Connection string 'IdentityContextConnection' not found.");

builder.Services.AddDbContext<IdentityContext>(options =>
    options.UseSqlServer(identityDbConnectionString));

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(applicationDbConnectionString));

builder.Services.AddDefaultIdentity<UserModel>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<IdentityContext>();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(1);
    options.LoginPath = "/Account/Login";
});


builder.Services.AddTransient<INotificationHandlers, NotificationHandlers>();

builder.Services.AddTransient<IOrganizationServices, OrganizationServices>();
builder.Services.AddTransient<IProjectServices, ProjectServices>();
builder.Services.AddTransient<ITaskServices, TaskServices>();


// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation(); ;

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
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
