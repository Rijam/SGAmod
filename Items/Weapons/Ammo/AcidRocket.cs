using SGAmod.Items.Materials.BossDrops;
using SGAmod.Projectiles.Ranged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Weapons.Ammo
{
    public class AcidRocket : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 64;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 8;
            Item.height = 8;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 3.5f;
            Item.value = Item.sellPrice(0,0,2,0);
            Item.rare = ItemRarityID.Lime;
            Item.shoot = ModContent.ProjectileType<AcidRocketProj>();
            Item.shootSpeed = 3f;
            Item.ammo = AmmoID.Rocket;
        }
        public override void PickAmmo(Item weapon, Player player, ref int type, ref float speed, ref StatModifier damage, ref float knockback)
        {
            if (weapon.type != ItemID.GrenadeLauncher && weapon.type != ItemID.FireworksLauncher && weapon.type != ItemID.ElectrosphereLauncher)
            {
                if (type != ProjectileID.GrenadeI || type != ProjectileID.GrenadeII || type != ProjectileID.GrenadeIII || type != ProjectileID.GrenadeIV)
                {
                    type = ModContent.ProjectileType<AcidRocketProj>();
                }
            }
            if (weapon.shoot == ProjectileID.GrenadeI)
            {
                type = ProjectileID.GrenadeI;
            }
            if(weapon.shoot == ProjectileID.ElectrosphereMissile)
            {
                type = ProjectileID.ElectrosphereMissile;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe(50)
                .AddIngredient<VialOfAcid>(3)
                .AddIngredient(ItemID.RocketIII, 50)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
