using SimpleBlogApp.Core.Models;
using SimpleBlogApp.ViewModels.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleBlogApp.Services.Interfaces
{
	public interface ITagService
	{
		Task<IEnumerable<Tag>> FindByNamesAndAddIfNotExists(IEnumerable<string> tags);
		Task<IEnumerable<TagViewModel>> FindFirsTagsLike(string name, int numOfRecords);
	}
}
