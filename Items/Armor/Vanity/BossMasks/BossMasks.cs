using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Armor.Vanity.BossMasks
{
	#region Spider Queen Mask
	[AutoloadEquip(EquipType.Head)]
	public class SpiderQueenMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Spider Queen Mask");
		}
		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 24;
			Item.value = Item.sellPrice(silver: 75);
			Item.rare = ItemRarityID.Blue;//1
			Item.vanity = true;
			Item.defense = 0;
		}
	}
	#endregion
}