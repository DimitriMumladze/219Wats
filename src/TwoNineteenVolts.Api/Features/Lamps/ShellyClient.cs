using System.Text.Json;
using Microsoft.Extensions.Options;
using TwoNineteenVolts.Api.Common;

namespace TwoNineteenVolts.Api.Features.Lamps;

public class ShellyClient(HttpClient http, IOptions<ShellyOptions> options, ILogger<ShellyClient> logger)
{
    private const int SwitchChannel = 0;
    private static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(2);

    public async Task<Result<LampStatus>> GetStatusAsync()
    {
        var ip = options.Value.LocalIp;
        var url = $"http://{ip}/rpc/Switch.GetStatus?id={SwitchChannel}";

        try
        {
            using var timeout = new CancellationTokenSource(RequestTimeout);
            var response = await http.GetFromJsonAsync<JsonElement>(url, timeout.Token);
            var isOn = response.GetProperty("output").GetBoolean();
            return Result<LampStatus>.Ok(new LampStatus(isOn));
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            logger.LogWarning(ex, "Lamp unreachable at {Ip}", ip);
            return Result<LampStatus>.Fail("Lamp unreachable");
        }
    }
}
