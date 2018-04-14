using System.ComponentModel.DataAnnotations;

namespace SimpleBlogApp.ViewModels.SaveViewModels
{
	public class SaveLoginViewModel
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public string Password { get; set; }
	}
}