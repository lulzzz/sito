using System.Threading.Tasks;
using Maddalena.Core.Identity.Model;
using Maddalena.Core.Identity.Stores;
using Maddalena.Core.Mongo;

namespace Maddalena.Core.Identity
{
	public class IdentityRoleCollection<TRole> : MongoObjectCollection<TRole>, IIdentityRoleCollection<TRole>
		where TRole : MongoRole
	{
		public IdentityRoleCollection(string connectionString, string collectionName) : base(connectionString, collectionName)
		{
		}

		public async Task<TRole> FindByNameAsync(string normalizedName)
		{
			return await FirstOrDefaultAsync(x => x.NormalizedName == normalizedName);
		}

		public new async Task<TRole> FindByIdAsync(string roleId)
		{
			return await FirstOrDefaultAsync(x => x.Id == roleId);
		}
	}
}