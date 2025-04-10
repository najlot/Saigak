
namespace Saigak.Blobs;

public interface IFileStorage
{
	Task<bool> Get(string id, Stream stream);
	Task Create(string id, Stream stream);
	Task Update(string id, Stream stream);
	void Delete(string id);
}