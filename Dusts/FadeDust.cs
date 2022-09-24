using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SGAmod.Dusts
{
	public class FadeDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.noGravity = true;
			dust.frame = new Rectangle(0, 0, ModContent.Request<Texture2D>("SGAmod/Dusts/FadeDust").Value.Width, ModContent.Request<Texture2D>("SGAmod/Dusts/FadeDust").Value.Height);
		}

		public override bool Update(Dust dust)
		{
			dust.position += dust.velocity;
			dust.velocity *= 0.95f;
			dust.scale -= 0.01f;
			dust.color *= dust.alpha / 100f;

			dust.alpha -= 1;
			if (dust.alpha < 1)
			{
				dust.scale -= 0.05f;
			}
			if (dust.scale <= 0f)
			{
				dust.active = false;
			}
			return false;
		}

		public override Color? GetAlpha(Dust dust, Color lightColor)
		{
			return new Color(lightColor.R, lightColor.G, lightColor.B, dust.alpha);
		}
	}
}