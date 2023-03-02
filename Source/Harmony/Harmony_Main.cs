using HarmonyLib;

namespace AutoNutrientPasteCommand
{
    [HarmonyPatch]
    static partial class HarmonyPatches
    {
        public static void Init()
        {
            var harmony = new Harmony("com.autonutrientpaste.jesterromut.mod");
            harmony.PatchAll();
        }
    }
}