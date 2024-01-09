using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using TestProject.Data;
using TestProject.Models;
using TestProject.Services;

public class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //Database connections
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<IDataRepository>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<IDataRepository>()
            .AddDefaultUI()
            .AddDefaultTokenProviders();

        //Localizations
        builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
        builder.Services.AddRazorPages()
            .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
            .AddDataAnnotationsLocalization();
        var cultureValues = new Dictionary<string, string>
{
    { "en", "us" },
    { "uk", "ua" }
};
        builder.Services.AddSingleton(cultureValues);

        //Swagger
        builder.Services.AddSwaggerGen(s =>
        {
            s.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "V1",
                Title = "TestProject API",
                Description = "Tasks API for different stuff"

            });
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            s.IncludeXmlComments(xmlPath);
        });

        //Miscellaneous
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        builder.Services.AddScoped<IFileService, FileService>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IDataRepository<Item>, ItemsRepository>();
        builder.Services.AddScoped<IDataService<Item>, ItemsService>();
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        //Localizations
        var supportedCultures = new[] { "en", "uk" };
        var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
            .AddSupportedCultures(supportedCultures)
            .AddSupportedUICultures(supportedCultures);
        app.UseRequestLocalization(localizationOptions);

        //Miscellaneous
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseMiddleware<NotFoundRedirectMiddleware>();
        app.UseRouting();
        app.UseAuthorization();
        app.MapRazorPages();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();

        });

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");


        //Swagger
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestProject API");
        });

        //Role Seed
        using (var scope = app.Services.CreateScope())
        {
            await DbSeeder.SeedRolesAndAdminAsync(scope.ServiceProvider);
        }

        app.Run();
    }
}