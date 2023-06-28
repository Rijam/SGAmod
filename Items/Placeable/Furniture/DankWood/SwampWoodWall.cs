using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Placeable.Furniture.DankWood
{
	public class SwampWoodWall : ModItem
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
			Item.createWall = ModContent.WallType<Tiles.Furniture.DankWood.SwampWoodWall>();
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Swamp Wood Wall");
			// Tooltip.SetDefault("'For when you wanna bit a bit closer to Nature'");
		}

		public override void AddRecipes()
		{
			CreateRecipe(4)
				.AddIngredient(ModContent.ItemType<DankWoodItem>())
				.AddTile(TileID.LivingLoom)
				.Register();
		}
	}
	
}
