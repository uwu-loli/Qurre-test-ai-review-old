using Qurre.API.Core;
using Qurre.Internal.Attributes;
using CheckpointDoorBase = Interactables.Interobjects.CheckpointDoor;

namespace Qurre.API.Entities.Doors.Implementations;

[EntityWrapBindForFactory(typeof(CheckpointDoorBase))]
internal sealed class CheckpointDoor(CheckpointDoorBase checkpointDoorBase) : Door(checkpointDoorBase), ICheckpointDoor
{
    /// <inheritdoc />
    public new UnityObjectWrapper<CheckpointDoorBase> Base { get; } = checkpointDoorBase;
}