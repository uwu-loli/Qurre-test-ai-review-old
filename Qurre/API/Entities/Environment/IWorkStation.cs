using JetBrains.Annotations;
using Qurre.API.Core;
using Qurre.API.Enums;
using Qurre.API.Exceptions;
using UnityEngine;
using WorkStationBase = InventorySystem.Items.Firearms.Attachments.WorkstationController;

namespace Qurre.API.Entities.Environment;

[PublicAPI]
public interface IWorkStation : ILevelEntity
{
    #region Methods

    /// <exception cref="ObjectDestroyedException" />
    bool WorkAreaContains(Vector3 worldPoint);

    #endregion

    #region Properties

    UnityObjectWrapper<WorkStationBase> Base { get; }

    /// <exception cref="ObjectDestroyedException" />
    WorkstationStatus Status { get; set; }

    #endregion
}