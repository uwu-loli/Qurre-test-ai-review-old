using Qurre.API.Core;
using Qurre.Internal.Attributes;
using PryableDoorBase = Interactables.Interobjects.PryableDoor;

namespace Qurre.API.Entities.Doors.Implementations;

[EntityWrapBindForFactory(typeof(PryableDoorBase))]
internal sealed class PryableDoor(PryableDoorBase pryableDoorBase) : Door(pryableDoorBase), IPryableDoor
{
    /// <inheritdoc />
    public new UnityObjectWrapper<PryableDoorBase> Base { get; } = pryableDoorBase;

    /// <inheritdoc />
    public bool IsBeingPried => Base.Instance.IsBeingPried;
}