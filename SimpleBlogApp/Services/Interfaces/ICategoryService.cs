using SimpleBlogApp.Core.Models;
using SimpleBlogApp.ViewModels.SaveViewModels;
using SimpleBlogApp.ViewModels.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleBlogApp.Services.Interfaces
{
	public interface ICategoryService
	{
		Task<IEnumerable<CategoryViewModel>> GetAllViewModelsAsync();
		Task<Category> AddIfNotExists(SaveCategoryViewModel saveCategory);
		void Remove(int id);
	}
}
