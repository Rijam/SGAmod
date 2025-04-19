using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SGAmod.Items.Materials.BossDrops;

namespace SGAmod.Items.Weapons.Ammo
{
    public class AcidBullet: ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 9;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 8;
            Item.height = 8;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 1.5f;
            Item.value = Item.sellPrice(0, 0, 0, 25);
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.AcidBullet>();
            Item.shootSpeed = 2.5f;
            Item.ammo = AmmoID.Bullet;
        }
        public override void AddRecipes()
        {
            CreateRecipe(50)
                .AddIngredient<VialOfAcid>()
                .AddIngredient(ItemID.MusketBall, 50)
                .AddTile(TileID.Anvils)
                .Register();

            
        }
    }
}
