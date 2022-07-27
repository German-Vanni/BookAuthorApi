using BookAuthor.Api.DataAccess;
using BookAuthor.Api.DataAccess.Repository.UnitOfWork;
using BookAuthor.Api.Model;
using BookAuthor.Api.Services.AuthManager;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BookAuthor.Api.Configurations
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var identityBuilder = services.AddIdentityCore<ApiUser>(
                opt => {
                    opt.User.RequireUniqueEmail = true;
                    opt.Password.RequireNonAlphanumeric = false;
                    opt.Password.RequireDigit = false;
                    opt.Password.RequireUppercase = false;
                }
            );
            identityBuilder = new IdentityBuilder(identityBuilder.UserType, typeof(IdentityRole), services);
            identityBuilder.AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
        }

        public static void AddApiServices(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(typeof(MapperInit));
            services.AddScoped<IAuthManager, AuthManager>();
        }

        public static void ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettingsSection = configuration.GetSection("Jwt");
            var key = Environment.GetEnvironmentVariable("ASPNET_API_SECRET");
            if(key == null)
            {
                key = jwtSettingsSection.GetSection("Key").Value;
            }
            if(key == null)
            {
                throw new Exception("No Jwt key provided.");
            }


            services.AddAuthentication(o => {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o => {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidateAudience = false,
                        ValidIssuer = jwtSettingsSection.GetSection("Issuer").Value,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                    };
                });
        }
    }
}
