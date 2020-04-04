using Harmony;
using UnityEngine;

namespace InteriorDecoration.Buildings.GlassSculpture
{
    class GlassSculpturePatches
    {
        const string unicornStageName = "Good5";

        [HarmonyPatch(typeof(BuildingComplete))]
        [HarmonyPatch("OnSpawn")]
        public static class BuildingComplete_OnSpawn_Patch
        {
            public static void Postfix(BuildingComplete __instance)
            {

                // Tweaks Material colors diamond tinting, otherwise it makes the diamond statues bubblegum pink
                if (__instance.name == GlassSculptureConfig.ID + "Complete")
                {
                    //if(Mod.compatibleMods["MaterialColor"].IsPresent)
                    ///{ 
                        var kAnimController = __instance.GetComponent<KBatchedAnimController>();
                        if (kAnimController == null) return;

                        var primaryElement = __instance.GetComponent<PrimaryElement>();
                        if (primaryElement == null) return;

                        var element = primaryElement.Element;

                        if (element.id == SimHashes.Diamond)
                        {

                            var color = new Color32(241, 172, 255, 255);
                            kAnimController.TintColour = color;
                        }
                    //}

                    // Need to refresh the component, or the rendering order will be wrong
                    // maybe there is a better way?
                    if (Mod.Settings.GlassSculpturesFabUnicorns)
                    { 
                        Artable artable = __instance.GetComponent<Artable>();
                        if(artable != null)
                        {
                            if(artable.CurrentStage == unicornStageName)
                            {
                                var fab = __instance.GetComponent<Fabulousness>();
                                if (fab == null)
                                    fab = __instance.gameObject.AddComponent<Fabulousness>();

                                fab.ForceToFront();
/*                                else
                                {
                                    var fab = __instance.GetComponent<Fabulousness>();
                                    fab.Deactivate();
                                    fab.Activate();
                                }*/
                            }
                        }
                    }
                }

                return;
            }
        }

        [HarmonyPatch(typeof(Artable))]
        [HarmonyPatch("SetStage")]
        public static class Artable_OnCompleteWork_Patch
        {
            public static void Postfix(Artable __instance)
            {
                if (Mod.Settings.GlassSculpturesFabUnicorns)
                {
                    var fab = __instance.GetComponent<Fabulousness>();
                    if (__instance.CurrentStage == unicornStageName)
                    {
                        if(fab == null)
                            __instance.gameObject.AddComponent<Fabulousness>();
                        else
                        {
                            fab.Deactivate();
                            fab.Activate();
                        }
                    }
                    else if(__instance.CurrentStage != unicornStageName && fab != null) // this is relevant is something like ReSculpt is used to change the statue
                    {
                        fab.Deactivate();
                        Object.Destroy(fab);
                    }
                }
            }
        }

    }
}
