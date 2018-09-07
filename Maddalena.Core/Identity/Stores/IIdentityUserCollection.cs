using System.Collections.Generic;
using System.Threading.Tasks;
using Maddalena.Core.Identity.Model;

namespace Maddalena.Core.Identity.Stores
{
	public interface IIdentityUserCollection<TUser> : IIdentityObjectCollection<TUser> where TUser : MongoUser
	{
		Task<TUser> FindByEmailAsync(string normalizedEmail);
		Task<TUser> FindByUserNameAsync(string username);
		Task<TUser> FindByNormalizedUserNameAsync(string normalizedUserName);
		Task<TUser> FindByLoginAsync(string loginProvider, string providerKey);
		Task<IEnumerable<TUser>> FindUsersByClaimAsync(string claimType, string claimValue);
		Task<IEnumerable<TUser>> FindUsersInRoleAsync(string roleName);
	}
}