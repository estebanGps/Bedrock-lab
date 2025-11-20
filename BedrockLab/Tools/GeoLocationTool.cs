namespace BedrockLab.Tools;

public class GeoLocationTool
{
    [BedrockTool("calculate_distance", "Calculate the distance between two geographical points using the Haversine formula.")]
    public async Task<double> CalculateDistance(
        [BedrockToolParam("latitude_1", "Latitude of the first point in decimal degrees.")] double latitude1,
        [BedrockToolParam("longitude_1", "Longitude of the first point in decimal degrees.")] double longitude1,
        [BedrockToolParam("latitude_2", "Latitude of the second point in decimal degrees.")] double latitude2,
        [BedrockToolParam("longitude_2", "Longitude of the second point in decimal degrees.")] double longitude2)
    {
        const double R = 6371e3; // Earth's radius in meters
        double lat1Rad = ToRadians(latitude1);
        double lat2Rad = ToRadians(latitude2);
        double deltaLatRad = ToRadians(latitude2 - latitude1);
        double deltaLonRad = ToRadians(longitude2 - longitude1);
        double a = Math.Sin(deltaLatRad / 2) * Math.Sin(deltaLatRad / 2) +
                   Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                   Math.Sin(deltaLonRad / 2) * Math.Sin(deltaLonRad / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        double distance = R * c; // in meters
        return await Task.FromResult(distance);
    }
    private static double ToRadians(double degrees)
    {
        return degrees * (Math.PI / 180);
    }
}