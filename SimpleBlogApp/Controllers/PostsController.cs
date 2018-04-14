using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleBlogApp.Core.Query;
using SimpleBlogApp.Services.Interfaces;
using SimpleBlogApp.ViewModels.QueryViewModels;
using SimpleBlogApp.ViewModels.SaveViewModels;
using SimpleBlogApp.ViewModels.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleBlogApp.Controllers
{
	[Produces("application/json")]
	[Route("api/Posts")]
	public class PostsController : Controller
	{
		private readonly IUnitOfWorkService unitOfWork;
		private readonly IPostService postService;
		private readonly ITagService tagService;

		public PostsController(IUnitOfWorkService unitOfWork, IPostService postService, ITagService tagService)
		{
			this.unitOfWork = unitOfWork;
			this.postService = postService;
			this.tagService = tagService;
		}

		[HttpGet]
		public async Task<IEnumerable<PostViewModel>> GetPosts()
		{
			return await postService.GetAllViewModelsAsync();
		}

		[HttpGet("query")]
		public async Task<QueryResult<PostViewModel>> GetPostsQuery(PostQueryViewModel query)
		{
			return await postService.GetBlogViewModels(query);
		}

		[HttpGet("admin")]
		public async Task<QueryResult<PostViewModel>> GetAdminQuery(PostQueryViewModel query)
		{
			return await postService.GetAdminViewModels(query);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetPost([FromRoute] int id)
		{
			var postViewModel = await postService.GetPostViewModel(id);
			if (postViewModel == null)
				return NotFound();
			return Ok(postViewModel);
		}

		[HttpPut("{id}")]
		[Authorize]
		public async Task<IActionResult> UpdatePost([FromRoute] int id, [FromBody] SavePostViewModel savePost)
		{
			return await UpdateOrCreate(id, savePost);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> CreatePost([FromBody] SavePostViewModel savePost)
		{
			return await UpdateOrCreate(null, savePost);
		}

		[HttpDelete("{id}")]
		[Authorize]
		public async Task<IActionResult> DeletePost([FromRoute] int id)
		{
			postService.Remove(id);

			if (!await unitOfWork.TrySaveChangesAsync())
				return StatusCode(StatusCodes.Status500InternalServerError);

			return Ok(id);
		}

		private async Task<IActionResult> UpdateOrCreate(int? id, SavePostViewModel savePost)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var tags = await tagService.FindByNamesAndAddIfNotExists(savePost.Tags);
			var post = await postService.UpdateOrAddPostIfIdIsNull(id, savePost, tags);

			if (post == null)
				return NotFound();

			if (!await unitOfWork.TrySaveChangesAsync())
				return StatusCode(StatusCodes.Status500InternalServerError);

			return Ok(post.Id);
		}
	}
}