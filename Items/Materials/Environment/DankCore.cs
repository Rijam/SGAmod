using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Materials.Environment
{
	public class DankCore : ModItem
	{
		public override void SetDefaults()
		{
			Item.value = 2500;
			Item.rare = ItemRarityID.Green;
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = Item.CommonMaxStack;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Dank Core");
			// Tooltip.SetDefault("'Dark, Dank, Dangerous...'");
		}
	}
}