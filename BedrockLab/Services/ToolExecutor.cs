using Amazon.Runtime.Documents;
using BedrockLab.Tools;

namespace BedrockLab.Services;

public class ToolExecutor
{
    public Document ExecuteTool(string toolName, Document input)
    {
        return toolName switch
        {
            "get_all_assets" => Document.FromObject(new { assets = AssetsTool.GetAllAssets() }),
            _ => Document.FromObject(new { error = "Unknown tool" })
        };
    }
}