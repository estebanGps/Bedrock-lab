using System.Text.Json;
using BedrockLab.Models;

namespace BedrockLab.Tools;

public class AssetsTool
{
    [BedrockTool("get_all_assets", "Retrieve a list of all the assets in the application in JSON format.")]
    public static string GetAllAssets()
    {
        return JsonSerializer.Serialize(_allAssets);
    }

    [BedrockTool("get_asset_by_id", "Retrieve a specific asset by its ID. The result is in JSON format.")]
    public static string GetAssetById([BedrockToolParam("asset_id", "The unique identifier of the asset.")] int assetId)
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