using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SGAmod.Projectiles.Melee;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Weapons.Melee.Spears
{
    public class TidalWave : ModItem, IFormerHavocItem
    {
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.damage = 15;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.useTurn = false;
            Item.noUseGraphic = true;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 7f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0,0,8,0);
            Item.rare = ItemRarityID.Blue;
            Item.shoot = ModContent.ProjectileType<TidalWaveProj>();
            Item.shootSpeed = 9f;
        }
    }
}
