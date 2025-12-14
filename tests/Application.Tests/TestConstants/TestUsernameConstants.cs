namespace Application.TestConstants;

public class TestUsernameConstants
{
    internal static IEnumerable<string> InValidCharactersUsernameCases => new List<string>
    {
        "1",
        "_",
        "_assay",
        "1assay",
        "assay%",
        "assay$",
        "assay#",
        "assay@",
        "assay\\",
        "assay:",
        "assay(",
        "assay)",
        "=assay+",
        "as say"
    };

    internal static IEnumerable<string> ValidUsernameCases => new List<string>
    {
        "assay",
        "assay_hanafi",
        "assay_hanafi1",
        "assay_hanafi12",
        "assay_hanafi1_2",
    };

    internal static IEnumerable<string> EmptyUsernameCases => new List<string>
    {
        "",
        " ",
        "  "
    };   
    internal static IEnumerable<string> TooBigUsernameCases => new List<string>
    {
        string.Concat(Enumerable.Repeat("a", 21)),
    };   
}