namespace Application.TestConstants;

internal static class TestEmailConstants
{
    internal static IEnumerable<string> InvalidEmailCases => new List<string>
    {
        "plainaddress",
        "@missingusername.com",
        "username@.com",
        "username@domain",
        "username@domain..com",
        "username@domain,com",
        "username@domain com",
        "username@-domain.com",
        "username@domain-.com",
        "user name@domain.com",
        "user@domain..co",
        "user@.domain.com",
        "user@domain.c",
        "user@127.0.0.1",
        ".user@domain.com",
        "user.@domain.com",
        "user..name@domain.com",
        "user@domain..example",
        "\"john\"@domain.com",
        "user@do_main.com",
        "user@domain@domain.com",
        "user@.com",
        "user@[123.123.123.123]",
        "user@domain.123",
        "user@@domain.com",
        "user@domain..co.uk",
        "user@.sub.domain.com",
        " user123@1example5.com"
    };

    internal static IEnumerable<string> ValidEmailCases => new List<string>
    {
        "jane.doe+newsletter@example.org",
        "test.user@example.com",
        "contact@example.net",
        "support@test-site.com",
        "admin@demo.co",
        "user123@example.com",
        "user123@1example5.com",
        "firstname.lastname@example.com",
        "firstname.lastname@exam-ple.com",
        "firstname.lastname@ex.am.ple.com",
        "first_name.last+name@ex.am.ple.com",
    };

    internal static IEnumerable<string> EmptyEmailCases => new List<string>
    {
        "",
        " ",
        "  "
    };
}