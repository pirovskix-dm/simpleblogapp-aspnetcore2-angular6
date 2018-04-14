using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace SimpleBlogApp.ViewModels.SaveViewModels
{
	public class SavePostViewModel
	{
		[Required]
		[StringLength(100)]
		public string Title { get; set; }

		[Required]
		public string Content { get; set; }

		[StringLength(500)]
		public string ShortContent { get; set; }

		[Required]
		public int CategoryId { get; set; }

		[Required]
		public ICollection<string> Tags { get; set; }

		public SavePostViewModel()
		{
			Tags = new Collection<string>();
		}
	}
}
