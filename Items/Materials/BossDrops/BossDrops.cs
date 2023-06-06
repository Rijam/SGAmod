using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Materials.BossDrops
{
	public class VialOfAcid : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Vial of Acid");
			// Tooltip.SetDefault("Highly Corrosive");
		}

		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.width = 16;
			Item.height = 16;
			Item.value = 100;
			Item.rare = ItemRarityID.Green;
		}
	}
}