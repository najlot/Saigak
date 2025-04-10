namespace Saigak.Data.Repositories;

public class JsonFileRepository(SaigakConfiguration configuration) : IJsonRepository
{
	private readonly string _basePath = configuration.AppDataPath;

	private string BuildPath(string type) => Path.Combine(_basePath, "data", type);
	private string BuildPath(string type, string id) => Path.Combine(_basePath, "data", type, id);

	public string[] GetAll(string type)
	{
		var path = BuildPath(type);

		if (Directory.Exists(path))
		{
			return Directory.GetFiles(path).Select(File.ReadAllText).ToArray();
		}

		return [];
	}

	public string? Get(string type, string id)
	{
		var path = BuildPath(type, id);

		if (File.Exists(path))
		{
			return File.ReadAllText(path);
		}

		return null;
	}

	public void Create(string type, string id, string json)
	{
		var path = BuildPath(type, id);
		var directory = BuildPath(type);

		if (!string.IsNullOrEmpty(directory))
		{
			Directory.CreateDirectory(directory);
		}

		if (File.Exists(path))
		{
			throw new Exception($"Entry {type} / {id} exists");
		}

		File.WriteAllText(path, json);
	}

	public void Update(string type, string id, string json)
	{
		var path = BuildPath(type, id);

		if (!File.Exists(path))
		{
			throw new Exception($"Entry {type} / {id} does not exists");
		}

		File.Delete(path);
		File.WriteAllText(path, json);
	}

	public void Delete(string type, string id)
	{
		var path = BuildPath(type, id);
		File.Delete(path);
	}
}