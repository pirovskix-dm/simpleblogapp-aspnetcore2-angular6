using Microsoft.AspNetCore.Identity;
using SimpleBlogApp.Core.Models;
using System.Threading.Tasks;

namespace SimpleBlogApp.Initializer
{
	public class UserInitializer
	{
		public static async Task InitializeAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			string adminEmail = "admin";
			string password = "admin";

			if (await userManager.FindByNameAsync(adminEmail) == null)
			{
				ApplicationUser admin = new ApplicationUser { Email = adminEmail, UserName = adminEmail };
				IdentityResult result = await userManager.CreateAsync(admin, password);
			}
		}
	}
}
