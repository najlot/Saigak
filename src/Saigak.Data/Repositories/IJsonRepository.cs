namespace Saigak.Data.Repositories;

public interface IJsonRepository
{
	string[] GetAll(string type);
	string? Get(string type, string id);
	void Create(string type, string id, string json);
	void Update(string type, string id, string json);
	void Delete(string type, string id);
}