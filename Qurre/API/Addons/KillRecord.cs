using System;
using JetBrains.Annotations;
using Qurre.API.Entities.Characters;
using Qurre.API.Enums;

namespace Qurre.API.Addons;

[PublicAPI]
public readonly struct KillRecord : IEquatable<KillRecord>
{
    internal KillRecord(Player killer, Player target, DamageTypes damageType, DateTime time)
    {
        Killer = new CachedPlayer(killer);
        Target = new CachedPlayer(target);
        DamageType = damageType;
        Time = time;
    }

    public CachedPlayer Killer { get; }
    public CachedPlayer Target { get; }
    public DamageTypes DamageType { get; }
    public DateTime Time { get; }

    public override bool Equals(object? obj)
    {
        return obj is KillRecord other && Equals(other);
    }

    public bool Equals(KillRecord obj)
    {
        return this == obj;
    }

    public override int GetHashCode()
    {
        return Tuple.Create(Killer, Target, DamageType, Time).GetHashCode();
    }

    public override string ToString()
    {
        return $"({Target} killed by {Killer} with {DamageType} at {Time})";
    }

    public static bool operator ==(KillRecord a, KillRecord b)
    {
        return a.Killer == b.Killer && a.Target == b.Target && a.Time == b.Time && a.DamageType == b.DamageType;
    }

    public static bool operator !=(KillRecord a, KillRecord b)
    {
        return !(a == b);
    }

    public static bool operator >(KillRecord a, KillRecord b)
    {
        return a.Time > b.Time;
    }

    public static bool operator <(KillRecord a, KillRecord b)
    {
        return a.Time < b.Time;
    }
}