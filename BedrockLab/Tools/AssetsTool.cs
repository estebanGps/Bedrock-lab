using System.Text.Json;
using Amazon.BedrockRuntime.Model;
using BedrockLab.Models;

namespace BedrockLab.Tools
{
    internal class AssetsTool
    {
        public static ToolSpecification GetAllAssetsToolSpec { get
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

        public static string GetAllAssets()
        {
            var assets = new List<Asset>
            {
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
                }
            };

            return JsonSerializer.Serialize(assets);
        }
    }
}
