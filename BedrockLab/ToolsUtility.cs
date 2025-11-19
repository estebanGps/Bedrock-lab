using Amazon.BedrockRuntime.Model;
using BedrockLab.Tools;

namespace BedrockLab
{
    internal class ToolsUtility
    {
        internal static List<Tool> GetAllTools()
        {
            return [
                new Tool() { ToolSpec = AssetsTool.GetAllAssetsToolSpec },
                new Tool() { ToolSpec = AssetsTool.GetAssetByIdToolSpec },
                new Tool() { ToolSpec = GroupsTool.GetAllGroupsToolSpec }
            ];
        }
    }
}
