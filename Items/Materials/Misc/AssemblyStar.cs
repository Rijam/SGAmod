using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Materials.Misc
{
    public class AssemblyStar : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.width = 14;
            Item.height = 14;
            Item.value = 0;
            Item.rare = ItemRarityID.Quest;
        }
        public override string Texture =>"Terraria/Images/SunOrb";
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Orange* MathHelper.Clamp((float)(Math.Sin(Main.GlobalTimeWrappedHourly) / 2)+1f, 0, 1);
        }
    }
}
