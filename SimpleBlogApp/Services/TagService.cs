using AutoMapper;
using SimpleBlogApp.Core.Interfaces;
using SimpleBlogApp.Core.Models;
using SimpleBlogApp.Services.Interfaces;
using SimpleBlogApp.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBlogApp.Services
{
	public class TagService : ITagService
	{
		private readonly ITagRepository tagRepository;
		private readonly IMapper mapper;

		public TagService(ITagRepository tagRepository, IMapper mapper)
		{
			this.tagRepository = tagRepository;
			this.mapper = mapper;
		}

		public async Task<IEnumerable<Tag>> FindByNamesAndAddIfNotExists(IEnumerable<string> tags)
		{
			var existingTags = await tagRepository.FindByNamesAsync(tags, t => t);
			var missingNames = tags.Where(t => !existingTags.Any(e => e.Name == t));

			if (missingNames == null || missingNames.Count() == 0)
				return existingTags;

			var dateCreated = DateTime.Now;
			var missingTags = missingNames.Select(name => new Tag() {
				Name = name,
				IsActive = true,
				DateCreated = dateCreated,
				DateLastUpdated = dateCreated
			}).ToList();
			tagRepository.AddRange(missingTags);

			missingTags.AddRange(existingTags);
			return missingTags;
		}

		public async Task<IEnumerable<TagViewModel>> FindFirsTagsLike(string name, int numOfRecords)
		{
			return await tagRepository.FindByNameAsync(name, numOfRecords, t => new TagViewModel()
			{
				Id = t.Id,
				Name = t.Name
			});
		}
	}
}
