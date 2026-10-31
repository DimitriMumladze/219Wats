namespace TwoNineteenVolts.Api.Features.Lamps;

public static class LampEndpoints
{
    public static void MapLampEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/lamps");

        group.MapGet("/status", GetStatusAsync);
    }

    private static async Task<IResult> GetStatusAsync(ShellyClient shelly)
    {
        var result = await shelly.GetStatusAsync();
        if (result.IsFailure)
        {
            return TypedResults.Problem(title: result.Error, statusCode: StatusCodes.Status503ServiceUnavailable);
        }

        return TypedResults.Ok(result.Value);
    }
}
