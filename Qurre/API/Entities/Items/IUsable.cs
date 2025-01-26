using InventorySystem.Items.Usables;
using JetBrains.Annotations;
using Qurre.API.Core;

namespace Qurre.API.Entities.Items;

[PublicAPI]
public interface IUsable : IItem
{
    new UnityObjectWrapper<UsableItem> Base { get; }

    bool IsEquippable { get; }
    bool IsHolsterable { get; }

    float UseTime { get; set; }
    float MaxCancellableTime { get; set; }
    float RemainingCooldown { get; set; }
}