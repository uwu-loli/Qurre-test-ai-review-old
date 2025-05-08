using System;
using System.Collections.Generic;
using AdminToys;
using Hazards;
using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using InventorySystem.Items.Firearms.Attachments;
using JetBrains.Annotations;
using MapGeneration.Distributors;
using Mirror;
using PlayerRoles.PlayableScps.Scp939;
using Qurre.API.Attributes;
using Qurre.API.Enums;
using Qurre.Events;
using Qurre.Loader;

namespace Qurre.API.Addons;

[PublicAPI]
public static class Prefabs
{
    public const uint AssetIdDoorLcz = 3038351124;
    public const uint AssetIdDoorHcz = 2295511789;
    public const uint AssetIdDoorEz = 1883254029;
    public const uint AssetIdDoorBulkHcz = 2176035362;

    public const uint AssetIdShootingTargetSport = 1704345398;
    public const uint AssetIdShootingTargetDBoy = 858699872;
    public const uint AssetIdShootingTargetBinary = 3613149668;

    private static readonly Dictionary<DoorPrefabs, DoorVariant> LocalDoors = [];
    private static readonly Dictionary<LockerPrefabs, Locker> LocalLockers = [];
    private static readonly Dictionary<ShootingTargetPrefabs, ShootingTarget> LocalShootingTargets = [];

    public static IReadOnlyDictionary<DoorPrefabs, DoorVariant> Doors => LocalDoors;
    public static IReadOnlyDictionary<LockerPrefabs, Locker> Lockers => LocalLockers;
    public static IReadOnlyDictionary<ShootingTargetPrefabs, ShootingTarget> ShootingTargets => LocalShootingTargets;

    public static WorkstationController? WorkStation { get; private set; }
    public static Scp079Generator? Generator { get; private set; }
    public static PrimitiveObjectToy? Primitive { get; private set; }
    public static LightSourceToy? Light { get; private set; }

    public static SpeakerToy? Speaker { get; private set; }
    public static TantrumEnvironmentalHazard? Tantrum { get; private set; }
    public static Scp939AmnesticCloudInstance? Scp939AmnesticCloud { get; private set; }

    static Prefabs()
    {
        EntryPoint.Init += OnInit;
    }

    [EventMethod(RoundEvents.Waiting)]
    private static void OnRoundWaiting()
    {
        if (World.Round.CurrentRound != 0) return;
        InitLate();
    }

    private static void OnInit()
    {
        try
        {
            foreach (var prefab in NetworkManager.singleton.spawnPrefabs)
            {
#if TESTS
                Log.Custom(prefab.name, "Init", ConsoleColor.Magenta);
#endif

                if (prefab.TryGetComponent(out BreakableDoor door))
                {
                    switch (prefab.name)
                    {
                        case "EZ BreakableDoor":
                            LocalDoors[DoorPrefabs.DoorEZ] = door;
                            break;
                        case "HCZ BreakableDoor":
                            LocalDoors[DoorPrefabs.DoorHCZ] = door;
                            break;
                        case "LCZ BreakableDoor":
                            LocalDoors[DoorPrefabs.DoorLCZ] = door;
                            break;
                    }
                }
                else if (prefab.TryGetComponent(out TantrumEnvironmentalHazard tantrum) &&
                         prefab.name == "TantrumObj")
                {
                    Tantrum = tantrum;
                }
            }
        }
        catch (Exception e)
        {
            Log.Error($"Error in Addons => Prefabs [Init]:\n{e}\n{e.StackTrace}");
        }
    }

    private static void InitLate()
    {
        try
        {
            foreach (var (key, prefab) in NetworkClient.prefabs)
            {
#if TESTS
                Log.Custom(prefab.Key + " - " + prefab.Value.name, "InitLate", ConsoleColor.Blue);
#endif

                if (prefab.TryGetComponent(out Locker locker))
                {
                    switch (key)
                    {
                        // Pedestal
                        case 2286635216:
                            LocalLockers[LockerPrefabs.Pedestal018] = locker;
                            break;
                        case 664776131:
                            LocalLockers[LockerPrefabs.Pedestal207] = locker;
                            break;
                        case 3724306703:
                            LocalLockers[LockerPrefabs.Pedestal244] = locker;
                            break;
                        case 3849573771:
                            LocalLockers[LockerPrefabs.Pedestal268] = locker;
                            break;
                        case 373821065:
                            LocalLockers[LockerPrefabs.Pedestal500] = locker;
                            break;
                        case 3372339835:
                            LocalLockers[LockerPrefabs.Pedestal1576] = locker;
                            break;
                        case 3962534659:
                            LocalLockers[LockerPrefabs.Pedestal1853] = locker;
                            break;
                        case 3578915554:
                            LocalLockers[LockerPrefabs.Pedestal2176] = locker;
                            break;

                        // Weapon
                        case 2830750618:
                            LocalLockers[LockerPrefabs.LargeGun] = locker;
                            break;
                        case 3352879624:
                            LocalLockers[LockerPrefabs.RifleRack] = locker;
                            break;
                        case 1964083310:
                            LocalLockers[LockerPrefabs.MiscLocker] = locker;
                            break;
                        case 2372810204:
                            LocalLockers[LockerPrefabs.Experimental] = locker;
                            break;

                        // Medkit
                        case 4040822781:
                            LocalLockers[LockerPrefabs.RegularMedkit] = locker;
                            break;
                        case 2525847434:
                            LocalLockers[LockerPrefabs.AdrenalineMedkit] = locker;
                            break;
                    }
                }
                else if (prefab.TryGetComponent(out BreakableDoor door))
                {
                    switch (key)
                    {
                        case AssetIdDoorLcz:
                            LocalDoors[DoorPrefabs.DoorLCZ] = door;
                            break;
                        case AssetIdDoorHcz:
                            LocalDoors[DoorPrefabs.DoorHCZ] = door;
                            break;
                        case AssetIdDoorEz:
                            LocalDoors[DoorPrefabs.DoorEZ] = door;
                            break;
                        case AssetIdDoorBulkHcz:
                            LocalDoors[DoorPrefabs.BulkHCZ] = door;
                            break;
                    }
                }
                else if (prefab.TryGetComponent(out ShootingTarget shootingTarget))
                {
                    switch (key)
                    {
                        case AssetIdShootingTargetSport:
                            LocalShootingTargets[ShootingTargetPrefabs.Sport] = shootingTarget;
                            break;
                        case AssetIdShootingTargetDBoy:
                            LocalShootingTargets[ShootingTargetPrefabs.Dboy] = shootingTarget;
                            break;
                        case AssetIdShootingTargetBinary:
                            LocalShootingTargets[ShootingTargetPrefabs.Binary] = shootingTarget;
                            break;
                    }
                }
                else
                {
                    switch (key)
                    {
                        case 2724603877 when prefab.TryGetComponent(out Scp079Generator generator):
                            Generator = generator;
                            break;

                        case 1783091262 when prefab.TryGetComponent(out WorkstationController workstation):
                            WorkStation = workstation;
                            break;

                        case 1321952889 when prefab.TryGetComponent(out PrimitiveObjectToy primitiveObjectToy):
                            Primitive = primitiveObjectToy;
                            break;
                        case 3956448839 when prefab.TryGetComponent(out LightSourceToy lightSourceToy):
                            Light = lightSourceToy;
                            break;
                        case 825024811
                            when prefab.TryGetComponent(out Scp939AmnesticCloudInstance scp939AmnesticCloud):
                            Scp939AmnesticCloud = scp939AmnesticCloud;
                            break;
                        case 712426663 when prefab.TryGetComponent(out SpeakerToy speakerToy):
                            Speaker = speakerToy;
                            break;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Log.Error($"Error in Addons => Prefabs [InitLate]:\n{e}\n{e.StackTrace}");
        }
    }
}