using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Weapons.Almighty
{
    public class TheJoker : ModItem
    {
        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.width = 14;
            Item.height = 14;
            Item.value = 0;
            Item.rare = ItemRarityID.Red;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (line.Mod == "Terraria" && line.Name == "ItemName")
                {
                    line.OverrideColor = Color.Lerp(Color.Red, Color.Black, 0.5f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 6f));
                }
            }
        }
    }
}
