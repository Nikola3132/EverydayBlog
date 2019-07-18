using System.Threading.Tasks;

namespace EveryDayBlog.Services.Data
{
    public interface IEmailService
    {
        Task<bool> SendEmailToUser(string callBackUrl, string email);
    }
}
