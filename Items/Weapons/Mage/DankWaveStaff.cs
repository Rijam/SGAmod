using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Audio;
using SGAmod.Items.Placeable.Furniture.DankWood;
using SGAmod.Items.Materials.Environment;
using SGAmod.Items.Materials.BossDrops;
using SGAmod.Projectiles.Magic;

namespace SGAmod.Items.Weapons.Mage
{
    public class DankWaveStaff : ModItem, IFormerHavocItem
    {

        public override void SetDefaults()
        {
            Item.damage = 12;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 8;
            Item.width = 50;
            Item.width = 52;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SwampWave>();
            Item.shootSpeed = 10f;
            

        }
        public override void SetStaticDefaults()
        {
            Item.staff[Type] = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<DankWoodItem>(15).AddIngredient<DankCore>().AddIngredient<VialOfAcid>(8).AddTile(TileID.WorkBenches).Register();

        }
    }
}
