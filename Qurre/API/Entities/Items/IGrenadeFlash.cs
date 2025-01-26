using JetBrains.Annotations;
using UnityEngine;

namespace Qurre.API.Entities.Items;

[PublicAPI]
public interface IGrenadeFlash : IThrowable
{
    AnimationCurve BlindAnimation { get; set; }
    AnimationCurve DeafenAnimation { get; set; }
    float SurfaceDistanceIntensifier { get; set; }
    float FuseTime { get; set; }

    void Spawn(Vector3 position, Quaternion rotation = default, Vector3 scale = default);
}