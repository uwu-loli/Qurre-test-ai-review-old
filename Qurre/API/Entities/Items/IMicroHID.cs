using InventorySystem.Items.MicroHID;
using JetBrains.Annotations;
using Qurre.API.Core;

namespace Qurre.API.Entities.Items;

[PublicAPI]
public interface IMicroHID : IItem
{
    new UnityObjectWrapper<MicroHIDItem> Base { get; }

    float Energy { get; set; }
}