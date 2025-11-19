using Amazon.Runtime.Documents;
using BedrockLab.Tools;

namespace BedrockLab.Services;

public class ToolExecutor
{
    public static Document ExecuteTool(string toolName, Document input)
    {
        return toolName switch
        {
            "get_all_assets" => Document.FromObject(new { assets = AssetsTool.GetAllAssets() }),
            "get_all_groups" => Document.FromObject(new { groups = GroupsTool.GetAllGroups() }),
            "get_asset_by_id" => Document.FromObject(new { asset = GetAssetById(input) }),
            _ => Document.FromObject(new { error = "Unknown tool" })
        };
    }

    private static string GetAssetById(Document input)
    {
        Dictionary<string, Document> inputData = input.AsDictionary();
        int assetId = inputData["asset_id"].AsInt();
        return AssetsTool.GetAssetById(assetId);
    }
}