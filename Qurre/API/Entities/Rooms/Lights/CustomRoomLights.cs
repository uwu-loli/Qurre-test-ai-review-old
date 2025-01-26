using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Qurre.API.Entities.Rooms.Lights;

[PublicAPI]
public class CustomRoomLights : IRoomLights, IReadOnlyList<CustomRoomLight>
{
    private static readonly List<CustomRoomLight> Instances = [];

    internal CustomRoomLights(ICustomRoom room)
    {
        Room = room;
    }

    public ICustomRoom Room { get; }

    /// <inheritdoc />
    IRoom IRoomLights.Room => Room;

    /// <inheritdoc />
    public float DisabledTimer
    {
        set => Instances.ForEach(light => light.DisabledTimer = value);
    }

    /// <inheritdoc />
    public bool IsEnabled
    {
        set => Instances.ForEach(light => light.IsEnabled = value);
    }

    /// <inheritdoc />
    public Color Color
    {
        set => Instances.ForEach(light => light.Color = value);
    }

    /// <inheritdoc />
    public float Intensity
    {
        set => Instances.ForEach(light => light.Intensity = value);
    }

    /// <inheritdoc />
    public void Disable(float? duration = null)
    {
        Instances.ForEach(light => light.Disable(duration));
    }

    /// <inheritdoc />
    public void Reset()
    {
        Instances.ForEach(light => light.Reset());
    }

    #region IReadOnlyList<T>

    /// <inheritdoc cref="IReadOnlyCollection{T}.Count" />
    public int Count => Instances.Count;

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return Instances.GetEnumerator();
    }

    #endregion

    #region IReadOnlyList<CustomRoomLight>

    /// <inheritdoc />
    public CustomRoomLight this[int index] => Instances[index];

    /// <inheritdoc />
    public IEnumerator<CustomRoomLight> GetEnumerator()
    {
        return Instances.GetEnumerator();
    }

    #endregion

    #region IReadOnlyList<IRoomLight>

    /// <inheritdoc />
    IRoomLight IReadOnlyList<IRoomLight>.this[int index] => Instances[index];

    /// <inheritdoc />
    IEnumerator<IRoomLight> IEnumerable<IRoomLight>.GetEnumerator()
    {
        return Instances.GetEnumerator();
    }

    #endregion
}