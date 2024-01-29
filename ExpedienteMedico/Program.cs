using System.Configuration;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ExpedienteMedico.Data;
using ExpedienteMedico.Models;
using ExpedienteMedico.Repository;
using ExpedienteMedico.Repository.IRepository;
using ExpedienteMedico.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;


var builder = WebApplication.CreateBuilder(args);

//CORS SERVICE with named policy
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "GeneralPolicy",
                      policy =>
                      {
                          policy.WithOrigins("http://127.0.0.1:5500/", 
                              "https://asmymedical.azurewebsites.net/")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders() //options => options.SignIn.RequireConfirmedAccount = true
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<ApplicationDbContext>();

builder.Services.AddRazorPages();

builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IEmailSender, EmailSender>();


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

//Usage of the CORS policy
app.UseCors("GeneralPolicy");

app.UseAuthentication();;
app.MapRazorPages();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
