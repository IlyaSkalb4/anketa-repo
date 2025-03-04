using AnketaDatabaseLibrary.Data;
using backend.Data.Models;
using backend.Services;
using backend.Services.Implementation;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string? connectionString = builder.Configuration.GetConnectionString("AnketaDb");

builder.Services.AddDbContext<AnketaDatabaseContext>(options =>
options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
	options.SignIn.RequireConfirmedEmail = true;
})
.AddEntityFrameworkStores<AnketaDatabaseContext>()
.AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
	options.ExpireTimeSpan = TimeSpan.FromDays(200);
	options.SlidingExpiration = true;
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
	options.LoginPath = "/Account/Login";
	options.LogoutPath = "/Account/Logout";
});

builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<IQuestionnaireService, QuestionnaireService>();

builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication()
	.AddGoogle(googleOptions =>
	{
		googleOptions.ClientId = Environment.GetEnvironmentVariable("GOOGLE_OAUTH_CLIENT_ID") ??
						  builder.Configuration["Authentication:Google:ClientId"];
		googleOptions.ClientSecret = Environment.GetEnvironmentVariable("GOOGLE_OAUTH_CLIENT_SECRET") ??
							  builder.Configuration["Authentication:Google:ClientSecret"];
	})
	.AddFacebook(facebookOptions =>
	{
		facebookOptions.AppId = Environment.GetEnvironmentVariable("FACEBOOK_OAUTH_CLIENT_ID") ??
						  builder.Configuration["Authentication:Facebook:AppId"];
		facebookOptions.AppSecret = Environment.GetEnvironmentVariable("FACEBOOK_OAUTH_CLIENT_SECRET") ??
							  builder.Configuration["Authentication:Facebook:AppSecret"];
	});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
