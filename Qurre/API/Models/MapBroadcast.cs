using System.Collections.Generic;
using JetBrains.Annotations;
using MEC;
using Qurre.API.Entities.Characters;
using Qurre.API.World.Entities.Player;

namespace Qurre.API.Controllers;

[PublicAPI]
public class MapBroadcast
{
    private readonly List<Broadcast> _list = [];
    private string _msg;

    public MapBroadcast(string message, ushort time, bool instant, bool adminBC)
    {
        _msg = message;
        Time = time;

        Start();

        foreach (var player in Player.List)
        {
            if (adminBC && !PermissionsHandler.IsPermitted(player.Sender.Permissions, PlayerPermissions.AdminChat))
                continue;

            var broadcast = player.Client.Broadcast(message, time, instant);
            _list.Add(broadcast);
        }
    }

    public ushort Time { get; }
    public bool Active { get; private set; }

    public string Message
    {
        get => _msg;
        set
        {
            if (value == _msg)
                return;

            _msg = value;
            foreach (Broadcast? bc in _list) bc.Message = value;
        }
    }

    public void Start()
    {
        if (Active)
            return;

        Active = true;

        Timing.CallDelayed(Time, End);
    }

    public void End()
    {
        if (!Active)
            return;

        Active = false;

        foreach (Broadcast? bc in _list)
            bc.End();
    }
}