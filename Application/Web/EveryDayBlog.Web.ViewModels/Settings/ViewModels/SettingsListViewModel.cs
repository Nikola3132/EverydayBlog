namespace EveryDayBlog.Web.ViewModels.Settings
{
    using EveryDayBlog.Web.ViewModels.Settings.ViewModels;
    using System.Collections.Generic;

    public class SettingsListViewModel
    {
        public IEnumerable<SettingViewModel> Settings { get; set; }
    }
}
