using Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Infrastructure
{
    public static class ApplicationDbContextInitialiser
    {
        public static IApplicationBuilder ConfigureCoreDb(this IApplicationBuilder app)
        {
            // configure app
            SeedData.Initialize(app.ApplicationServices);
            return app;
        }

    }

    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                // auto migration
                context.Database.MigrateAsync();

                // Seed the database.
                // InitializeUserAndRoles(context);
                // InitializeFunctionAndResource(context);
            }
        }

        private static void InitializeUserAndRoles(ApplicationDbContext context)
        {
            // init user and roles  
        }

        private static void InitializeFunctionAndResource(ApplicationDbContext context)
        {
            using (StreamReader r = new StreamReader("DataSeed/function.json"))
            {
                string json = r.ReadToEnd();
                var Items = JsonConvert.DeserializeObject<List<Function>>(json);
                //modelBuilder.Entity<Function>().HasData(Items);
            }

            using (StreamReader r = new StreamReader("DataSeed/resource.json"))
            {
                string json = r.ReadToEnd();
                var Items = JsonConvert.DeserializeObject<List<Resource>>(json);
                //modelBuilder.Entity<Resource>().HasData(Items);
            }
        }
    }
}
