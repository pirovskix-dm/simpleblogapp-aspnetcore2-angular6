using SimpleBlogApp.Core.Models;
using SimpleBlogApp.Core.Query;
using SimpleBlogApp.ViewModels.QueryViewModels;
using SimpleBlogApp.ViewModels.SaveViewModels;
using SimpleBlogApp.ViewModels.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleBlogApp.Services.Interfaces
{
	/// <summary>
	/// Содержит методы для обслуживания модели Post
	/// </summary>
	public interface IPostService
	{
		/// <summary>
		/// Читает все записи модели Post из БД в виде модели представления PostViewModel.
		/// </summary>
		/// <returns>Все записи из БД модели Post в виде модели представления PostViewModel</returns>
		Task<IEnumerable<PostViewModel>> GetAllViewModelsAsync();
		/// <summary>
		/// Читает выборку записей модели Post из БД в виде модели представления PostViewModel.
		/// Модель представления содержит только поля необходимы для отображения блога.
		/// </summary>
		/// <param name="queryViewModel">Запрос для фильтрации записей</param>
		/// <returns>Выборка записей из БД модели Post в виде модели представления PostViewModel</returns>
		Task<QueryResult<PostViewModel>> GetBlogViewModels(PostQueryViewModel queryViewModel);
		/// <summary>
		/// Читает выборку записей модели Post из БД в виде модели представления PostViewModel.
		/// Модель представления содержит только поля необходимы для администрирования блога.
		/// </summary>
		/// <param name="queryViewModel">Запрос для фильтрации записей</param>
		/// <returns>Выборка записей из БД модели Post в виде модели представления PostViewModel</returns>
		Task<QueryResult<PostViewModel>> GetAdminViewModels(PostQueryViewModel queryViewModel);
		/// <summary>
		/// Читает определенную запись модели Post из БД в виде модели представления PostViewModel.
		/// Модель представления содержит только поля необходимы для отображения статьи.
		/// </summary>
		/// <param name="id">Id записи модели Post</param>
		/// <returns>Запись модели Post в виде модели представления PostViewModel</returns>
		Task<PostViewModel> GetPostViewModel(int id);
		/// <summary>
		/// Создает модели Post в соответствии с моделью представления SavePostViewModel и списком тегов.
		/// </summary>
		/// <param name="savePost">Модель представления SavePostViewModel, содержащая данные для создания модели Post</param>
		/// <param name="tags">Список тегов для добавления в модель Post</param>
		/// <returns>Модель, которая будет создана в БД после вызова SaveAsync</returns>
		Post AddPost(SavePostViewModel savePost, IEnumerable<Tag> tags);
		/// <summary>
		/// Изменяет модель Post в соответствии с моделью представления SavePostViewModel
		/// и списком тегов.
		/// </summary>
		/// <param name="savePost">Модель представления SavePostViewModel, содержащая данные для изменения модели Post</param>
		/// <param name="tags">Новый список тегов для добавления в модель Post</param>
		/// <param name="id">Id записи модели Post</param>
		/// <returns>Модель, которая будет записана в БД после вызова SaveAsync</returns>
		Task<Post> UpdatePost(int id, SavePostViewModel savePost, IEnumerable<Tag> tags);
		/// <summary>
		/// Изменяет данные модели Post если предоставлен идентификатор записи, 
		/// иначе создает новую в соответствии с моделью представления SavePostViewModel.
		/// </summary>
		/// <param name="savePost">Модель представления SavePostViewModel, содержащая данные для изменения или создания модели Post</param>
		/// <param name="tags">Новый список тегов для модели Post</param>
		/// <param name="id">nullable Id записи модели Post</param>
		/// <returns>Модель, которая будет записана или изменена в БД после вызова SaveAsync</returns>
		Task<Post> UpdateOrAddPostIfIdIsNull(int? id, SavePostViewModel savePost, IEnumerable<Tag> tags);
		/// <summary>
		/// Помечает запись как удаленную.
		/// </summary>
		/// <param name="id">Id записи модели Post</param>
		void Remove(int id);
	}
}
