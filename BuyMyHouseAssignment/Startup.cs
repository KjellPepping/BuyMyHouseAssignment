using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Service;
using DAL;

[assembly: FunctionsStartup(typeof(Kennemerland.Startup.Program))]

namespace Kennemerland.Startup
{
    public class Program : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.Register();
        }
    }

    public static class Injector
    {
        public static void Register(this IServiceCollection services)
        {
            services.AddTransient<IOfferService, OfferService>();
            services.AddTransient<IOfferRepository, OfferRepository>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IHouseService, HouseService>();
            services.AddTransient<IHouseRepository, HouseRepository>();

        }
    }
}


