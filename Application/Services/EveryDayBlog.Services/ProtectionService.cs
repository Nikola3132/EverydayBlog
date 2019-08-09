using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EveryDayBlog.Services
{
    public class ProtectionService : IProtectionService
    {
        private readonly IAuthorizationService authorizationService;

        public ProtectionService(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }

        public async Task<bool> IsAuthorized<TResource>(ClaimsPrincipal User, TResource resource, string policy)
        {
            var isAuthorized = await this.authorizationService.AuthorizeAsync(
                User,
                resource,
                policyName: policy);

            return isAuthorized.Succeeded;
        }
    }
}
