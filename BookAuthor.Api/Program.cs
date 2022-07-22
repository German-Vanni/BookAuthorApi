using BookAuthor.Api.Configurations;
using BookAuthor.Api.DataAccess;
using BookAuthor.Api.DataAccess.Repository.UnitOfWork;
using BookAuthor.Api.Model;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

namespace BookAuthor.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
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
            

            var app = builder.Build();

            // Configure the HTTP request pipeline.

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
    }
}