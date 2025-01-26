using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Qurre.API.Entities.Rooms.Lights;

[PublicAPI]
public class GameRoomLights : IRoomLights, IReadOnlyList<IGameRoomLight>
{
    private static readonly List<IGameRoomLight> Instances = [];

    internal GameRoomLights(IGameRoom room)
    {
        Room = room;

        foreach (var roomLightBase in room.GameObject.Instance.GetComponentsInChildren<RoomLightController>())
            Instances.Add(new GameRoomLight(roomLightBase));

        Main = Instances.FirstOrDefault();
    }

    public IGameRoom Room { get; }

    public IGameRoomLight? Main { get; }

    public Color DefaultColor
    {
        set => Instances.ForEach(light => light.DefaultColor = value);
    }

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

    #region IReadOnlyList<IGameRoomLight>

    /// <inheritdoc />
    public IGameRoomLight this[int index] => Instances[index];

    /// <inheritdoc />
    public IEnumerator<IGameRoomLight> GetEnumerator()
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