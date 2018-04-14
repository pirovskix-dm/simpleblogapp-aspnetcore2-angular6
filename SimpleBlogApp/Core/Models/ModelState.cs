using System;

namespace SimpleBlogApp.Core.Models
{
	public class ModelState
	{
		public bool IsActive { get; set; }
		public DateTime? DateCreated { get; set; }
		public DateTime? DateLastUpdated { get; set; }
	}
}
