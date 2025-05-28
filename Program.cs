using Microsoft.EntityFrameworkCore;
using ProjektKunde.Data;

namespace ProjektKunde;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddControllersWithViews();
        builder.Services.AddDbContext<AppDbContext>(opts =>
        {
            opts.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
        });
        
        var app = builder.Build();
        
        app.UseStaticFiles();

        
        app.MapControllerRoute(
            name:"default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        EnsureDatabase.ExecuteMigrations(app);
        EnsureDatabase.SeedDatabase(app);

        app.Run();
    }
}
