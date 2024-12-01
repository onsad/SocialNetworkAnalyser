using Microsoft.EntityFrameworkCore;
using SocialNetworkAnalyser.DAL;
using SocialNetworkAnalyser.Services;

var builder = WebApplication.CreateBuilder(args);

var conString = builder.Configuration.GetConnectionString("DBConnection") ?? throw new InvalidOperationException("Connection string 'DBConnection not found.");

builder.Services.AddDbContext<SocialNetworkAnalyserContext>(options => options.UseSqlServer(conString));
builder.Services.AddScoped<IAnalysisService, AnalysisService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<SocialNetworkAnalyserContext>();
    dataContext.Database.EnsureCreated();
}

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
