using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using SGAmod.Items.Materials.BossDrops;
using SGAmod.Tiles.CraftingStations;

namespace SGAmod.Items.Materials.Crafted
{
    public class ManaBattery : ModItem
    {
        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.width = 16;
            Item.height = 26;
            Item.value = 15000;
            Item.rare = ItemRarityID.Orange;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<BronzeWraithShard>(3)
                .AddIngredient(ItemID.ManaCrystal)
                //.AddIngredient<UnmanedBar>(3)
                .AddTile<ReverseEngineeringStation>()
                .Register();
        }
    }
    public class BottledMud : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 14;
            Item.maxStack = 9999;
            Item.value = 50;
            Item.rare = ItemRarityID.Blue;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Bottle)
                .AddRecipeGroup("SGAmod:Mud", 3)
                .AddCondition(Condition.NearWater)
                .Register();
        }
    }
}
