using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using MapGeneration.Distributors;
using Mirror;
using Qurre.API.Controllers.Components;
using Qurre.API.Controllers.Structs;
using Qurre.API.Objects;
using UnityEngine;

namespace Qurre.API.Controllers;

using BaseLocker = MapGeneration.Distributors.Locker;

[PublicAPI]
public class Locker : GeneratedNetworkEntity<BaseLocker, Locker>
{
    protected sealed override BaseLocker UnsafeBase { get; }

    private LockerType _typeCached = LockerType.Unknown;

    private Locker(BaseLocker locker)
    {
        UnsafeBase = locker;
        Chambers = Base.Chambers.Select(x => new Chamber(x, this)).ToArray();
        
        BaseToWrap[Base] = this;
        AddEntityLink();
    }

    public Locker(Vector3 position, LockerPrefabs type, Quaternion? rotation = null)
    {
        PrefabType = type;

        UnsafeBase = Object.Instantiate(type.GetPrefab());

        Transform.position = position;
        Transform.rotation = rotation ?? new Quaternion();

        Chambers = Base.Chambers.Select(lc => new Chamber(lc, this)).ToArray();

        Spawn();
        
        BaseToWrap[Base] = this;
        AddEntityLink();
    }

    public LockerPrefabs PrefabType { get; init; }

    public Chamber[] Chambers { get; private set; }

    public LockerLoot[] Loot => Base.Loot;
    public AudioClip GrantedBeep => Base._grantedBeep;
    public AudioClip DeniedBeep => Base._deniedBeep;
    public string Name => Base.name;

    public LockerType Type
    {
        get
        {
            if (_typeCached is LockerType.Unknown) _typeCached = GetLockerType();
            return _typeCached;

            LockerType GetLockerType()
            {
                if (Name.Contains("AdrenalineMedkit")) return LockerType.AdrenalineMedkit;
                if (Name.Contains("RegularMedkit")) return LockerType.RegularMedkit;
                if (Name.Contains("Pedestal")) return LockerType.Pedestal;
                if (Name.Contains("MiscLocker")) return LockerType.MiscLocker;
                if (Name.Contains("RifleRack")) return LockerType.RifleRack;
                if (Name.Contains("LargeGunLocker")) return LockerType.LargeGun;
                return LockerType.Unknown;
            } // end void 'Get'
        } // end Type_get
    } // end field

    public static Locker? Get(BaseLocker lockerBase)
    {
        if (!lockerBase) return null;
        return BaseToWrap.TryGetValue(lockerBase, out var locker) ? locker : new Locker(lockerBase);
    }

    public static bool TryGet(BaseLocker lockerBase, [NotNullWhen(true)] out Locker? locker)
    {
        locker = Get(lockerBase);
        return locker is not null;
    }
}