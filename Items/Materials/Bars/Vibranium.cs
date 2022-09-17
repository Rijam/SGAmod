using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using System;

namespace SGAmod.Items.Materials.Bars
{
	public class VibraniumCrystal : ModItem //, IRadioactiveItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibranium Crystal");
			Tooltip.SetDefault("'Makes a humming sound while almost shaking out your hands'");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 16;
			Item.height = 16;
			Item.value = 500;
			Item.rare = ItemRarityID.Red;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = Mod.Find<ModTile>("VibraniumCrystalTile").Type;
		}

		public int RadioactiveHeld()
		{
			return 3;
		}

		public int RadioactiveInventory()
		{
			return 3;
		}

	}
	public class VibraniumPlating : VibraniumCrystal
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibranium Plating");
			Tooltip.SetDefault("'Dark cold steel; it constantly vibrates to the touch'");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
		}

		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 16;
			Item.height = 16;
			Item.value = 400;
			Item.rare = ItemRarityID.Purple;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Bars.VibraniumPlatingTile>();
		}
	}

	public class VibraniumBar : ModItem // VibraniumText
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibranium Bar");
			Tooltip.SetDefault("'This alloy is just barely stable enough to not phase out of existance'");
		}

		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 16;
			Item.height = 16;
			Item.value = 2500;
			Item.rare = ItemRarityID.Purple;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Bars.BarTiles>();
			Item.placeStyle = 8;
		}
		public override void AddRecipes()
		{
			/*
			Recipe.Create(2)
				.AddIngredient(ModContent.ItemType<VibraniumCrystal>(), 3)
				.AddIngredient(ModContent.ItemType<VibraniumPlating>(), 3)
				.AddIngredient(ItemID.LunarBar, 2)
				.AddIngredient(ModContent.ItemType<LunarRoyalGel>(), 1)
				.AddTile(ModContent.TileType<Tiles.CraftingStations.ReverseEngineeringStation>())
				.AddCondition(Recipe.Condition.NearLava)
				.Register();
			*/
		}
	}
}