using SimpleBlogApp.Core.Interfaces;
using System.Threading.Tasks;

namespace SimpleBlogApp.EntityFrameworkCore.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly SimpleBlogAppDbContext _context;

		public UnitOfWork(SimpleBlogAppDbContext context)
		{
			_context = context;
		}

		public async Task SaveAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
