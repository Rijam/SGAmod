using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using System;

namespace SGAmod.Items.Materials.Bars
{
	public class PrismalOre : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismal Ore");
			Tooltip.SetDefault("The power inside is cracked wide open, ready to be used");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
		}
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 99;
			Item.value = 7500;
			Item.rare = ItemRarityID.Yellow;
			Item.alpha = 0;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Bars.PrismalTile>();

		}

		public override void AddRecipes()
		{
			/*
			Recipe.Create(16)
				.AddIngredient(ModContent.ItemType<UnmanedOre>(), 8)
				.AddIngredient(ModContent.ItemType<NoviteOre>(), 8)
				.AddIngredient(ModContent.ItemType<WraithFragment3>(), 1)
				.AddIngredient(ModContent.ItemType<Fridgeflame>(), 3)
				.AddIngredient(ModContent.ItemType<OmniSoul>(), 2)
				.AddIngredient(ItemID.CrystalShard, 3)
				.AddIngredient(ItemID.BeetleHusk, 1)
				.AddTile(ModContent.TileType<Tiles.CraftingStations.PrismalStation>())
				.Register();
			*/
		}
	}
	public class PrismalBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismal Bar");
			Tooltip.SetDefault("It radiates the true energy of Novus");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 20;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 0, 40, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Bars.BarTiles>();
			Item.placeStyle = 6;
		}
		public override void AddRecipes()
		{
			Recipe.Create(1)
				.AddIngredient(ModContent.ItemType<PrismalOre>(), 4)
				.AddTile(TileID.AdamantiteForge)
				.Register();
		}

	}
}