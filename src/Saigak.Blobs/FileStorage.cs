namespace Saigak.Blobs;

public class FileStorage(SaigakConfiguration config) : IFileStorage
{
	private readonly string _filesPath = Path.Combine(config.AppDataPath, "files");

	public async Task<bool> Get(string id, Stream stream)
	{
		var path = Path.Combine(_filesPath, id);

		if (File.Exists(path))
		{
			using var fs = File.OpenRead(path);
			await fs.CopyToAsync(stream).ConfigureAwait(false);
			return true;
		}

		return false;
	}

	public async Task Create(string id, Stream stream)
	{
		var path = Path.Combine(_filesPath, id);
		Directory.CreateDirectory(_filesPath);

		if (File.Exists(path))
		{
			throw new Exception($"File {id} already exists");
		}

		using var file = File.OpenWrite(path);
		await stream.CopyToAsync(file).ConfigureAwait(false);
	}

	public async Task Update(string id, Stream stream)
	{
		var path = Path.Combine(_filesPath, id);

		if (!File.Exists(path))
		{
			throw new Exception($"File {id} does not exist");
		}

		File.Delete(path);
		using var file = File.OpenWrite(path);
		await stream.CopyToAsync(file).ConfigureAwait(false);
	}

	public void Delete(string id)
	{
		var path = Path.Combine(_filesPath, id);
		File.Delete(path);
	}
}
