namespace Qurre.API.Controllers.Structs;

internal static class BcComponent
{
    private static global::Broadcast? _bc;

    internal static global::Broadcast Component
        => _bc ??= Server.Host.GameObject.GetComponent<global::Broadcast>();

    internal static void Refresh()
    {
        _bc = null;
    }
}