using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using SimpleBlogApp.Core.Interfaces;
using SimpleBlogApp.Core.Models;
using SimpleBlogApp.EntityFrameworkCore;
using SimpleBlogApp.EntityFrameworkCore.Repositories;
using SimpleBlogApp.Extensions.Logger;
using SimpleBlogApp.Services;
using SimpleBlogApp.Services.Interfaces;
using System.IO;

namespace SimpleBlogApp
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			RepositoryImplementation(services);
			ServiceImplementation(services);
			services.AddScoped<IUnitOfWork, UnitOfWork>();

			services.AddAutoMapper();

			services.AddDbContext<SimpleBlogAppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default")));

			//services.BuildServiceProvider().GetService<SimpleBlogAppDbContext>().Database.Migrate();

			services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<SimpleBlogAppDbContext>();

			//var policy = new AuthorizationPolicyBuilder()
			//	.RequireAuthenticatedUser()
			//	.Build();

			//services.AddMvc(opt => opt.Filters.Add(new AuthorizeFilter(policy)));
			services.AddMvc();
		}

		private void RepositoryImplementation(IServiceCollection services)
		{
			services.AddScoped<IPostRepository, PostRepository>();
			services.AddScoped<ICategoryRepository, CategoryRepository>();
			services.AddScoped<ITagRepository, TagRepository>();
		}

		private void ServiceImplementation(IServiceCollection services)
		{
			services.AddScoped<ITagService, TagService>();
			services.AddScoped<IPostService, PostService>();
			services.AddScoped<ICategoryService, CategoryService>();
			services.AddScoped<IUnitOfWorkService, UnitOfWorkService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			if (env.IsDevelopment())
			{
				loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));
				loggerFactory.CreateLogger<FileLogger>();

				app.UseDeveloperExceptionPage();
				app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
				{
					HotModuleReplacement = true
				});
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}


			var provider = new FileExtensionContentTypeProvider();
			app.UseStaticFiles(new StaticFileOptions()
			{
				FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot")),
				RequestPath = new PathString(""),
				ContentTypeProvider = provider,
				ServeUnknownFileTypes = true
			});

			app.UseAuthentication();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");

				routes.MapSpaFallbackRoute(
					name: "spa-fallback",
					defaults: new { controller = "Home", action = "Index" });
			});
		}
	}
}
