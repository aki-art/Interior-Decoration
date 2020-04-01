/*using System.Collections.Generic;
using TUNING;
using static InteriorDecoration.Mod;

namespace InteriorDecoration.Buildings.StainedGlassTiles
{
    class StainedGlassVariants
    {
        private const string LOCALPATH = "assets/tiles";
        public static List<SimHashes> tileOverrides = new List<SimHashes>()
            {
                    //SimHashes.Bitumen,
                    SimHashes.Ceramic,
                    SimHashes.Copper,
                    //SimHashes.Diamond,
                    //SimHashes.Fossil,
                    SimHashes.Gold,
                    SimHashes.Granite,
                    //SimHashes.Ice,
                    SimHashes.Iron,
                    SimHashes.Lead,
                    //SimHashes.Obsidian,
                    SimHashes.Regolith,
                    SimHashes.Salt,
                    //SimHashes.SedimentaryRock,
                    SimHashes.SlimeMold,
                    //SimHashes.SandStone,
                    SimHashes.Steel,
                    //SimHashes.Sulfur,
                    SimHashes.SuperInsulator
                    //SimHashes.TempConductorSolid
        };

        public static Dictionary<SimHashes, BuildingDef> defs;
        public static void InitDefs()
        {
            defs = new Dictionary<SimHashes, BuildingDef>(); 
            tileOverrides.ForEach(e => defs.Add(e, GetMaterialStainedGlassDef(e)));
            foreach(var kvp in defs)
            {
                Debug.Log(kvp.Key + " " + kvp.Value.name);
            }
        }

        public static BuildingDef GetMaterialStainedGlassDef(SimHashes element)
        {
            string elementTag = ElementLoader.FindElementByHash(element).tag.ToString();
            string ID = elementTag + Mod.TILE_POSTFIX;
            return Assets.GetBuildingDef(ID) ?? Assets.GetBuildingDef(StainedGlassTileConfig.ID) ?? null;
        }
    }
}
*/