using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Accessories.Expert
{
	public class AlkalescentHeart : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Alkalescent Heart");
			/* Tooltip.SetDefault("'The Spider Queen's toxic blood pumps through you!'\nDealing crits debuff enemies, doing more damage while they are debuffed as follows:\nWhile not poisoned, poison enemies\nWhile poisoned, do 5% more damage and next crit Venoms\n" +
				"While Venomed, do 10% more damage and next crit Acid Burns\nWhile Acid Burned, do 15% more damage" +
				"\nThese damage boosts do not stack; highest takes priority\nMinions may infict this based off your highest crit chance"); */
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			//player.GetModPlayer<SGAPlayer>().alkalescentHeart = true; #TODO
		}

		public override void SetDefaults()
		{
			Item.maxStack = 1;
			Item.width = 26;
			Item.defense = 0;
			Item.accessory = true;
			Item.height = 14;
			Item.value = Item.buyPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.expert = true;
		}
	}
}