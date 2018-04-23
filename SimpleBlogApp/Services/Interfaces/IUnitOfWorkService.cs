using System.Threading.Tasks;

namespace SimpleBlogApp.Services.Interfaces
{
	public interface IUnitOfWorkService
	{
		/// <summary>
		/// Сохраняет все изменения, запланированные другими сервисами.
		/// </summary>
		/// <returns>ture eсли сохранение прошло успешно, иначе false</returns>
		Task<bool> TrySaveChangesAsync();
	}
}
