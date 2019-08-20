namespace EveryDayBlog.Services
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;

    public class ProtectionService : IProtectionService
    {
        private readonly IAuthorizationService authorizationService;

        public ProtectionService(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }

        public async Task<bool> IsAuthorized<TResource>(ClaimsPrincipal user, TResource resource, string policy)
        {
            var isAuthorized = await this.authorizationService.AuthorizeAsync(
                user,
                resource,
                policyName: policy);

            return isAuthorized.Succeeded;
        }
    }
}
