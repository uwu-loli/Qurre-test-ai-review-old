using System.Collections.Generic;
using CameraShaking;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Attachments;
using InventorySystem.Items.Firearms.Attachments.Components;
using InventorySystem.Items.Firearms.Modules;
using JetBrains.Annotations;
using Qurre.API.Enums;

namespace Qurre.API.Entities.Items;

[PublicAPI]
public interface IGun : IItem
{
    new Firearm Base { get; }

    AmmoTypes AmmoType { get; }
    int MaxAmmo { get; }

    int TotalStoredAmmo { get; }
    int TotalMaxAmmo { get; }
    int Ammo { get; set; }
    bool IsFlashlightEnabled { get; set; }
    bool IsMagazineInserted { get; set; }
    float FireRate { get; set; }
    RecoilSettings Recoil { get; set; }


    #region Cached Modules

    public ModuleBase[] Modules { get; set; }

    public IActionModule? ActionModule { get; }
    public IAdsModule? AdsModule { get; }
    public IAdsPreventerModule[] AdsPreventerModule { get; }
    public IAmmoContainerModule[] AmmoContainerModule { get; }
    public IBusyIndicatorModule[] BusyIndicatorModule { get; }
    public IDisplayableAmmoProviderModule[] DisplayableAmmoProviderModule { get; }
    public IDisplayableInaccuracyProviderModule[] DisplayableInaccuracyProviderModule { get; }
    public IDisplayableRecoilProviderModule? DisplayableRecoilProviderModule { get; }
    public IEquipperModule? EquipperModule { get; }
    public IHitregModule? HitregModule { get; }
    public IInaccuracyProviderModule[] InaccuracyProviderModule { get; }
    public IInspectPreventerModule[] InspectPreventerModule { get; }
    public IMagazineControllerModule? MagazineControllerModule { get; }
    public IPrimaryAmmoContainerModule? PrimaryAmmoContainerModule { get; }
    public IRecoilScalingModule[] RecoilScalingModule { get; }
    public IRecommendableModule[] RecommendableModule { get; }
    public IReloaderModule? ReloaderModule { get; }
    public IReloadUnloadValidatorModule[] ReloadUnloadValidatorModule { get; }
    public ISpectatorSyncModule? SpectatorSyncModule { get; }
    public ISwayModifierModule[] SwayModifierModule { get; }
    public ITriggerControllerModule? TriggerControllerModule { get; }
    public ITriggerPressPreventerModule[] TriggerPressPreventerModule { get; }

    // IActionModule
    public AutomaticActionModule? AutomaticActionModule { get; }
    public DisruptorActionModule? DisruptorActionModule { get; }
    public DoubleActionModule? DoubleActionModule { get; }
    public PumpActionModule? PumpActionModule { get; }

    // IAdsModule
    public LinearAdsModule? LinearAdsModule { get; }

    // IDisplayableRecoilProviderModule
    public RecoilPatternModule? RecoilPatternModule { get; }

    // IMagazineControllerModule
    public MagazineModule? MagazineModule { get; }

    public void RefreshCachedModules();

    public TModule? GetModuleNullable<TModule>() where TModule : class;
    public TModule[] GetModules<TModule>() where TModule : class;

    #endregion


    #region Cached Attachments

    public Attachment[] Attachments { get; set; }

    public FlashlightAttachment? FlashlightAttachment { get; }

    public IReadOnlyDictionary<AttachmentSlot, Attachment[]> AttachmentsBySlot { get; }

    public Attachment? GetActiveAttachment(AttachmentSlot slot);

    public void RefreshCachedAttachments();

    #endregion
}