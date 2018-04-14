using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleBlogApp.Services.Interfaces;
using SimpleBlogApp.ViewModels.SaveViewModels;
using SimpleBlogApp.ViewModels.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleBlogApp.Controllers
{
	[Produces("application/json")]
	[Route("api/Categories")]
	public class CategoriesController : Controller
	{
		private readonly IMapper mapper;
		private readonly IUnitOfWorkService unitOfWork;
		private readonly ICategoryService categoryService;

		public CategoriesController(IMapper mapper, IUnitOfWorkService unitOfWork, ICategoryService categoryService)
		{
			this.mapper = mapper;
			this.unitOfWork = unitOfWork;
			this.categoryService = categoryService;
		}

		[HttpGet]
		public async Task<IEnumerable<CategoryViewModel>> GetCategories()
		{
			return await categoryService.GetAllViewModelsAsync();
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> CreateCategory([FromBody] SaveCategoryViewModel saveCategory)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var category = await categoryService.AddIfNotExists(saveCategory);
			if (category == null)
			{
				ModelState.AddModelError("", $"{saveCategory.Name} category already exists");
				return BadRequest(ModelState);
			}

			if (!await unitOfWork.TrySaveChangesAsync())
				return StatusCode(StatusCodes.Status500InternalServerError);

			return Ok(category.Id);
		}

		[HttpDelete("{id}")]
		[Authorize]
		public async Task<IActionResult> DeleteCategory([FromRoute] int id)
		{
			categoryService.Remove(id);

			if (!await unitOfWork.TrySaveChangesAsync())
				return StatusCode(StatusCodes.Status500InternalServerError);

			return Ok(id);
		}
	}
}
