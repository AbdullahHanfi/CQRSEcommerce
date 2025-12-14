namespace Application.TestConstants;

internal class TestPasswordConstants
{
    internal static IEnumerable<string> InvalidPasswordCases => new List<string>
    {
        "1",
        "12",
        "123",
        "1234",
        "12345"
    };

    internal static IEnumerable<string> ValidPasswordCases => new List<string>
    {
        "123456",
        "1234567",
        "12345678",
    };

    internal static IEnumerable<string> EmptyPasswordCases => new List<string>
    {
        "",
        " ",
        "  "
    };
}