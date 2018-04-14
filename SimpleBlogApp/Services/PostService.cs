using AutoMapper;
using SimpleBlogApp.Core.Interfaces;
using SimpleBlogApp.Core.Models;
using SimpleBlogApp.Core.Query;
using SimpleBlogApp.Core.Query.Models;
using SimpleBlogApp.Services.Interfaces;
using SimpleBlogApp.ViewModels.QueryViewModels;
using SimpleBlogApp.ViewModels.SaveViewModels;
using SimpleBlogApp.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SimpleBlogApp.Services
{
	public class PostService : IPostService
	{
		private readonly IPostRepository postRepository;
		private readonly IMapper mapper;

		private Expression<Func<IdObject<Tag>, IdObject<TagViewModel>>> tagExp;

		public PostService(IPostRepository postRepository, IMapper mapper)
		{
			this.postRepository = postRepository;
			this.mapper = mapper;

			tagExp = (IdObject<Tag> idTag) => new IdObject<TagViewModel>()
			{
				Id = idTag.Id,
				Object = new TagViewModel()
				{
					Id = idTag.Object.Id,
					Name = idTag.Object.Name
				}
			};
		}

		public async Task<IEnumerable<PostViewModel>> GetAllViewModelsAsync()
		{
			var tags = await postRepository.GetTagsAsync(tagExp);
			return await postRepository.GetAllAsync((Post p) => new PostViewModel()
			{
				Id = p.Id,
				Title = p.Title,
				Content = p.Content,
				ShortContent = p.ShortContent,
				Category = new CategoryViewModel()
				{
					Id = p.Category.Id,
					Name = p.Category.Name
				},
				DateCreated = p.DateCreated,
				Tags = tags.Where(pt => pt.Id == p.Id).Select(pt => pt.Object).ToList()
			});
		}

		public async Task<QueryResult<PostViewModel>> GetBlogViewModels(PostQueryViewModel queryViewModel)
		{
			var query = mapper.Map<PostQueryViewModel, PostQuery>(queryViewModel);
			var result = await postRepository.GetQueryResultAsync(query, (Post p) => new PostViewModel()
			{
				Id = p.Id,
				Title = p.Title,
				ShortContent = p.ShortContent,
				Category = new CategoryViewModel()
				{
					Name = p.Category.Name
				},
				DateCreated = p.DateCreated,
			});
			var tags = await postRepository.GetTagsAsync(result.Items.Select(p => p.Id), tagExp);
			foreach (var p in result.Items)
				p.Tags = tags.Where(t => t.Id == p.Id).Select(t => t.Object).ToList();

			return result;
		}

		public async Task<QueryResult<PostViewModel>> GetAdminViewModels(PostQueryViewModel queryViewModel)
		{
			var query = mapper.Map<PostQueryViewModel, PostQuery>(queryViewModel);
			var result = await postRepository.GetQueryResultAsync(query, (Post p) => new PostViewModel()
			{
				Id = p.Id,
				Title = p.Title,
				Category = new CategoryViewModel()
				{
					Name = p.Category.Name
				},
				DateCreated = p.DateCreated,
			});
			var tags = await postRepository.GetTagsAsync(result.Items.Select(p => p.Id), tagExp);
			foreach (var p in result.Items)
				p.Tags = tags.Where(t => t.Id == p.Id).Take(6).Select(t => t.Object).ToList();

			return result;
		}

		public async Task<PostViewModel> GetPostViewModel(int id)
		{
			var result = await postRepository.GetAsync(id, (Post p) => new PostViewModel()
			{
				Id = p.Id,
				Title = p.Title,
				Content = p.Content,
				ShortContent = p.ShortContent,
				Category = new CategoryViewModel()
				{
					Id = p.Category.Id,
					Name = p.Category.Name
				},
				DateCreated = p.DateCreated,
				Tags = p.Tags.Select(pt => new TagViewModel() { Id = pt.Tag.Id, Name = pt.Tag.Name }).ToList()
			});
			return result;
		}

		public async Task<Post> UpdateOrAddPostIfIdIsNull(int? id, SavePostViewModel savePost, IEnumerable<Tag> tags)
		{
			return id.HasValue ? await UpdatePost(id.Value, savePost, tags) : AddPost(savePost, tags);
		}

		public async Task<Post> UpdatePost(int id, SavePostViewModel savePost, IEnumerable<Tag> tags)
		{
			var post = await postRepository.GetAsync(id, p => p);

			if (post == null)
				return null;

			ValidateShortContent(savePost);

			mapper.Map<SavePostViewModel, Post>(savePost, post);
			post.DateLastUpdated = DateTime.Now;
			SetPostTags(post.Tags, tags);

			postRepository.Update(post);

			return post;
		}

		public Post AddPost(SavePostViewModel savePost, IEnumerable<Tag> tags)
		{
			ValidateShortContent(savePost);

			var post = mapper.Map<SavePostViewModel, Post>(savePost);
			post.IsActive = true;
			post.DateCreated = DateTime.Now;
			post.DateLastUpdated = DateTime.Now;
			SetPostTags(post.Tags, tags);

			postRepository.Add(post);

			return post;
		}

		public void Remove(int id)
		{
			postRepository.Remove(new Post() { Id = id });
		}

		private void SetPostTags(ICollection<PostTag> postTags, IEnumerable<Tag> tags)
		{
			if (tags == null || tags.Count() == 0)
			{
				postTags.Clear();
				return;
			}

			if (postTags == null || postTags.Count() == 0)
			{
				foreach (var t in tags)
					postTags.Add(new PostTag() { TagId = t.Id });
				return;
			}

			// Remove removed tags
			var removedTags = postTags
				.Where(pt => !tags.Any(t => t.Id == pt.TagId))
				.ToList();
			foreach (var t in removedTags)
				postTags.Remove(t);

			// Add new tags
			var addedTags = tags
				.Where(t => !postTags.Any(pt => pt.TagId == t.Id))
				.Select(t => new PostTag() { TagId = t.Id })
				.ToList();
			foreach (var t in addedTags)
				postTags.Add(t);
		}

		private void ValidateShortContent(SavePostViewModel savePost)
		{
			if (string.IsNullOrWhiteSpace(savePost.ShortContent))
				savePost.ShortContent = savePost.Content.Substring(0, Math.Min(savePost.Content.Length, 500));
		}
	}
}
