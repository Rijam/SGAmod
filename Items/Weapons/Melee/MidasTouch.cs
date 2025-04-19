using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;

namespace SGAmod.Items.Weapons.Melee
{
    public class MidasTouch : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 70;
            Item.DamageType = DamageClass.Melee;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = 50000;
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            for (int dustScale = 0; dustScale < 3; dustScale++)
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.SandstormInABottle);
                Main.dust[dust].scale = 0.5f + (((float)dustScale) / 3.5f);
                Vector2 randomCircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomCircle.Normalize();
                Main.dust[dust].velocity = (randomCircle / 2f) + player.itemRotation.ToRotationVector2();
                Main.dust[dust].noGravity = true;
            }
            Lighting.AddLight(player.position, 0.1f, 0.1f, 0.9f);
        }
        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (target.HasBuff(BuffID.Midas) && !player.HasBuff(BuffID.WeaponImbueGold))
            {
                modifiers.SetCrit();
                modifiers.FinalDamage *= 2;
            }
        }
    }
}
