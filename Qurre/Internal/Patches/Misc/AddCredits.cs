using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using JetBrains.Annotations;
using Qurre.API;
using Qurre.Loader;

namespace Qurre.Internal.Patches.Misc;

[HarmonyPatch(typeof(ServerConsole), nameof(ServerConsole.RefreshServerNameSafe))]
internal static class AddCredits
{
    [HarmonyTranspiler]
    [UsedImplicitly]
    private static IEnumerable<CodeInstruction> Call(IEnumerable<CodeInstruction> instructions)
    {
        List<CodeInstruction> list = [.. instructions];

        if (!Configs.ShowCredits)
            return list.AsEnumerable();

        int index = -1;
        for (int i = 0; i < list.Count; i++)
        {
            CodeInstruction? ins = list[i];
            if (ins.opcode == OpCodes.Ret && list[i - 1].opcode == OpCodes.Ldloc_0)
                index = i;
        }

        if (index < 1)
            return list.AsEnumerable();

        list.InsertRange(index,
        [
            new CodeInstruction(OpCodes.Ldstr, $"<color=#00000000><size=1> Qurre {EventCore.Version}</size></color>"),
            new CodeInstruction(OpCodes.Call,
                AccessTools.Method(typeof(string), nameof(string.Concat), [typeof(object), typeof(object)]))
        ]);

        return list.AsEnumerable();
    }
}