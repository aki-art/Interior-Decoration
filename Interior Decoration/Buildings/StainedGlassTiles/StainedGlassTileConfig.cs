using TUNING;
using UnityEngine;
using static InteriorDecoration.Mod;

namespace InteriorDecoration.Buildings.StainedGlassTiles
{
    // Default stained glass tile
    // Does not get built in this mod, but can appear if a mod adds a mineral and they assign "StainedGlassMaterial" and no associated tileconfig.
    // Otherwise this is just there for the build menu
    [StringsPath(typeof(InteriorDecorStrings.BUILDINGS.PREFABS.STAINED_GLASS_TILE))]
    [BuildMenu("Base")]
    [ResearchTree("GlassFurnishings")]
    class StainedGlassTileConfig : IBuildingConfig
    {
        public static readonly int BlockTileConnectorID = Hash.SDBMLower("tiles_glass_tops");
        public const string ID = Mod.MOD_PREFIX + "DefaultStainedGlassTile";

        private const string LOCALPATH = "assets/tiles";
        private const string mat = "default";

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = BuildingTemplates.CreateBuildingDef(
              id: ID,
              width: 1,
              height: 1,
              anim: "floor_basic_kanim",
              hitpoints: 100,
              construction_time: 30f,
              construction_mass: new float[] { 50f, 50f },
              construction_materials: new string[] { "StainedGlassMaterial", "Glass" },
              melting_point: 1600f,
              build_location_rule: BuildLocationRule.Tile,
              decor: new EffectorValues(20, 3),
              noise: NOISE_POLLUTION.NONE
          );

            BuildingTemplates.CreateFoundationTileDef(def);

            // testing value. change later
            def.ThermalConductivity = 10f;

            def.Floodable = false;
            def.Overheatable = false;
            def.Entombable = false;
            def.UseStructureTemperature = false;
            def.AudioCategory = "Metal";
            def.AudioSize = "small";
            def.BaseTimeUntilRepair = -1f;
            def.SceneLayer = Grid.SceneLayer.GlassTile;
            def.isKAnimTile = true;
            def.isSolidTile = true;
            def.BlockTileIsTransparent = true;
            def.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
            def.DragBuild = true;

            def.BlockTileMaterial = Assets.GetMaterial("tiles_solid");

            def.BlockTileAtlas = GetCustomAtlas(LOCALPATH, mat + "_glass_tiles", Assets.GetTextureAtlas("tiles_metal"));
            def.BlockTilePlaceAtlas = GetCustomAtlas(LOCALPATH, mat + "_glass_tiles_place", Assets.GetTextureAtlas("tiles_metal"));

            BlockTileDecorInfo decorBlockTileInfo = UnityEngine.Object.Instantiate(Assets.GetBlockTileDecorInfo("tiles_glass_tops_decor_info"));
            decorBlockTileInfo.atlas = GetCustomAtlas(LOCALPATH, mat + "_glass_tiles_tops", decorBlockTileInfo.atlas);
            def.DecorBlockTileInfo = decorBlockTileInfo;
            def.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_glass_tops_decor_place_info");
            def.BlockTileShineAtlas = GetCustomAtlas(LOCALPATH, mat + "_glass_tiles_spec", Assets.GetTextureAtlas("tiles_metal"));

            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            TileHelper.ConfigureTileBuildingTemplate(go, prefab_tag);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            GeneratedBuildings.RemoveLoopingSounds(go);
            go.GetComponent<KPrefabID>().AddTag(GameTags.FloorTiles, false);
        }

        public override void DoPostConfigureUnderConstruction(GameObject go)
        {
            base.DoPostConfigureUnderConstruction(go);
            go.AddOrGet<KAnimGridTileVisualizer>();
        }

    }

}

