using Microsoft.AspNetCore.Mvc;
using SimpleBlogApp.Services.Interfaces;
using SimpleBlogApp.ViewModels.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleBlogApp.Controllers
{
	[Produces("application/json")]
	[Route("api/Tags")]
	public class TagsController
	{
		private readonly ITagService tagService;

		public TagsController(ITagService tagService)
		{
			this.tagService = tagService;
		}

		[HttpGet]
		public async Task<IEnumerable<TagViewModel>> GetTags(string name, int records)
		{
			return await tagService.FindFirsTagsLike(name, records);
		}
	}
}