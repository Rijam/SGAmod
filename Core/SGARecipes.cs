using SGAmod.Items.Materials.Bars;
using SGAmod.NPCs.Bosses.CopperWraith;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;


namespace SGAmod
{
	public class SGARecipes : ModSystem
	{
		public override void AddRecipes()
		{
			base.AddRecipes();
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];

                if (recipe.HasTile(TileID.Furnaces))
                {
                    recipe.AddOnCraftCallback(SGARecipeCallbacks.WraithWarning);
                }
            }
        }
		

		

		// Instead of writing the string in the AddRecipeGroup, add the constant here.
		// Example: SGARecipes.EvilBossMaterials
		// No chance of misspelling the group because VS will tell you if you wrote it wrong (+ autocomplete is awesome).
		/// <summary> TileID.Sets.Mud[Item.createTile]</summary>
		public const string Mud = "SGAmod:Mud";
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
		/// <summary> ItemID.CobaltOre, ItemID.PalladiumOre</summary>
		public const string Tier1HardmodeOre = "SGAmod:Tier1HardmodeOre";

		public override void AddRecipeGroups()
		{
			List<int> chests = new List<int>();
            List<int> mud = new List<int>();

			for (int  i= 0; i < TextureAssets.Item.Length; i++)
			{
				Item item = new Item();
				item.SetDefaults(i);

				if (!item.consumable || item.createTile < 0 || (item.ModItem != null && item.ModItem.Mod == Mod)) continue;

				if (TileID.Sets.BasicChest[item.createTile])
				{
					chests.Add(item.type);
					continue;
				}
				if (TileID.Sets.Mud[item.createTile])
				{
					mud.Add(item.type);
					continue;
				}
			}
			RecipeGroup groupspecial = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + "Chest", chests.ToArray());
			RecipeGroup.RegisterGroup("SGAmod:Chests", groupspecial);

			groupspecial = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + "Chest", mud.ToArray());
			RecipeGroup.RegisterGroup("SGAmod:Mud", groupspecial);

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
			group = new(() => Language.GetTextValue("LegacyMisc.37" + " Cobalt or Palladium ore"), new int[]
			{
				ItemID.CobaltOre,
				ItemID.PalladiumOre
			});
			RecipeGroup.RegisterGroup(Tier1HardmodeOre, group);
		}

        public override void PostAddRecipes()
        {
            
        }
    }

	public static class SGARecipeCallbacks
	{
		public static void WraithWarning(Recipe recipe, Item item, List<Item> consumedItems, Item destinationStack)
		{
			if ((recipe.HasTile(TileID.Furnaces) || recipe.requiredTile.Any(tile => tile == TileID.Furnaces)) && SGAWorld.downedWraiths < 1)
			{
				if (!NPC.AnyNPCs(ModContent.NPCType<CopperWraith>()))
				{
					if(Main.netMode > NetmodeID.SinglePlayer)
					{
						
					}
					else
					{
						SGAWorld.CraftWarning();
					}
				}
			}
		}
	}
}