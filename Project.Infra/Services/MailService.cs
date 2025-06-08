using Project.Domain.Interfaces.Infra;
using Project.Domain.Models;

namespace Project.Infra.Services;
public sealed class MailService() : IMailService
{
    public Task SendMailAsync(MailRequest request)
    {
        throw new NotImplementedException();
    }
}
