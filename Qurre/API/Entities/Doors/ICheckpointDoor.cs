using JetBrains.Annotations;
using Qurre.API.Core;
using CheckpointDoorBase = Interactables.Interobjects.CheckpointDoor;

namespace Qurre.API.Entities.Doors;

[PublicAPI]
public interface ICheckpointDoor : IDoor
{
    #region Properties

    new UnityObjectWrapper<CheckpointDoorBase> Base { get; }

    #endregion
}