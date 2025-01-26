using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Qurre.API.Entities.Rooms;

[PublicAPI]
public interface IRoomLights : IReadOnlyList<IRoomLight>
{
    IRoom Room { get; }

    float DisabledTimer { set; }

    bool IsEnabled { set; }

    Color Color { set; }

    float Intensity { set; }

    void Disable(float? duration = null);

    void Reset();
}