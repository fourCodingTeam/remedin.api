using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Remedin.Application.Interfaces;
using Remedin.Domain.Interfaces;
using Remedin.Infrastructure.Auth;
using Remedin.Infrastructure.Persistence;

namespace Remedin.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<RemedinDbContext>(options =>
        options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

        services.AddSingleton(sp =>
        {
            var url = config["Supabase:Url"];
            var key = config["Supabase:ServiceRoleKey"]; 

            var options = new Supabase.Gotrue.ClientOptions
            {
                Url = $"{url}/auth/v1",
                Headers = new Dictionary<string, string> { { "apikey", key! } }
            };

            return new Supabase.Gotrue.AdminClient(options.ToString());
        });


        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IMedicineRepository, MedicineRepository>(); 
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IScheduleRepository, ScheduleRepository>();
        services.AddScoped<IAdminAuthService, SupabaseAdminService>();

        return services;
    }
}
