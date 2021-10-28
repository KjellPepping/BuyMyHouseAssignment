using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service;
using DAL;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;

namespace Kennemerland.Startup
{
	public class Program : FunctionsStartup
	{ 
        public override void Configure(IFunctionsHostBuilder builder)
        {
			builder.Services.AddHttpContextAccessor();
			builder.Services.AddSingleton<IHouseRepository, HouseRepository>();
			builder.Services.AddSingleton<IUserRepository, UserRepository>();
			builder.Services.AddSingleton<IHouseService, HouseService>();
			builder.Services.AddSingleton<IUserService, UserService>();
		}
    }
}


