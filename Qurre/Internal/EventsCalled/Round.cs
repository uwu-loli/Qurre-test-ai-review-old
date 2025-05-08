using System.Linq;
using MapGeneration;
using Qurre.API;
using Qurre.API.Addons;
using Qurre.API.Addons.Audio;
using Qurre.API.Attributes;
using Qurre.API.Controllers.Structs;
using Qurre.API.Core.Implementations;
using Qurre.API.Entities;
using Qurre.API.Entities.Rooms;
using Qurre.API.World;
using Qurre.Events;
using Qurre.Internal.Patches.PlayerEvents.Admins;
using Scp914;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Qurre.Internal.EventsCalled;

internal static class Round
{
    static Round()
    {
        SceneManager.sceneUnloaded += SceneUnloaded;
    }

    [EventMethod(RoundEvents.Start)]
    private static void Started()
    {
        API.World.Round.LocalStarted = true;
        API.World.Round.LocalWaiting = false;
    }

    [EventMethod(RoundEvents.Restart)]
    private static void DestroyAudio()
    {
        API.Audio.LocalHostAudioPlayer = null;
    }

    [EventMethod(RoundEvents.Waiting)]
    private static void Waiting()
    {
        Server.WaitingRefresh();

        API.World.Round.LocalStarted = false;
        API.World.Round.ForceEnd = false;
        API.World.Round.LocalWaiting = true;
        API.World.Round.ActiveGenerators = 0;

        BcComponent.Refresh();

        Extensions.DamagesCached.Clear();
        Banned.Cached.Clear();

        API.World.Round.CurrentRound++;

        RoundSummary.RoundLock = false;

        MapRoundInit();
    }

    private static void MapRoundInit()
    {
        Map.AmbientSoundPlayer = Server.Host.GameObject.GetComponent<AmbientSoundPlayer>();

        foreach (var identifier in RoomIdentifier.AllRoomIdentifiers)
            _ = EntityManager.Get<IGameRoom>(identifier);

        foreach (var levelEntity in EntityManager.GetAll<LevelEntity>())
            levelEntity.MarkAsLevelGenerated();

        API.World.Scp914.Controller = Object.FindObjectOfType<Scp914Controller>();
    }

    private static void MapClearLists()
    {
        Map.CassieList.Clear();
        Banned.Cached.Clear();
        Extensions.DamagesCached.Clear();
    }

    private static void SceneUnloaded(Scene _)
    {
        Fields.Player.Ids.Clear();
        Fields.Player.Args.Clear();
        Fields.Player.Hubs.Clear();
        Fields.Player.Dictionary.Clear();
        MapClearLists();
    }
}