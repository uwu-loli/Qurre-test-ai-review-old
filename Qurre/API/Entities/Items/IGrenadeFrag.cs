using JetBrains.Annotations;
using UnityEngine;

namespace Qurre.API.Entities.Items;

[PublicAPI]
public interface IGrenadeFrag : IThrowable
{
    float MaxRadius { get; set; }
    float ScpMultiplier { get; set; }
    float BurnDuration { get; set; }
    float DeafenDuration { get; set; }
    float ConcussDuration { get; set; }
    float FuseTime { get; set; }
    void Spawn(Vector3 position, Quaternion rotation = default, Vector3 scale = default);
}