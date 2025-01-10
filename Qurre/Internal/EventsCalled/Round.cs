using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Hazards;
using Interactables.Interobjects.DoorUtils;
using InventorySystem.Items.Firearms.Attachments;
using MapGeneration;
using MEC;
using Qurre.API;
using Qurre.API.Addons;
using Qurre.API.Addons.Audio;
using Qurre.API.Addons.Items;
using Qurre.API.Attributes;
using Qurre.API.Controllers;
using Qurre.API.Controllers.Structs;
using Qurre.API.World;
using Qurre.Events;
using Qurre.Internal.Patches.PlayerEvents.Admins;
using Scp914;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Qurre.Internal.EventsCalled;

[SuppressMessage("ReSharper", "UnusedMember.Local")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
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

        foreach (AudioPlayerBot player in AudioPlayerBot.Players.ToList())
            player.DestroySelf();

        BaseAudioPlayer.Players.Clear();
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

        if (API.World.Round.CurrentRound == 0)
            Prefabs.InitLate();

        API.World.Round.CurrentRound++;

        RoundSummary.RoundLock = false;

        MapRoundInit();
    }

    private static void MapRoundInit()
    {
        Map.AmbientSoundPlayer = Server.Host.GameObject.GetComponent<AmbientSoundPlayer>();

        foreach (var roomBase in RoomIdentifier.AllRoomIdentifiers)
            _ = Room.Get(roomBase);

        foreach (var doorBase in Server.GetObjectsOf<DoorVariant>())
        {
            if (!Door.TryGet(doorBase, out var door)) continue;
            door.MarkAsMapGenerated();
        }

        foreach (var sinkholeBase in Server.GetObjectsOf<SinkholeEnvironmentalHazard>())
        {
            if (!Sinkhole.TryGet(sinkholeBase, out var sinkhole)) continue;
            sinkhole.MarkAsMapGenerated();
        }

        foreach (var teslaBase in Server.GetObjectsOf<TeslaGate>())
        {
            if (!Tesla.TryGet(teslaBase, out var tesla)) continue;
            tesla.MarkAsMapGenerated();
        }

        foreach (var windowBase in Server.GetObjectsOf<BreakableWindow>())
        {
            if (!Window.TryGet(windowBase, out var window)) continue;
            window.MarkAsMapGenerated();
        }

        foreach (var workstationBase in WorkstationController.AllWorkstations)
        {
            if (!WorkStation.TryGet(workstationBase, out var workstation)) continue;
            workstation.MarkAsMapGenerated();
        }
        
        API.World.Scp914.Controller = Object.FindObjectOfType<Scp914Controller>();


        List<Door> updateDoors = [.. Door.List];

        UpdateDoors();
        return;

        void UpdateDoors()
        {
            List<Door> updates = [.. updateDoors];

            foreach (var door in updates)
                try
                {
                    foreach (var room in door.Rooms)
                        room.Doors.Add(door);
                    updateDoors.Remove(door);
                }
                catch
                {
                    // ignored
                }

            updates.Clear();

            if (updateDoors.Count == 0)
                return;

            Timing.CallDelayed(0.5f, UpdateDoors);
        }
    }

    private static void MapClearLists()
    {
        foreach (var x in Tesla.List)
        {
            x.ImmunityRoles.Clear();
            x.ImmunityPlayers.Clear();
        }

        Map.Cassies.Clear();

        LightPoint.ClearCache();
        Primitive.ClearCache();
        ShootingTarget.ClearCache();
        Speaker.ClearCache();
        WorkStation.ClearCache();

        API.Controllers.Camera.ClearCache();
        Door.ClearCache();
        Generator.ClearCache();
        Lift.ClearCache();
        Locker.ClearCache();
        Corpse.ClearCache();
        Room.ClearCache();
        Sinkhole.ClearCache();
        Tesla.ClearCache();
        Window.ClearCache();

        Room.ClearCache();

        Banned.Cached.Clear();

        Extensions.DamagesCached.Clear();

        Item.ClearCache();
        Pickup.ClearCache();
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