using UnityEngine;

namespace InteriorDecoration.Buildings.StainedGlassTiles
{
    [StringsPath(typeof(InteriorDecorStrings.BUILDINGS.PREFABS.STAINED_GLASS_TILE))]
    public class DiamondStainedGlassTileConfig : IBuildingConfig
    {
        public static readonly int BlockTileConnectorID = Hash.SDBMLower("tiles_glass_tops");
        public const string ID = Mod.MOD_PREFIX + "DiamondStainedGlassTile";

        public override BuildingDef CreateBuildingDef()
        {
            return TileHelper.CreateTileBuildingDef(ID, "Diamond");
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            TileHelper.ConfigureTileBuildingTemplate(go, prefab_tag);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            GeneratedBuildings.RemoveLoopingSounds(go);
            go.GetComponent<KPrefabID>().AddTag(GameTags.FloorTiles, false);
            go.GetComponent<KPrefabID>().AddTag(TagManager.Create("Colorshifting"), false);
            go.AddComponent<TileUpdater>();
        }

        public override void DoPostConfigureUnderConstruction(GameObject go)
        {
            base.DoPostConfigureUnderConstruction(go);
            go.AddOrGet<KAnimGridTileVisualizer>();
        }


    }
}