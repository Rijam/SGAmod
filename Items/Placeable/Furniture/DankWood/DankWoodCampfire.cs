using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Placeable.Furniture.DankWood
{
	public class DankWoodCampfire : ModItem
	{
		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.DankWood.DankWoodCampfire>(), 0);
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddRecipeGroup(RecipeGroupID.Wood, 10)
				.AddIngredient<DankWoodTorch>(5)
				.Register();
		}
	}
}
