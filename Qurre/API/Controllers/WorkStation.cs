using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using InventorySystem.Items.Firearms.Attachments;
using JetBrains.Annotations;
using Mirror;
using Qurre.API.Addons;
using Qurre.API.Controllers.Components;
using Qurre.API.Exceptions;
using Qurre.API.Objects;
using Qurre.API.World;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Qurre.API.Controllers;

[PublicAPI]
public class WorkStation : GeneratedNetworkEntity<WorkstationController, WorkStation>
{
    protected override WorkstationController UnsafeBase { get; }

    private WorkStation(WorkstationController station)
    {
        UnsafeBase = station;

        BaseToWrap[Base] = this;
        AddEntityLink();
        Destroyed += OnDestroyed;
    }
    
    public WorkStation(Vector3 position, Quaternion? rotation = null, Vector3? scale = null)
    {
        if (Prefabs.WorkStation == null)
            throw new NullReferenceException(nameof(Prefabs.WorkStation));

        UnsafeBase = Object.Instantiate(Prefabs.WorkStation, position, rotation ?? Quaternion.identity);

        Transform.localScale = scale ?? Vector3.one;
        NetworkServer.Spawn(Base.gameObject);

        BaseToWrap[Base] = this;
        AddEntityLink();
        Destroyed += OnDestroyed;
    }

    public WorkStation(Vector3 position, Vector3 rotationEuler, Vector3? scale = null)
        : this(position, Quaternion.Euler(rotationEuler), scale)
    {
    }

    public Player? KnownUser
    {
        get => Base._knownUser.GetPlayer();
        set => Base._knownUser = value?.ReferenceHub;
    }

    public bool IsActivated
    {
        get => Status == WorkstationStatus.Online;
        set => Status = value ? WorkstationStatus.Online : WorkstationStatus.Offline;
    }

    public WorkstationStatus Status
    {
        get => (WorkstationStatus)Base.Status;
        set
        {
            Base.NetworkStatus = (byte)value; 
            OnStatusChanged();
        }
    }

    private void OnDestroyed()
    {
        if (!BaseToWrap.ContainsKey(Base)) return;
        BaseToWrap.Remove(Base);
    }

    private void OnStatusChanged()
    {
        switch (Status)
        {
            case WorkstationStatus.Offline:
            {
                Base._serverStopwatch.Stop();
                break;
            }
            case WorkstationStatus.PoweringUp:
            case WorkstationStatus.PoweringDown:
            case WorkstationStatus.Online:
            default:
            {
                Base._serverStopwatch.Restart();
                break;
            }
        }
    }

    public static WorkStation? Get(WorkstationController workstationBase)
    {
        if (!workstationBase) return null;
        return BaseToWrap.TryGetValue(workstationBase, out var workstation) ? workstation : new WorkStation(workstationBase);
    }

    public static bool TryGet(WorkstationController workstationBase, [NotNullWhen(true)] out WorkStation? workstation)
    {
        workstation = Get(workstationBase);
        return workstation is not null;
    }
}
