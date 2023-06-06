using Idglibrary;
using SGAmod.Buffs.Debuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Accessories.Defense
{
	[AutoloadEquip(EquipType.Face)]
	public class CorrodedSkull : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Corroded Skull");
			// Tooltip.SetDefault("'It seems suprisingly intact, yet corroded by the Spider Queen'\nGrants immunity against Acid Burn\nGrants 25% increased radiation resistance");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.buffImmune[ModContent.BuffType<AcidBurn>()] = true;
			player.GetModPlayer<IdgPlayer>().radresist += 0.25f;
		}

		public override void SetDefaults()
		{
			Item.maxStack = 1;
			Item.width = 26;
			Item.defense = 0;
			Item.accessory = true;
			Item.height = 14;
			Item.value = 5000;
			Item.rare = ItemRarityID.Green;
			Item.expert = false;
		}
	}
}