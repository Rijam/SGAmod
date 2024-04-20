using Idglibrary.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace SGAmod.Projectiles.Melee
{
    public class MossthornProj : ProjectileSpearBase
    {
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

            movein = 3f;
            moveout = 3f;
            thrustspeed = 3.0f;
        }
        /*public new float movementFactor
        {
            get { return Projectile.ai[0]; }
            set { Projectile.ai[0] = value; }
        }*/
        
    }
}
