namespace EveryDayBlog.Web.Authorization.Posts
{
    using System.Threading.Tasks;

    using EveryDayBlog.Common;
    using EveryDayBlog.Data.Models;
    using EveryDayBlog.Web.ViewModels.Posts.InputModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Authorization.Infrastructure;
    using Microsoft.AspNetCore.Identity;

    public class PostIsOwnerAuthorizationHandler
                : AuthorizationHandler<OperationAuthorizationRequirement, Post>
    {
        readonly UserManager<IdentityUser> userManager;

        public PostIsOwnerAuthorizationHandler(UserManager<IdentityUser>
            userManager)
        {
            this.userManager = userManager;
        }

        protected override Task
            HandleRequirementAsync(
                AuthorizationHandlerContext context,
                OperationAuthorizationRequirement requirement,
                Post resource)
        {
            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }

            // If we're not asking for CRUD permission, return.

            if (requirement.Name != GlobalConstants.CreateOperationName &&
                requirement.Name != GlobalConstants.ReadOperationName &&
                requirement.Name != GlobalConstants.UpdateOperationName &&
                requirement.Name != GlobalConstants.DeleteOperationName)
            {
                return Task.CompletedTask;
            }

            if (resource.UserId == userManager.GetUserId(context.User))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
