using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Placeable.Furniture.DankWood
{
	public class DankDoorItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("Confuses hostile mobs when hit due to the released stench");
		}

		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.DankWood.DankDoorClosed>());
			Item.width = 14;
			Item.height = 28;
			Item.value = 150;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.WoodenDoor)
				.AddIngredient(ModContent.ItemType<DankWoodItem>(), 15)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}