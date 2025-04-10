using Microsoft.AspNetCore.Mvc;

namespace Saigak.Blobs.Controllers;

[ApiController]
[Route("[controller]")]
public class FilesController(IFileStorage storage) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult> Get(string id)
    {
		var ms = new MemoryStream();
		var result = await storage.Get(id, ms).ConfigureAwait(false);

		if (!result)
		{
			return NotFound();
		}

		ms.Position = 0;
		return File(ms, "application/octet-stream");
	}

	[HttpPost("{id}")]
	public async Task<ActionResult> Post(string id)
	{
		if (!Request.HasFormContentType || !Request.Form.Files.Any())
		{
			return BadRequest("No files were uploaded.");
		}

		using var readStream = Request.Form.Files[0].OpenReadStream();
		await storage.Create(id, readStream).ConfigureAwait(false);
		return Ok();
	}

	[HttpPut("{id}")]
	public async Task<ActionResult> Put(string id)
	{
		if (!Request.HasFormContentType || !Request.Form.Files.Any())
		{
			return BadRequest("No files were uploaded.");
		}

		using var readStream = Request.Form.Files[0].OpenReadStream();
		await storage.Update(id, readStream).ConfigureAwait(false);
		return Ok();
	}

	[HttpDelete("{id}")]
	public ActionResult Delete(string id)
	{
		storage.Delete(id);
		return Ok();
	}
}
