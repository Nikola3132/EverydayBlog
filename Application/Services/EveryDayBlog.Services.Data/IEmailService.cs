using System.Threading.Tasks;

namespace EveryDayBlog.Services.Data
{
    public interface IEmailService
    {
        Task<bool> SendEmailToUserAsync(string callBackUrl, string email);
    }
}
