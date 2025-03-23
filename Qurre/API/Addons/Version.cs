using System.Linq;
using JetBrains.Annotations;

namespace Qurre.API.Addons;

[PublicAPI]
public class Version
{
    private const string SMajor = "3";
    private const string SMinor = "0";
    private const string SBuild = "0";
    private const string SRevision = "7";

    private const string SName = "alpha";

    internal const string AssemblyVersion = $"{SMajor}.{SMinor}.{SBuild}.{SRevision}";
    internal const string AssemblyCustom = $"v3-{SName}.{SBuild}";

    internal Version()
    {
    }

    public static uint Major { get; } = uint.Parse(SMajor);

    public static uint Minor { get; } = uint.Parse(SMinor);

    public static uint Build { get; } = uint.Parse(SBuild);

    public static uint Revision { get; } = uint.Parse(SRevision);

    public static string Name => SName;

    public override string ToString()
    {
        string prefix = string.IsNullOrEmpty(Name) ? "" : $"v3-{Name}.{Build}";

        var parts = new[] { Major, Minor, Build, Revision }
            .TakeWhile(part => part > 0);

        string versionNumbers = string.Join(".", parts);

        return $"{prefix} ({versionNumbers})";
    }
}