using Contracts;
using Contracts.DBF;
using Contracts.Repository;
using DbfFile;
using DbfShowService;
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

            //////////////////////////////////////////////////////////////////////////////////
            #region CORS
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
            #endregion

            //////////////////////////////////////////////////////////////////////////////////
            #region ����
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)                
                .WriteTo.Console()
                .Enrich.FromLogContext()
                .CreateLogger();
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);
            builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
            #endregion

            //////////////////////////////////////////////////////////////////////////////////
            #region DATABASE
            builder.Services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection"), b => b.MigrationsAssembly("WebDBFShow"));
                //options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("WebDBFShow"));
            });
            #endregion


            builder.Services.AddScoped<IShowService, ShowService>();
            builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
            builder.Services.AddScoped<IFileDbReader, DbfReader>();
           

            var app = builder.Build();


            app.UseCors();

            //���������� Exception Handler
            app.ConfigureExceptionHandler(app.Logger);

            //app.UseMiddleware<BlockSubdomainsMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCookiePolicy();            
            //app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}