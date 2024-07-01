using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SGAmod.Buffs.Debuffs
{
    public class DankSlow : ModBuff
    {
        public static string DankText => Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl) ? Language.GetTextValue("Mods.SGAmod.Buffs.DankSlow.SlowExplain") : Language.GetTextValue("Mods.SGAmod.Buffs.DankSlow.SlowPrompt");
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override bool ReApply(NPC npc, int time, int buffIndex)
        {
            npc.buffTime[buffIndex] = (int)Math.Pow(npc.buffTime[buffIndex] + (int)time, 0.98);
            return true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<SGAnpcs>().TimeSlow += (npc.buffTime[buffIndex] / (60f * 5f));
            npc.GetGlobalNPC<SGAnpcs>().DankSlow = true;
        }
    }
}
