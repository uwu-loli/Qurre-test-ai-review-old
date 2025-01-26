using JetBrains.Annotations;
using Qurre.API.Core;
using Qurre.API.Entities.Rooms;
using Qurre.API.Exceptions;
using UnityEngine;

namespace Qurre.API.Entities.Environment;

[PublicAPI]
public interface ITesla : INetworkEntity
{
    #region Properties

    bool IsEnabled { get; set; }

    bool IsScp079InteractionAllowed { get; set; }

    UnityObjectWrapper<TeslaGate> Base { get; }

    IGameRoom Room { get; }

    /// <exception cref="ObjectDestroyedException" />
    bool IsIdling { get; }

    /// <exception cref="ObjectDestroyedException" />
    float CooldownTime { get; set; }

    /// <exception cref="ObjectDestroyedException" />
    float IdleDistance { get; set; }

    /// <exception cref="ObjectDestroyedException" />
    float TriggerDistance { get; set; }

    /// <exception cref="ObjectDestroyedException" />
    float WindupTime { get; }

    /// <exception cref="ObjectDestroyedException" />
    Vector3 KillerDistance { get; }

    /// <exception cref="ObjectDestroyedException" />
    AudioClip IdleStartClip { get; }

    /// <exception cref="ObjectDestroyedException" />
    AudioClip IdleLoopClip { get; }

    /// <exception cref="ObjectDestroyedException" />
    AudioClip IdleEndClip { get; }

    /// <exception cref="ObjectDestroyedException" />
    AudioClip[] WarmupClips { get; }

    /// <exception cref="ObjectDestroyedException" />
    AudioClip[] ShockClips { get; }

    #endregion

    #region Methods

    /// <exception cref="ObjectDestroyedException" />
    void Trigger(bool instant = false);

    /// <exception cref="ObjectDestroyedException" />
    bool IdleRangeContains(Vector3 worldPoint);

    /// <exception cref="ObjectDestroyedException" />
    bool TriggerRangeContains(Vector3 worldPoint);

    #endregion
}