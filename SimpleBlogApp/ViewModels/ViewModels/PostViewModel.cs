using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SimpleBlogApp.ViewModels.ViewModels
{
	public class PostViewModel
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public string ShortContent { get; set; }
		public CategoryViewModel Category { get; set; }
		public DateTime? DateCreated { get; set; }
		public ICollection<TagViewModel> Tags { get; set; }

		public PostViewModel()
		{
			Tags = new Collection<TagViewModel>();
		}
	}
}
