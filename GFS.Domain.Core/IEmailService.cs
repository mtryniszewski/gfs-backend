using System.Threading.Tasks;

namespace GFS.Domain.Core
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}