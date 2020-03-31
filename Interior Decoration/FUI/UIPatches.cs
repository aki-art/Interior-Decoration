using Harmony;
using KMod;
using System.Collections;
using UnityEngine;
using InteriorDecoration.Settings;
namespace InteriorDecoration.FUI
{
    class UIPatches
    {        // Injecting the Settings Button to the mods menu
        [HarmonyPatch(typeof(ModsScreen), "BuildDisplay")]
        public static class ModsScreen_BuildDisplay_Patch
        {
            public static void Postfix(object ___displayedMods, ModsScreen __instance)
            {
                foreach (var modEntry in (IEnumerable)___displayedMods)
                {
                    int index = Traverse.Create(modEntry).Field("mod_index").GetValue<int>();
                    KMod.Mod mod = Global.Instance.modManager.mods[index];

                    // checks if the current mod entry is this mod
                    if (index >= 0 && mod.file_source.GetRoot() == Mod.ModPath)
                    {
                        Transform transform = Traverse.Create(modEntry).Field("rect_transform").GetValue<RectTransform>();
                        if (transform != null)
                        {
                            // find an existing subscription button to copy
                            KButton subButton = null;
                            foreach (Transform child in transform)
                            {
                                if (child.gameObject.name == "ManageButton")
                                    subButton = child.gameObject.GetComponent<KButton>();
                            }

                            // copy the subscription button
                            KButton configButton = UIHelper.MakeKButton(
                                info: new UIHelper.ButtonInfo(
                                    text: "Settings",
                                    action: new System.Action(OpenModSettingsScreen),
                                    font_size: 14),
                                buttonPrefab: subButton.gameObject,
                                parent: subButton.transform.parent.gameObject,
                                index: subButton.transform.GetSiblingIndex() - 1);
                        }
                    }
                }

            }
        }

        private static void OpenModSettingsScreen()
        {
            if (Mod.modSettingsScreenPrefab == null)
            {
                Log.Warning("Could not display UI: Mod Settings screen prefab is null.");
                return;
            }

            Transform parent = UIHelper.GetACanvas("AsphaltModSettings").transform;
            GameObject settingsScreen = Object.Instantiate(Mod.modSettingsScreenPrefab.gameObject, parent);
            ModSettingsScreen settingsScreenComponent = settingsScreen.AddComponent<ModSettingsScreen>();

            settingsScreenComponent.ShowDialog();
        }
    }
}
