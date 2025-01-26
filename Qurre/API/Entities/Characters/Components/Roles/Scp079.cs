using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using MapGeneration;
using Mirror;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp079;
using PlayerRoles.PlayableScps.Scp079.Pinging;
using PlayerRoles.Subroutines;
using Qurre.API.Core;
using Qurre.API.Entities.Environment;
using Qurre.API.Enums;
using Qurre.API.Exceptions;
using RelativePositioning;
using UnityEngine;

namespace Qurre.API.Entities.Characters.Components.Roles;

[PublicAPI]
public sealed class Scp079 : Role
{
    /// <exception cref="ObjectDestroyedException" />
    internal Scp079(Scp079Role scp079RoleBase) : base(scp079RoleBase)
    {
        Base = scp079RoleBase;
        SubroutineManager = scp079RoleBase.SubroutineModule;

        if (!SubroutineManager.Instance.TryGetSubroutine(out Scp079AuxManager auxManager))
            Log.Error("[Roles > Scp079] >> AuxManager is null");

        if (!SubroutineManager.Instance.TryGetSubroutine(out Scp079BlackoutRoomAbility blackoutRoomAbility))
            Log.Error("[Roles > Scp079] >> BlackoutRoomAbility is null");

        if (!SubroutineManager.Instance.TryGetSubroutine(out Scp079BlackoutZoneAbility blackoutZoneAbility))
            Log.Error("[Roles > Scp079] >> BlackoutZoneAbility is null");

        if (!SubroutineManager.Instance.TryGetSubroutine(out Scp079DoorLockChanger doorLockChanger))
            Log.Error("[Roles > Scp079] >> DoorLockChanger is null");

        if (!SubroutineManager.Instance.TryGetSubroutine(out Scp079DoorLockReleaser doorLockReleaser))
            Log.Error("[Roles > Scp079] >> DoorLockReleaser is null");

        if (!SubroutineManager.Instance.TryGetSubroutine(out Scp079DoorStateChanger doorStateChanger))
            Log.Error("[Roles > Scp079] >> DoorStateChanger is null");

        if (!SubroutineManager.Instance.TryGetSubroutine(out Scp079ElevatorStateChanger elevatorStateChanger))
            Log.Error("[Roles > Scp079] >> ElevatorStateChanger is null");

        if (!SubroutineManager.Instance.TryGetSubroutine(out Scp079LockdownRoomAbility lockdownRoomAbility))
            Log.Error("[Roles > Scp079] >> LockdownRoomAbility is null");

        if (!SubroutineManager.Instance.TryGetSubroutine(out Scp079LostSignalHandler lostSignalHandler))
            Log.Error("[Roles > Scp079] >> LostSignalHandler is null");

        if (!SubroutineManager.Instance.TryGetSubroutine(out Scp079ScannerMenuToggler scannerMenuToggler))
            Log.Error("[Roles > Scp079] >> ScannerMenuToggler is null");

        if (!SubroutineManager.Instance.TryGetSubroutine(out Scp079ScannerTeamFilterSelector scannerTeamFilterSelector))
            Log.Error("[Roles > Scp079] >> ScannerTeamFilterSelector is null");

        if (!SubroutineManager.Instance.TryGetSubroutine(out Scp079ScannerTracker scannerTracker))
            Log.Error("[Roles > Scp079] >> ScannerTracker is null");

        if (!SubroutineManager.Instance.TryGetSubroutine(out Scp079ScannerZoneSelector scannerZoneSelector))
            Log.Error("[Roles > Scp079] >> ScannerZoneSelector is null");

        if (!SubroutineManager.Instance.TryGetSubroutine(out Scp079SpeakerAbility speakerAbility))
            Log.Error("[Roles > Scp079] >> SpeakerAbility is null");

        if (!SubroutineManager.Instance.TryGetSubroutine(out Scp079TeslaAbility teslaAbility))
            Log.Error("[Roles > Scp079] >> TeslaAbility is null");

        if (!SubroutineManager.Instance.TryGetSubroutine(out Scp079PingAbility pingAbility))
            Log.Error("[Roles > Scp079] >> PingAbility is null");

        if (!SubroutineManager.Instance.TryGetSubroutine(out Scp079TierManager tierManager))
            Log.Error("[Roles > Scp079] >> TierManager is null");

        // Aux Manager
        AuxManager = auxManager;

        // Blackout Manager
        BlackoutRoomAbility = blackoutRoomAbility;
        BlackoutZoneAbility = blackoutZoneAbility;

        // Facility Manager
        DoorLockChanger = doorLockChanger;
        DoorLockReleaser = doorLockReleaser;
        DoorStateChanger = doorStateChanger;
        ElevatorStateChanger = elevatorStateChanger;
        LockdownRoomAbility = lockdownRoomAbility;

        // Lost Signal Manager
        LostSignalHandler = lostSignalHandler;

        // Tesla Manager
        TeslaAbility = teslaAbility;

        // Ping Manager
        PingAbility = pingAbility;

        // Recontain Manager
        // -

        // Scanner Manager
        ScannerMenuToggler = scannerMenuToggler;
        ScannerTeamFilterSelector = scannerTeamFilterSelector;
        ScannerTracker = scannerTracker;
        ScannerZoneSelector = scannerZoneSelector;

        // Speaker Manager
        SpeakerAbility = speakerAbility;

        // Tier Manager
        TierManager = tierManager;
    }

    #region Properties

    public new UnityObjectWrapper<Scp079Role> Base { get; }

    public UnityObjectWrapper<SubroutineManagerModule> SubroutineManager { get; }

    /// <exception cref="ObjectDestroyedException" />
    public IScp079Camera CurrentCamera
    {
        get => EntityManager.Get<IScp079Camera>(Base.Instance.CurrentCamera) ?? throw new ObjectDestroyedException();
        set => Base.Instance._curCamSync.CurrentCamera = value.Base.Instance;
    }

    #endregion

    #region Aux Manager

    public UnityObjectWrapper<Scp079AuxManager> AuxManager { get; }

    /// <summary>
    ///     Получить примерное количество секунд, через которое текущее количество
    ///     энергии будет полностью восполнено.
    /// </summary>
    /// <seealso cref="CalculateRegenerateEta(float)" />
    public float RemainingEnergyRegenerateEta => CalculateRegenerateEta(MaxEnergyOnCurrentTier - Energy);

    /// <summary>
    ///     Получить примерное количество секунд, затрачиваемых для полного восстановления
    ///     энергии с нуля.
    /// </summary>
    /// <seealso cref="CalculateRegenerateEta(float)" />
    public float FullEnergyRegenerateFromZeroEta => CalculateRegenerateEta(MaxEnergyOnCurrentTier);

    /// <exception cref="ObjectDestroyedException" />
    public float Energy
    {
        get => AuxManager.Instance.CurrentAux;
        set => AuxManager.Instance.CurrentAux = value;
    }

    /// <summary>
    ///     Максимальное кол-во энергии на разных уровнях.
    /// </summary>
    /// <exception cref="ObjectDestroyedException" />
    public float[] MaxEnergyPerTier
    {
        get => AuxManager.Instance._maxPerTier;
        set => AuxManager.Instance._maxPerTier = value;
    }

    /// <summary>
    ///     Максимальное кол-во энергии на текущем уровне.
    /// </summary>
    /// <exception cref="ObjectDestroyedException" />
    public float MaxEnergyOnCurrentTier
    {
        get => MaxEnergyPerTier[TierIndex];
        set => MaxEnergyPerTier[TierIndex] = value;
    }

    /// <summary>
    ///     Скорость регенерации энергии на разных уровнях.
    /// </summary>
    /// <exception cref="ObjectDestroyedException" />
    public float[] EnergyRegenerationPerTier
    {
        get => AuxManager.Instance._regenerationPerTier;
        set => AuxManager.Instance._regenerationPerTier = value;
    }

    /// <summary>
    ///     Скорость регенерации энергии на текущем уровне.
    /// </summary>
    /// <exception cref="ObjectDestroyedException" />
    public float EnergyRegenerationOnCurrentTier
    {
        get => EnergyRegenerationPerTier[TierIndex];
        set => EnergyRegenerationPerTier[TierIndex] = value;
    }

    /// <summary>
    ///     Посчитать примерное количество секунд,
    ///     затрачиваемых для регенерации определённого количества энергии.
    /// </summary>
    /// <param name="energyAmount">Количество энергии</param>
    /// <returns>Время в секундах</returns>
    public float CalculateRegenerateEta(float energyAmount)
    {
        var regenerationSpeed = EnergyRegenerationOnCurrentTier;
        if (regenerationSpeed == 0) return float.PositiveInfinity;
        return Mathf.Max(energyAmount, 0.0F) / regenerationSpeed;
    }

    #endregion

    #region Blackout Manager

    public UnityObjectWrapper<Scp079BlackoutRoomAbility> BlackoutRoomAbility { get; }

    public UnityObjectWrapper<Scp079BlackoutZoneAbility> BlackoutZoneAbility { get; }

    #endregion

    #region Facility Manager

    public UnityObjectWrapper<Scp079DoorLockChanger> DoorLockChanger { get; }

    public UnityObjectWrapper<Scp079DoorLockReleaser> DoorLockReleaser { get; }

    public UnityObjectWrapper<Scp079DoorStateChanger> DoorStateChanger { get; }

    public UnityObjectWrapper<Scp079ElevatorStateChanger> ElevatorStateChanger { get; }

    public UnityObjectWrapper<Scp079LockdownRoomAbility> LockdownRoomAbility { get; }

    #endregion

    #region Lost Signal Manager

    public UnityObjectWrapper<Scp079LostSignalHandler> LostSignalHandler { get; }

    /// <summary>
    ///     Получает или задаёт оставшееся время "потери сигнала" в секундах.
    /// </summary>
    /// <exception cref="ObjectDestroyedException" />
    /// <seealso cref="LostSignal(double)" />
    public double LostSignalTimer
    {
        get => Math.Max(LostSignalHandler.Instance._recoveryTime - NetworkTime.time, 0);
        set => LostSignal(value);
    }

    /// <exception cref="ObjectDestroyedException" />
    /// <seealso cref="LostSignal(double)" />
    public void LostSignal(float duration)
    {
        LostSignalHandler.Instance.ServerLoseSignal(duration);
    }

    /// <exception cref="ObjectDestroyedException" />
    /// <seealso cref="LostSignal(float)" />
    public void LostSignal(double duration)
    {
        LostSignalHandler.Instance._recoveryTime = NetworkTime.time + duration;
        LostSignalHandler.Instance.ServerSendRpc(true);
    }

    #endregion

    #region Ping Manager

    public UnityObjectWrapper<Scp079PingAbility> PingAbility { get; }

    /// <exception cref="ObjectDestroyedException" />
    public bool IsPingReady => PingAbility.Instance.IsReady;

    /// <summary>
    ///     Создать "пинг" точку SCP-079.
    /// </summary>
    /// <exception cref="ObjectDestroyedException" />
    public void Ping(Vector3 position, Scp079PingTypes pingType = Scp079PingTypes.Default, Vector3 normal = default,
        bool local = false)
    {
        // source: PlayerRoles.PlayableScps.Scp079.Pinging.Scp079PingAbility.WriteSyncVars(NetworkWriter)
        // and: PlayerRoles.PlayableScps.Scp079.Pinging.Scp079PingAbility.ServerProcessCmd(NetworkReader)

        PingAbility.Instance._syncProcessorIndex = (byte)pingType;
        PingAbility.Instance._syncPos = new RelativePosition(position);
        PingAbility.Instance._syncNormal = normal;

        if (local)
        {
            PingAbility.Instance.ServerSendRpc(false);
            return;
        }

        PingAbility.Instance.ServerSendRpc(
            hub => PingAbility.Instance.ServerCheckReceiver(hub, position, (int)pingType));
    }

    #endregion

    #region Recontain Manager

    // TODO: добавить Recontain Manager

    #endregion

    #region Scanner Manager

    public UnityObjectWrapper<Scp079ScannerMenuToggler> ScannerMenuToggler { get; }

    public UnityObjectWrapper<Scp079ScannerTeamFilterSelector> ScannerTeamFilterSelector { get; }

    public UnityObjectWrapper<Scp079ScannerTracker> ScannerTracker { get; }

    public UnityObjectWrapper<Scp079ScannerZoneSelector> ScannerZoneSelector { get; }

    /// <exception cref="ObjectDestroyedException" />
    public bool IsScannerReady => ScannerMenuToggler.Instance.IsReady;

    /// <exception cref="ObjectDestroyedException" />
    public bool IsScannerOpen
    {
        get => ScannerMenuToggler.Instance.IsVisible;
        set => ScannerMenuToggler.Instance.SyncState = value;
    }

    /// <exception cref="ObjectDestroyedException" />
    /// <seealso cref="SetScannerTeamFilter(System.Collections.Generic.HashSet{PlayerRoles.Team})" />
    public IReadOnlyList<Team> ScannerTeamFilter => ScannerTeamFilterSelector.Instance.SelectedTeams;

    /// <exception cref="ObjectDestroyedException" />
    public Scp079ScannerSequence ScannerSequence => ScannerTracker.Instance._sequence;

    /// <exception cref="ObjectDestroyedException" />
    public IReadOnlyList<FacilityZone> ScannerZoneFilter => ScannerZoneSelector.Instance.SelectedZones;

    /// <exception cref="ObjectDestroyedException" />
    public void SetScannerTeamFilter(HashSet<Team> teams)
    {
        var availableFilters = ScannerTeamFilterSelector.Instance._availableFilters;

        for (var i = 0; i < availableFilters.Length; i++)
            ScannerTeamFilterSelector.Instance._selectedTeams[i] = teams.Contains(availableFilters[i]);

        ScannerTeamFilterSelector.Instance.ServerSendRpc(true);
    }

    /// <exception cref="ObjectDestroyedException" />
    public void SetScannerZoneFilter(HashSet<FacilityZone> zones)
    {
        var availableFilters = Scp079ScannerZoneSelector.AllZones;

        for (var i = 0; i < availableFilters.Length; i++)
            ScannerZoneSelector.Instance._selectedZones[i] = zones.Contains(availableFilters[i]);

        ScannerZoneSelector.Instance.ServerSendRpc(true);
    }

    #endregion

    #region Speaker Manager

    public UnityObjectWrapper<Scp079SpeakerAbility> SpeakerAbility { get; }

    /// <exception cref="ObjectDestroyedException" />
    public bool IsSpeakerReady => SpeakerAbility.Instance.IsReady;

    /// <exception cref="ObjectDestroyedException" />
    public bool IsSpeakerInUse => SpeakerAbility.Instance.IsUsingSpeaker;

    #endregion

    #region Tesla Manager

    public UnityObjectWrapper<Scp079TeslaAbility> TeslaAbility { get; }

    /// <exception cref="ObjectDestroyedException" />
    public bool IsTeslaReady => TeslaAbility.Instance.IsReady;

    /// <exception cref="ObjectDestroyedException" />
    public float TeslaDefaultCooldown
    {
        get => TeslaAbility.Instance._cooldown;
        set => TeslaAbility.Instance._cooldown = value;
    }

    /// <exception cref="ObjectDestroyedException" />
    public double TeslaCooldown
    {
        get => Math.Clamp(TeslaAbility.Instance._nextUseTime - NetworkTime.time, 0, TeslaDefaultCooldown);
        set
        {
            TeslaAbility.Instance._nextUseTime = Math.Max(value, 0) + NetworkTime.time;
            TeslaAbility.Instance.ServerSendRpc(false);
        }
    }

    #endregion

    #region Tier Manager

    public UnityObjectWrapper<Scp079TierManager> TierManager { get; }

    /// <summary>
    ///     Получает общее количество опыта для каждого из игровых уровней по его индексу.
    ///     От 0 до 4!
    /// </summary>
    /// <exception cref="ObjectDestroyedException" />
    /// <seealso cref="TierIndex" />
    public int[] LeveUpThresholds => TierManager.Instance.AbsoluteThresholds;

    /// <summary>
    ///     Получает общее количество опыта, которое необходимо иметь,
    ///     чтобы увеличить уровень SCP-079.
    /// </summary>
    /// <exception cref="ObjectDestroyedException" />
    /// <seealso cref="TotalExperienceNeedForLevelUp" />
    public int TotalExperienceNeedForLevelUp => LeveUpThresholds[TierIndex];

    /// <summary>
    ///     Получает количество опыта, которое необходимо получить,
    ///     чтобы увеличить уровень SCP-079.
    /// </summary>
    /// <exception cref="ObjectDestroyedException" />
    /// <seealso cref="TotalExperience" />
    /// <seealso cref="TierExperience" />
    public int ExperienceLeftForNextLevel => TotalExperienceNeedForLevelUp - TotalExperience;

    /// <summary>
    ///     Получает или задаёт общее количество опыта SCP-079.
    /// </summary>
    /// <exception cref="ObjectDestroyedException" />
    /// <seealso cref="TierExperience" />
    /// <seealso cref="ExperienceLeftForNextLevel" />
    public int TotalExperience
    {
        get => TierManager.Instance.TotalExp;
        set => TierManager.Instance.TotalExp = value;
    }

    /// <summary>
    ///     Получает или задаёт количество опыта относительно текущего уровня SCP-079.
    /// </summary>
    /// <exception cref="ObjectDestroyedException" />
    /// <seealso cref="TotalExperience" />
    /// <seealso cref="ExperienceLeftForNextLevel" />
    public int TierExperience
    {
        get => TierManager.Instance.RelativeExp;
        set
        {
            var totalWithoutRelative = TotalExperience - TierExperience;
            TotalExperience = totalWithoutRelative + value;
        }
    }

    /// <summary>
    ///     Получает или задаёт индекс текущего уровня SCP-079.
    ///     От 0 до 4.
    /// </summary>
    /// <exception cref="ObjectDestroyedException" />
    /// <seealso cref="Level" />
    public int TierIndex
    {
        get => TierManager.Instance.AccessTierIndex;
        set
        {
            if (value == 0)
            {
                TotalExperience = 0;
                return;
            }

            var levelUpThresholdIndex = Math.Clamp(value - 1, 0, LeveUpThresholds.Length);
            TotalExperience = LeveUpThresholds[levelUpThresholdIndex];
        }
    }

    /// <summary>
    ///     Получает или задаёт текущий уровень SCP-079.
    ///     От 1 до 5.
    /// </summary>
    /// <exception cref="ObjectDestroyedException" />
    /// <seealso cref="TierIndex" />
    public int Level
    {
        get => TierIndex + 1;
        set => TierIndex = value - 1;
    }

    #endregion
}