using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBlogApp.Core.Models
{
	[Table("Category")]
	public class Category : ModelState
	{
		public int Id { get; set; }

		[Required]
		[StringLength(100)]
		public string Name { get; set; }

		public ICollection<Post> Posts { get; set; }

		public Category()
		{
			Posts = new Collection<Post>();
		}
	}
}