using System.Threading.Tasks;

namespace SimpleBlogApp.Core.Interfaces
{
	public interface IUnitOfWork
	{
		Task SaveAsync();
	}
}
