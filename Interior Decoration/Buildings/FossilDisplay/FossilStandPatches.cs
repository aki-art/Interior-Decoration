using Harmony;

namespace InteriorDecoration.Buildings.FossilDisplay
{
    // Add inspired effect
    class FossilStandPatches
    {
        [HarmonyPatch(typeof(ModifierSet))]
        [HarmonyPatch(nameof(ModifierSet.Initialize))]
        public static class ModifierSet_Initialize_Patch
        {
            public static void Postfix(ModifierSet __instance)
            {
                foreach (var effect in InspiredEffects.GetEffectsList())
                {
                    __instance.effects.Add(effect);
                }
            }
        }
    }
}
