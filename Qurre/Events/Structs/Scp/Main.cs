using JetBrains.Annotations;
using Qurre.API.Controllers;
using Qurre.API.Entities.Characters;
using Qurre.API.Enums;
using Qurre.API.World.Entities.Player;

// ReSharper disable once CheckNamespace
namespace Qurre.Events.Structs;

[PublicAPI]
public class ScpAttackEvent : IBaseEvent
{
    private const uint EventID = ScpEvents.Attack;

    internal ScpAttackEvent(Player attacker, Player target, ScpAttackTypes type)
    {
        Attacker = attacker;
        Target = target;
        Type = type;
        Allowed = true;
    }

    public Player Attacker { get; }
    public Player Target { get; }
    public ScpAttackTypes Type { get; }
    public float Damage { get; set; }
    public bool Allowed { get; set; }
    public uint EventId { get; } = EventID;
}