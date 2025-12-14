namespace Application.Common.Constants;

public static class ValidationMessages
{
    public const string EmailRequired = "Email is required.";
    public const string InvalidEmailFormat = "Invalid email format.";
    public const string PasswordRequired = "Your password cannot be empty.";
    public const string PasswordTooShort = "Your password must be at least 6 characters long.";
    public const string ConfirmPasswordRequired = "Please confirm your password.";
    public const string ConfirmPasswordNotMatch = "The passwords do not match.";
    public const string UsernameRequired = "Username is required.";
    public const string UsernameLength = "Username must be between 1 and 20 characters.";
    public const string UsernameInvalidCharacters = "Username can contain only letters, numbers, and underscores, and must start with a letter.";
}