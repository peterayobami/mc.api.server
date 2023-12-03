using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;

namespace Mc.Api.Server
{
    /// <summary>
    /// Application extensions
    /// </summary>
    public static class ApplicationExtensions
    {
        /// <summary>
        /// This will apply pending migrations to the db
        /// </summary>
        /// <param name="app"></param>
        public static void ApplyMigrations(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var service = scope.ServiceProvider;

                var loggerFactory = service.GetRequiredService<ILoggerFactory>();

                try
                {
                    var context = service.GetRequiredService<ApplicationDbContext>();

                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();

                    logger.LogError("An error occurred while applying migrations. Details: {error}", ex.Message);
                }
            }
        }

        /// <summary>
        /// Configure app db context
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/></param>
        /// <param name="configuration">The <see cref="IConfiguration"/></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            // Set the connection string
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            // Add ApplicationDbContext to DI
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                    sqlOptions => sqlOptions.MigrationsAssembly(typeof(Program).Assembly.GetName().Name));
            });

            // Return services for further chaining
            return services;
        }

        /// <summary>
        /// Set up cors
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader()
                    .AllowAnyOrigin()
                    .AllowAnyMethod();
                });
            });

            // Return services for further chaining
            return services;
        }

        /// <summary>
        /// Add our domain services to IOC container
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<ArticleManagement>()
                .AddScoped<AuthorManagement>()
                .AddScoped<TagManagement>();

            // Return services for further chaining
            return services;
        }

        public static IServiceCollection AddCloudinary(this IServiceCollection services, IConfiguration configuration)
        {
            // Add cloudinary as a scoped service
            services.AddScoped(x => new Cloudinary(new Account
            {
                ApiKey = configuration["Cloudinary:Key"],
                ApiSecret = configuration["Cloudinary:Secret"],
                Cloud = configuration["Cloudinary:Cloud"]
            }));

            services.AddScoped<CloudinaryService>();

            // Return services for further chaining
            return services;
        }
    }
}
