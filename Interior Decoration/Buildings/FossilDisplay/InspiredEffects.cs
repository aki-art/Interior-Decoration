using Klei.AI;
using System.Collections.Generic;

namespace InteriorDecoration.Buildings.FossilDisplay
{
    class InspiredEffects
    {
		private static Effect CreateLittleInspiredEffect()
		{
			var inspired = new Effect("Inspired1", InteriorDecorStrings.DUPLICANTS.STATUSITEMS.INSPIREDRESEARCHEFFICIENCYBONUS.NAME1, InteriorDecorStrings.DUPLICANTS.STATUSITEMS.INSPIREDRESEARCHEFFICIENCYBONUS.TOOLTIP, 60f, true, false, false)
			{
				SelfModifiers = new List<AttributeModifier>() {
					new AttributeModifier("Learning", 2)
				}
			};

			return inspired;
		}
		private static Effect CreateInspiredEffect()
		{
			var inspired = new Effect("Inspired2", InteriorDecorStrings.DUPLICANTS.STATUSITEMS.INSPIREDRESEARCHEFFICIENCYBONUS.NAME2, InteriorDecorStrings.DUPLICANTS.STATUSITEMS.INSPIREDRESEARCHEFFICIENCYBONUS.TOOLTIP, 90f, true, false, false)
			{
				SelfModifiers = new List<AttributeModifier>() {
					new AttributeModifier("Learning", 4)
				}
			};

			return inspired;
		}
		private static Effect CreateSuperInspiredEffect()
		{
			var inspired = new Effect("Inspired3", InteriorDecorStrings.DUPLICANTS.STATUSITEMS.INSPIREDRESEARCHEFFICIENCYBONUS.NAME3, InteriorDecorStrings.DUPLICANTS.STATUSITEMS.INSPIREDRESEARCHEFFICIENCYBONUS.TOOLTIP, 120f, true, false, false)
			{
				SelfModifiers = new List<AttributeModifier>() {
					new AttributeModifier("Learning", 8)
				}
			};

			return inspired;
		}

		public static List<Effect> GetEffectsList()
		{
			return new List<Effect>
			{
				CreateLittleInspiredEffect(),
				CreateInspiredEffect(),
				CreateSuperInspiredEffect()
			};
		}
	}
}
