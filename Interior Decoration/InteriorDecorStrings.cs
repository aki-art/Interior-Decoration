﻿using STRINGS;

namespace InteriorDecoration
{

    public static class InteriorDecorStrings
    {

        public class BUILDINGS
        {
            public class PREFABS
            {
                public class GLASS_SCULPTURE
                {
                    public static LocString NAME = UI.FormatAsLink("Glass Block", Buildings.GlassSculpture.GlassSculptureConfig.ID.ToUpperInvariant());
                    public static LocString DESC = "Duplicants who have learned art skills can produce more decorative sculptures.";
                    public static LocString EFFECT = "Majorly increases " + UI.FormatAsLink("Decor", "DECOR") + ", contributing to " + UI.FormatAsLink("Morale", "MORALE") + ".\n\nMust be sculpted by a Duplicant.";
                    public static LocString POORQUALITYNAME = "\"Abstract\" Glass Sculpture";
                    public static LocString AVERAGEQUALITYNAME = "Mediocre Glass Sculpture";
                    public static LocString EXCELLENTQUALITYNAME = "Genius Glass Sculpture";
                }

                public class MOODLAMP
                {
                    public static LocString NAME = UI.FormatAsLink("Window Tile", "GLASSTILE");
                    public static LocString DESC = "Light reduces Duplicant stress and is required to grow certain plants.";
                    public static LocString EFFECT = "Provides " + UI.FormatAsLink("Light", "LIGHT") + " when " + UI.FormatAsLink("Powered", "POWER") + ".\n\nDuplicants can operate buildings more quickly when the building is lit.";
                }

                public class STAINED_GLASS_TILE
                {
                    public static LocString NAME = UI.FormatAsLink("Stained Glass Tile", Buildings.StainedGlassTiles.StainedGlassTileConfig.ID.ToUpperInvariant());
                    public static LocString DESC = "Window tiles provide a barrier against liquid and gas and are completely transparent.";
                    public static LocString EFFECT = $"Used to build the walls and floors of rooms.\n\nAllows {UI.FormatAsLink("Light", "LIGHT")} and {UI.FormatAsLink("Decor", "DECOR")} to pass through.";
                }

                public class FOSSIL_STAND
                {
                    public static LocString NAME = UI.FormatAsLink("Fossil Display", Buildings.FossilDisplay.FossilStandConfig.ID.ToUpperInvariant());
                    public static LocString DESC = "Duplicants who have learned science skills can produce more believable reconstructions.";
                    public static LocString EFFECT = $"Majorly increases {UI.FormatAsLink("Decor", "DECOR")}, contributing to  + {UI.FormatAsLink("Morale", "MORALE")}.\n\nMust be sculpted by a Duplicant.";
                    public static LocString POORQUALITYNAME = "Questionable Fossil Display";
                    public static LocString AVERAGEQUALITYNAME = "Speculative Fossil Display";
                    public static LocString EXCELLENTQUALITYNAME = "Marvelous Fossil Display";
                }

                public class LANTERN
                {

                    public static LocString NAME = UI.FormatAsLink("Lantern", Buildings.Lantern.LanternConfig.ID.ToUpperInvariant());
                    public static LocString DESC = "Light reduces Duplicant stress and is required to grow certain plants.";
                    public static LocString OILEFFECT = "Provides " + STRINGS.UI.FormatAsLink("Light", "LIGHT") + " when powered with " + STRINGS.UI.FormatAsLink("Crude Oil", "CRUDEOIL") + ".\n\nDuplicants can operate buildings more quickly when the building is lit.";
                    public static LocString PETROLEUMEFFECT = "Provides " + STRINGS.UI.FormatAsLink("Light", "LIGHT") + " when powered with " + STRINGS.UI.FormatAsLink("Petroleum", "PETROLEUM") + ".\n\nDuplicants can operate buildings more quickly when the building is lit.";
                }
            }
        }

        public class DUPLICANTS
        {
            public class STATUSITEMS
            {
                public class INSPIREDRESEARCHEFFICIENCYBONUS
                {
                    public static LocString NAME1 = "Mildly Curious";
                    public static LocString NAME2 = "Curious";
                    public static LocString NAME3 = "Super Curious";
                    public static LocString TOOLTIP = "This Duplicant can't wait to learn more about their World!";
                }
            }
        }

        public class IDUI
        {
            public class IDMODSETTINGSSCREEN
            {
                public class PACUCYLCE
                {
                    public static LocString STASIS = "Stasis";
                    public static LocString STASISDESC = "Pacus in aquariums do not age or die.\nProduction and reproduction halted.";
                    public static LocString NORMAL = "Normal";
                    public static LocString NORMALDESC = "Pacus produce and die at a normal rate.";
                    public static LocString TENTIMES = "10x";
                    public static LocString TENTIMESDESC = "Pacus produce 10 times slower, but die much later.";
                }

                public class LANTERNCYCLE
                {
                    public static LocString OIL = "Oil";
                    public static LocString PETROLEUM = "Petroleum";
                }

                public class STAINEDSPEED
                {
                    public static LocString NOBONUS = "No bonus";
                    public static LocString SMALL = "Small Bonus";
                    public static LocString REGULAR = "Regular tiles";
                    public static LocString HASTY = "Hasty";
                    public static LocString METAL = "Metal tiles";
                    public static LocString FAST = "Fast";

                }
            }
        }
    }
}
