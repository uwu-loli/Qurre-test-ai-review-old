using MapGeneration.Distributors;
using Qurre.API.Core;
using Qurre.API.Enums;
using Qurre.Internal.Attributes;
using UnityEngine;
using LockerBase = MapGeneration.Distributors.Locker;

namespace Qurre.API.Entities.Structures.Implementations;

[EntityWrapBindForFactory(typeof(LockerBase))]
internal sealed class Locker(LockerBase lockerBase) : StructureEntity(lockerBase), ILocker
{
    /// <inheritdoc />
    public new UnityObjectWrapper<LockerBase> Base { get; } = lockerBase;

    /// <inheritdoc />
    public LockerTypes LockerType { get; }

    /// <inheritdoc />
    public LockerPrefabs PrefabType { get; }

    /// <inheritdoc />
    public LockerChamber[] Chambers => Base.Instance.Chambers;

    /// <inheritdoc />
    public LockerLoot[] Loot => Base.Instance.Loot;

    /// <inheritdoc />
    public AudioClip GrantedBeepClip => Base.Instance._grantedBeep;

    /// <inheritdoc />
    public AudioClip DeniedBeepClip => Base.Instance._deniedBeep;
}