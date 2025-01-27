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
using Qurre.API.Enums;
using UnityEngine;

namespace Qurre.API.Addons;

[PublicAPI]
public static class Prefabs
{
    public const uint AssetIdDoorLCZ = 3038351124;
    public const uint AssetIdDoorHCZ = 2295511789;
    public const uint AssetIdDoorEZ = 1883254029;
    public const uint AssetIdDoorBulkHCZ = 2176035362;

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


    internal static void Init()
    {
        try
        {
            foreach (var prefab in NetworkManager.singleton.spawnPrefabs)
            {
#if TESTS
                Log.Custom(prefab.name, "Init", ConsoleColor.Magenta);
#endif

                switch (prefab.name)
                {
                    case "EZ BreakableDoor" when prefab.TryGetComponent(out BreakableDoor door):
                        LocalDoors[DoorPrefabs.DoorEZ] = door;
                        break;
                    case "HCZ BreakableDoor" when prefab.TryGetComponent(out BreakableDoor door):
                        LocalDoors[DoorPrefabs.DoorHCZ] = door;
                        break;
                    case "LCZ BreakableDoor" when prefab.TryGetComponent(out BreakableDoor door):
                        LocalDoors[DoorPrefabs.DoorLCZ] = door;
                        break;

                    case "TantrumObj" when prefab.TryGetComponent(out TantrumEnvironmentalHazard tantrum):
                        Tantrum = tantrum;
                        break;
                }
            }
        }
        catch (Exception e)
        {
            Log.Error($"Error in Addons => Prefabs [Init]:\n{e}\n{e.StackTrace}");
        }
    }

    internal static void InitLate()
    {
        try
        {
            foreach (var prefab in NetworkClient.prefabs)
            {
#if TESTS
                Log.Custom(prefab.Key + " - " + prefab.Value.name, "InitLate", ConsoleColor.Blue);
#endif

                switch (prefab.Key)
                {
                    case 2724603877 when prefab.Value.TryGetComponent(out Scp079Generator generator):
                        Generator = generator;
                        break;

                    case 1783091262 when prefab.Value.TryGetComponent(out WorkstationController workstation):
                        WorkStation = workstation;
                        break;

                    case 2286635216 when prefab.Value.TryGetComponent(out Locker locker):
                        LocalLockers[LockerPrefabs.Pedestal018] = locker;
                        break;
                    case 664776131 when prefab.Value.TryGetComponent(out Locker locker):
                        LocalLockers[LockerPrefabs.Pedestal207] = locker;
                        break;
                    case 3724306703 when prefab.Value.TryGetComponent(out Locker locker):
                        LocalLockers[LockerPrefabs.Pedestal244] = locker;
                        break;
                    case 3849573771 when prefab.Value.TryGetComponent(out Locker locker):
                        LocalLockers[LockerPrefabs.Pedestal268] = locker;
                        break;
                    case 373821065 when prefab.Value.TryGetComponent(out Locker locker):
                        LocalLockers[LockerPrefabs.Pedestal500] = locker;
                        break;
                    case 3372339835 when prefab.Value.TryGetComponent(out Locker locker):
                        LocalLockers[LockerPrefabs.Pedestal1576] = locker;
                        break;
                    case 3962534659 when prefab.Value.TryGetComponent(out Locker locker):
                        LocalLockers[LockerPrefabs.Pedestal1853] = locker;
                        break;
                    case 3578915554 when prefab.Value.TryGetComponent(out Locker locker):
                        LocalLockers[LockerPrefabs.Pedestal2176] = locker;
                        break;

                    case 2830750618 when prefab.Value.TryGetComponent(out Locker locker):
                        LocalLockers[LockerPrefabs.LargeGun] = locker;
                        break;
                    case 3352879624 when prefab.Value.TryGetComponent(out Locker locker):
                        LocalLockers[LockerPrefabs.RifleRack] = locker;
                        break;
                    case 1964083310 when prefab.Value.TryGetComponent(out Locker locker):
                        LocalLockers[LockerPrefabs.MiscLocker] = locker;
                        break;
                    case 2372810204 when prefab.Value.TryGetComponent(out Locker locker):
                        LocalLockers[LockerPrefabs.Experimental] = locker;
                        break;

                    case 4040822781 when prefab.Value.TryGetComponent(out Locker locker):
                        LocalLockers[LockerPrefabs.RegularMedkit] = locker;
                        break;
                    case 2525847434 when prefab.Value.TryGetComponent(out Locker locker):
                        LocalLockers[LockerPrefabs.AdrenalineMedkit] = locker;
                        break;

                    case AssetIdDoorLCZ when prefab.Value.TryGetComponent(out BreakableDoor door):
                        LocalDoors[DoorPrefabs.DoorLCZ] = door;
                        break;
                    case AssetIdDoorHCZ when prefab.Value.TryGetComponent(out BreakableDoor door):
                        LocalDoors[DoorPrefabs.DoorHCZ] = door;
                        break;
                    case AssetIdDoorEZ when prefab.Value.TryGetComponent(out BreakableDoor door):
                        LocalDoors[DoorPrefabs.DoorEZ] = door;
                        break;
                    case AssetIdDoorBulkHCZ when prefab.Value.TryGetComponent(out PryableDoor door):
                        LocalDoors[DoorPrefabs.BulkHCZ] = door;
                        break;

                    case 1704345398 when prefab.Value.TryGetComponent(out ShootingTarget shootingTarget):
                        LocalShootingTargets[ShootingTargetPrefabs.Sport] = shootingTarget;
                        break;
                    case 858699872 when prefab.Value.TryGetComponent(out ShootingTarget shootingTarget):
                        LocalShootingTargets[ShootingTargetPrefabs.Dboy] = shootingTarget;
                        break;
                    case 3613149668 when prefab.Value.TryGetComponent(out ShootingTarget shootingTarget):
                        LocalShootingTargets[ShootingTargetPrefabs.Binary] = shootingTarget;
                        break;

                    case 1321952889 when prefab.Value.TryGetComponent(out PrimitiveObjectToy primitiveObjectToy):
                        Primitive = primitiveObjectToy;
                        break;
                    case 3956448839 when prefab.Value.TryGetComponent(out LightSourceToy lightSourceToy):
                        Light = lightSourceToy;
                        break;
                    case 825024811 when prefab.Value.TryGetComponent(out Scp939AmnesticCloudInstance scp939AmnesticCloud):
                        Scp939AmnesticCloud = scp939AmnesticCloud;
                        break;
                    case 712426663 when prefab.Value.TryGetComponent(out SpeakerToy speakerToy):
                        Speaker = speakerToy;
                        break;
                }
            }
        }
        catch (Exception e)
        {
            Log.Error($"Error in Addons => Prefabs [InitLate]:\n{e}\n{e.StackTrace}");
        }
    }
}