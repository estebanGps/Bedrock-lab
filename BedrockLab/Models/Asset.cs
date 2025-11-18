namespace BedrockLab.Models;

public class Asset
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public Position Position { get; set; } = new();
    public DateTime Utc { get; set; }
    public DateTime DeviceActivity { get; set; }
    public Velocity Velocity { get; set; } = new();
    public List<AssetVariable> Variables { get; set; } = new();
}

public class Position
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Altitude { get; set; }
}

public class Velocity
{
    public double GroundSpeed { get; set; }
    public double Heading { get; set; }
}

public class AssetVariable
{
    public string Name { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public DateTime Time { get; set; }

    public string Value { get; set; } = string.Empty;
}