using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleBlogApp.EntityFrameworkCore;
using Xunit.Abstractions;

namespace SimpleBlogApp.IntegrationTests.EntityFrameworkCore.Repositories
{
	public class RepositoryTests
	{
		protected readonly ITestOutputHelper output;
		protected readonly SimpleBlogAppDbContext context;

		public RepositoryTests(ITestOutputHelper output)
		{
			this.output = output;
			context = GetContext();
		}

		public void Dispose()
		{
			context.Dispose();
		}

		private SimpleBlogAppDbContext GetContext()
		{
			var serviceProvider = new ServiceCollection()
				.AddEntityFrameworkInMemoryDatabase()
				.BuildServiceProvider();

			var builder = new DbContextOptionsBuilder<SimpleBlogAppDbContext>()
				.UseInMemoryDatabase("RepositoryTestDbContext")
				.UseInternalServiceProvider(serviceProvider);

			SimpleBlogAppDbContext simpleBlogAppDbContext = new SimpleBlogAppDbContext(builder.Options);
			simpleBlogAppDbContext.Database.EnsureDeleted();
			simpleBlogAppDbContext.Database.EnsureCreated();
			return simpleBlogAppDbContext;
		}
	}
}
