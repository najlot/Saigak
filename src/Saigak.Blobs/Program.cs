using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace Saigak.Blobs;

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

		// Configure max upload size
		long maxFileSize = 512 * 1024 * 1024; // 512 MB

		builder.Services.Configure<KestrelServerOptions>(options =>
		{
			options.Limits.MaxRequestBodySize = maxFileSize; // Default value is 30 MB
		});

		builder.Services.Configure<FormOptions>(options =>
		{
			options.MultipartBodyLengthLimit = maxFileSize; // Default value is 128 MB
		});

		// Add services to the container.
		builder.Services.AddControllers();

		builder.Services.AddSingleton(config);
		builder.Services.AddSingleton<IFileStorage, FileStorage>();

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		app.MapControllers();
		//app.UseAuthorization();

		app.Run();
	}
}
