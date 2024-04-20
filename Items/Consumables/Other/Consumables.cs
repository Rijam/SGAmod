using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Consumables.Other
{
    public class TrueCopperWraithNotch : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 14;
            Item.maxStack = 30;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(gold: 1);
            Item.useStyle = 2;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item9;
            Item.consumable = true;
        }

        public override string Texture => "SGAmod/Items/Consumables/Other/TrueWraithNotch";

        public override Color? GetAlpha(Color lightColor)
        {
            return Main.hslToRgb((Main.GlobalTimeWrappedHourly * 0.916f) % 1f, 0.8f, 0.75f) ;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (line.Mod == "Terraria" && line.Name == "ItemName")
                {
                    line.OverrideColor = Color.Lerp(Color.Yellow, Color.Blue, 0.5f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 0.5f));
                }
            }
        }
        public override bool CanUseItem(Player player)
        {
            SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();
            return !sgaplayer.Drakenshopunlock;
        }
        public override bool? UseItem(Player player)
        {
            SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();
            sgaplayer.Drakenshopunlock = true;
            return base.UseItem(player);
        }
    }
}
