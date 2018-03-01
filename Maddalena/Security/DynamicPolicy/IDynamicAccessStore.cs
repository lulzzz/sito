using System.Security.Claims;
using System.Threading.Tasks;

namespace Maddalena.Security.DynamicPolicy
{
    public interface IDynamicAccessStore
    {
        Task<bool> IsAuthorizedAsync(ClaimsPrincipal user, string area);
    }
}
