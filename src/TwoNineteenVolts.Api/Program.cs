using Microsoft.Extensions.Options;
using Scalar.AspNetCore;
using TwoNineteenVolts.Api.Features.Devices;
using TwoNineteenVolts.Api.Features.Devices.ApiKey;
using TwoNineteenVolts.Api.Features.Devices.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options => options.AddDocumentTransformer<ApiKeyOpenApiTransformer>());
builder.Services.AddProblemDetails();
builder.Services.AddDevices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    var localApiKey = app.Services.GetRequiredService<IOptions<TwoNineteenVolts.Api.Features.Devices.ApiKey.ApiKeyOptions>>().Value.Key;
    app.MapScalarApiReference(options => options
        .AddPreferredSecuritySchemes(ApiKeyOpenApiTransformer.SchemeName)
        .AddApiKeyAuthentication(ApiKeyOpenApiTransformer.SchemeName, apiKey => apiKey.Value = localApiKey));
}

app.UseExceptionHandler();
app.UseHttpsRedirection();

app.MapDeviceEndpoints();

app.Run();
