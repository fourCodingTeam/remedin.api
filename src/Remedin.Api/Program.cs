using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var supabaseProjectRef = configuration["SUPABASE_PROJECT_REF"]
    ?? throw new InvalidOperationException("SUPABASE_PROJECT_REF missing");

var supabaseUrl = $"https://{supabaseProjectRef}.supabase.co";
var jwksUrl = $"{supabaseUrl}/auth/v1/.well-known/jwks.json";
var validIssuer = supabaseUrl;
var validAudience = "authenticated";

builder.Services.AddRouting();
builder.Services.AddAuthorization();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            ValidateIssuerSigningKey = true,
            IssuerSigningKeyResolver = (token, securityToken, kid, validationParameters) =>
            {
                using var client = new HttpClient();
                var json = client.GetStringAsync(jwksUrl).Result;
                var keys = new JsonWebKeySet(json).Keys;
                return keys;
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
