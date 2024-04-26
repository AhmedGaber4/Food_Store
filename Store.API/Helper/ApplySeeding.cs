using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Store.Data;
using Store.Data.Entities;
using Store.Data.IdentityEntities;
using Store.Repository;

namespace Store.API.Helper
{
    public class ApplySeeding
    {
        public static async Task ApplySeedingAsync(WebApplication web)
        {
            using (var scope= web.Services.CreateScope())
            {
                var services= scope.ServiceProvider;
                var loggerfactory= services.GetRequiredService<ILoggerFactory>() ;
                try
                {
                    var context = services.GetRequiredService<StoreDBContext>();
                    var User = services.GetRequiredService <UserManager<AppUser>>();
                    await context.Database.MigrateAsync();
                    await StoreContextSeed.SeedAsync(context, loggerfactory);
                    await AppIdentityContextSeed.SeeduserAsync(User);


                }
                catch (Exception ex)
                {
                    var logger = loggerfactory.CreateLogger<StoreContextSeed>();
                    logger.LogError(ex.Message);

                }
            }
        }
    }
}
