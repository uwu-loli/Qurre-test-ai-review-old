using JetBrains.Annotations;
using Qurre.API.Core;
using Qurre.API.Exceptions;
using PryableDoorBase = Interactables.Interobjects.PryableDoor;

namespace Qurre.API.Entities.Doors;

[PublicAPI]
public interface IPryableDoor : IDoor
{
    new UnityObjectWrapper<PryableDoorBase> Base { get; }

    /// <exception cref="ObjectDestroyedException" />
    bool IsBeingPried { get; }
}