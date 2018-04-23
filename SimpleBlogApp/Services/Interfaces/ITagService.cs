using SimpleBlogApp.Core.Models;
using SimpleBlogApp.ViewModels.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleBlogApp.Services.Interfaces
{
	public interface ITagService
	{
		/// <summary>
		/// Ищет записи модели Tag в соответствии со списком имен. 
		/// Если какие то записи не нашлись, то создает новые.
		/// </summary>
		/// <param name="tags">Список имен для поиска записей модели Tag</param>
		/// <returns>Записи модели Tag в соответствии с запрошенным списком имен.</returns>
		Task<IEnumerable<Tag>> FindByNamesAndAddIfNotExists(IEnumerable<string> tags);
		/// <summary>
		/// Ищет определенное количество записей модели Tag в виде модели представления TagViewModel, 
		/// имя которых содержит определённую строку
		/// </summary>
		/// <param name="name">Строка для поиска записей</param>
		/// <param name="numOfRecords">Необходимое количество записей</param>
		/// <returns>Записи модели Tag в виде модели представления TagViewModel</returns>
		Task<IEnumerable<TagViewModel>> FindFirsTagsLike(string name, int numOfRecords);
	}
}
