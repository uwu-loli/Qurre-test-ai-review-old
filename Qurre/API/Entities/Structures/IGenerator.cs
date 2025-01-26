using JetBrains.Annotations;
using MapGeneration.Distributors;
using Qurre.API.Core;
using UnityEngine;

namespace Qurre.API.Entities.Structures;

[PublicAPI]
public interface IGenerator : IStructure
{
    new UnityObjectWrapper<Scp079Generator> Base { get; }

    float ActivationTime { get; }

    bool IsOpen { get; set; }

    bool IsLocked { get; set; }

    bool IsActive { get; set; }

    bool IsActivating { get; set; }

    bool IsDeactivating { get; set; }

    bool IsEngaged { get; set; }

    short RemainingTime { get; set; }

    float DeniedCooldownTime { get; set; }

    float UnlockCooldownTime { get; set; }

    #region Audio API

    AudioClip DeniedClip { get; }

    AudioClip OpenClip { get; }

    AudioClip CloseClip { get; }

    AudioClip CountdownClip { get; }

    #endregion
}