using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.Buffs.Debuffs;

namespace SGAmod.NPCs.Bosses.SpiderQueen
{
	public class SpiderVenom : ModProjectile
	{
		private Vector2[] oldPos = new Vector2[6];
		//private float[] oldRot = new float[6];
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Spider Venom");
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.ignoreWater = true;        //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 1200;
			Projectile.penetrate = 1;
			Projectile.extraUpdates = 5;
			AIType = ProjectileID.Bullet;
		}

		public override bool PreKill(int timeLeft)
		{
			Projectile.type = ProjectileID.CursedFlameFriendly;

			for (int i = 0; i < 20; i++)
			{
				Vector2 randomcircle = new(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				randomcircle *= Main.rand.NextFloat(0f, 2f);
				int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Mod.Find<ModDust>("AcidDust").Type);
				Main.dust[dust].scale = 1f;
				Main.dust[dust].noGravity = false;
				Main.dust[dust].velocity = Projectile.velocity * (float)(Main.rand.Next(60, 100) * 0.01f);
				Main.dust[dust].velocity += new Vector2(randomcircle.X, randomcircle.Y);
			}
			SoundEngine.PlaySound(SoundID.Item111.WithVolumeScale(0.33f).WithPitchOffset(0.25f), Projectile.Center);

			if (Projectile.hostile)
			{
				for (int pro = 0; pro < Main.maxPlayers; pro += 1)
				{
					Player ply = Main.player[pro];
					if (ply.active && (ply.Center - Projectile.Center).Length() < 48)
					{
						ply.AddBuff(ModContent.BuffType<AcidBurn>(), 45);
					}
				}
			}

			if (Projectile.friendly)
			{
				for (int pro = 0; pro < Main.maxNPCs; pro += 1)
				{
					NPC ply = Main.npc[pro];
					if (ply.active && !ply.friendly && (ply.Center - Projectile.Center).Length() < 72)
					{
						ply.AddBuff(ModContent.BuffType<AcidBurn>(), 60 * 2);
					}
				}
			}

			return true;
		}

		public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.Bullet; 

		private readonly Texture2D tex = ModContent.Request<Texture2D>("SGAmod/NPCs/Bosses/SpiderQueen/SpiderVenom").Value;
		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height) / 2f;

			//oldPos.Length - 1
			for (int k = oldPos.Length - 1; k >= 0; k -= 1)
			{
				Vector2 drawPos = ((oldPos[k] - Main.screenPosition)) + new Vector2(0f, 0f);
				Color color = Color.Lerp(Color.White, lightColor, (float)k / (oldPos.Length + 1));
				float alphaz = (1f - (float)(k + 1) / (float)(oldPos.Length + 2)) * 0.25f;
				Main.spriteBatch.Draw(tex, drawPos, null, color * alphaz, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}

		public override void AI()
		{

			if (Main.rand.Next(0, 3) == 1)
			{
				int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Mod.Find<ModDust>("AcidDust").Type);
				Main.dust[dust].scale = 0.75f;
				Main.dust[dust].noGravity = false;
				Main.dust[dust].velocity = Projectile.velocity * (float)(Main.rand.Next(60, 100) * 0.01f);
			}

			Projectile.position -= Projectile.velocity * 0.8f;

			for (int k = oldPos.Length - 1; k > 0; k--)
			{
				oldPos[k] = oldPos[k - 1];
			}
			oldPos[0] = Projectile.Center;

			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.Next(0, 3) < 2)
				target.AddBuff(ModContent.BuffType<AcidBurn>(), 180 * (Main.rand.Next(0, 3) == 1 ? 2 : 1));
			Projectile.Kill();
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			if (Main.rand.Next(0, 3) < 2)
				target.AddBuff(ModContent.BuffType<AcidBurn>(), 60 * (Main.rand.Next(0, 3) == 1 ? 2 : 1));
			Projectile.Kill();
		}
	}
}