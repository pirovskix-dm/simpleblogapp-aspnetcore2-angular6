namespace SimpleBlogApp.ViewModels.QueryViewModels
{
	public class PostQueryViewModel
	{
		public int? CategoryId { get; set; }
		public int? TagId { get; set; }
		public string Search { get; set; }

		public string SortBy { get; set; }
		public bool IsSortAscending { get; set; }
		public int Page { get; set; }
		public int PageSize { get; set; }
	}
}
