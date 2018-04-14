using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimpleBlogApp.Core.Models;
using SimpleBlogApp.ViewModels.SaveViewModels;
using System.Threading.Tasks;

namespace SimpleBlogApp.Controllers
{
	[Produces("application/json")]
	[Route("api/Account")]
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;

		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
		}

		[HttpGet("signed")]
		public bool IsSignedIn()
		{
			//userManager.GetUserAsync(HttpContext.User);
			return signInManager.IsSignedIn(HttpContext.User);
		}

		[HttpPost("Login")]
		public async Task<IActionResult> Login([FromBody] SaveLoginViewModel loginModel)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await signInManager.PasswordSignInAsync(loginModel.Name, loginModel.Password, false, false);
			if (result.Succeeded)
			{
				return Ok();
			}
			else
			{
				ModelState.AddModelError("", "The username or password is incorrect.");
				return BadRequest(ModelState);
			}
		}

		[HttpPost("Logoff")]
		public async Task<IActionResult> LogOff()
		{
			await signInManager.SignOutAsync();
			return Ok();
		}
	}
}
