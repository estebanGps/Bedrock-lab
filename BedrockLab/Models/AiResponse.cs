namespace BedrockLab.Models;

public class AiResponse
{
    public string Text { get; set; } = string.Empty;
    public long TotalTokenCount { get; set; }
    public string Error { get; set; } = string.Empty;
}