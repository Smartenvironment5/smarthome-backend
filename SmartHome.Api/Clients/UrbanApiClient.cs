namespace SmartHome.Api.Clients;

public class UrbanApiClient(HttpClient http)
{
	public Task<UrbanData?> GetLatestAsync(string deviceId, CancellationToken ct = default)
		=> http.GetFromJsonAsync<UrbanData>($"devices/{deviceId}/latest", ct);
}