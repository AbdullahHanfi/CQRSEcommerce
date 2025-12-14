using Application.Features.Auth.Command.Register;

namespace Application.Builders;

public class RegisterCommandBuilder
{
    private string _email = "test.user@example.com";
    private string _password = "Password123!";
    private string _confirmPassword = "Password123!";
    private string _userName = "testuser";

    public RegisterCommandBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public RegisterCommandBuilder WithPassword(string password)
    {
        _password = password;
        return this;
    }

    public RegisterCommandBuilder WithConfirmPassword(string confirmPassword)
    {
        _confirmPassword = confirmPassword;
        return this;
    }

    public RegisterCommandBuilder WithUserName(string userName)
    {
        _userName = userName;
        return this;
    }

    public RegisterCommand Build()
    {
        return new RegisterCommand(_email, _password, _confirmPassword, _userName);
    }
}