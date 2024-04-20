using SGAmod.Items.Weapons.Almighty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace SGAmod.Items.Accessories
{
    public class CardDeckPersona : TheJoker
    {
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<SGAPlayer>().personaDeck = true;
        }
        public override void SetDefaults()
        {
            Item.maxStack = 1;
            Item.width = 16;
            Item.height = 16;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
        }
        public override string Texture => "SGAmod/Items/Accessories/CooldownStack/CardDeckPersona";
    }
}
