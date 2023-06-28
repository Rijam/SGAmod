using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Placeable.Environment
{
	public class MoistStone : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = Item.CommonMaxStack;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Environment.MoistStone>();
		}
	}
	public class MoistSand : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = Item.CommonMaxStack;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Environment.MoistSand>();
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Moist Sand");
			// Tooltip.SetDefault("'expect nothing else from sand thrown into water'\nPlace a Cactus block under sand to stop it from becoming moist in water");
		}
		public override void AddRecipes()
		{
			Recipe.Create(ItemID.SandBlock, 1)
				.AddIngredient(ModContent.ItemType<MoistSand>())
				.AddTile(TileID.Furnaces)
				.Register();

			CreateRecipe(1)
				.AddIngredient(ItemID.SandBlock)
				.AddCondition(Condition.NearWater)
				.Register();
		}
	}
}