using TUNING;
using UnityEngine;
using static InteriorDecoration.Mod;

namespace InteriorDecoration.Buildings.StainedGlassTiles
{
    class TileHelper
    {
        private const string LOCALPATH = "assets/tiles";
        public static BuildingDef CreateTileBuildingDef(string ID, string materialName, bool spec = true)
        {
            string mat = materialName.ToLower();
            BuildingDef def = BuildingTemplates.CreateBuildingDef(
              id: ID,
              width: 1,
              height: 1,
              anim: "floor_basic_kanim",
              hitpoints: 100,
              construction_time: 30f,
              construction_mass: new float[] { 50f, 50f },
              construction_materials: new string[] { "BuildableRaw", "Glass" },
              melting_point: 1600f,
              build_location_rule: BuildLocationRule.Tile,
              decor: new EffectorValues(20, 1),
              noise: NOISE_POLLUTION.NONE
          );

            BuildingTemplates.CreateFoundationTileDef(def);

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
            def.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_glass_tops_decor_place_info"); // for placement original glass is perfectly good

            if (spec) def.BlockTileShineAtlas = GetCustomAtlas(LOCALPATH, mat + "_glass_tiles_spec", Assets.GetTextureAtlas("tiles_metal"));

            return def;
        }
        public static void ConfigureTileBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            GeneratedBuildings.MakeBuildingAlwaysOperational(go);
            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);

            SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
            simCellOccupier.movementSpeedMultiplier = DUPLICANTSTATS.MOVEMENT.BONUS_2;
            simCellOccupier.strengthMultiplier = 1.5f;
            simCellOccupier.doReplaceElement = true;
            simCellOccupier.setTransparent = true;

            go.AddOrGet<TileTemperature>();
            go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = GlassTileConfig.BlockTileConnectorID;
            go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
            go.GetComponent<KPrefabID>().AddTag(GameTags.Window, false);
        }
    }
}
