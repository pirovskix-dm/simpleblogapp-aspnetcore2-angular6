using System.ComponentModel.DataAnnotations;

namespace SimpleBlogApp.ViewModels.SaveViewModels
{
	public class SaveCategoryViewModel
	{
		[Required]
		[StringLength(100)]
		public string Name { get; set; }
	}
}
