using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SGAmod.Items.Armor.HallowedVisor
{
	[AutoloadEquip(EquipType.Head)]
	public class AncientHallowedVisor : ModItem, IFormerThrowingItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Hallowed Visor");
			// Tooltip.SetDefault("12% increased throwing damage\n10% increased throwing crit\n20% increased throwing velocity");
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(0,5);
			Item.rare = ItemRarityID.Pink;
			Item.defense = 16;
		}
		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Throwing) += 0.12f;
			player.GetCritChance(DamageClass.Throwing) += 10;
			player.ThrownVelocity += 0.20f;
		}
	}

	[AutoloadEquip(EquipType.Head)]
	public class HallowedVisor : AncientHallowedVisor, IFormerThrowingItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Future Hallowed Visor");
			// Tooltip.SetDefault("12% increased throwing damage\n10% increased throwing crit\n20% increased throwing velocity\n'Reverse 1.4 lol'");
		}
	}
}