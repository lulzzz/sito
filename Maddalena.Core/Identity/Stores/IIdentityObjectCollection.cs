using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Maddalena.Core.Identity.Model;
using Maddalena.Core.Mongo;

namespace Maddalena.Core.Identity.Stores
{
	public interface IIdentityObjectCollection<TItem> where TItem : MongoObject
	{
		Task<IEnumerable<TItem>> GetAll();
		Task<TItem> CreateAsync(TItem obj);
		Task UpdateAsync(TItem obj);
		Task DeleteAsync(TItem obj);
		Task<TItem> FindByIdAsync(string itemId);
	}
}