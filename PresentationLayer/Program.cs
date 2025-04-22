using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Repositories;
using DataAccessLayer.Repositories.Interfaces;
using Domain;
using BusinessLogicLayer.Services.Interfaces;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.Mapping;
using PresentationLayer.Middleware;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace PresentationLayer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var columnOptions = new ColumnOptions();
            columnOptions.Store.Clear();
            columnOptions.Store.Add(StandardColumn.Id);
            columnOptions.Store.Add(StandardColumn.Message);
            columnOptions.Store.Add(StandardColumn.MessageTemplate);
            columnOptions.Store.Add(StandardColumn.Level);
            columnOptions.Store.Add(StandardColumn.TimeStamp);
            columnOptions.Store.Add(StandardColumn.Exception);
            columnOptions.Store.Add(StandardColumn.Properties);
            columnOptions.Store.Add(StandardColumn.LogEvent);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)  
                .Enrich.FromLogContext()
                //.WriteTo.Console()
                .WriteTo.MSSqlServer(
                    connectionString: configuration.GetConnectionString("DefaultConnection"),
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = "EFCoreLogs",
                        AutoCreateSqlTable = true
                    },
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    columnOptions: columnOptions
                )
                .CreateLogger();
            Serilog.Debugging.SelfLog.Enable(msg =>
            {
                 Console.Error.WriteLine(msg);
                //System.IO.File.AppendAllText("serilog-selflog.txt", msg + Environment.NewLine);
            });

            Log.Information("Application Starting Up");

            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog();

            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IApplicationLogService, ApplicationLogService>();
            builder.Services.AddScoped<IApplicationLogRepository, ApplicationLogRepository>();
            builder.Services.AddScoped<IHttpLogRepository, HttpLogRepository>();
            builder.Services.AddScoped<IHttpLogService, HttpLogService>();
            builder.Services.AddScoped<ITranslationService, TranslateTol33tsp34kService>();
            builder.Services.AddScoped<ICRUDService<User>, UserService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);
            builder.Services.AddScoped<ICRUDRepository<User>, UserRepository>();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("PresentationLayer"))
                       .EnableSensitiveDataLogging()
                       .EnableDetailedErrors());

            builder.Services.AddAuthentication("Cookies").AddCookie(options =>
            {
                options.LoginPath = "/Authorization/Login";
                options.LogoutPath = "/Authorization/Logout";
                options.AccessDeniedPath = "/Authorization/AccessDenied";
            });
            builder.Services.AddAuthorization(
            options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
                options.AddPolicy("User", policy => policy.RequireRole("User"));
            }
            );

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseMiddleware<HttpLogMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Authorization}/{action=Login}/{id?}");

            try
            {
                app.Run();
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
