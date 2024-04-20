using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SGAmod.Items.Weapons.Almighty
{
    [Autoload(false)]
    public class AlmightyWeapon : ModItem
    {
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            float[] highestDamage = { player.GetDamage(DamageClass.Melee).Additive , player.GetDamage(DamageClass.Ranged).Additive, player.GetDamage(DamageClass.Magic).Additive, player.GetDamage(DamageClass.Summon).Additive};
            damage += highestDamage.OrderBy(testby => testby).Reverse().ToArray()[0];
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
            if ( (tt != null))
            {
                string[] thetext = tt.Text.Split(' ');
                string newline = "";
                List<string> valuez = new List<string>();
                foreach (string text2 in thetext)
                {
                    valuez.Add(text2 + " ");
                }

                valuez.Insert(1, "Almighty ");
                foreach (string text3 in valuez)
                {
                    newline += text3;
                }
                tt.Text = newline;
            }
            if (GetType() == typeof(NuclearOption) && ModLoader.TryGetMod("Calamity", out Mod calamidty))
                tooltips.Add(new TooltipLine(Mod, "NuclearInferdumbFallout", Language.GetTextValue("Mods.SGAmod.Items.Almighty.Inferdumb")));
            tooltips.Add(new TooltipLine(Mod, "AlmightyText", Language.GetTextValue("Mods.SGAmod.Items.Almighty.AlmightyText")));
        }
    }
}
