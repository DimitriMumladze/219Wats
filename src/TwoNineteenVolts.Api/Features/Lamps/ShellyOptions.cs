using System.ComponentModel.DataAnnotations;

namespace TwoNineteenVolts.Api.Features.Lamps;

public class ShellyOptions
{
    public const string SectionName = "Shelly";

    [Required]
    public string LocalIp { get; set; } = "";
}
