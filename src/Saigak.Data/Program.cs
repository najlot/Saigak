using Saigak.Data.Repositories;

namespace Saigak.Data;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);
		var config = new SaigakConfiguration
		{
			AppDataPath = builder.Environment.ContentRootPath,
		};

		var section = builder.Configuration.GetSection("Saigak");

		if (section.Exists())
		{
			section.Bind(config);
		}

		// Add services to the container.
		builder.Services.AddControllers();

		builder.Services.AddSingleton(config);

		if (config.Backend.Equals("LiteDB", StringComparison.OrdinalIgnoreCase))
		{
			builder.Services.AddSingleton<IJsonRepository, JsonLiteDbRepository>();
		}
		else if (config.Backend.Equals("InMemory", StringComparison.OrdinalIgnoreCase))
		{
			builder.Services.AddSingleton<IJsonRepository, JsonInMemoryRepository>();
		}
		else // File
		{
			builder.Services.AddSingleton<IJsonRepository, JsonFileRepository>();
		}

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		app.MapControllers();
		// app.UseAuthorization();

		app.Run();
	}
}
