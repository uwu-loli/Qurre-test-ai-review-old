using System;
using JetBrains.Annotations;
using Mirror;

namespace Qurre.Internal.Misc;

[PublicAPI]
public class SpawnMessageCache(SpawnMessage cachedSpawnMessage)
{
    private const float CacheLifetimeSeconds = 15F;
    private static readonly TimeSpan CacheLifetime = TimeSpan.FromSeconds(CacheLifetimeSeconds);

    private DateTime _cachedTime = DateTime.Now;

    public bool IsExpired => DateTime.Now - _cachedTime > CacheLifetime;
    public SpawnMessage CachedSpawnMessage => cachedSpawnMessage;
}