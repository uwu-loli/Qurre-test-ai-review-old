using System;
using System.Collections.Generic;
using MapGeneration;
using Qurre.API.Core.Implementations;
using Qurre.API.Entities.Characters;
using Qurre.API.Entities.Rooms.Lights;
using UnityEngine;

namespace Qurre.API.Entities.Rooms.Implementations;

internal sealed class CustomRoom : TransformEntity, ICustomRoom
{
    public CustomRoom(GameObject gameObject) : base(gameObject)
    {
    }

    /// <inheritdoc />
    public Bounds[] LocalSubBounds { get; }

    /// <inheritdoc />
    public Bounds[] WorldSubBounds { get; }

    /// <inheritdoc />
    public Bounds EntireBounds { get; }

    /// <inheritdoc />
    public FacilityZone Zone { get; }

    /// <inheritdoc />
    public CustomRoomLights Lights { get; }

    /// <inheritdoc />
    IRoomLights IRoom.Lights => Lights;

    /// <inheritdoc />
    public IEnumerable<Player> GetPlayersInside()
    {
        throw new NotImplementedException();
    }
}