using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBlogApp.Core.Models
{
	[Table("Tag")]
	public class Tag : ModelState
	{
		public int Id { get; set; }

		[Required]
		[StringLength(100)]
		public string Name { get; set; }

		public ICollection<PostTag> Posts { get; set; }

		public Tag()
		{
			Posts = new Collection<PostTag>();
		}
	}
}
