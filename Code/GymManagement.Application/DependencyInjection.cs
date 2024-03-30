using Microsoft.Extensions.DependencyInjection;

namespace GymManagement.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            //Register services
            //services.AddScoped<ISubscriptionsWriteService, SubscriptionsWriteService>();

            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection));
            });

            return services;
        }
    }
}