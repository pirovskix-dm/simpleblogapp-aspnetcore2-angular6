using SimpleBlogApp.Core.Models;
using SimpleBlogApp.Core.Query;
using SimpleBlogApp.ViewModels.QueryViewModels;
using SimpleBlogApp.ViewModels.SaveViewModels;
using SimpleBlogApp.ViewModels.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleBlogApp.Services.Interfaces
{
	public interface IPostService
	{
		Task<IEnumerable<PostViewModel>> GetAllViewModelsAsync();
		Task<QueryResult<PostViewModel>> GetBlogViewModels(PostQueryViewModel queryViewModel);
		Task<QueryResult<PostViewModel>> GetAdminViewModels(PostQueryViewModel queryViewModel);
		Task<PostViewModel> GetPostViewModel(int id);
		Post AddPost(SavePostViewModel savePost, IEnumerable<Tag> tags);
		Task<Post> UpdatePost(int id, SavePostViewModel savePost, IEnumerable<Tag> tags);
		Task<Post> UpdateOrAddPostIfIdIsNull(int? id, SavePostViewModel savePost, IEnumerable<Tag> tags);
		void Remove(int id);
	}
}
