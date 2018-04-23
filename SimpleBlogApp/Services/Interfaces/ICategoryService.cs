using SimpleBlogApp.Core.Models;
using SimpleBlogApp.ViewModels.SaveViewModels;
using SimpleBlogApp.ViewModels.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleBlogApp.Services.Interfaces
{
	/// <summary>
	/// Содержит методы для обслуживания модели Category
	/// </summary>
	public interface ICategoryService
	{
		/// <summary>
		/// Читает все записи модели Category из БД в виде модели представления CategoryViewModel.
		/// </summary>
		/// <returns>Все записи из БД модели Post в виде PostViewModel</returns>
		Task<IEnumerable<CategoryViewModel>> GetAllViewModelsAsync();
		/// <summary>
		/// Если БД не содержит данную модель Category, 
		/// то создает новую в соответствии с моделью представления SaveCategoryViewModel.
		/// </summary>
		/// <param name="saveCategory">Модель представления SaveCategoryViewModel, содержащая данные для создания модели Category</param>
		/// <returns>Модель, которая будет создана в БД после вызова SaveAsync</returns>
		Task<Category> AddIfNotExists(SaveCategoryViewModel saveCategory);
		/// <summary>
		/// Помечает запись как удаленную.
		/// </summary>
		/// <param name="id">Id записи модели Category</param>
		void Remove(int id);
	}
}
