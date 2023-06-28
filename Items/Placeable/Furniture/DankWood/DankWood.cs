using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace SGAmod.Items.Placeable.Furniture.DankWood
{
	public class DankWoodItem : ModItem
	{
		public override void SetDefaults()
		{
			Item.value = 50;
			Item.rare = ItemRarityID.Blue;
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = Item.CommonMaxStack;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Furniture.DankWood.DankWoodBlock>();
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Dank Wood");
			// Tooltip.SetDefault("It smells odd...");
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<DankWoodFence>(), 4);
			recipe.Register();

			recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<BrokenDankWoodFence>(), 4);
			recipe.Register();

			recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<DankWoodWall>(), 4);
			recipe.Register();

			recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<DankWoodPlatform>(), 2);
			recipe.Register();

			recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<SwampWoodWall>(), 4);
			recipe.Register();
		}
	}
}