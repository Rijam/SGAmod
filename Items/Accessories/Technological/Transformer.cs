using SGAmod.Items.Materials.Bars;
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
    [AutoloadEquip(EquipType.Back)]
    public class Transformer : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 24;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0,1,0,0);
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            SGAPlayer sgaply = player.GetModPlayer<SGAPlayer>();
            sgaply.electricRechargeRate += player.HasBuff(BuffID.Electrified) ? 10 : 2;
            sgaply.electricChargeMax += 2000;
            sgaply.transformerAccessory = true;
            
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<ManaBattery>(3)
                .AddIngredient<VialOfAcid>(12)
                .AddIngredient<NoviteBar>(6)
                .AddTile<ReverseEngineeringStation>()
                .Register();
        }
    }
}
