using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using SGAmod.Items.Materials.BossDrops;
using SGAmod.Items.Placeable.Environment;

namespace SGAmod.Items.Consumables.Potions
{
    public class EnergizerBattery : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(0,0,10,0);
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item2;
            Item.consumable = true;
        }
        public override bool CanUseItem(Player player)
        {

            return player.GetModPlayer<SGAPlayer>().CooldownStacks.Count < player.GetModPlayer<SGAPlayer>().MaxCooldownStacks;
        }
        public override bool? UseItem(Player player)
        {
            int max = 2000; //+(SGAWorld.downedSharkvern ? 1000 : 0) + (SGAWorld.downedWraiths > 3 ? 2000 : 0); add when either of them are added.
            SGAPlayer sgaply = player.GetModPlayer<SGAPlayer>();

            if (sgaply.electricpermboost < max)
                sgaply.electricpermboost += 200;
            sgaply.ActionCooldownStack_AddCooldownStack(60 * 40, 1);
            sgaply.AddElectricCharge((int)((float)sgaply.electricChargeMax * 0.2f));
            return true;
            
        }

        public override void AddRecipes()
        {
            CreateRecipe(5)
                //.AddIngredient<BottledMud>(5)
                .AddIngredient<VialOfAcid>(15)
                //.AddIngredient<Biomass>(5)
                .AddIngredient<MoistSand>(6)
                .AddIngredient(ItemID.Bunny)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
