using Microsoft.AspNetCore.Mvc;
using Saigak.Data.Repositories;
using System.Text.Json;

namespace Saigak.Data.Controllers;

[ApiController]
[Route("[controller]")]
public class DataController(IJsonRepository repository) : ControllerBase
{
	[HttpGet("{type}")]
	public ActionResult<string> Get(string type)
	{
		var jsonArray = repository.GetAll(type.ToLower());
		var json = string.Concat('[', string.Join(",", jsonArray), ']');
		return Ok(json);
	}

	[HttpGet("{type}/{id}")]
	public ActionResult<string> Get(string type, string id)
	{
		var json = repository.Get(type.ToLower(), id.ToLower());
		if (json is null)
		{
			return NotFound();
		}

		return Ok(json);
	}

	[HttpPost("{type}/{id}")]
	public ActionResult Post(string type, string id, [FromBody] JsonElement obj)
	{
		var json = obj.GetRawText();
		repository.Create(type.ToLower(), id.ToLower(), json);
		return NoContent();
	}

	[HttpPut("{type}/{id}")]
	public ActionResult Put(string type, string id, [FromBody] JsonElement obj)
	{
		var json = obj.GetRawText();
		repository.Update(type.ToLower(), id.ToLower(), json);
		return NoContent();
	}

	[HttpDelete("{type}/{id}")]
	public ActionResult Delete(string type, string id)
	{
		repository.Delete(type.ToLower(), id.ToLower());
		return NoContent();
	}
}