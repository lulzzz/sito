using System.Threading.Tasks;
using Maddalena.Core.Identity.Model;

namespace Maddalena.Core.Identity.Stores
{
	public interface IIdentityRoleCollection<TRole> : IIdentityObjectCollection<TRole> where TRole : MongoRole
	{
		Task<TRole> FindByNameAsync(string normalizedName);
	}
}