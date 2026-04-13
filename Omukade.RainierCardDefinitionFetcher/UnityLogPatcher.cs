using HarmonyLib;

namespace Omukade.Tools.RainierCardDefinitionFetcher
{
    internal static class UnityLogPatcher
    {
        [HarmonyPatch(typeof(UnityEngine.Debug), nameof(UnityEngine.Debug.LogError), [typeof(object)])]
        [HarmonyPrefix]
        public static bool UnityEngineDebugLogErrorPrefix()
        {
            return false;
        }
    }
}
