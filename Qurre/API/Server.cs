using System;
using System.Collections.Generic;
using System.Linq;
using CustomPlayerEffects;
using InventorySystem;
using JetBrains.Annotations;
using Qurre.API.Entities.Characters;
using RoundRestarting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Qurre.API;

[PublicAPI]
public static class Server
{
    private static Player? _host;
    private static Inventory? _hostInv;

    public static ushort Port => ServerStatic.ServerPort;

    public static string Ip => ServerConsole.Ip;

    public static double Tps => Math.Round(1f / Time.smoothDeltaTime);

    public static Player Host
    {
        get
        {
            if (_host?.ReferenceHub)
                return _host;

            if (!ReferenceHub.TryGetHostHub(out var hub) || !hub)
                throw new NullReferenceException("ReferenceHub could not be found");

            _host = Player.Get(hub)!;
            return _host;
        }
    }

    public static Inventory InventoryHost => _hostInv ??= Host.ReferenceHub.inventory;

    public static bool FriendlyFire
    {
        get => ServerConsole.FriendlyFire;
        set
        {
            if (FriendlyFire == value) return;

            ServerConsole.FriendlyFire = value;
            ServerConfigSynchronizer.Singleton.RefreshMainBools();
            ServerConfigSynchronizer.OnRefreshed?.Invoke();
            //AttackerDamageHandler.RefreshConfigs(); // подписан на ServerConfigSynchronizer

            foreach (var player in Player.List)
                player.AllowFriendlyDamage = value;
        }
    }

    public static float SpawnProtectDuration
    {
        get => SpawnProtected.SpawnDuration;
        set => SpawnProtected.SpawnDuration = value;
    }

    public static List<TObject> GetObjectsOf<TObject>() where TObject : Object
    {
        return Object.FindObjectsOfType<TObject>().ToList();
    }

    public static TObject GetObjectOf<TObject>() where TObject : Object
    {
        return Object.FindObjectOfType<TObject>();
    }

    public static void Restart()
    {
        ServerStatic.StopNextRound = ServerStatic.NextRoundAction.Restart;
        RoundRestart.ChangeLevel(true);
    }

    public static void Exit()
    {
        Shutdown.Quit();
    }

    internal static void WaitingRefresh()
    {
        _host = null;
        _hostInv = null;
    }
}