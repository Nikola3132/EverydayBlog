namespace EveryDayBlog.Services
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    public interface IProtectionService
    {
        Task<bool> IsAuthorized<TResource>(ClaimsPrincipal user, TResource resource, string policy);
    }
}
