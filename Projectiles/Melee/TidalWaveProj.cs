using Idglibrary.Bases;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace SGAmod.Projectiles.Melee
{
    public class TidalWaveProj : ProjectileSpearBase
    {
        //note: this projectile is funky.
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
            Projectile.scale = 1.2f;
            Projectile.aiStyle = 19;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 90;
            Projectile.hide = true;
            
            movein = 0.8f;
            moveout = 0.2f;
            thrustspeed = 3f;
        }
        
        public override void MakeProjectile()
        {
            Vector2 center = new Vector2(Projectile.position.X + (Projectile.width / 2), Projectile.position.Y + (Projectile.width / 2));
            Vector2 launchvector = new Vector2((float)Math.Cos(truedirection),(float)Math.Sin(truedirection));
            int launchedProj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), center + launchvector*42, launchvector * 8, ModContent.ProjectileType<TidalWaveProj2>(), 1, 0f);
            Main.projectile[launchedProj].damage = Projectile.damage;
            Main.projectile[launchedProj].owner = Projectile.owner;
        }
        
    }
}
