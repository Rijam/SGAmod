using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using Terraria.DataStructures;

namespace SGAmod.Projectiles.Magic
{
	//Not a perfect copy, but very close. The vanilla projectile has a shader applied to it which is not implemented here. (well now it is, lol-IDG)
	public class DD2DrakinShotFriendly : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.DD2DrakinShot;
        public override void SetStaticDefaults()
        {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;    //The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;        //The recording mode
		}
        public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.DD2DrakinShot);
			Projectile.DamageType = DamageClass.Magic;
			Projectile.friendly = true;
			Projectile.hostile = false;
			AIType = ProjectileID.DD2DrakinShot;
		}
		public override void PostDraw(Color lightColor)
		{

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			Texture2D texz = TextureAssets.Projectile[Projectile.type].Value;

				//Redraw the projectile with the color not influenced by light
				Vector2 drawOrigin = new Vector2(texz.Width * 0.5f, texz.Height * 0.5f);

			ArmorShaderData stardustsshader = GameShaders.Armor.GetShaderFromItemId(ItemID.StardustDye);

			DrawData value8 = new DrawData(texz, new Vector2(Main.GlobalTimeWrappedHourly*6f, 64f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 96, 48)), Microsoft.Xna.Framework.Color.White, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			stardustsshader.UseColor(Color.Purple.ToVector3());
			stardustsshader.UseOpacity(0.5f);
			stardustsshader.Apply(null, new DrawData?(value8));

			Main.EntitySpriteDraw(texz, Projectile.Center, null, Projectile.GetAlpha(lightColor), Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);

			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				float alpha = ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * alpha;

				stardustsshader.UseColor(Color.Purple.ToVector3()* alpha);
				//stardustsshader.UseOpacity(0.5f);

				Main.EntitySpriteDraw(texz, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

		}

        public override bool PreDraw(ref Color lightColor)
        {
			return false;
        }

        public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 30; i++)
			{
				int selectRand = Utils.SelectRandom(Main.rand, 27, 27, 62); //27 = Shadowflame, 62 = Purple Torch
				Dust killDust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, selectRand)];
				killDust.noGravity = true;
				killDust.scale = 1.25f + Main.rand.NextFloat();
				killDust.fadeIn = 0.25f;
				Dust killDust2 = killDust;
				Dust killDust3 = killDust2;
				killDust3.velocity *= 2f;
				killDust.noLight = true;
			}
		}
		public override Color? GetAlpha(Color newColor)
		{
			return new Color(255, 255, 255, 255) * Projectile.Opacity;
		}
	}
}