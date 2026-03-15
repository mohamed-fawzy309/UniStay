using Microsoft.AspNetCore.Hosting;
using System.Text.Json;
using System.Text.Json.Serialization;

public class Governorate
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name_ar")]
    public string NameAr { get; set; } = null!;

    [JsonPropertyName("name_en")]
    public string NameEn { get; set; } = null!;

    [JsonPropertyName("code")]
    public string Code { get; set; } = null!;

    [JsonPropertyName("centers")]
    public List<Center> Centers { get; set; } = new();
}

public class Center
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name_ar")]
    public string NameAr { get; set; } = null!;

    [JsonPropertyName("name_en")]
    public string NameEn { get; set; } = null!;

    [JsonPropertyName("code")]
    public string Code { get; set; } = null!;

    [JsonPropertyName("cities")]
    public List<City> Cities { get; set; } = new();
}

public class City
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name_ar")]
    public string NameAr { get; set; } = null!;

    [JsonPropertyName("name_en")]
    public string NameEn { get; set; } = null!;

    [JsonPropertyName("code")]
    public string Code { get; set; } = null!;
}

public class LocationService
{
    private readonly List<Governorate> _governorates;

    public LocationService(IWebHostEnvironment env)
    {
        var path = Path.Combine(env.WebRootPath, "data", "egypt_locations.json");
        var json = File.ReadAllText(path);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        _governorates = JsonSerializer.Deserialize<List<Governorate>>(json, options) ?? new();
    }

    public List<Governorate> GetGovernorates() => _governorates;

    public List<Center> GetCenters(int governorateId) =>
        _governorates.FirstOrDefault(g => g.Id == governorateId)?.Centers ?? new();

    public List<City> GetCities(int centerId)
    {
        foreach (var gov in _governorates)
        {
            var center = gov.Centers.FirstOrDefault(c => c.Id == centerId);
            if (center != null) return center.Cities;
        }
        return new();
    }
}