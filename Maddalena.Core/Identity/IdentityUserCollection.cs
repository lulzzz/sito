using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maddalena.Core.Identity.Model;
using Maddalena.Core.Identity.Stores;
using Maddalena.Core.Mongo;

namespace Maddalena.Core.Identity
{
	public class IdentityUserCollection<TUser> : MongoObjectCollection<TUser>, IIdentityUserCollection<TUser>
		where TUser : MongoUser
	{
		public IdentityUserCollection(string connectionString, string collectionName) : base(connectionString, collectionName)
		{
		}


		public async Task<TUser> FindByEmailAsync(string normalizedEmail)
		{
			return await FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);
		}

		public async Task<TUser> FindByUserNameAsync(string username)
		{
			return await FirstOrDefaultAsync(u => u.UserName == username);
		}

		public async Task<TUser> FindByNormalizedUserNameAsync(string normalizedUserName)
		{
			return await FirstOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName);
		}

		public async Task<TUser> FindByLoginAsync(string loginProvider, string providerKey)
		{
			return await FirstOrDefaultAsync(u =>
				u.Logins.Any(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey));
		}

		public async Task<IEnumerable<TUser>> FindUsersByClaimAsync(string claimType, string claimValue)
		{
			return await WhereAsync(u => u.Claims.Any(c => c.Type == claimType && c.Value == claimValue));
		}

		public async Task<IEnumerable<TUser>> FindUsersInRoleAsync(string roleName)
		{
			return await AnyEqualAsync(x => x.Roles, roleName);
		}
	}
}