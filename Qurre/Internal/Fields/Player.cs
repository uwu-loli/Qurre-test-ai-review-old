using System.Collections.Generic;
using UnityEngine;

namespace Qurre.Internal.Fields;

internal static class Player
{
    internal static readonly Dictionary<GameObject, API.Entities.Characters.Player> Dictionary = [];
    internal static readonly Dictionary<ReferenceHub, API.Entities.Characters.Player> Hubs = [];
    internal static readonly Dictionary<int, API.Entities.Characters.Player> Ids = [];
    internal static readonly Dictionary<string, API.Entities.Characters.Player> Args = [];
}