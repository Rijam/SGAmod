using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SGAmod.Items.Placeable.Furniture.DankWood
{
	public class DankWoodWall : ModItem
	{
		public override void SetDefaults() {
			Item.width = 12;
			Item.height = 12;
			Item.maxStack = Item.CommonMaxStack;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 7;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createWall = ModContent.WallType<Tiles.Furniture.DankWood.DankWoodWall>();
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(4);
			recipe.AddIngredient(ModContent.ItemType<DankWoodItem>(), 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
	public class DankWoodFence : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 12;
			Item.height = 12;
			Item.maxStack = Item.CommonMaxStack;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 7;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createWall = ModContent.WallType<Tiles.Furniture.DankWood.DankWoodFence>();
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(4);
			recipe.AddIngredient(ModContent.ItemType<DankWoodItem>(), 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
	public class BrokenDankWoodFence : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 12;
			Item.height = 12;
			Item.maxStack = Item.CommonMaxStack;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 7;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createWall = ModContent.WallType<Tiles.Furniture.DankWood.BrokenDankWoodFence>();
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(4);
			recipe.AddIngredient(ModContent.ItemType<DankWoodItem>(), 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}