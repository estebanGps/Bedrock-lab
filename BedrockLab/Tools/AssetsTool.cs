using System.Text.Json;
using Amazon.BedrockRuntime.Model;
using BedrockLab.Models;

namespace BedrockLab.Tools;

public class AssetsTool
{
    public static ToolSpecification GetAllAssetsToolSpec
    {
        get
        {
            return new ToolSpecification
            {
                Name = "get_all_assets",
                Description = "Retrieve a list of all the assets in the application in JSON format.",
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

    public static ToolSpecification GetAssetByIdToolSpec
    {
        get
        {
            return new ToolSpecification
            {
                Name = "get_asset_by_id",
                Description = "Retrieve a specific asset by its ID in JSON format.",
                InputSchema = new ToolInputSchema
                {
                    Json = Amazon.Runtime.Documents.Document.FromObject(new
                    {
                        type = "object",
                        properties = new
                        {
                            asset_id = new
                            {
                                type = "integer",
                                description = "The unique identifier of the asset."
                            }
                        },
                        required = new[] { "asset_id" }
                    })
                }
            };
        }
    }

    public static string GetAllAssets()
    {
        return JsonSerializer.Serialize(_allAssets);
    }

    public static string GetAssetById(int assetId)
    {
        Asset? asset = _allAssets.Find(a => a.Id == assetId);
        if (asset != null)
        {
            return JsonSerializer.Serialize(asset);
        }
        return JsonSerializer.Serialize(new { error = "Asset not found" });
    }

    private static readonly List<Asset> _allAssets =
    [
        new() {
                Id = 1,
                Name = "Asset One",
                Utc = DateTime.UtcNow,
                DeviceActivity = DateTime.UtcNow,
                Position = new Position { Latitude = 57, Longitude = 12 },
                Variables = [
                    new AssetVariable
                    {
                        Name = "Speed",
                        Time = DateTime.UtcNow.AddMinutes(-10),
                        Type = "number",
                        Value = "0"
                    }
                ]
            },
            new() {
                Id = 2,
                Name = "Asset Two",
                Utc = DateTime.UtcNow,
                DeviceActivity = DateTime.UtcNow,
                Position = new Position { Latitude = 50, Longitude = 10 },
                Variables = [
                    new AssetVariable
                    {
                        Name = "Speed",
                        Time = DateTime.UtcNow.AddMinutes(-10),
                        Type = "number",
                        Value = "10"
                    }
                ]
            },
            new() {
                Id = 3,
                Name = "Asset Three",
                Utc = DateTime.UtcNow.AddDays(-5),
                DeviceActivity = DateTime.UtcNow.AddDays(-5),
                Position = new Position { Latitude = 48, Longitude = 11 },
                Variables = [
                    new AssetVariable
                    {
                        Name = "Speed",
                        Time = DateTime.UtcNow.AddDays(-5),
                        Type = "number",
                        Value = "12"
                    }
                ]
            }
    ];
}