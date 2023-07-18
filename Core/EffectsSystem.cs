using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace SGAmod
{
	public class EffectsSystem : ModSystem
	{
		public static Effect TrailEffect;
		public static Effect HallowedEffect;
		public static Effect TrippyRainbowEffect;
		public static Effect FadeInEffect;
		public static Effect RadialEffect;
		public static Effect CircleEffect;
		public static Effect BloomEffect;
		public static Effect SphereMapEffect;
		public static Effect VoronoiEffect;
		public static Effect CataEffect;
		public static Effect TextureBlendEffect;
		public static Effect RotateTextureEffect;

		public static List<ScreenExplosion> screenExplosions = new List<ScreenExplosion>();

		protected static float _screenShake = 0;


		public static float ScreenShake
		{
			get
			{
				if (Main.gameMenu)
					return 0;

				return Math.Max(_screenShake * (SGAConfigClient.Instance.ScreenShakeMul / 100f), 0);
			}
			set
			{
				_screenShake = value;
			}
		}

		public static void AddScreenShake(float ammount, float distance = -1, Vector2 origin = default)
		{
			if (Main.dedServ)
				return;

			if (origin != default)
			{
				ammount *= MathHelper.Clamp((1f - (Vector2.Distance(origin, Main.LocalPlayer.Center) / distance)), 0f, 1f);
			}

			_screenShake += ammount;
		}

		public class ScreenExplosion
		{
			public Vector2 where;
			public int time = 0;
			public int timeLeft = 0;
			public int timeLeftMax = 0;
			public float strength = 16f;
			public float decayTime = 16f;
			public float warmupTime = 16f;
			public float distance = 1600f;
			public float alpha = 0.10f;
			public float perscreenscale = 1.15f;
			public Func<float, float> strengthBasedOnPercent;

			public ScreenExplosion(Vector2 there, int time, float str, float decayTime = 16)
			{
				where = there;
				this.time = 0;
				this.timeLeft = time;
				this.timeLeftMax = time;
				this.strength = str;
				this.decayTime = decayTime;
			}
			public void Update()
			{
				timeLeft -= 1;
				time += 1;
			}
		}

		public static ScreenExplosion AddScreenExplosion(Vector2 here, int time, float str, float distance = 3200)
		{
			if (Main.dedServ)
				return null;

			if (!SGAConfigClient.Instance.ScreenFlashExplosions)
				return null;

			ScreenExplosion explode = new ScreenExplosion(here, time, str);

			//Vector2 centerpos = Main.LocalPlayer.Center;

			//explode.strength = explode.strength *= MathHelper.Clamp((here- centerpos).Length()/ distance,0f,1f);
			screenExplosions.Add(explode);

			//Overlays.Scene.Activate("SGAmod:ScreenExplosions"); TODO
			return explode;
		}
	}
}