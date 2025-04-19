using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Qurre.API.Addons;
using Qurre.API.Attributes;
using Qurre.Events;
using Qurre.Events.Structs;

namespace Qurre.API.Entities.Characters.Components;

[PublicAPI]
public sealed class StatsInformation
{
    private readonly List<KillRecord> _localKills = [];
    private readonly Player _player;

    internal StatsInformation(Player pl)
    {
        _player = pl;
        DeathsCount = 0;
    }

    public int DeathsCount { get; private set; }

    public IReadOnlyList<KillRecord> Kills => _localKills;

    [EventMethod(PlayerEvents.Dead, int.MinValue)]
    private static void OnPlayerDead(DeadEvent ev)
    {
        ev.Target.Broadcasts.Add(new Controllers.Broadcast(ev.Target, "You died", 10));

        ev.Target.StatsInformation.DeathsCount++;

        if (ev.Target == ev.Attacker)
            return;

        ev.Attacker.StatsInformation._localKills.Add(
            new KillRecord(ev.Attacker, ev.Target, ev.DamageType, DateTime.Now));
    }
}