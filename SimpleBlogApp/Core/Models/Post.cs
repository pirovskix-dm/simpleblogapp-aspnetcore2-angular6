using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBlogApp.Core.Models
{
	[Table("Post")]
	public class Post : ModelState
	{
		public int Id { get; set; }

		[Required]
		[StringLength(100)]
		public string Title { get; set; }

		[Required]
		public string Content { get; set; }

		[Required]
		[StringLength(500)]
		public string ShortContent { get; set; }

		[Required]
		public int CategoryId { get; set; }
		public Category Category { get; set; }

		public ICollection<PostTag> Tags { get; set; }

		public Post()
		{
			Tags = new Collection<PostTag>();
		}
	}
}