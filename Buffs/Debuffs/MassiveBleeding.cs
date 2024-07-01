using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Buffs.Debuffs
{
    public class MassiveBleeding : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<SGAPlayer>().MassiveBleeding = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.buffImmune[BuffID.Bleeding])
            {
                npc.DelBuff(buffIndex);
                return;
            }
            npc.GetGlobalNPC<SGAnpcs>().MassiveBleeding = true;
        }
    }
    
}
