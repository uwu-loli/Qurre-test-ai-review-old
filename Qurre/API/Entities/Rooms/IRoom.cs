using System.Collections.Generic;
using JetBrains.Annotations;
using MapGeneration;
using Qurre.API.Core;
using Qurre.API.Entities.Characters;
using UnityEngine;

namespace Qurre.API.Entities.Rooms;

[PublicAPI]
public interface IRoom : IReadOnlyTransformEntity
{
    #region Methods

    IEnumerable<Player> GetPlayersInside();

    #endregion

    #region Properties

    Bounds[] LocalSubBounds { get; }

    Bounds[] WorldSubBounds { get; }

    Bounds EntireBounds { get; }

    FacilityZone Zone { get; }

    IRoomLights Lights { get; }

    #endregion
}