namespace EveryDayBlog.Web.Controllers
{
    using EveryDayBlog.Services.Data;
    using Microsoft.AspNetCore.Mvc;

    public class UsersController : BaseController
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        //[HttpGet]
        //public IActionResult Profile()
        //{
        //    var userProfileViewModel
        //        = this.usersService.GetUserByUsername<UserProfileInputModel>(this.User.Identity.Name);

        //    return this.View(userProfileViewModel);
        //}

        //[HttpPut]
        //public IActionResult Profile(UserProfileInputModel userProfileInputModel)
        //{
        //    var userProfileViewModel
        //        = this.usersService.GetUserByUsername<UserProfileInputModel>(this.User.Identity.Name);

        //    if (!this.ModelState.IsValid)
        //    {
        //        return this.View(userProfileInputModel);
        //    }



        //    return this.View(userProfileViewModel);
        //}

    }
}