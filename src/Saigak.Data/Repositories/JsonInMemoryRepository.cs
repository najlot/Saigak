using System.Collections.Concurrent;

namespace Saigak.Data.Repositories;

public class JsonInMemoryRepository : IJsonRepository
{
	private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, string>> _data = [];

	public string[] GetAll(string type)
	{
		if (_data.TryGetValue(type, out var keys))
		{
			return keys.Values.ToArray();
		}

		return [];
	}

	public string? Get(string type, string id)
	{
		if (_data.TryGetValue(type, out var keys) && keys.TryGetValue(id, out var value))
		{
			return value;
		}

		return null;
	}

	public void Create(string type, string id, string json)
	{
		if (!_data.TryGetValue(type, out var keys))
		{
			keys = [];
			_data[type] = keys;
		}

		if (keys.ContainsKey(id))
		{
			throw new Exception("Key already exists");
		}

		keys[id] = json;
	}

	public void Update(string type, string id, string json)
	{
		if (!_data.TryGetValue(type, out var keys))
		{
			keys = [];
			_data[type] = keys;
		}

		if (!keys.ContainsKey(id))
		{
			throw new Exception("Key does not exist");
		}

		keys[id] = json;
	}

	public void Delete(string type, string id)
	{
		if (!_data.TryGetValue(type, out var keys))
		{
			keys = [];
			_data[type] = keys;
		}

		if (!keys.ContainsKey(id))
		{
			throw new Exception("Key does not exist");
		}

		keys.Remove(id, out var _);
	}
}