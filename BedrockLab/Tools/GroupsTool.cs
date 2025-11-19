using Amazon.BedrockRuntime.Model;
using BedrockLab.Models;

namespace BedrockLab.Tools;

public class GroupsTool
{
    public static ToolSpecification GetAllGroupsToolSpec
    {
        get
        {
            return new ToolSpecification
            {
                Name = "get_all_groups",
                Description = "Retrieve a list of all the groups in the application in JSON format.",
                InputSchema = new ToolInputSchema
                {
                    Json = Amazon.Runtime.Documents.Document.FromObject(new
                    {
                        type = "object",
                    })
                }
            };
        }
    }

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