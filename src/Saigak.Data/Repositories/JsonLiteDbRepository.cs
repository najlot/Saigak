using LiteDB;

namespace Saigak.Data.Repositories;

public sealed class JsonLiteDbRepository : IJsonRepository, IDisposable
{
	private readonly LiteDatabase _db;

	public JsonLiteDbRepository(SaigakConfiguration configuration)
	{
		var path = Path.Combine(configuration.AppDataPath, "data.db");
		_db = new LiteDatabase($"Filename={path};Connection=direct");
	}

	public string[] GetAll(string type)
	{
		var collection = _db.GetCollection(type);
		return collection
			.FindAll()
			.Select(document => JsonSerializer.Serialize(document))
			.ToArray();
	}

	public string? Get(string type, string id)
	{
		var collection = _db.GetCollection(type);
		var document = collection.FindById(new BsonValue(id));

		if (document is null)
		{
			return null;
		}

		return JsonSerializer.Serialize(document);
	}

	public void Create(string type, string id, string json)
	{
		var collection = _db.GetCollection(type);
		var document = JsonSerializer.Deserialize(json).AsDocument;
		collection.Insert(new BsonValue(id), document);
	}

	public void Update(string type, string id, string json)
	{
		var collection = _db.GetCollection(type);
		var document = JsonSerializer.Deserialize(json).AsDocument;
		collection.Update(new BsonValue(id), document);
	}

	public void Delete(string type, string id)
	{
		var collection = _db.GetCollection(type);
		collection.Delete(new BsonValue(id));
	}

	public void Dispose() => _db.Dispose();
}
