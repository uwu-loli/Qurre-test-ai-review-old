using System.Reflection;

namespace Qurre.Loader;

public readonly struct AssemblyDefine(Assembly assembly, string path)
{
    public Assembly Assembly { get; } = assembly;
    public string Path { get; } = path;
}