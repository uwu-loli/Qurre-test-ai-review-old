using JetBrains.Annotations;
using UnityEngine;

namespace Qurre.API.Entities.Rooms;

[PublicAPI]
public interface IRoomLight
{
    float DisabledTimer { get; set; }

    bool IsEnabled { get; set; }

    Color Color { get; set; }

    float Intensity { get; set; }

    Color RealColor { get; set; }

    void Disable(float? duration = null);

    void Reset();
}