using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace InteriorDecoration.Buildings.Lantern
{
    [StringsPath(typeof(InteriorDecorStrings.BUILDINGS.PREFABS.LANTERN))]
    [BuildMenu("Furniture")]
    class LanternConfig : IBuildingConfig
    {
        public static readonly string ID = Mod.MOD_PREFIX + "Lantern";

        private const float CAPACITY = 10.0f;
        private const float CONSUMPTION_RATE = 0.01f;

        private static Color KELVIN1500 = new Color32(255, 109, 0, 255);
        private const float RANGE = 4f;
        private const int LUX = 2500;

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(
               id: ID,
               width: 1,
               height: 1,
               anim: "id_lantern_kanim",
               hitpoints: 100,
               construction_time: 120f,
               construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER4,
               construction_materials: MATERIALS.ALL_METALS,
               melting_point: 1600f,
               build_location_rule: BuildLocationRule.OnFoundationRotatable,
               decor: BUILDINGS.DECOR.BONUS.TIER1,
               noise: NOISE_POLLUTION.NONE
            );

            buildingDef.ExhaustKilowattsWhenActive = +0.5f;
            buildingDef.UtilityInputOffset = new CellOffset(0, 0);
            buildingDef.InputConduitType = ConduitType.Liquid;
            buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
            buildingDef.AudioCategory = "Glass";
            buildingDef.BaseTimeUntilRepair = -1f;
            buildingDef.ViewMode = OverlayModes.Decor.ID;
            buildingDef.PermittedRotations = PermittedRotations.R360;

            return buildingDef;
        }

        public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
        {
            LightShapePreview lightShapePreview = go.AddComponent<LightShapePreview>();
            lightShapePreview.lux = LUX;
            lightShapePreview.radius = RANGE;
            lightShapePreview.shape = LightShape.Circle;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.LightSource, false);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {

            go.AddOrGet<LoopingSounds>();
            Light2D light2D = go.AddOrGet<Light2D>();
            light2D.overlayColour = LIGHT2D.FLOORLAMP_OVERLAYCOLOR;
            light2D.Color = KELVIN1500;
            light2D.Range = RANGE;
            light2D.Offset = new Vector2(0f, 0.5f);
            light2D.shape = LightShape.Circle;
            light2D.drawOverlay = true;
            light2D.Lux = LUX;
            light2D.overlayColour = Color.red;

            go.AddOrGet<Lantern>();
            SimHashes element = SimHashes.CrudeOil; //Config.SettingsManager.Settings.LanternPowerSource == "Oil" ? SimHashes.CrudeOil : SimHashes.Petroleum;
            Tag elementTag = element.CreateTag();

            Storage storage = go.AddOrGet<Storage>();

            storage.capacityKg = CAPACITY;
            storage.showInUI = true;
            storage.allowItemRemoval = false;
            storage.storageFilters = new List<Tag> { elementTag };

            ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
            manualDeliveryKG.SetStorage(storage);
            manualDeliveryKG.requestedItemTag = elementTag;
            manualDeliveryKG.capacity = CAPACITY;
            manualDeliveryKG.refillMass = 1.5f;
            manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
            manualDeliveryKG.operationalRequirement = FetchOrder2.OperationalRequirement.Functional;
            manualDeliveryKG.allowPause = true;

            ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
            conduitConsumer.conduitType = ConduitType.Liquid;
            conduitConsumer.consumptionRate = CONSUMPTION_RATE;
            conduitConsumer.capacityTag = GameTagExtensions.Create(element);
            conduitConsumer.capacityKG = CAPACITY;
            conduitConsumer.forceAlwaysSatisfied = true;
            conduitConsumer.ignoreMinMassCheck = true;
            conduitConsumer.alwaysConsume = true;
            conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;

            ElementConverter elementConverter = go.AddComponent<ElementConverter>();
            elementConverter.consumedElements = new ElementConverter.ConsumedElement[1]
            {
                new ElementConverter.ConsumedElement(ElementLoader.FindElementByHash(element).tag, CONSUMPTION_RATE)
            };
            elementConverter.outputElements = new ElementConverter.OutputElement[]
            {
                new ElementConverter.OutputElement(0.01f, SimHashes.CarbonDioxide, 303.15f, false, false, 0f, 1f, 1f, byte.MaxValue, 0)
            };
            go.AddOrGetDef<LightController.Def>();

            go.GetComponent<RequireInputs>().SetRequirements(false, false);
            Prioritizable.AddRef(go);

        }
    }
}
