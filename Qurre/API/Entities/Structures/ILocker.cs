using JetBrains.Annotations;
using MapGeneration.Distributors;
using Qurre.API.Core;
using Qurre.API.Enums;
using UnityEngine;
using LockerBase = MapGeneration.Distributors.Locker;

namespace Qurre.API.Entities.Structures;

[PublicAPI]
public interface ILocker : IStructure
{
    new UnityObjectWrapper<LockerBase> Base { get; }

    LockerTypes LockerType { get; }

    LockerPrefabs PrefabType { get; }

    LockerChamber[] Chambers { get; }

    LockerLoot[] Loot { get; }

    AudioClip GrantedBeepClip { get; }

    AudioClip DeniedBeepClip { get; }
}