using JetBrains.Annotations;
using MapGeneration;
using UnityEngine;

namespace Qurre.API.Utils;

[PublicAPI]
public static class BoundsHelper
{
    public static Bounds CombineBounds(params Bounds[] boundsArray)
    {
        if (boundsArray.Length == 0)
            return new Bounds();

        var combinedBounds = boundsArray[0];
        boundsArray.ForEach(bounds => combinedBounds.Encapsulate(bounds));
        return combinedBounds;
    }

    public static Bounds CalculateEntireBounds(Vector3 position, params Bounds[] localBoundsArray)
    {
        if (localBoundsArray.Length == 0)
            return new Bounds(position, Vector3.zero);

        var bounds = new Bounds(position, Vector3.zero);
        localBoundsArray.ForEach(localBounds => bounds.Encapsulate(localBounds));
        return bounds;
    }

    public static Bounds CalculateWorldBoundsByRelative(Bounds srcBounds, Transform transformRelative)
    {
        var worldCenter = transformRelative.TransformPoint(srcBounds.center);
        var worldSize = transformRelative.rotation * srcBounds.size;

        worldSize.x = Mathf.Abs(worldSize.x);
        worldSize.y = Mathf.Abs(worldSize.y);
        worldSize.z = Mathf.Abs(worldSize.z);

        return new Bounds(worldCenter, worldSize);
    }

    public static Bounds CalculateRoomEntireBounds(RoomIdentifier roomIdentifier)
    {
        if (!roomIdentifier)
            return new Bounds();

        var bounds = new Bounds(roomIdentifier.transform.position, Vector3.zero);
        var gridScale = RoomIdentifier.GridScale;

        foreach (var gridCoord in roomIdentifier.OccupiedCoords)
        {
            var localPoint = Vector3.Scale(gridCoord, gridCoord);
            var localBounds = new Bounds(localPoint, gridScale);
            bounds.Encapsulate(localBounds);
        }

        return bounds;
    }

    public static bool IsBoundsFullyContainedInOther(Bounds outer, Bounds inner)
    {
        return outer.Contains(inner.min) && outer.Contains(inner.max);
    }
}