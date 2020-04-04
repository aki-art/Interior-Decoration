using Harmony;
using InteriorDecoration.Buildings.StainedGlassTiles;
using InteriorDecoration.Buildings.GlassSculpture;
using InteriorDecoration.Buildings.FossilDisplay;
using InteriorDecoration.Buildings.Lantern;
using System.Collections.Generic;
using static InteriorDecoration.BuildingUtil;

namespace InteriorDecoration
{
    class HarmonyPatches
    {
        public static class Mod_OnLoad
        {
            public static void OnLoad(string path)
            {
                Mod.ModPath = path;
                var mod = typeof(Log).Assembly.GetName();
                Log.Info($"Loaded {mod.Name} version {mod.Version}");

                Settings.SettingsManager.Initialize();
                Mod.LoadAll();

            }
        }

        public static class Mod_PostLoad
        {
            public static void PostPatch()
            {
                Log.Info("Postload");
                //Mod.SetUpModCompatibility();
            }
        }

        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {

                var buildingsToRegister = new List<IDBuilding>()
                {
                    new IDBuilding(GlassSculptureConfig.ID, typeof(GlassSculptureConfig)),
                    new IDBuilding(StainedGlassTileConfig.ID, typeof(StainedGlassTileConfig)),
                    new IDBuilding(AlgaeStainedGlassTileConfig.ID, typeof(AlgaeStainedGlassTileConfig)),
                    new IDBuilding(CarbonStainedGlassTileConfig.ID, typeof(CarbonStainedGlassTileConfig)),
                    new IDBuilding(CopperStainedGlassTileConfig.ID, typeof(CopperStainedGlassTileConfig)),
                    new IDBuilding(DiamondStainedGlassTileConfig.ID, typeof(DiamondStainedGlassTileConfig)),
                    new IDBuilding(IronStainedGlassTileConfig.ID, typeof(IronStainedGlassTileConfig)),
                    new IDBuilding(LeadStainedGlassTileConfig.ID, typeof(LeadStainedGlassTileConfig)),
                    new IDBuilding(GoldStainedGlassTileConfig.ID, typeof(GoldStainedGlassTileConfig)),
                    new IDBuilding(GraniteStainedGlassTileConfig.ID, typeof(GraniteStainedGlassTileConfig)),
                    new IDBuilding(IgneousRockStainedGlassTileConfig.ID, typeof(IgneousRockStainedGlassTileConfig)),
                    new IDBuilding(MaficRockStainedGlassTileConfig.ID, typeof(MaficRockStainedGlassTileConfig)),
                    new IDBuilding(ObsidianStainedGlassTileConfig.ID, typeof(ObsidianStainedGlassTileConfig)),
                    new IDBuilding(RegolithStainedGlassTileConfig.ID, typeof(RegolithStainedGlassTileConfig)),
                    new IDBuilding(RustStainedGlassTileConfig.ID, typeof(RustStainedGlassTileConfig)),
                    new IDBuilding(SaltStainedGlassTileConfig.ID, typeof(SaltStainedGlassTileConfig)),
                    new IDBuilding(SandstoneStainedGlassTileConfig.ID, typeof(SandstoneStainedGlassTileConfig)),
                    new IDBuilding(SedimentaryRockStainedGlassTileConfig.ID, typeof(SedimentaryRockStainedGlassTileConfig)),
                    new IDBuilding(SlimeMoldStainedGlassTileConfig.ID, typeof(SlimeMoldStainedGlassTileConfig)),
                    new IDBuilding(SteelStainedGlassTileConfig.ID, typeof(SteelStainedGlassTileConfig)),
                    new IDBuilding(SuperInsulatorStainedGlassTileConfig.ID, typeof(SuperInsulatorStainedGlassTileConfig)),
                    new IDBuilding(TempConductorSolidStainedGlassTileConfig.ID, typeof(TempConductorSolidStainedGlassTileConfig)),
                    new IDBuilding(FossilStandConfig.ID, typeof(FossilStandConfig)),
                    new IDBuilding(LanternConfig.ID, typeof(LanternConfig))
                };
                
                RegisterAllBuildings(buildingsToRegister);
            }
        }
    }

}