using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Qurre.API.Interfaces;

[PublicAPI]
public interface ITransformableEntity : IEntity
{
    event Action PositionUpdated;
    event Action RotationUpdated;
    event Action ScaleUpdated;
    
    Transform Transform { get; }
    
    Vector3 WorldPosition { get; set; }
    Quaternion WorldRotation { get; set; }
    Vector3 WorldRotationEuler { get; set; }
    Vector3 WorldScale { get; set; }
    
    Vector3 LocalPosition { get; set; }
    Quaternion LocalRotation { get; set; }
    Vector3 LocalRotationEuler { get; set; }
    Vector3 LocalScale { get; set; }
}