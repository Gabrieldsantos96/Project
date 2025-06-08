using Project.Domain.Models;

namespace Project.Domain.Interfaces.Infra;
public interface IMailService
{
    Task SendMailAsync(MailRequest request);
}