using SGAmod.Items.Materials.Bars;
using System;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace SGAmod
{
	public class SGARecipes : ModSystem
	{
		public override void AddRecipes()
		{
			base.AddRecipes();
<<<<<<< Updated upstream
		}
=======
            
        }
		

		
>>>>>>> Stashed changes

		// Instead of writing the string in the AddRecipeGroup, add the constant here.
		// Example: SGARecipes.EvilBossMaterials
		// No chance of misspelling the group because VS will tell you if you wrote it wrong (+ autocomplete is awesome).

		/// <summary> ItemID.CopperOre, ItemID.TinOre </summary>
		public const string Tier1Ore = "SGAmod:Tier1Ore";
		/// <summary> ItemID.IronBar, ItemID.LeadBar </summary>
		public const string Tier2Bars = "SGAmod:Tier2Bars";
		/// <summary> ItemID.SilverBar, ItemID.TungstenBar </summary>
		public const string Tier3Bars = "SGAmod:Tier3Bars";
		/// <summary> ItemID.DemoniteBar, ItemID.CrimtaneBar </summary>
		public const string Tier5Bars = "SGAmod:Tier5Bars";
		/// <summary> NovusBar, NoviteBar </summary>
		public const string NoviteNovusBars = "SGAmod:NoviteNovusBars";
		/// <summary> ItemID.ShadowScale, ItemID.TissueSample </summary>
		public const string EvilBossMaterials = "SGAmod:EvilBossMaterials";
<<<<<<< Updated upstream

=======
		/// <summary> ItemID.CobaltOre, ItemID.PalladiumOre</summary>
		public const string Tier1HardmodeOre = "SGAmod:Tier1HardmodeOre";
		/// <summary> ItemID.SolarFragment, ItemID.VortexFragment, ItemID.NebulaFragment, ItemID.StardustFragment </summary>
		public const string CelestialFragments = "SGAmod:CelestialFragments";
		
>>>>>>> Stashed changes
		public override void AddRecipeGroups()
		{
			RecipeGroup group = new(() => Language.GetTextValue("LegacyMisc.37") + "  Copper or Tin ore", new int[]
			{
				ItemID.CopperOre,
				ItemID.TinOre
			});
			RecipeGroup.RegisterGroup(Tier1Ore, group);
			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Iron or Lead bars", new int[]
			{
				ItemID.IronBar,
				ItemID.LeadBar
			});
			RecipeGroup.RegisterGroup(Tier2Bars, group);
			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Silver or Tungsten Bars", new int[]
			{
				ItemID.SilverBar,
				ItemID.TungstenBar
			});
			RecipeGroup.RegisterGroup(Tier3Bars, group);
			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Crimtane or Demonite Bars", new int[]
			{
				ItemID.DemoniteBar,
				ItemID.CrimtaneBar
			});
			RecipeGroup.RegisterGroup(Tier5Bars, group);
			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Novus or Novite Bars", new int[]
			{
				ModContent.ItemType<NovusBar>(),
				ModContent.ItemType<NoviteBar>()
			});
			RecipeGroup.RegisterGroup(NoviteNovusBars, group);
			group = new(() => Language.GetTextValue("LegacyMisc.37") + " Evil Boss Materials", new int[]
			{
				ItemID.ShadowScale,
				ItemID.TissueSample
			});
			RecipeGroup.RegisterGroup(EvilBossMaterials, group);
<<<<<<< Updated upstream
=======
			group = new(() => Language.GetTextValue("LegacyMisc.37" + " Cobalt or Palladium ore"), new int[]
			{
				ItemID.CobaltOre,
				ItemID.PalladiumOre
			});
			RecipeGroup.RegisterGroup(Tier1HardmodeOre, group);
			group = new(() => Language.GetTextValue("LegacyMisc.37" + "Lunar Fragments"), new int[]
			{
				ItemID.FragmentSolar,
				ItemID.FragmentVortex,
				ItemID.FragmentNebula,
				ItemID.FragmentStardust
			});
			RecipeGroup.RegisterGroup(CelestialFragments, group);
		}

        public override void PostAddRecipes()
        {
			for (int i = 0; i < Recipe.numRecipes; i++)
			{
				Recipe recipe = Main.recipe[i];

				if ((recipe.HasTile(TileID.Furnaces) || recipe.requiredTile.Any(tile => tile == TileID.Furnaces)) && !SGAWorld.downedCopperWraith)
				{
					recipe.AddOnCraftCallback(SGARecipeCallbacks.WraithWarning);
				}
				if ((recipe.HasResult(ItemID.TitaniumForge) || recipe.HasResult(ItemID.AdamantiteForge)) && !SGAWorld.downedCobaltWraith)
				{
					recipe.AddCondition(new Condition("Mods.SGAmod.Common.Conditions.CobaltWraith", () => SGAWorld.downedCobaltWraith));
				}
			}
			
		}
    }
	
	public static class SGARecipeCallbacks
	{
		public static void WraithWarning(Recipe recipe, Item item, List<Item> consumedItems, Item destinationStack)
		{
				if (!NPC.AnyNPCs(ModContent.NPCType<CopperWraith>()))
				{
					if(Main.netMode > NetmodeID.MultiplayerClient)
					{
						
					}
					else
					{
						SGAWorld.CraftWarning();
					}
				}
>>>>>>> Stashed changes
		}
	}
}