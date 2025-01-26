using JetBrains.Annotations;

using Qurre.API.Controllers;
using Qurre.API.Entities.Characters;
using Qurre.API.World.Entities.Player;
using UserSettings.ServerSpecific;

namespace Qurre.API.Addons.ServerSpecificSettings;

[PublicAPI]
public abstract class BaseSSSVariant(Player owner)
{
    public Player Owner { get; } = owner;

    public virtual void OnEnabled()
    {
    }

    public virtual void OnDisabled()
    {
    }

    public abstract ServerSpecificSettingBase[] Serialize();

    public abstract void OnProcessUserInput(ServerSpecificSettingBase specificSettingBase);
}
