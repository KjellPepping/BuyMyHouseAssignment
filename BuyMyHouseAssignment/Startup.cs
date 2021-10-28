using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service;
using DAL;

namespace Kennemerland.Startup
{
	public class Program
	{
		static void Configure(HostBuilderContext Builder, IServiceCollection Services)
		{
			Services.AddSingleton<IHouseService, HouseService>();
			Services.AddSingleton<IUserService, UserService>();
			Services.AddSingleton<IUserRepository, UserRepository>();
			Services.AddSingleton<IHouseRepository, HouseRepository>();
		}
	}
}


