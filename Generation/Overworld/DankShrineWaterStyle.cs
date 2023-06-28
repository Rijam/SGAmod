using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace SGAmod.Generation.Overworld
{
	public class DankShrineWaterStyle : ModWaterStyle
	{
		public override int ChooseWaterfallStyle() {
			return ModContent.Find<ModWaterfallStyle>("SGAmod/DankShrineWaterfallStyle").Slot;
		}

		public override int GetSplashDust() {
			return ModContent.DustType<Dusts.DankShrineWaterDust>();
		}

		public override int GetDropletGore() {
			return ModContent.Find<ModGore>("SGAmod/DankShrineWaterDrip").Type;
		}

		public override void LightColorMultiplier(ref float r, ref float g, ref float b) {
			r = 0.75f;
			g = 1f;
			b = 0.75f;
		}

		public override Color BiomeHairColor() {
			return Color.DarkOliveGreen;
		}
	}

	public class DankShrineWaterfallStyle : ModWaterfallStyle
	{

	}
}