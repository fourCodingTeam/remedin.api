using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var supabaseProjectRef = configuration["SUPABASE_PROJECT_REF"] ?? throw new InvalidOperationException("SUPABASE_PROJECT_REF missing");
var supabaseUrl = $"https://{supabaseProjectRef}.supabase.co";
var jwksUrl = $"{supabaseUrl}/auth/v1/.well-known/jwks.json";
var validIssuer = supabaseUrl;
var validAudience = "authenticated";

builder.Services.AddRouting();
builder.Services.AddAuthorization();
builder.Services.AddMemoryCache();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = validIssuer,
        ValidateAudience = true,
        ValidAudience = validAudience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(2),
        ValidateIssuerSigningKey = true
    };

    var httpClient = new HttpClient();

    var memoryCache = builder.Services.BuildServiceProvider().GetRequiredService<IMemoryCache>();

    options.ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
        $"{jwksUrl}",
        new OpenIdConnectConfigurationRetriever(),
        new HttpDocumentRetriever(httpClient)
        {
            RequireHttps = true
        }
    );

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = ctx => Task.CompletedTask,
        OnAuthenticationFailed = ctx =>
        {
            return Task.CompletedTask;
        },
        OnTokenValidated = ctx =>
        {
            return Task.CompletedTask;
        }
    };
});

var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Exemplo de endpoint público
app.MapGet("/", () => Results.Ok(new { msg = "Remedin API up" }));

// Endpoint protegido
app.MapGet("/me", (ClaimsPrincipal user) =>
{
    var sub = user.FindFirst("sub")?.Value;
    var email = user.FindFirst("email")?.Value;
    return Results.Ok(new { sub, email });
}).RequireAuthorization();

app.Run();
