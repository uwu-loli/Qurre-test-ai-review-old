using JetBrains.Annotations;
using LightContainmentZoneDecontamination;

namespace Qurre.API.World;

[PublicAPI]
public static class Decontamination
{
    public static DecontaminationController Controller => DecontaminationController.Singleton;
    public static bool Begun => LabApi.Features.Wrappers.Decontamination.IsDecontaminating;
    public static bool InProgress => Controller._decontaminationBegun;

    public static DecontaminationController.DecontaminationStatus Status
    {
        get => LabApi.Features.Wrappers.Decontamination.Status;
        set => LabApi.Features.Wrappers.Decontamination.Status = value;
    }

    public static string ElevatorsText
    {
        get => LabApi.Features.Wrappers.Decontamination.ElevatorsText;
        set => LabApi.Features.Wrappers.Decontamination.ElevatorsText = value;
    }

    public static bool Locked
    {
        get => Controller._stopUpdating;
        set => Controller._stopUpdating = value;
    }

    public static void InstantStart()
    {
        Controller.FinishDecontamination();
    }
}