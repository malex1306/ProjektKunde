using Microsoft.EntityFrameworkCore;
using ProjektKunde.Models;

namespace ProjektKunde.Data;

public class EnsureDatabase
{
    public static void ExecuteMigrations(IApplicationBuilder app)
    {
        var context = app
            .ApplicationServices
            .CreateScope()
            .ServiceProvider
            .GetRequiredService<AppDbContext>();

        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
    }

    public static void SeedDatabase(IApplicationBuilder app)
    {
        var context = app
            .ApplicationServices
            .CreateScope()
            .ServiceProvider
            .GetRequiredService<AppDbContext>();

      
        if (!context.Customers.Any())
        {
            var ikea = new Customer()
            {
                Company = "Ikea",
                Adress = "Beispieladress"
            };

            var roller = new Customer()
            {
                Company = "Roller",
                Adress = "Beispielweg"
            };

            context.Customers.AddRange(ikea, roller);
            context.SaveChanges();
        }

      
        if (!context.Projects.Any())
        {
           
            var ikea = context.Customers.FirstOrDefault(c => c.Company == "Ikea");
            var roller = context.Customers.FirstOrDefault(c => c.Company == "Roller");

            if (ikea != null && roller != null)
            {
                context.Projects.AddRange(
                    new Project()
                    {
                        title = "Geschäftsidee",
                        description = "Neuer vorschlag",
                        Start = new DateTime(2025, 5, 30),
                        End = new DateTime(2025, 6, 30),
                        CustomerId = ikea.Id
                    },
                    new Project()
                    {
                        title = "Kundenvorschlag",
                        description = "Neuer vorschlag",
                        Start = new DateTime(2025, 8, 30),
                        End = new DateTime(2025, 10, 30),
                        CustomerId = roller.Id
                    });
                context.SaveChanges();
            }
        }
    }
}
