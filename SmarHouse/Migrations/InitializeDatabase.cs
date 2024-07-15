using SmartHouse.Data;
using SmartHouse.Models;

namespace SmartHouse.Migrations
{
    public class InitializeDatabase
    {
        public static void SeedData(IApplicationBuilder app)
        {
            using (var serviceScope =
                app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var serviceProvider = serviceScope.ServiceProvider;

                using (var db = serviceProvider.GetService<ArduinoDbContext>())
                {
                  
          
                }
            }
        }

       
}
}