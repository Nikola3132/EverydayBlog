namespace EveryDayBlog.Services.Data.Tests
{
    using EveryDayBlog.Services.Data.Tests.Config;

    public class BaseTest
    {
        public BaseTest()
        {
            bool initialized = false;

            if (!initialized)
            {
               InitilizeAutoMapper.InitializeMapper();
               initialized = true;
            }
        }
    }
}
