namespace EveryDayBlog.Web.Authorization.Posts
{
    using System.Threading.Tasks;
    using EveryDayBlog.Common;
    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Data.Models.Enums;
    using EveryDayBlog.Web.ViewModels.Posts.InputModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Authorization.Infrastructure;

    public class PostAdministratorsAuthorizationHandler
                    : AuthorizationHandler<OperationAuthorizationRequirement, Post>
    {
        protected override Task HandleRequirementAsync(
                                              AuthorizationHandlerContext context,
                                              OperationAuthorizationRequirement requirement,
                                              Post resource)
        {
            if (context.User == null)
            {
                return Task.CompletedTask;
            }

            // Administrators can do anything.
            if (context.User.IsInRole(Role.Admin.ToString()))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
