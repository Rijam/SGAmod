using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using System;

namespace SGAmod.Items.Materials.Bars
{
	public class NoviteOre : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Novite Ore");
			// Tooltip.SetDefault("Brassy scrap metal from a time along ago, might be of electronical use...");
			Item.ResearchUnlockCount = 100;
		}
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = Item.CommonMaxStack;
			Item.value = 10;
			Item.rare = ItemRarityID.Blue;
			Item.alpha = 0;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Bars.NoviteOreTile>();

		}
	}
	public class NoviteBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Novite Bar");
			// Tooltip.SetDefault("This Brassy alloy reminds you of 60s scifi");
			Item.ResearchUnlockCount = 25;
		}
		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.width = 16;
			Item.height = 16;
			Item.value = 25;
			Item.rare = ItemRarityID.Blue;
			Item.alpha = 0;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Bars.BarTiles>();
			Item.placeStyle = 1;
		}
		public override void AddRecipes()
		{
			/*
			Recipe.Create(2)
				.AddIngredient(ModContent.ItemType<NoviteOre>(), 4)
				.AddRecipeGroup("SGAmod:BasicWraithShards", 3)
				.AddTile(TileID.Furnaces)
				.Register();
			*/
		}
	}
	public class AdvancedPlating : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Advanced Plating");
			// Tooltip.SetDefault("Advanced for the land of Terraria's standards, that is");
			Item.ResearchUnlockCount = 25;
		}

		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.width = 26;
			Item.height = 14;
			Item.value = 1000;
			Item.rare = ItemRarityID.Green;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Bars.AdvancedPlatingTile>();
		}
		public override void AddRecipes()
		{
			Recipe.Create(3)
				.AddIngredient(ModContent.ItemType<NoviteBar>(), 2)
				.AddIngredient(ItemID.Wire, 10)
				.AddTile(ModContent.TileType<Tiles.CraftingStations.ReverseEngineeringStation>())
				.Register();
		}
	}
}