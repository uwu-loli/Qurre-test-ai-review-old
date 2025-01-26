using System;
using JetBrains.Annotations;
using Qurre.API.Addons;
using Qurre.API.Entities.Environment.Implementations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Qurre.API.Entities.Environment;

[PublicAPI]
public class EnvironmentFactory
{
    #region Lift Factory
    
    // TODO: нужно реализовать создание ElevatorChamber с отвязкой от базового ElevatorGroup
    // если это невозможно, нужно удалить регион
    
    #endregion
    
    #region WorkStation Factory

    public IWorkStation CreateWorkStation(Vector3 position,
        Quaternion? rotation = null,
        Vector3? scale = null,
        bool doSpawn = true)
    {
        if (Prefabs.WorkStation is null)
            throw new NullReferenceException("Prefab.WorkStation is null.");
        
        var spawnRotation = rotation ?? Quaternion.identity;
        var spawnScale = scale ?? Vector3.one;

        var workstationInstance = Object.Instantiate(Prefabs.WorkStation, position, spawnRotation);
        workstationInstance.transform.localScale = spawnScale;

        var workstation = new WorkStation(workstationInstance);
        if (doSpawn) workstation.Spawn();
        else workstation.UnSpawn();
        return workstation;
    }

    public IWorkStation CreateWorkStation(Vector3 position,
        Vector3 eulerAngles,
        Vector3? scale = null,
        bool doSpawn = true)
    {
        var rotationQuaternion = Quaternion.Euler(eulerAngles);
        return CreateWorkStation(position, rotationQuaternion, scale, doSpawn);
    }
    
    #endregion
}