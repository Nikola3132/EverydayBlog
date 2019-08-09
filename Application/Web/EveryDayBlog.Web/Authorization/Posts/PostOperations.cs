using EveryDayBlog.Common;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace EveryDayBlog.Web.Authorization.Posts
{
    public static class PostOperations
    {
        public static OperationAuthorizationRequirement Create =
          new OperationAuthorizationRequirement { Name = GlobalConstants.CreateOperationName };

        public static OperationAuthorizationRequirement Read =
          new OperationAuthorizationRequirement { Name = GlobalConstants.ReadOperationName };

        public static OperationAuthorizationRequirement Update =
          new OperationAuthorizationRequirement { Name = GlobalConstants.UpdateOperationName };

        public static OperationAuthorizationRequirement Delete =
          new OperationAuthorizationRequirement { Name = GlobalConstants.DeleteOperationName };
    }
}