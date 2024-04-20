using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace SGAmod.Items.Weapons.Mage
{
   /* public class PhilanthropistShower : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 40;
            Item.DamageType = DamageClass.Magic;
            Item.width = 34;
            Item.height = 24;
            Item.mana = 8;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 6;
            Item.value = 100000;
            Item.rare = ItemRarityID.Lime;
            Item.noMelee = true;
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 8f;
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = true;
            Item.useTurn = false;
            Item.staff[Item.type] = true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.CountItem(ItemID.CopperCoin) + player.CountItem(ItemID.SilverCoin) + player.CountItem(ItemID.GoldCoin) + player.CountItem(ItemID.PlatinumCoin) > 0;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int taketype = 3;
            int[] types = { ItemID.CopperCoin, ItemID.SilverCoin, ItemID.GoldCoin, ItemID.PlatinumCoin };
            int silver = player.CountItem(ItemID.SilverCoin);
            int gold = player.CountItem(ItemID.GoldCoin);
            int plat = player.CountItem(ItemID.PlatinumCoin);
            taketype = plat > 0 ? 3 : (gold > 0 ? 2 : (silver > 0 ? 1 : 0));
            int coincount = player.CountItem(types[taketype]);
            if(coincount > 0)
            {
                player.ConsumeItem(types[taketype]);
                float[,] typesproj = { { ModContent.ProjectileType<GlowingCopperCoinPlayer>(), 1f }, { ModContent.ProjectileType<GlowingSilverCoinPlayer>(), 1.25f }, { ModContent.ProjectileType<GlowingGoldCoinPlayer>(), 1.75f }, { ModContent.ProjectileType<GlowingPlatinumCoinPlayer>(), 2.5f } };
                int numberProjectiles = 8 + Main.rand.Next(5);
                for (int index = 0; index < numberProjectiles; index++)
                {
                    
                }
            }
        }
    }
    public class GlowingCopperCoinPlayer : ModProjectile, IDrawAdditive
    {
    }

    public class GlowingSilverCoinPlayer : ModProjectile, IDrawAdditive
    {

    }

    public class GlowingGoldCoinPlayer : ModProjectile, IDrawAdditive
    {

    }

    public class GlowingPlatinumCoinPlayer : ModProjectile, IDrawAdditive
    {

    }*/
}
