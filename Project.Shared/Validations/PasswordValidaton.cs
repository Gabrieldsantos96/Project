using System.ComponentModel.DataAnnotations;
using static System.Text.RegularExpressions.Regex;

namespace Project.Shared.Validations;
public class PasswordPolicyAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is not string password)
        {
            return false;
        }

        bool hasUpperCase = IsMatch(password, "[A-Z]");
        bool hasLowerCase = IsMatch(password, "[a-z]");
        bool hasDigit = IsMatch(password, "[0-9]");
        bool hasSpecialChar = IsMatch(password, "[^a-zA-Z0-9]");

        return hasUpperCase && hasLowerCase && hasDigit && hasSpecialChar;
    }
}
