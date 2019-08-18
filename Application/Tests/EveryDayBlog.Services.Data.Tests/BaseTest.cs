using EveryDayBlog.Services.Data.Tests.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace EveryDayBlog.Services.Data.Tests
{
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
