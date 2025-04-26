using ELawyer.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ELawyer.DataAccess.DbInitilizer;
using ELawyer.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using ELawyer.DataAccess.Repository.IRepository;
using ELawyer.DataAccess.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Bulky.Utility;
using Stripe;
using Bulky.DataAccess.Dbinitilizer;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("Legal"))


);



//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(
//        builder.Configuration.GetConnectionString("Cs"),
//        sqlServerOptions =>
//        {
//            sqlServerOptions.EnableRetryOnFailure(
//                maxRetryCount: 5,
//                maxRetryDelay: TimeSpan.FromSeconds(10),
//                errorNumbersToAdd: null
//            );
//        }));


builder.Services.AddRazorPages();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                   .AddRoles<IdentityRole>()
                   .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));





builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
    //options.Cookie.SecurePolicy = CookieSecurePolicy.None;
});


builder.Services.AddAuthentication().AddFacebook(option =>
{
    //Note:
    // we get their from (Meta for developer) Site
    option.AppId = "1409252836698368";
    option.AppSecret = "5d47d74ca5c8621837806eeafe77f98e";
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);

});
builder.Services.AddScoped<IDbInitilizer, DbInitializer>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IEmailSender, EmailSender>();

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

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:Secretkey").Get<string>();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
SeedDatabase();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Lawyer}/{action=Index}/{id?}");

app.Run();

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitilizer = scope.ServiceProvider.GetRequiredService<IDbInitilizer>();
        dbInitilizer.Initilalize();
    }
}


