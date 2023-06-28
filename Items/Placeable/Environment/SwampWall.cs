using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Placeable.Environment
{
	public class SwampWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Debug: Swamp Wall");
			// Tooltip.SetDefault("Use this in case your Dank Shrines get messed up to fix them");
		}
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
			Item.createWall = ModContent.WallType<Tiles.Environment.SwampWall>();
		}
	}
}
