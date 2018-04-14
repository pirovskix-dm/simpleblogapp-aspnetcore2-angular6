using AutoMapper;
using SimpleBlogApp.Core.Models;
using SimpleBlogApp.Core.Query.Models;
using SimpleBlogApp.ViewModels.QueryViewModels;
using SimpleBlogApp.ViewModels.SaveViewModels;
using SimpleBlogApp.ViewModels.ViewModels;
using System.Linq;

namespace SimpleBlogApp.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMapPost();
			CreateMapCategory();
			CreateMap<PostQueryViewModel, PostQuery>();
			CreateMap<Tag, TagViewModel>();
		}

		private void CreateMapPost()
		{
			CreateMap<Post, PostViewModel>()
				.ForMember(pvm => pvm.Tags, opt => opt.MapFrom(p => p.Tags.Select(t => new TagViewModel() { Id = t.Tag.Id, Name = t.Tag.Name })));

			CreateMap<SavePostViewModel, Post>()
				.ForMember(v => v.Id, opt => opt.Ignore())
				.ForMember(v => v.IsActive, opt => opt.Ignore())
				.ForMember(v => v.DateCreated, opt => opt.Ignore())
				.ForMember(v => v.DateLastUpdated, opt => opt.Ignore())
				.ForMember(v => v.Category, opt => opt.Ignore())
				.ForMember(v => v.Tags, opt => opt.Ignore())
				;
		}

		private void CreateMapCategory()
		{
			CreateMap<Category, CategoryViewModel>();
			CreateMap<SaveCategoryViewModel, Category>()
				.ForMember(v => v.Id, opt => opt.Ignore())
				.ForMember(v => v.IsActive, opt => opt.Ignore())
				.ForMember(v => v.DateCreated, opt => opt.Ignore())
				.ForMember(v => v.DateLastUpdated, opt => opt.Ignore())
				.ForMember(v => v.Posts, opt => opt.Ignore())
				;
		}
	}
}
