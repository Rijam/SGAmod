using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Buffs.Debuffs
{
	public class AcidBurn : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Acid Burn");
			// Description.SetDefault("Reduced Defense and your defense works again your life");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<SGAPlayer>().acidBurn = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<SGAnpcs>().acidBurn = true;
			npc.GetGlobalNPC<SGAnpcs>().reducedDefense += 5;
		}
	}
}