using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace SGAmod.Items.Materials.Bars
{
	public class Entrophite : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Entrophite");
			Tooltip.SetDefault("Corrupted beyond the veils of life");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
		}
		public override void SetDefaults()
		{
			Item.value = 100;
			Item.rare = ItemRarityID.Lime;
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Bars.Entrophite>();
		}
	}

	public class WovenEntrophite : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Woven Entrophite");
			Tooltip.SetDefault("Suprisingly strong, after being interlaced with souls");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
		}

		public override void SetDefaults()
		{
			Item.value = 250;
			Item.rare = ItemRarityID.Lime;
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Bars.BarTiles>();
			Item.placeStyle = 5;
		}
		public override void AddRecipes()
		{
			/*
			Recipe.Create(10)
				.AddIngredient(ModContent.ItemType<OmniSoul>(), 1)
				.AddIngredient(ModContent.ItemType<Entrophite>(), 10)
				.AddTile(TileID.Loom)
				.Register();
			*/
		}
	}
}