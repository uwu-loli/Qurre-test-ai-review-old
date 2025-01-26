using JetBrains.Annotations;
using PlayerStatsSystem;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Entities.Characters;
using Qurre.API.Enums;
using Qurre.API.World.Entities.Player;

// ReSharper disable once CheckNamespace
namespace Qurre.Events.Structs;

[PublicAPI]
public class DeadEvent : IBaseEvent
{
    private const uint EventID = PlayerEvents.Dead;

    internal DeadEvent(Player attacker, Player target, DamageHandlerBase damageInfo, DamageTypes type)
    {
        Attacker = attacker;
        Target = target;
        DamageType = type;
        DamageInfo = damageInfo;
        LiteType = damageInfo.GetLiteDamageTypes();
    }

    public Player Attacker { get; }
    public Player Target { get; }
    public DamageTypes DamageType { get; }
    public DamagePrimitiveTypes LiteType { get; }
    public DamageHandlerBase DamageInfo { get; }
    public uint EventId { get; } = EventID;
}

[PublicAPI]
public class DiesEvent : ICancellableEvent
{
    private const uint EventID = PlayerEvents.Dies;
    private DamagePrimitiveTypes _liteType = DamagePrimitiveTypes.Unknown;

    private DamageTypes _type = DamageTypes.Unknown;

    internal DiesEvent(Player attacker, Player target, DamageHandlerBase damageInfo)
    {
        Attacker = attacker;
        Target = target;
        DamageInfo = damageInfo;
        IsAllowed = true;
    }

    public Player Attacker { get; }
    public Player Target { get; }
    public DamageHandlerBase DamageInfo { get; }
    public bool IsAllowed { get; set; }

    public DamageTypes DamageType
    {
        get
        {
            if (_type is DamageTypes.Unknown) _type = DamageInfo.GetDamageType();
            return _type;
        }
    }

    public DamagePrimitiveTypes LiteType
    {
        get
        {
            if (_liteType is DamagePrimitiveTypes.Unknown) _liteType = DamageInfo.GetLiteDamageTypes();
            return _liteType;
        }
    }

    public uint EventId { get; } = EventID;
}

[PublicAPI]
public class DamageEvent : IBaseEvent
{
    private const uint EventID = PlayerEvents.Damage;
    private DamagePrimitiveTypes _liteType = DamagePrimitiveTypes.Unknown;

    private DamageTypes _type = DamageTypes.Unknown;

    internal DamageEvent(Player attacker, Player target, DamageHandlerBase damageInfo, float damage)
    {
        Attacker = attacker;
        Target = target;
        DamageInfo = damageInfo;
        Damage = damage;
        Allowed = true;
    }

    public Player Attacker { get; }
    public Player Target { get; }
    public DamageHandlerBase DamageInfo { get; }
    public float Damage { get; set; }
    public bool Allowed { get; set; }

    public DamageTypes DamageType
    {
        get
        {
            if (_type is DamageTypes.Unknown) _type = DamageInfo.GetDamageType();
            return _type;
        }
    }

    public DamagePrimitiveTypes LiteType
    {
        get
        {
            if (_liteType is DamagePrimitiveTypes.Unknown) _liteType = DamageInfo.GetLiteDamageTypes();
            return _liteType;
        }
    }

    public uint EventId { get; } = EventID;
}

[PublicAPI]
public class AttackEvent : IBaseEvent
{
    private const uint EventID = PlayerEvents.Attack;
    private DamagePrimitiveTypes _liteType = DamagePrimitiveTypes.Unknown;

    private DamageTypes _type = DamageTypes.Unknown;

    internal AttackEvent(Player attacker, Player target, AttackerDamageHandler damageInfo, float damage,
        bool friendlyFire, bool allowed)
    {
        Attacker = attacker;
        Target = target;
        DamageInfo = damageInfo;
        Damage = damage;
        FriendlyFire = friendlyFire;
        Allowed = allowed;
    }

    public Player Attacker { get; }
    public Player Target { get; }
    public AttackerDamageHandler DamageInfo { get; }
    public float Damage { get; set; }
    public bool FriendlyFire { get; set; }
    public bool Allowed { get; set; }

    public DamageTypes DamageType
    {
        get
        {
            if (_type is DamageTypes.Unknown) _type = DamageInfo.GetDamageType();
            return _type;
        }
    }

    public DamagePrimitiveTypes LiteType
    {
        get
        {
            if (_liteType is DamagePrimitiveTypes.Unknown) _liteType = DamageInfo.GetLiteDamageTypes();
            return _liteType;
        }
    }

    public uint EventId { get; } = EventID;
}

[PublicAPI]
public class HealEvent : ICancellableEvent
{
    private const uint EventID = PlayerEvents.Heal;

    internal HealEvent(Player player, float amount)
    {
        Player = player;
        Amount = amount;
        IsAllowed = true;
    }

    public Player Player { get; }
    public float Amount { get; set; }
    public bool IsAllowed { get; set; }
    public uint EventId { get; } = EventID;
}