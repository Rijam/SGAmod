using SGAmod.Items.Materials.BossDrops;
using SGAmod.Items.Materials.Crafted;
using SGAmod.Tiles.CraftingStations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Accessories.Technological
{
    public class BlinkTech : ModItem
    {
        public override void SetDefaults()
        {
            Item.maxStack = 1;
            Item.width = 16;
            Item.height = 16;
            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
            
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            SGAPlayer sgaply = player.GetModPlayer<SGAPlayer>();
            if (!hideVisual) sgaply.maxBlink += 3;
            sgaply.techDamage += 0.05f;
            sgaply.electricChargeMax += 1500;
            sgaply.electricRechargeRate += 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Glass, 10)
                .AddIngredient(ItemID.MeteoriteBar, 3)
                .AddIngredient<ManaBattery>()
                .AddIngredient<VialOfAcid>(12)
                .AddTile<ReverseEngineeringStation>()
                .Register();

        }
    }
}
