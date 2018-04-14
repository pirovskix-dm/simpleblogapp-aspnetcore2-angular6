using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimpleBlogApp.Core.Models;

namespace SimpleBlogApp.EntityFrameworkCore
{
	public class SimpleBlogAppDbContext : IdentityDbContext<ApplicationUser>
	{
		public DbSet<Post> Posts { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Tag> Tags { get; set; }
		public DbSet<PostTag> PostTags { get; set; }

		public SimpleBlogAppDbContext(DbContextOptions<SimpleBlogAppDbContext> options)
			: base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder
				.Entity<PostTag>()
				.HasKey(pt => new { pt.PostId, pt.TagId });
		}
	}
}