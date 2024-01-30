using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
		builder.Services.AddDbContext<ApplicationDbContext>(options =>
			options.UseSqlServer(connectionString));
		builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
			.AddEntityFrameworkStores<ApplicationDbContext>()
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

		//JWT
		var keyManager = new KeyManager();
		builder.Services.AddSingleton<KeyManager>();
		builder.Services.AddAuthentication()
			.AddJwtBearer("jwt", o =>
			{
				o.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateAudience = false,
					ValidateIssuer = false,
				};
				o.Events = new()
				{
					OnMessageReceived = ctx => {
						if (ctx.Request.Query.TryGetValue("t", out var token)) {
							ctx.Token = token;
						}
						return Task.CompletedTask;
					}
				};
				o.Configuration = new Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectConfiguration()
				{
					SigningKeys = {
					new RsaSecurityKey(keyManager.rsaKey)
					}
				};
			});
		builder.Services.AddAuthorization(b => {
			b.DefaultPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
			.RequireAuthenticatedUser()
			.AddAuthenticationSchemes(IdentityConstants.ApplicationScheme, "jwt")
			.Build();
			b.AddPolicy("policy", pb => pb
			.RequireAuthenticatedUser()
			.RequireClaim("role", "Admin"));
		});

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
			s.EnableAnnotations();
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