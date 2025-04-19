using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using AdminToys;
using CustomPlayerEffects;
using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using InventorySystem.Items;
using InventorySystem.Items.Firearms.Attachments;
using InventorySystem.Items.Usables.Scp244.Hypothermia;
using JetBrains.Annotations;
using MapGeneration;
using MapGeneration.Distributors;
using Mirror;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.PlayableScps;
using PlayerRoles.Ragdolls;
using PlayerStatsSystem;
using Qurre.API.Addons;
using Qurre.API.Core;
using Qurre.API.Entities;
using Qurre.API.Entities.AdminToys;
using Qurre.API.Entities.Characters;
using Qurre.API.Entities.Doors;
using Qurre.API.Entities.Environment;
using Qurre.API.Entities.Rooms;
using Qurre.API.Entities.Structures;
using Qurre.API.Enums;
using Qurre.API.Models;
using Qurre.Events.Structs;
using Qurre.Internal.EventsManager;
using Qurre.Internal.Misc;
using UnityEngine;
using Random = UnityEngine.Random;
using Sinkhole = CustomPlayerEffects.Sinkhole;

namespace Qurre.API;

[PublicAPI]
public static class Extensions
{
    public static bool TryFind<TSource>(this IEnumerable<TSource> enumerable, out TSource found,
        Func<TSource, bool> predicate)
    {
        foreach (var item in enumerable)
            if (predicate(item))
            {
                found = item;
                return true;
            }

        found = default!;
        return false;
    }

    public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
    {
        if (action is null) throw new ArgumentNullException(nameof(action));

        foreach (T t in list)
            action(t);
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        RNGCryptoServiceProvider provider = new();
        int n = list.Count;
        while (n > 1)
        {
            byte[] box = new byte[1];
            do
            {
                provider.GetBytes(box);
            } while (!(box[0] < n * (byte.MaxValue / n)));

            int k = box[0] % n;
            n--;
            (list[n], list[k]) = (list[k], list[n]);
        }
    }

    public static float Difference(this float first, float second)
    {
        return Math.Abs(first - second);
    }


    #region GameObjects

    public static void NetworkRespawn(this GameObject gameObject)
    {
        NetworkServer.UnSpawn(gameObject);
        NetworkServer.Spawn(gameObject);
    }

    #endregion


    #region GetLocker

    public static ILocker? GetLocker(this Locker lockerBase)
    {
        return EntityManager.Get<ILocker>(lockerBase);
    }

    #endregion


    #region GetCorpse

    public static ICorpse? GetCorpse(this BasicRagdoll ragdollBase)
    {
        return EntityManager.Get<ICorpse>(ragdollBase);
    }

    #endregion


    #region GetWorkStation

    public static IWorkStation? GetWorkStation(this WorkstationController workstationBase)
    {
        return EntityManager.Get<IWorkStation>(workstationBase);
    }

    #endregion


    #region GetShootingTarget

    public static IShootingTarget? GetShootingTarget(this ShootingTarget targetBase)
    {
        return EntityManager.Get<IShootingTarget>(targetBase);
    }

    #endregion

    #region Vector3

    public static Vector3 SafeUnscale(this Vector3 vector, Vector3 scale)
    {
        return new Vector3(
            x: vector.x / (scale.x == 0 ? 1 : scale.x),
            y: vector.y / (scale.y == 0 ? 1 : scale.y),
            z: vector.z / (scale.z == 0 ? 1 : scale.z));
    }

    #endregion

    public static T? GetEntity<T>(this GameObject gameObject) where T : class, IEntity
    {
        if (!gameObject) return null;
        return gameObject.TryGetComponent(out EntityLink entityLink) ? entityLink.Entity as T : null;
    }

    #region Ragdoll Data

    public static RagdollData CopyWithReplace(this RagdollData ragdollData,
        ReferenceHub? newHub = null,
        DamageHandlerBase? newDamageHandler = null,
        Vector3? newPosition = null,
        Quaternion? newRotation = null,
        string? newNickname = null,
        RoleTypeId? newRole = null,
        ushort? newSerial = null,
        Vector3? curPosition = null,
        Quaternion? curRotation = null)
    {
        return new RagdollData(
            newHub ?? ragdollData.OwnerHub,
            newDamageHandler ?? ragdollData.Handler,
            newPosition ?? curPosition ?? ragdollData.StartPosition,
            newRotation ?? curRotation ?? ragdollData.StartRotation,
            newSerial ?? ragdollData.Serial);
    }

    #endregion


    #region GetRoom

    public static IGameRoom? GetRoom(this RoomName roomName)
    {
        return EntityManager.GetAll<IGameRoom>().FirstOrDefault(room => room.Name == roomName);
    }

    public static IGameRoom? GetRoom(this RoomTypes roomType)
    {
        return EntityManager.GetAll<IGameRoom>().FirstOrDefault(room => room.RoomType == roomType);
    }

    public static IGameRoom GetRoom(this RoomIdentifier identifier)
    {
        return EntityManager.Get<IGameRoom>(identifier)!;
    }

    #endregion


    #region GetTesla

    public static ITesla? GetTesla(this TeslaGate teslaBase)
    {
        return EntityManager.Get<ITesla>(teslaBase);
    }

    public static ITesla? GetTesla(this GameObject gameObject)
    {
        return gameObject.TryGetComponent<TeslaGate>(out var teslaGate)
            ? EntityManager.Get<ITesla>(teslaGate)
            : null;
    }

    #endregion


    #region GetGenerator

    public static IGenerator? GetGenerator(this Scp079Generator generatorBase)
    {
        return EntityManager.Get<IGenerator>(generatorBase);
    }

    public static IGenerator? GetGenerator(this GameObject gameObject)
    {
        return gameObject.TryGetComponent<Scp079Generator>(out var generator)
            ? EntityManager.Get<IGenerator>(generator)
            : null;
    }

    #endregion


    #region GetLift

    public static ILift? GetLift(this ElevatorChamber liftBase)
    {
        return EntityManager.Get<ILift>(liftBase);
    }

    public static ILift? GetLift(this Vector3 worldPoint)
    {
        return EntityManager.GetAll<ILift>().FirstOrDefault(lift => lift.WorldBounds.Contains(worldPoint));
    }

    #endregion


    #region GetDoor

    public static IDoor? GetDoor(this DoorVariant doorBase)
    {
        return EntityManager.Get<IDoor>(doorBase);
    }

    public static IDoor? GetDoor(this DoorTypes doorType)
    {
        return EntityManager.GetAll<IDoor>().FirstOrDefault(door => door.DoorType == doorType);
    }

    #endregion


    #region Player.Get

    public static IEnumerable<Player> GetPlayer(this Team team)
    {
        return Player.List.Where(player => player.RoleInformation.Team == team);
    }

    public static IEnumerable<Player> GetPlayer(this RoleTypeId role)
    {
        return Player.List.Where(player => player.RoleInformation.RoleType == role);
    }


    public static Player? GetPlayer(this CommandSender? sender)
    {
        return sender == null ? null : GetPlayer(sender.SenderId);
    }

    public static Player? GetPlayer(this uint netId)
    {
        return ReferenceHub.TryGetHubNetID(netId, out ReferenceHub hub) ? GetPlayer(hub) : null;
    }


    public static Player? GetPlayer(this ReferenceHub? referenceHub)
    {
        if (referenceHub == null)
            return null;

        Internal.Fields.Player.Hubs.TryGetValue(referenceHub, out Player player);

        return player;
    }

    public static Player? GetPlayer(this GameObject gameObject)
    {
        if (gameObject == null)
            return null;

        Internal.Fields.Player.Dictionary.TryGetValue(gameObject, out Player player);

        return player;
    }

    public static Player? GetPlayer(this int playerId)
    {
        if (Internal.Fields.Player.Ids.TryGetValue(playerId, out Player? plId))
            return plId;

        foreach (Player pl in Player.List)
        {
            if (pl.UserInformation.Id != playerId)
                continue;

            Internal.Fields.Player.Ids.Add(playerId, pl);
            return pl;
        }

        return null;
    }

    public static Player? GetPlayer(this string args)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(args))
                return null;

            if (Internal.Fields.Player.Args.TryGetValue(args, out Player playerFound) &&
                playerFound?.ReferenceHub is not null)
                return playerFound;

            if (int.TryParse(args, out int id))
                return GetPlayer(id);

            if (args.Contains("@"))
            {
                if (Internal.Fields.Player.Dictionary.Values.TryFind(out var player,
                        x => x.UserInformation.UserId == args))
                    playerFound = player;
            }
            else
            {
                int lastnameDifference = 31;
                string firstString = args.ToLower();

                foreach (Player player in Internal.Fields.Player.Dictionary.Values)
                {
                    if (player.UserInformation.Nickname.IndexOf(args, StringComparison.OrdinalIgnoreCase) == -1)
                        continue;

                    string secondString = player.UserInformation.Nickname;
                    int nameDifference = secondString.Length - firstString.Length;

                    if (nameDifference >= lastnameDifference)
                        continue;

                    lastnameDifference = nameDifference;
                    playerFound = player;
                }
            }

            if (playerFound is not null)
                Internal.Fields.Player.Args[args] = playerFound;

            return playerFound;
        }
        catch (Exception ex)
        {
            Log.Error($"[API.Player.Get(string)] Error: {ex}");
            return null;
        }
    }

    #endregion


    #region Player

    public static Player? GetAttacker(this DamageHandlerBase handler)
    {
        return handler is AttackerDamageHandler adh ? adh.Attacker.Hub.GetPlayer() : null;
    }

    public static float DistanceTo(this Player source, Player player)
    {
        return Vector3.Distance(source.MovementState.Position, player.MovementState.Position);
    }

    public static float DistanceTo(this Player source, Vector3 position)
    {
        return Vector3.Distance(source.MovementState.Position, position);
    }

    public static float DistanceTo(this Player source, GameObject target)
    {
        return Vector3.Distance(source.MovementState.Position, target.transform.position);
    }

    public static HashSet<Player> GetLookingAtPlayers(this Player player, float distance = 0)
    {
        HashSet<Player> hash = [];

        foreach (Player target in Player.List)
        {
            if (player == target)
                continue;

            if (target.RoleInformation.Team is Team.Dead)
                continue;

            if (target.Effects.CheckActive<Invisible>())
                continue;

            if (target.RoleInformation.Base is not IFpcRole fpcRole)
                continue;

            if (!VisionInformation.GetVisionInformation(
                    player.ReferenceHub,
                    player.CameraTransform,
                    fpcRole.FpcModule.Position,
                    fpcRole.FpcModule.CharacterControllerSettings.Radius,
                    distance, true, true, 0, false).IsLooking)
                continue;

            hash.Add(target);
        }

        return hash;
    }

    public static Player? GetLookingAtPlayer(this Player player, float distance = 0)
    {
        foreach (Player target in Player.List)
        {
            if (player == target)
                continue;

            if (target.RoleInformation.Team is Team.Dead)
                continue;

            if (target.Effects.CheckActive<Invisible>())
                continue;

            if (target.RoleInformation.Base is not IFpcRole fpcRole)
                continue;

            if (!VisionInformation.GetVisionInformation(
                    player.ReferenceHub,
                    player.CameraTransform,
                    fpcRole.FpcModule.Position,
                    fpcRole.FpcModule.CharacterControllerSettings.Radius,
                    distance, true, true, 0, false).IsLooking)
                continue;

            return target;
        }

        return null;
    }

    public static EscapeEvent InvokeEscape(this Player pl, RoleTypeId role)
    {
        EscapeEvent @event = new(pl, role);
        @event.InvokeEvent();

        return @event;
    }

    #endregion


    #region Damages

    public static DamagePrimitiveTypes GetLiteDamageTypes(this DamageHandlerBase handler)
    {
        return handler switch
        {
            CustomReasonDamageHandler _ => DamagePrimitiveTypes.Custom,
            DisruptorDamageHandler _ => DamagePrimitiveTypes.Disruptor,
            ExplosionDamageHandler _ => DamagePrimitiveTypes.Explosion,
            FirearmDamageHandler _ => DamagePrimitiveTypes.Gun,
            JailbirdDamageHandler _ => DamagePrimitiveTypes.Jailbird,
            MicroHidDamageHandler _ => DamagePrimitiveTypes.MicroHid,
            RecontainmentDamageHandler _ => DamagePrimitiveTypes.Recontainment,
            Scp018DamageHandler _ => DamagePrimitiveTypes.Scp018,
            Scp049DamageHandler _ => DamagePrimitiveTypes.Scp049,
            Scp096DamageHandler _ => DamagePrimitiveTypes.Scp096,
            ScpDamageHandler _ => DamagePrimitiveTypes.ScpDamage,
            UniversalDamageHandler _ => DamagePrimitiveTypes.Universal,
            WarheadDamageHandler _ => DamagePrimitiveTypes.Warhead,
            //SnowballDamageHandler _ => LiteDamageTypes.Snowball,
            _ => DamagePrimitiveTypes.Unknown
        };
    }

    internal static readonly Dictionary<DamageHandlerBase, DamageTypes> DamagesCached = [];

    public static DamageTypes GetDamageType(this DamageHandlerBase handler)
    {
        if (DamagesCached.TryGetValue(handler, out DamageTypes damageType))
            return damageType;

        DamageTypes type = GetType();
        DamagesCached.Add(handler, type);
        return type;

        DamageTypes GetType()
        {
            return handler switch
            {
                CustomReasonDamageHandler _ => DamageTypes.Custom,
                DisruptorDamageHandler _ => DamageTypes.Disruptor,
                ExplosionDamageHandler _ => DamageTypes.Explosion,
                JailbirdDamageHandler _ => DamageTypes.Jailbird,
                MicroHidDamageHandler _ => DamageTypes.MicroHid,

                //SnowballDamageHandler _ => DamageTypes.Snowball,

                FirearmDamageHandler fr => fr.WeaponType switch
                {
                    ItemType.GunCOM15 => DamageTypes.Com15,
                    ItemType.GunCOM18 => DamageTypes.Com18,
                    ItemType.GunCom45 => DamageTypes.Com45,
                    ItemType.GunRevolver => DamageTypes.Revolver,

                    ItemType.GunFSP9 => DamageTypes.FSP9,
                    ItemType.GunCrossvec => DamageTypes.CrossVec,

                    ItemType.GunE11SR => DamageTypes.E11SR,
                    ItemType.GunFRMG0 => DamageTypes.FRMG0,

                    ItemType.GunAK => DamageTypes.AK,
                    ItemType.GunLogicer => DamageTypes.Logicer,
                    ItemType.GunShotgun => DamageTypes.Shotgun,
                    ItemType.GunA7 => DamageTypes.A7,

                    ItemType.ParticleDisruptor => DamageTypes.Disruptor,
                    ItemType.Jailbird => DamageTypes.Jailbird,

                    _ => DamageTypes.Unknown
                },

                RecontainmentDamageHandler _ => DamageTypes.Recontainment,
                Scp018DamageHandler _ => DamageTypes.Scp018,
                Scp049DamageHandler _ => DamageTypes.Scp049,
                Scp096DamageHandler _ => DamageTypes.Scp096,
                WarheadDamageHandler _ => DamageTypes.Warhead,
                ScpDamageHandler sr => ParseTranslation(sr._translationId),
                UniversalDamageHandler tr => ParseTranslation(tr.TranslationId),
                _ => DamageTypes.Unknown
            };
        }

        DamageTypes ParseTranslation(byte translationId)
        {
            return translationId switch
            {
                0 => DamageTypes.Recontainment,
                1 => DamageTypes.Warhead,
                2 => DamageTypes.Scp049,
                4 => DamageTypes.Asphyxiation,
                5 => DamageTypes.Bleeding,
                6 => DamageTypes.Falldown,
                7 => DamageTypes.Pocket,
                8 => DamageTypes.Decontamination,
                9 => DamageTypes.Poison,
                10 => DamageTypes.Scp207,
                11 => DamageTypes.SeveredHands,
                12 => DamageTypes.MicroHid,
                13 => DamageTypes.Tesla,
                14 => DamageTypes.Explosion,
                15 => DamageTypes.Scp096,
                16 => DamageTypes.Scp173,
                17 => DamageTypes.Scp939Lunge,
                18 => DamageTypes.Zombie,
                20 => DamageTypes.Crushed,
                22 => DamageTypes.FriendlyFireDetector,
                23 => DamageTypes.Hypothermia,
                24 => DamageTypes.CardiacArrest,
                25 => DamageTypes.Scp939,
                26 => DamageTypes.Scp3114,
                27 => DamageTypes.MarshmallowMan,
                28 => DamageTypes.Scp1507,
                _ => DamageTypes.Unknown
            };
        }
    }

    #endregion


    #region Prefabs

    public static DoorVariant GetPrefab(this DoorPrefabs prefab)
    {
        return Prefabs.Doors.TryGetValue(prefab, out DoorVariant? door) ? door : Prefabs.Doors.First().Value;
    }

    public static Locker GetPrefab(this LockerPrefabs prefab)
    {
        if (prefab is LockerPrefabs.Pedestal)
            prefab = Random.Range(0, 100) switch
            {
                > 80 => LockerPrefabs.Pedestal268,
                > 60 => LockerPrefabs.Pedestal207,
                > 40 => LockerPrefabs.Pedestal500,
                > 20 => LockerPrefabs.Pedestal018,
                _ => LockerPrefabs.Pedestal2176
            };

        return Prefabs.Lockers.TryGetValue(prefab, out var locker)
            ? locker
            : Prefabs.Lockers.First().Value;
    }

    #endregion


    #region Items

    public static ItemCategory GetCategory(this ItemType itemType)
    {
        return itemType switch
        {
            ItemType.KeycardJanitor or ItemType.KeycardScientist or ItemType.KeycardResearchCoordinator or
                ItemType.KeycardZoneManager or ItemType.KeycardGuard or ItemType.KeycardContainmentEngineer or
                ItemType.KeycardMTFPrivate or ItemType.KeycardMTFOperative or ItemType.KeycardMTFCaptain or
                ItemType.KeycardFacilityManager or ItemType.KeycardChaosInsurgency
                or ItemType.KeycardO5 => ItemCategory.Keycard,

            ItemType.Radio => ItemCategory.Radio,
            ItemType.MicroHID or ItemType.ParticleDisruptor => ItemCategory.SpecialWeapon,

            ItemType.Medkit or ItemType.Adrenaline or ItemType.Painkillers => ItemCategory.Medical,

            ItemType.GunCOM15 or ItemType.GunE11SR or ItemType.GunCrossvec or ItemType.GunFSP9 or
                ItemType.GunLogicer or ItemType.GunCOM18 or ItemType.GunRevolver or ItemType.GunAK or
                ItemType.GunShotgun or ItemType.GunCom45 or ItemType.Jailbird
                or ItemType.GunFRMG0 or ItemType.GunA7 => ItemCategory.Firearm,

            ItemType.GrenadeHE or ItemType.GrenadeFlash => ItemCategory.Grenade,

            ItemType.SCP500 or ItemType.SCP207 or ItemType.SCP018 or ItemType.SCP268 or ItemType.SCP330 or
                ItemType.SCP2176 or ItemType.SCP244a or ItemType.SCP244b or ItemType.SCP1853 or ItemType.SCP1576 or
                ItemType.AntiSCP207 => ItemCategory.SCPItem,

            ItemType.Ammo12gauge or ItemType.Ammo556x45 or ItemType.Ammo44cal or ItemType.Ammo762x39
                or ItemType.Ammo9x19 => ItemCategory.Ammo,

            ItemType.ArmorLight or ItemType.ArmorCombat or ItemType.ArmorHeavy => ItemCategory.Armor,

            _ => ItemCategory.None
        };
    }

    internal static AmmoTypes GetAmmoType(this ItemType itemType)
    {
        return itemType switch
        {
            ItemType.Ammo556x45 => AmmoTypes.Ammo556,
            ItemType.Ammo762x39 => AmmoTypes.Ammo762,
            ItemType.Ammo9x19 => AmmoTypes.Ammo9,
            ItemType.Ammo12gauge => AmmoTypes.Ammo12Gauge,
            ItemType.Ammo44cal => AmmoTypes.Ammo44Cal,
            _ => AmmoTypes.None
        };
    }

    internal static ItemType GetItemType(this AmmoTypes ammoType)
    {
        return ammoType switch
        {
            AmmoTypes.Ammo556 => ItemType.Ammo556x45,
            AmmoTypes.Ammo762 => ItemType.Ammo762x39,
            AmmoTypes.Ammo9 => ItemType.Ammo9x19,
            AmmoTypes.Ammo12Gauge => ItemType.Ammo12gauge,
            AmmoTypes.Ammo44Cal => ItemType.Ammo44cal,
            _ => ItemType.None
        };
    }

    internal static ItemBase CreateItemInstance(this ItemType type, Player? owner = null)
    {
        ItemIdentifier identifier = new(type, ItemSerialGenerator.GenerateNext());

        owner ??= Server.Host;

        return owner.Inventory.Base.CreateItemInstance(identifier, false);
    }

    #endregion


    #region Effects

    public static Type Type(this EffectType effect)
    {
        return effect switch
        {
            EffectType.AmnesiaItems => typeof(AmnesiaItems),
            EffectType.AmnesiaVision => typeof(AmnesiaVision),
            EffectType.AntiScp207 => typeof(AntiScp207),
            EffectType.Asphyxiated => typeof(Asphyxiated),
            EffectType.BecomingFlamingo => typeof(BecomingFlamingo),
            EffectType.Bleeding => typeof(Bleeding),
            EffectType.Blindness => typeof(Blindness),
            EffectType.BodyshotReduction => typeof(BodyshotReduction),
            EffectType.Burned => typeof(Burned),
            EffectType.CardiacArrest => typeof(CardiacArrest),
            EffectType.Concussed => typeof(Concussed),
            EffectType.Corroding => typeof(Corroding),
            EffectType.DamageReduction => typeof(DamageReduction),
            EffectType.Deafened => typeof(Deafened),
            EffectType.Decontaminating => typeof(Decontaminating),
            EffectType.Disabled => typeof(Disabled),
            EffectType.Ensnared => typeof(Ensnared),
            EffectType.Exhausted => typeof(Exhausted),
            EffectType.Flashed => typeof(Flashed),
            EffectType.FogControl => typeof(FogControl),
            EffectType.Ghostly => typeof(Ghostly),
            EffectType.Hemorrhage => typeof(Hemorrhage),
            EffectType.Hypothermia => typeof(Hypothermia),
            EffectType.InsufficientLighting => typeof(InsufficientLighting),
            EffectType.Invigorated => typeof(Invigorated),
            EffectType.Invisible => typeof(Invisible),
            EffectType.MovementBoost => typeof(MovementBoost),
            EffectType.PocketCorroding => typeof(PocketCorroding),
            EffectType.Poisoned => typeof(Poisoned),
            EffectType.RainbowTaste => typeof(RainbowTaste),
            EffectType.Scanned => typeof(Scanned),
            EffectType.Scp1853 => typeof(Scp1853),
            EffectType.Scp207 => typeof(Scp207),
            EffectType.SeveredHands => typeof(SeveredHands),
            EffectType.SilentWalk => typeof(SilentWalk),
            EffectType.Sinkhole => typeof(Sinkhole),
            EffectType.Slowness => typeof(Slowness),
            EffectType.SoundtrackMute => typeof(SoundtrackMute),
            EffectType.SpawnProtected => typeof(SpawnProtected),
            EffectType.Stained => typeof(Stained),
            EffectType.Strangled => typeof(Strangled),
            EffectType.Snowed => typeof(Snowed),
            EffectType.Traumatized => typeof(Traumatized),
            EffectType.Vitality => typeof(Vitality),

            _ => throw new InvalidOperationException("Invalid effect enum provided")
        };
    }

    public static EffectType GetEffectType(this StatusEffectBase ef)
    {
        return ef switch
        {
            AmnesiaItems => EffectType.AmnesiaItems,
            AmnesiaVision => EffectType.AmnesiaVision,
            AntiScp207 => EffectType.AntiScp207,
            Asphyxiated => EffectType.Asphyxiated,
            BecomingFlamingo => EffectType.BecomingFlamingo,
            Bleeding => EffectType.Bleeding,
            Blindness => EffectType.Blindness,
            BodyshotReduction => EffectType.BodyshotReduction,
            Burned => EffectType.Burned,
            CardiacArrest => EffectType.CardiacArrest,
            Concussed => EffectType.Concussed,
            Corroding => EffectType.Corroding,
            DamageReduction => EffectType.DamageReduction,
            Deafened => EffectType.Deafened,
            Decontaminating => EffectType.Decontaminating,
            Disabled => EffectType.Disabled,
            Ensnared => EffectType.Ensnared,
            Exhausted => EffectType.Exhausted,
            Flashed => EffectType.Flashed,
            FogControl => EffectType.FogControl,
            Ghostly => EffectType.Ghostly,
            Hemorrhage => EffectType.Hemorrhage,
            Hypothermia => EffectType.Hypothermia,
            InsufficientLighting => EffectType.InsufficientLighting,
            Invigorated => EffectType.Invigorated,
            Invisible => EffectType.Invisible,
            MovementBoost => EffectType.MovementBoost,
            PocketCorroding => EffectType.PocketCorroding,
            Poisoned => EffectType.Poisoned,
            RainbowTaste => EffectType.RainbowTaste,
            Scanned => EffectType.Scanned,
            Scp1853 => EffectType.Scp1853,
            Scp207 => EffectType.Scp207,
            SeveredHands => EffectType.SeveredHands,
            SilentWalk => EffectType.SilentWalk,
            Sinkhole => EffectType.Sinkhole,
            Slowness => EffectType.Slowness,
            SoundtrackMute => EffectType.SoundtrackMute,
            SpawnProtected => EffectType.SpawnProtected,
            Stained => EffectType.Stained,
            Strangled => EffectType.Strangled,
            Snowed => EffectType.Snowed,
            Traumatized => EffectType.Traumatized,
            Vitality => EffectType.Vitality,

            _ => EffectType.None
        };
    }

    #endregion

    #region Spawnpoints

    public static Vector3 GetPosition(this RoleTypeId role)
    {
        return GetSpawnPoint(role).Position;
    }

    public static Transform GetSpawnTransform(this RoleTypeId role)
    {
        GameObject obj = new("SpawnPoint (Clone)")
        {
            transform =
            {
                position = GetSpawnPoint(role).Position
            }
        };
        return obj.transform;
    }

    public static SpawnPoint GetSpawnPoint(this RoleTypeId role)
    {
        PlayerRoleBase? roleBase = Server.Host.ReferenceHub.roleManager.GetRoleBase(role);

        if (roleBase is not IFpcRole fpc)
            return new SpawnPoint(Vector3.zero, 0);

        if (fpc.SpawnpointHandler is null)
            return new SpawnPoint(Vector3.zero, 0);

        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (!fpc.SpawnpointHandler.TryGetSpawnpoint(out Vector3 pos, out float horizontal))
            return new SpawnPoint(Vector3.zero, 0);

        return new SpawnPoint(pos, horizontal);
    }

    #endregion

    #region Transform

    public static Vector3 SafeGetParentPosition(this Transform transform)
    {
        var parentPosition = Vector3.zero;
        if (transform.parent) parentPosition = transform.parent.position;
        return parentPosition;
    }

    public static Quaternion SafeGetParentRotation(this Transform transform)
    {
        var parentRotation = Quaternion.identity;
        if (transform.parent) parentRotation = transform.parent.rotation;
        return parentRotation;
    }

    public static Vector3 SafeGetParentScale(this Transform transform)
    {
        var parentScale = Vector3.one;
        if (transform.parent) parentScale = transform.parent.position;
        return parentScale;
    }

    #endregion
}