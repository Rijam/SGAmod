using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace SGAmod.Items.Materials.Bars
{
	public class VirulentOre : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Virulent Ore");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
			ItemSets.havocItem.Add(Type);
		}

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 99;
			Item.value = 100;
			Item.rare = ItemRarityID.Pink;
			Item.alpha = 0;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Bars.VirulentOre>();
		}
	}
	public class VirulentBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Virulent Bar");
			Tooltip.SetDefault("Condensed life essence in bar form");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
			ItemSets.havocItem.Add(Type);
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 14;
			Item.maxStack = 99;
			Item.value = 1000;
			Item.rare = ItemRarityID.Pink;
			Item.alpha = 0;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Bars.BarTiles>();
			Item.placeStyle = 4;
		}

		public override void AddRecipes()
		{
			Recipe.Create(1)
				.AddIngredient(ModContent.ItemType<PhotosyteBar>(), 1)
				.AddIngredient(ModContent.ItemType<VirulentOre>(), 3)
				.AddTile(TileID.Hellforge)
				.Register();
		}
	}
}