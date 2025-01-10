using JetBrains.Annotations;
using UnityEngine;

namespace Qurre.API.Interfaces;

[PublicAPI]
public interface IControllerWrapperEntity<out T> : IEntity where T : MonoBehaviour
{
    T Base { get; }
}
