using TUNING;
using UnityEngine;
using static InteriorDecoration.InteriorDecorStrings.BUILDINGS.PREFABS;

namespace InteriorDecoration.Buildings.FossilDisplay
{
    [StringsPath(typeof(FOSSIL_STAND))]
    [BuildMenu("Furniture")]
    [ResearchTree("Artistry")]
    public class FossilStandConfig : IBuildingConfig
    {
        public static readonly string ID = Mod.MOD_PREFIX + "FossilStand";
        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(
               id: ID,
               width: 3,
               height: 2,
               anim: "fossildisplay_kanim",
               hitpoints: BUILDINGS.HITPOINTS.TIER2,
               construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER4,
               construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER4,
               construction_materials: MATERIALS.TRANSPARENTS,
               melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER1,
               build_location_rule: BuildLocationRule.OnFloor,
               decor: BUILDINGS.DECOR.BONUS.TIER2,
               noise: NOISE_POLLUTION.NONE
           );

            buildingDef.Floodable = false;
            buildingDef.Overheatable = false;
            buildingDef.AudioCategory = "Glass";
            buildingDef.BaseTimeUntilRepair = -1f;
            buildingDef.ViewMode = OverlayModes.Decor.ID;
            buildingDef.DefaultAnimState = "slab";
            buildingDef.PermittedRotations = PermittedRotations.FlipH;

            return buildingDef;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.AddOrGet<BuildingComplete>().isArtable = false;
            go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration);
            go.AddOrGet<FossilStand>();
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            Assemblable assemblable = go.AddComponent<Assemblable>();
            assemblable.stages.Add(new Artable.Stage("Default", FOSSIL_STAND.NAME, "base", -5, false, Artable.Status.Ready));
            assemblable.stages.Add(new Artable.Stage("Bad", FOSSIL_STAND.POORQUALITYNAME, "bad_1", 5, false, Artable.Status.Ugly));
            assemblable.stages.Add(new Artable.Stage("Average", FOSSIL_STAND.AVERAGEQUALITYNAME, "okay_1", 10, false, Artable.Status.Okay));
            assemblable.stages.Add(new Artable.Stage("Good1", FOSSIL_STAND.EXCELLENTQUALITYNAME, "good_1", 15, true, Artable.Status.Great));
            assemblable.stages.Add(new Artable.Stage("Good2", FOSSIL_STAND.EXCELLENTQUALITYNAME, "good_2", 15, true, Artable.Status.Great));
        }
    }
}
