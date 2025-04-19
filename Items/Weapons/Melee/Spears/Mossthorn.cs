
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;

using SGAmod.Projectiles.Melee;

namespace SGAmod.Items.Weapons.Melee.Spears
{
    public class Mossthorn : ModItem, IFormerHavocItem
    {
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.damage = 30;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.useTurn = false;
            Item.noUseGraphic = true;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 10;
            Item.knockBack = 4.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0,3,0,0);
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ModContent.ProjectileType<MossthornProj>();
            Item.shootSpeed = 4.5f;
			
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
    }
}
