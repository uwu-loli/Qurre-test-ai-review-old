using System;
using Qurre.API.Core;
using Qurre.API.Core.Implementations;
using Qurre.Internal.Attributes;
using AdminToyBase = AdminToys.AdminToyBase;

namespace Qurre.API.Entities.AdminToys.Implementations;

[EntityWrapBindForFactory(typeof(AdminToyBase))]
internal abstract class AdminToy(AdminToyBase adminToyBase) : NetworkEntity(adminToyBase.gameObject), IAdminToy
{
    /// <inheritdoc />
    protected override bool CanModifyTransform => !IsStatic;

    /// <inheritdoc />
    public event Action? IsStaticChanged;

    /// <inheritdoc />
    public event Action? SmoothingChanged;

    /// <inheritdoc />
    public UnityObjectWrapper<AdminToyBase> Base { get; } = adminToyBase;

    /// <inheritdoc />
    public bool IsStatic
    {
        get => Base.Instance.NetworkIsStatic;
        set
        {
            Base.Instance.NetworkIsStatic = value;
            IsStaticChanged?.Invoke();
        }
    }

    /// <inheritdoc />
    public byte Smoothing
    {
        get => Base.Instance.NetworkMovementSmoothing;
        set
        {
            Base.Instance.NetworkMovementSmoothing = value;
            SmoothingChanged?.Invoke();
        }
    }

    /// <inheritdoc />
    protected override void OnPositionChanged()
    {
        Base.Instance.NetworkPosition = WorldPosition;
    }

    /// <inheritdoc />
    protected override void OnRotationChanged()
    {
        Base.Instance.NetworkRotation = WorldRotation;
    }

    /// <inheritdoc />
    protected override void OnScaleChanged()
    {
        Base.Instance.NetworkScale = WorldScale;
    }
}