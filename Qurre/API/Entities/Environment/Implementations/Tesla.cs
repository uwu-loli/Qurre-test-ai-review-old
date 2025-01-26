using Qurre.API.Core;
using Qurre.API.Core.Implementations;
using Qurre.API.Entities.Rooms;
using Qurre.Internal.Attributes;
using UnityEngine;

namespace Qurre.API.Entities.Environment.Implementations;

[EntityWrapBindForFactory(typeof(TeslaGate))]
internal sealed class Tesla(TeslaGate teslaBase) : NetworkEntity(teslaBase.gameObject), ITesla
{
    /// <inheritdoc />
    public bool IsEnabled { get; set; }

    /// <inheritdoc />
    public bool IsScp079InteractionAllowed { get; set; }

    /// <inheritdoc />
    public UnityObjectWrapper<TeslaGate> Base { get; } = teslaBase;

    public IGameRoom Room { get; } = EntityManager.GetOrException<IGameRoom>(teslaBase.Room);

    /// <inheritdoc />
    public bool IsIdling => Base.Instance.isIdling;

    /// <inheritdoc />
    public float CooldownTime
    {
        get => Base.Instance.cooldownTime;
        set => Base.Instance.cooldownTime = value;
    }

    /// <inheritdoc />
    public float IdleDistance
    {
        get => Base.Instance.distanceToIdle;
        set => Base.Instance.distanceToIdle = value;
    }

    /// <inheritdoc />
    public float TriggerDistance
    {
        get => Base.Instance.sizeOfTrigger;
        set => Base.Instance.sizeOfTrigger = value;
    }

    /// <inheritdoc />
    public float WindupTime => Base.Instance.windupTime;

    /// <inheritdoc />
    public Vector3 KillerDistance => Base.Instance.sizeOfKiller;

    /// <inheritdoc />
    public AudioClip IdleStartClip => Base.Instance.idleStart;

    /// <inheritdoc />
    public AudioClip IdleLoopClip => Base.Instance.idleLoop;

    /// <inheritdoc />
    public AudioClip IdleEndClip => Base.Instance.idleEnd;

    /// <inheritdoc />
    public AudioClip[] WarmupClips => Base.Instance.clipsWarmup;

    /// <inheritdoc />
    public AudioClip[] ShockClips => Base.Instance.clipsShock;

    /// <inheritdoc />
    public void Trigger(bool instant = false)
    {
        if (instant) Base.Instance.RpcInstantBurst();
        else Base.Instance.RpcPlayAnimation();
    }

    /// <inheritdoc />
    public bool IdleRangeContains(Vector3 worldPoint)
    {
        return Vector3.Distance(WorldPosition, worldPoint) < IdleDistance;
    }

    /// <inheritdoc />
    public bool TriggerRangeContains(Vector3 worldPoint)
    {
        return Vector3.Distance(WorldPosition, worldPoint) < TriggerDistance;
    }
}