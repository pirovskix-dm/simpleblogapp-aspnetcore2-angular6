using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SimpleBlogApp.Core.Models;
using SimpleBlogApp.Initializer;
using System.Threading.Tasks;

namespace SimpleBlogApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var host = BuildWebHost(args);

			InitApp(host).Wait();

			host.Run();
		}

		public static IWebHost BuildWebHost(string[] args)
		{
			return WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>()
				.Build();
		}

		private static async Task InitApp(IWebHost host)
		{
			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
				var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
				await UserInitializer.InitializeAsync(userManager, rolesManager);
			}
		}
	}
}
