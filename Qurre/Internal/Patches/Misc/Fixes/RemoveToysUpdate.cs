using System.Collections.Generic;
using System.Reflection.Emit;
using AdminToys;
using HarmonyLib;
using JetBrains.Annotations;

namespace Qurre.Internal.Patches.Misc.Fixes;

[HarmonyPatch(typeof(AdminToyBase), nameof(AdminToyBase.LateUpdate))]
internal static class RemoveToysUpdate
{
    [HarmonyTranspiler]
    [UsedImplicitly]
    private static IEnumerable<CodeInstruction> Call(IEnumerable<CodeInstruction> _)
    {
        yield return new CodeInstruction(OpCodes.Ret);
    }
}