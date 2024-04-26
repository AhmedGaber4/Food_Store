using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Store.Data.Entities;
using Store.Data.IdentityEntities;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Store.API.Extensions
{
    public static class IdentityServiceExtensions
    {
        
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration _configuration)
        {
           
            var builder = services.AddIdentityCore<AppUser>() ;
            builder= new IdentityBuilder(builder.UserType, builder.Services);
            builder.AddEntityFrameworkStores<StoreIdentityDbContext>();
            builder.AddSignInManager<SignInManager<AppUser>>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option => {
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                  IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:Key"])),
                  ValidateIssuer=true,
                    ValidIssuer = _configuration["Token:Issuer"] ,
                    ValidateAudience=false
                };
            });
            return services;
        }
    }
}
