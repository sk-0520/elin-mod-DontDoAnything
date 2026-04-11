using Elin.Plugin.Main.Models.Impl;
using HarmonyLib;

namespace Elin.Plugin.Main.Patches
{
    [HarmonyPatch(typeof(DramaCustomSequence))]
    internal static class DramaCustomSequencePatch
    {
        #region function

        [HarmonyPatch(nameof(DramaCustomSequence.Choice2), new[] { typeof(string), typeof(string) })]
        [HarmonyPrefix]
        public static bool Choice2Prefix(DramaCustomSequence __instance, string lang, string idJump)
        {
            return DramaCustomSequenceImpl.Choice2Prefix(__instance, lang, idJump);
        }

        #endregion
    }
}
