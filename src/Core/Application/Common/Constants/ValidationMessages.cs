namespace Application.Common.Constants;

public static class ValidationMessages
{
    public const string EmailRequired = "Email is required.";
    public const string InvalidEmailFormat = "Invalid email format.";
    public const string PasswordRequired = "Your password cannot be empty.";
    public const string PasswordTooShort = "Your password must be at least 6 characters long.";
    public const string ConfirmPasswordRequired = "Please confirm your password.";
    public const string ConfirmPasswordNotMatch = "The passwords do not match.";
    public const string UserNameRequired = "Username is required.";
}