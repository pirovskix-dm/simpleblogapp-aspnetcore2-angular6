using SimpleBlogApp.Core.Models;
using System.Threading.Tasks;

namespace SimpleBlogApp.Core.Interfaces
{
	public interface ICategoryRepository : IRepository<Category>
	{
		/// <summary>
		/// Проверяет существует ли определенная запись модели Category в БД.
		/// </summary>
		/// <param name="name">Строка для поиска записи</param>
		/// <returns>ture eсли запись существует, иначе false</returns>
		Task<bool> IsExistAsync(string name);
	}
}
