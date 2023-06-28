using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using System;

namespace SGAmod.Items.Materials.Bars
{
	public class NovusOre : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Novus Ore");
			// Tooltip.SetDefault("Stone laden with doment power...");
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
			Item.createTile = ModContent.TileType<Tiles.Bars.NovusOreTile>();

		}
	}
	public class NovusBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Novus Bar");
			// Tooltip.SetDefault("This alloy of Novus and the power of the wraiths have awakened some of its dorment power\nMay be interchanged for iron bars in some crafting recipes");
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
			Item.placeStyle = 2;
		}
		public override void AddRecipes()
		{
			/*
			Recipe.Create(2)
				.AddIngredient(ModContent.ItemType<NovusOre>(), 4)
				.AddRecipeGroup("SGAmod:BasicWraithShards", 3)
				.AddTile(TileID.Furnaces)
				.Register();
			*/
		}
	}
}