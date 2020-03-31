using TUNING;
using UnityEngine;
using static InteriorDecoration.InteriorDecorStrings.BUILDINGS.PREFABS;

namespace InteriorDecoration.Buildings.GlassSculpture
{
    [BuildMenu("Furniture")]
    [ResearchTree("GlassFurnishings")]
    [StringsPath(typeof(GLASS_SCULPTURE))]
    public class GlassSculptureConfig : IBuildingConfig
    {
        public const string ID = Mod.MOD_PREFIX + "GlassSculptureNoPaint"; // -NoPaint Is a MaterialColor compatibility to ignore this building

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = BuildingTemplates.CreateBuildingDef(
               id: ID,
               width: 2,
               height: 2,
               anim: "sculpture_glass_kanim",
               hitpoints: 100,
               construction_time: 120f,
               construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER4,
               construction_materials: MATERIALS.TRANSPARENTS,
               melting_point: 1600f,
               build_location_rule: BuildLocationRule.OnFloor,
               decor: new EffectorValues(20, 8),
               noise: NOISE_POLLUTION.NONE
           );

            def.Floodable = false;
            def.Overheatable = false;
            def.AudioCategory = "Glass";
            def.BaseTimeUntilRepair = -1f;
            def.ViewMode = OverlayModes.Decor.ID;
            def.DefaultAnimState = "slab";
            def.PermittedRotations = PermittedRotations.FlipH;

            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.AddOrGet<BuildingComplete>().isArtable = true;
            go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            Artable artable = go.AddComponent<Sculpture>();
            artable.stages.Add(new Artable.Stage("Default", GLASS_SCULPTURE.NAME, "slab", 0, false, Artable.Status.Ready));
            artable.stages.Add(new Artable.Stage("Bad", GLASS_SCULPTURE.POORQUALITYNAME, "bad_1", 5, false, Artable.Status.Ugly));
            artable.stages.Add(new Artable.Stage("Average", GLASS_SCULPTURE.AVERAGEQUALITYNAME, "good_1", 10, false, Artable.Status.Okay));
            artable.stages.Add(new Artable.Stage("Good1", GLASS_SCULPTURE.EXCELLENTQUALITYNAME, "amazing_1", 15, true, Artable.Status.Great));
            if (Mod.Settings.GlassSculpturesIceHatch)
                artable.stages.Add(new Artable.Stage("Good2", GLASS_SCULPTURE.EXCELLENTQUALITYNAME, "amazing_2", 15, true, Artable.Status.Great));
            artable.stages.Add(new Artable.Stage("Good3", GLASS_SCULPTURE.EXCELLENTQUALITYNAME, "amazing_3", 15, true, Artable.Status.Great));
            artable.stages.Add(new Artable.Stage("Good4", GLASS_SCULPTURE.EXCELLENTQUALITYNAME, "amazing_4", 15, true, Artable.Status.Great));
            artable.stages.Add(new Artable.Stage("Good5", GLASS_SCULPTURE.EXCELLENTQUALITYNAME, "amazing_5", 15, true, Artable.Status.Great));
        }
    }

}
