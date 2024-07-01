using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using SGAmod.Items.Materials.BossDrops;
using SGAmod.Items.Materials.Bars;
using SGAmod.Projectiles.Melee;

namespace SGAmod.Items.Weapons.Melee.Spears
{
    public class Contagion : ModItem, IFormerHavocItem
    {
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.damage = 40;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.useTurn = false;
            Item.noUseGraphic = true;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 20;
            Item.knockBack = 3f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.shoot = ModContent.ProjectileType<ContagionProj>();
            Item.shootSpeed = 11f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Mossthorn>()
                .AddIngredient<TidalWave>()
                .AddIngredient<VialOfAcid>(12)
                .AddIngredient<VirulentBar>(10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
