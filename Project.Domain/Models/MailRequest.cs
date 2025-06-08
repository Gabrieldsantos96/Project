namespace Project.Domain.Models;

public class MailRequest
{
    public required string ContactName { get; set; }
    public required string? ContactMail { get; set; }
    public required string Subject { get; set; }
    public required string BodyHtml { get; set; }
}
