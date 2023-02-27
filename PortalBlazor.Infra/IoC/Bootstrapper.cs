using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PortalBlazor.Core.Services.JobJourney;
using PortalBlazor.Infra.Data;
using PortalBlazor.Infra.Data.Repository;

namespace PortalBlazor.Infra.IoC
{
    public static class Bootstrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            //service
            services.AddScoped<IJobJourneyService, JobJourneyService>();

            //data
            services.AddScoped<DataContext>();
            services.AddScoped<IUserRepository, UserRepository>();

            //Identity
            services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<DataContext>()
                    .AddDefaultTokenProviders();
        }
    }
}