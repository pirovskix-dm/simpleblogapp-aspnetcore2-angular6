using AutoMapper;
using SimpleBlogApp.Core.Interfaces;
using SimpleBlogApp.Core.Models;
using SimpleBlogApp.Extensions;
using SimpleBlogApp.Services.Interfaces;
using SimpleBlogApp.ViewModels.SaveViewModels;
using SimpleBlogApp.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleBlogApp.Services
{
	public class CategoryService : ICategoryService
	{
		private readonly ICategoryRepository categoryRepository;
		private readonly IMapper mapper;

		public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
		{
			this.categoryRepository = categoryRepository;
			this.mapper = mapper;
		}

		public async Task<IEnumerable<CategoryViewModel>> GetAllViewModelsAsync()
		{
			return await categoryRepository.GetAllAsync(c => new CategoryViewModel()
			{
				Id = c.Id,
				Name = c.Name
			});
		}

		public async Task<Category> AddIfNotExists(SaveCategoryViewModel saveCategory)
		{
			saveCategory.NotNull();

			if (await categoryRepository.IsExistAsync(saveCategory.Name))
				return null;

			var dateCreated = DateTime.Now;
			var category = mapper.Map<SaveCategoryViewModel, Category>(saveCategory);
			category.IsActive = true;
			category.DateCreated = dateCreated;
			category.DateLastUpdated = dateCreated;

			categoryRepository.Add(category);

			return category;
		}

		public void Remove(int id)
		{
			categoryRepository.Remove(new Category() { Id = id });
		}
	}
}
