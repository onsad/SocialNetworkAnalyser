using Microsoft.EntityFrameworkCore;
using SocialNetworkAnalyser.DAL;
using SocialNetworkAnalyser.ExceptionHandler;
using SocialNetworkAnalyser.Repositories;
using SocialNetworkAnalyser.Services;

var builder = WebApplication.CreateBuilder(args);

var conString = builder.Configuration.GetConnectionString("DBConnection") ?? throw new InvalidOperationException("Connection string 'DBConnection not found.");

builder.Services.AddDbContext<SocialNetworkAnalyserContext>(options => options.UseSqlServer(conString));
builder.Services.AddTransient<ISocialNetworkAnalysisRepository, SocialNetworkAnalysisRepository>();
builder.Services.AddTransient<IAnalysisService, AnalysisService>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddExceptionHandler<ExceptionHandler>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<SocialNetworkAnalyserContext>();
    dataContext.Database.EnsureCreated();
}

app.UseExceptionHandler("/Home/Error");
app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
