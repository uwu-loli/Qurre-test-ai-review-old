using JetBrains.Annotations;
using Qurre.API.Entities.Characters;
using UserSettings.ServerSpecific;

namespace Qurre.API.Addons.ServerSpecificSettings;

[PublicAPI]
public abstract class SpecificServerSettingsVariant(Player owner)
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