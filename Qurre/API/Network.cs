using System;
using System.Reflection;
using JetBrains.Annotations;
using Mirror;

namespace Qurre.API;

[PublicAPI]
public static class Network
{
    private static MethodInfo? _sendSpawnMessage;

    public static MethodInfo? SendSpawnMessage
    {
        get
        {
            _sendSpawnMessage ??= typeof(NetworkServer).GetMethod("SendSpawnMessage", BindingFlags.Instance |
                BindingFlags.InvokeMethod |
                BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public);
            return _sendSpawnMessage;
        }
    }

    public static void InvokeStaticMethod(this Type type, string methodName, object[] param)
    {
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic |
                                   BindingFlags.Static | BindingFlags.Public;
        MethodInfo? info = type.GetMethod(methodName, flags);
        info?.Invoke(null, param);
    }


    public static void SendDataToClient<T>(this NetworkConnectionToClient connection, T message)
        where T : struct, NetworkMessage
    {
        if (!connection.isReady) return;

        using var networkWriterPooled = NetworkWriterPool.Get();
        NetworkMessages.Pack(message, networkWriterPooled);
        var segment = networkWriterPooled.ToArraySegment();

        connection.Send(segment);
    }
}