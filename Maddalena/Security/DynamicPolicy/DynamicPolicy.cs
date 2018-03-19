using Microsoft.AspNetCore.Authorization;

namespace Maddalena.Security.DynamicPolicy
{
    public class DynamicPolicy : IAuthorizationRequirement
    {
        public string Area { get; set; }
    }
}