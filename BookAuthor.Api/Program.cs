using BookAuthor.Api.Configurations;
using BookAuthor.Api.DataAccess;
using BookAuthor.Api.DataAccess.Repository.UnitOfWork;
using BookAuthor.Api.Middleware;
using BookAuthor.Api.Model;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;

namespace BookAuthor.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLog.LogManager.Setup()
                //.LoadConfiguration()
                .GetCurrentClassLogger();
            logger.Debug("init main");
            try
            {
                logger.Info("Starting!");
                var builder = WebApplication.CreateBuilder(args);

                ConfigurationManager Configuration = builder.Configuration;

                // Add services to the container.
                builder.Services.AddApiServices();

                builder.Services.AddDbContext<AppDbContext>(opt =>
                    opt.UseSqlServer(Configuration.GetConnectionString("SqlServerCS"))
                );

                builder.Services.AddAuthentication();
                builder.Services.ConfigureIdentity();
                builder.Services.ConfigureJwtAuthentication(Configuration);

                builder.Services.AddCors(options => {
                    options.AddPolicy(name: "AllowAnyOrigin",
                                      policy => {
                                          policy.AllowAnyOrigin();
                                          policy.AllowAnyMethod();
                                      });
                });


                builder.Services.AddControllers().AddNewtonsoftJson(op =>
                    op.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    );

                builder.Services.AddSwaggerGen();

                builder.Logging.ClearProviders();
                builder.Host.UseNLog();

                var app = builder.Build();

                // Configure the HTTP request pipeline.

                app.UseMiddleware<ErrorHandlingMiddleware>();

                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseCors("AllowAnyOrigin");

                app.UseHttpsRedirection();

                app.UseAuthentication();
                app.UseAuthorization();


                app.MapControllers();

                app.Run();
            }
            catch (Exception e)
            {
                logger.Error(e,  $"Stopped program because of exception: {e.Message}" );
                throw;
            }
            finally
            {

            }
            
        }
    }
}