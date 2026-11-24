using System.Diagnostics;

using SmartHome.Api.Clients;

namespace SmartHome.Api;

public class UrbanApiPoller(UrbanApiClient client, ILogger<UrbanApiPoller> logger) : BackgroundService
{
	public UrbanData? Latest { get; private set; }

	protected override async Task ExecuteAsync(CancellationToken ct)
	{
		using var timer = new PeriodicTimer(TimeSpan.FromSeconds(30));
		do
		{
			try
			{
				Latest = await client.GetLatestAsync("urban-sense-rijswijk-01", ct);
				if (Latest != null) logger.LogInformation("Latest: {Data}", Latest);
			}
			catch (Exception ex) when (ex is not OperationCanceledException)
			{
				logger.LogWarning(ex, "Polling external API failed");
			}
		}
		while (await timer.WaitForNextTickAsync(ct));
	}
}

// {
// "deviceId": "urban-sense-rijswijk-01",
// "deviceName": "Meetpunt 01",
// "msgCount": 42,
// "timestamp": "2026-09-11T14:59:14.000Z",
// "temperature": 22.04,
// "temperatureUnit": "C",
// "pressure": 1021.32,
// "pressureUnit": "hPa",
// "humidity": 67.36,
// "humidityUnit": "percent"
// }

public record UrbanData(
	string deviceId,
	string deviceName,
	int msgCount,
	DateTime timestamp,
	double temperature,
	string temperatureUnit,
	double pressure,
	string pressureUnit,
	double humidity,
	String humidityUnit);