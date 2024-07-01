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
    public class RadioDebuff : ModBuff
    {
        public static string RadioactiveDebuffText => Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl) ? Language.GetTextValue("Mods.SGAmod.Buffs.RadioDebuff.RadioExplain") : Language.GetTextValue("Mods.SGAmod.Buffs.RadioDebuff.RadioPrompt");

        public override string Texture => "SGAmod/Buffs/BuffTemplate";
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.buffTime[buffIndex] < 200) 
            {
                npc.GetGlobalNPC<SGAnpcs>().IrradiatedAmount_ = 0;
                npc.GetGlobalNPC<SGAnpcs>().IrradiatedAmount = 0;
            }
        }
    }
}
