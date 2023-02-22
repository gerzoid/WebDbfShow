
using Contracts;
using Contracts.DBF;
using DbfFile;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Repository;
using Serilog;
using System.Text;
using WebDBFShow.Extensions;

namespace WebDBFShow
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Policy1", policy =>
                {
                    policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    //.WithMethods("POST", "GET", "PUT", "DELETE")
                    .AllowAnyHeader();
                });
            });

            //Логи
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .WriteTo.Console()
                .Enrich.FromLogContext()
                .CreateLogger();
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);
            builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);

            builder.Services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection"), b => b.MigrationsAssembly("WebDBFShow"));
                //options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("WebDBFShow"));
            });

            builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
            builder.Services.AddScoped<IFileDbReader, DbfReader>();
           
            var app = builder.Build();

            app.UseCors();

            //Глобальный Exception Handler
            //app.ConfigureExceptionHandler(app.Logger);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //Глобальный Exception Handler
                app.ConfigureExceptionHandler(app.Logger);
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCookiePolicy();
            
            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}