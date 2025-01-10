using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Footprinting;
using JetBrains.Annotations;
using Mirror;
using Qurre.API.Controllers.Components;
using Qurre.API.Exceptions;
using UnityEngine;

namespace Qurre.API.Controllers;

[PublicAPI]
public class Window : GeneratedNetworkEntity<BreakableWindow, Window>
{
    protected override BreakableWindow UnsafeBase { get; }
    
    public bool IsBreakAllowed { get; set; } = true;

    public Footprint LastAttacker => Base.LastAttacker;

    public bool PreventScpDamage
    {
        get => Base._preventScpDamage;
        set => Base._preventScpDamage = value;
    }

    public float Health
    {
        get => Base.health;
        set => Base.health = value;
    }

    public bool IsBroken
    {
        get => Base.NetworkisBroken;
        set => Base.NetworkisBroken = value;
    }

    private Window(BreakableWindow window)
    {
        UnsafeBase = window;
    }

    private void OnDestroyed()
    {
        if (!BaseToWrap.ContainsKey(Base)) return;
        BaseToWrap.Remove(Base);
    }

    public static Window? Get(BreakableWindow windowBase)
    {
        if (!windowBase) return null;
        return BaseToWrap.TryGetValue(windowBase, out var window) ? window : new Window(windowBase);
    }

    public static bool TryGet(BreakableWindow windowBase, [NotNullWhen(true)] out Window? window)
    {
        window = Get(windowBase);
        return window is not null;
    }
}