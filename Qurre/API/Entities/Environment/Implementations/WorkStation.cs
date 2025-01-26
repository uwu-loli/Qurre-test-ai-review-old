using Qurre.API.Core;
using Qurre.API.Core.Implementations;
using Qurre.API.Enums;
using Qurre.Internal.Attributes;
using UnityEngine;
using WorkStationBase = InventorySystem.Items.Firearms.Attachments.WorkstationController;

namespace Qurre.API.Entities.Environment.Implementations;

[EntityWrapBindForFactory(typeof(WorkStationBase))]
internal sealed class WorkStation(WorkStationBase workStationBase)
    : LevelEntity(workStationBase.gameObject), IWorkStation
{
    /// <inheritdoc />
    public UnityObjectWrapper<WorkStationBase> Base { get; } = workStationBase;

    /// <inheritdoc />
    public WorkstationStatus Status
    {
        get => (WorkstationStatus)Base.Instance.NetworkStatus;
        set => Base.Instance.NetworkStatus = (byte)value;
    }

    /// <inheritdoc />
    public bool WorkAreaContains(Vector3 worldPoint)
    {
        return Vector3.Distance(WorldPosition, worldPoint) < WorkStationBase.StandbyDistance;
    }
}