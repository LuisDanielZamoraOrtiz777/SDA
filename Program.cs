using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using VulnerableApp.Data;
using VulnerableApp.Models;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure DbContext (uses connection string from appsettings.json)
var connectionString = builder.Configuration.GetConnectionString("VulnerableDb") ??
                       "Server=(localdb)\\mssqllocaldb;Database=VulnerableDb;Trusted_Connection=True;MultipleActiveResultSets=true";
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

// Sessions
builder.Services.AddSession();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Ensure database is created and seed initial users if not present
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.Users.Any())
    {
        db.Users.AddRange(
            new User { Username = "admin", Password = "admin", Email = "admin@test.com", Balance = 1000m, CreatedAt = System.DateTime.UtcNow },
            new User { Username = "user1", Password = "123456", Email = "user@test.com", Balance = 500m, CreatedAt = System.DateTime.UtcNow },
            new User { Username = "user2", Password = "password", Email = "user2@test.com", Balance = 750m, CreatedAt = System.DateTime.UtcNow }
        );
        db.SaveChanges();
    }
}

app.Run();
