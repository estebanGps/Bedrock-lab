using BedrockLab.Models;

namespace BedrockLab.Tools;

public class GroupsTool
{
    [BedrockTool("get_all_groups", "Retrieve a list of all the groups in the application in JSON format.")]
    public static string GetAllGroups()
    {
        return System.Text.Json.JsonSerializer.Serialize(_allGroups);
    }

    private static readonly List<Group> _allGroups =
    [
        new()
        {
            Id = 1,
            Name = "Group One",
            AssetIds = [1, 2]
        },
        new()
        {
            Id = 2,
            Name = "Group Two",
            AssetIds = [2]
        }
    ];
}