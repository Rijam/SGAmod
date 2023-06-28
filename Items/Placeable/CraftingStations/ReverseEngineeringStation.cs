using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.ID;
using Idglibrary;
using Terraria;

namespace SGAmod.Items.Placeable.CraftingStations
{
	public class ReverseEngineeringStation : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Reverse Engineering Station");
			// Tooltip.SetDefault("Allows weaponization of unusual tidbits and crafting of advanced machinery\nSome formerly uncraftable items may be crafted here\nDoubles as a Tinkerer's Workbench");
		}
		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.width = 26;
			Item.height = 14;
			Item.value = Item.sellPrice(0,0,75,0);
			Item.rare = ItemRarityID.Blue;
			Item.alpha = 0;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.CraftingStations.ReverseEngineeringStation>();
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			string s = "Not Binded!";
			/*
			foreach (string key in SGAmod.ToggleRecipeHotKey.GetAssignedKeys())
			{
				s = key;
			}
			*/

			tooltips.Add(new TooltipLine(Mod, "uncraft", Idglib.ColorText(Color.CornflowerBlue, "Allows you to uncraft non-favorited held items on right click")));
			tooltips.Add(new TooltipLine(Mod, "uncraft", Idglib.ColorText(Color.CornflowerBlue, "Press the 'Toggle Recipe' (" + s + ") Hotkey to swap between recipes")));
			tooltips.Add(new TooltipLine(Mod, "uncraft", Idglib.ColorText(Color.CornflowerBlue, "There is a net loss in materials on uncraft, this can however be reduced")));
		}

		public override void AddRecipes()
		{
			/*
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.TinkerersWorkshop, 1);
			recipe.AddIngredient(ItemID.MeteoriteBar, 8);
			recipe.AddIngredient(ModContent.ItemType<Consumables.EnergizerBattery>(), 5);
			recipe.AddIngredient(ModContent.ItemType<Weapons.Technical.LaserMarker>(), 10);
			recipe.AddIngredient(ModContent.ItemType<VialofAcid>(), 25);
			recipe.AddRecipeGroup("SGAmod:EvilBossMaterials", 15);
			//recipe.AddIngredient(mod.ItemType("WraithFragment3"), 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();

			recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.TinkerersWorkshop, 1);
			recipe.AddIngredient(ItemID.MeteoriteBar, 3);
			recipe.AddIngredient(ModContent.ItemType<VialofAcid>(), 8);
			recipe.AddRecipeGroup("SGAmod:PressurePlates", 2);
			//recipe.AddIngredient(mod.ItemType("WraithFragment3"), 10);
			recipe.AddRecipeGroup("SGAmod:EvilBossMaterials", 8);
			recipe.AddRecipeGroup("SGAmod:TechStuff", 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
			*/
		}
	}
}