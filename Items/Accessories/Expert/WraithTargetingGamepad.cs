using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Input;
using Idglibrary;
using Microsoft.Xna.Framework;

namespace SGAmod.Items.Accessories.Expert
{
    public class WraithTargetingGamepad : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.rare = ItemRarityID.Expert;
            Item.value = Item.sellPrice(0,0,25,0);
            Item.accessory = true;
            Item.expert = true;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            string s = "Not Binded!";
            foreach (string keys in SGAmod.ToggleGamepadKey.GetAssignedKeys())
            {
                s = keys;
            }
            tooltips.Add(new TooltipLine(Mod, "uncraft", Idglib.ColorText(Color.CornflowerBlue, "Press the 'Toggle Aiming Style' (" + s + ") Hotkey to toggle modes")));
        }
        public override string Texture => "Terraria/Images/UI/HotbarRadial_2";
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();
            sgaplayer.gamePadAutoAim = 2;
			
			
        }
    }
}
