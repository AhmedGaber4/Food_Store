using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace Store.API.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(option=>{
                option.SwaggerDoc("v1",new OpenApiInfo { Title="StoreAPI" , Version="v1" });
                var securityScheme = new OpenApiSecurityScheme
                {
                    Description= "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name= "Authorization",
                    In=ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme="bearer",
                    Reference =new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "bearer"
                    }


                };
                option.AddSecurityDefinition("bearer",securityScheme);
                var securityRequirements = new OpenApiSecurityRequirement
                {
                    {securityScheme,new[]{ "bearer" } }
                };
                option.AddSecurityRequirement(securityRequirements);
            });
            return services;
        }
    }
}
