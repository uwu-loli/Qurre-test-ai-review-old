using Footprinting;
using JetBrains.Annotations;
using PlayerRoles;
using Qurre.API.Core;
using Qurre.API.Exceptions;
using WindowBase = BreakableWindow;

namespace Qurre.API.Entities.Environment;

[PublicAPI]
public interface IWindow : ILevelEntity
{
    #region Properties

    UnityObjectWrapper<WindowBase> Base { get; }

    /// <exception cref="ObjectDestroyedException" />
    bool IsBroken { get; set; }

    /// <exception cref="ObjectDestroyedException" />
    Footprint LastAttacker { get; }

    /// <exception cref="ObjectDestroyedException" />
    float Health { get; set; }

    /// <exception cref="ObjectDestroyedException" />
    bool PreventScpDamage { get; set; }

    #endregion

    #region Methods

    /// <exception cref="ObjectDestroyedException" />
    bool CheckCanDamage(RoleTypeId roleType);

    /// <exception cref="ObjectDestroyedException" />
    void Damage(float amount);

    #endregion
}