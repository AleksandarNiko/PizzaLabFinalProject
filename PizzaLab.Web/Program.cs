using Microsoft.EntityFrameworkCore;

using PizzaLab.Web;
using PizzaLab.Data;
using PizzaLab.Data.Models;
using PizzaLab.Web.Infrastructure.Extensions;
using PizzaLab.Services.Data.Interfaces;
using PizzaLab.Web.Infrastructure.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.Language.Intermediate;

using static PizzaLab.Common.GeneralApplicationConstants;
using PizzaLab.Data.Models;
using PizzaLab.Web.Infrastructure.ModelBinders;

var builder = WebApplication.CreateBuilder(args);

string connectionString =
                builder.Configuration.GetConnectionString("DefaultConnection") ??
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<PizzaLabDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount =
        builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedAccount");
    options.Password.RequireLowercase =
        builder.Configuration.GetValue<bool>("Identity:Password:RequireLowercase");
    options.Password.RequireUppercase =
        builder.Configuration.GetValue<bool>("Identity:Password:RequireUppercase");
    options.Password.RequireNonAlphanumeric =
        builder.Configuration.GetValue<bool>("Identity:Password:RequireNonAlphanumeric");
    options.Password.RequiredLength =
        builder.Configuration.GetValue<int>("Identity:Password:RequiredLength");
})
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<PizzaLabDbContext>();

builder.Services.AddApplicationServices(typeof(IPizzaService));

builder.Services.AddMemoryCache();

builder.Services
    .AddControllersWithViews()
    .AddMvcOptions(options =>
    {
        options.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider());
        options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error/500");
    app.UseStatusCodePagesWithRedirects("/Home/Error?statusCode={0}");

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.EnableOnlineUsersCheck();

if (app.Environment.IsDevelopment())
{
    app.SeedAdministrator(DevelopmentAdminEmail);
}

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "admin",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
});
app.MapRazorPages();

await app.RunAsync();
public partial class Program { }