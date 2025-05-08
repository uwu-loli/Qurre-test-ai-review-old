using System;
using System.Collections.Concurrent;
using System.Reflection;
using JetBrains.Annotations;
using Mirror;

namespace Qurre.API;

[PublicAPI]
public static class Network
{
    private static readonly ConcurrentDictionary<(Type, string), MethodInfo> StaticMethodCache = new();

    public static void SendSpawnMessage(NetworkIdentity identity, NetworkConnection connection)
    {
        NetworkServer.SendSpawnMessage(identity, connection);
    }

    public static void InvokeStaticMethod(this Type type, string methodName, object[] param)
    {
        const BindingFlags staticMethodFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        if (!StaticMethodCache.TryGetValue((type, methodName), out var methodInfo))
        {
            methodInfo = type.GetMethod(methodName, staticMethodFlags);
            if (methodInfo == null) throw new MissingMethodException(type.Name, methodName);
            StaticMethodCache.TryAdd((type, methodName), methodInfo);
        }

        methodInfo.Invoke(null, param);
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