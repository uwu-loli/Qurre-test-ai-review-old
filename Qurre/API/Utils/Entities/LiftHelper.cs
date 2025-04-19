using System.Collections.Generic;
using System.Linq;
using Interactables.Interobjects;
using JetBrains.Annotations;
using Qurre.API.Entities;
using Qurre.API.Entities.Environment;

namespace Qurre.API.Utils.Entities;

[PublicAPI]
public static class LiftHelper
{
    public static ILift? GetLiftByGroup(ElevatorGroup group)
    {
        return EntityManager.GetAll<ILift>().FirstOrDefault(lift => lift.AssignedGroup == group);
    }

    public static ILift GetLiftByGroupOrException(ElevatorGroup group)
    {
        return GetLiftByGroup(group) ?? throw new KeyNotFoundException($"Lift: {group} is not found");
    }
}