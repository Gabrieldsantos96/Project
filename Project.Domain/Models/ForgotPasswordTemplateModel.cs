namespace Project.Domain.Models;

public class ForgotPasswordModel
{
    private readonly string UserConfirmationUrl = "https://link/confirm-account";
    public string? Name { get; set; }
    public string? Url { get; set; }
    public ForgotPasswordModel(string name)
    {
        Name = name;
        Url = UserConfirmationUrl;
    }

    public ForgotPasswordModel() {}
}