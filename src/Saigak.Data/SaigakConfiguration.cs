namespace Saigak.Data;

public class SaigakConfiguration
{
	public string AppDataPath { get; set; } = ".";
	public string Backend { get; set; } = "File"; // "File", "LiteDB"
}