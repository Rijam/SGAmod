using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Misc
{
	public class ExpertiseItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("Expertise!");
			// Tooltip.SetDefault("Earn Expertise by slaying certain enemies!\nSpend Expertise at the Draken's shop!");
		}

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 999;
			Item.rare = ItemRarityID.White;
			Item.value = 0;
		}
	}
}