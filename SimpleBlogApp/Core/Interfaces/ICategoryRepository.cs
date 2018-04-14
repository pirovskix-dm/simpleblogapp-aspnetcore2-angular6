using SimpleBlogApp.Core.Models;
using System.Threading.Tasks;

namespace SimpleBlogApp.Core.Interfaces
{
	public interface ICategoryRepository : IRepository<Category>
	{
		Task<bool> IsExistAsync(string name);
	}
}
