using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace InteriorDecoration.Settings
{
    public class UserSettings
    {
        [JsonProperty]
        public bool GlassSculpturesIceHatch { get; set; } = true;
        [JsonProperty]
        public bool GlassSculpturesFabUnicorns { get; set; } = false;
        [JsonProperty]
        public float StainedGlassTileSpeedMultiplier { get; set; } = 1.1f;
        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public LanternFuel LanternCycle { get; set; } = LanternFuel.Oil;
        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public PacuLifeCycle AquariumPacuLifeCycle { get; set; } = PacuLifeCycle.Stasis;
        [JsonProperty]
        public bool BiscuitLiquid { get; set; } = false;
        [JsonProperty]
        public bool UseSafeFolder { get; set; } = true;
        [JsonProperty]
        public bool DisableVisualFX { get; set; } = false;

        public enum PacuLifeCycle
        {
            Stasis,
            Normal,
            TenTimes
        }

        public enum LanternFuel
        {
            Oil,
            Petroleum
        }
    }
}
