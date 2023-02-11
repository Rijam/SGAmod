using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace SGAmod.Items.Materials.Bars
{
	public class Photosyte : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Photosyte");
			// Tooltip.SetDefault("'Parasitic plant matter'\nIs found largely infesting clouds where it can gain the most sunlight");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
			ItemSets.havocItem.Add(Type);
		}

		public override void SetDefaults()
		{

			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.rare = ItemRarityID.Green;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Bars.Photosyte>();
		}
	}
	public class PhotosyteBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Photosyte Bar");
			// Tooltip.SetDefault("A hardened bar made from parasitic biomass reacting from murky gel and moss");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
			ItemSets.havocItem.Add(Type);
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 14;
			Item.maxStack = 99;
			Item.value = 100;
			Item.rare = ItemRarityID.Green;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Bars.BarTiles>();
			Item.placeStyle = 0;
		}

		public override void AddRecipes()
		{
			/*
			Recipe.Create(3)
				.AddIngredient(ModContent.ItemType<Photosyte>(), 5)
				.AddIngredient(ModContent.ItemType<MurkyGel>(), 2)
				.AddIngredient(ModContent.ItemType<DecayedMoss>(), 1)
				.AddIngredient(ModContent.ItemType<SwampSeeds>(), 2)
				.AddIngredient(ModContent.ItemType<MoistSand>(), 1)
				.AddTile(TileID.Furnaces)
				.Register();
			*/
		}
	}
}