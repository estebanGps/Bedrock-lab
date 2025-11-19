namespace BedrockLab.Models;

public class Group
{
    public int ApplicationId { get; set; }
    public List<int> AssetIds { get; set; } = new();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Id { get; set; } = 0;
}
