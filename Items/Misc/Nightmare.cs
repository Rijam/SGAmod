using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SGAmod.Items.Misc
{
	/*
	public class Nightmare : ModItem
	{
		private float effect = 0;
		public static ModItem instance;
		public Func<float, int, int, float> colorgen = (dist, x, y) => ((-Main.GlobalTimeWrappedHourly + ((float)(dist) / 10f)) / 3f);
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hellion's Gift");
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (Main.LocalPlayer.GetModPlayer<SGAPlayer>().nightmareplayer)
			{
				if (Main.expertMode)
				{
					tooltips.Add(new TooltipLine(Mod, "Nmxx", "'I have provided you with one of my chromatic mirrors, and it will keep returning to your hand as you keep returning to " + Main.worldName + "'"));
					tooltips.Add(new TooltipLine(Mod, "Nmxx", "You just might go far " + SGAmod.HellionUserName + ", just might..."));
					tooltips.Add(new TooltipLine(Mod, "Nmxx", Idglib.ColorText(Color.OrangeRed, "By being granted this item your character is in Nightmare Mode, which does the following:")));

					tooltips.Add(new TooltipLine(Mod, "Nm1", Idglib.ColorText(Color.Red, "Enemies have 20% more HP")));
					tooltips.Add(new TooltipLine(Mod, "Nm1", Idglib.ColorText(Color.Red, "Your health is tripled, however you take triple damage")));
					tooltips.Add(new TooltipLine(Mod, "Nm1", Idglib.ColorText(Color.Red, "Life regen is completely disabled during bosses")));
					tooltips.Add(new TooltipLine(Mod, "Nm1", Idglib.ColorText(Color.Red, "Nurse's service is outside your paycheck during bosses")));
					tooltips.Add(new TooltipLine(Mod, "Nm1", Idglib.ColorText(Color.Red, "Some SGAmod bosses gain new abilities")));
					tooltips.Add(new TooltipLine(Mod, "Nm1", Idglib.ColorText(Color.Red, "Many optional SGAmod config settings are forced on")));

					tooltips.Add(new TooltipLine(Mod, "Nm2", Idglib.ColorText(Color.Lime, "Your Expertise gain is increased by 25%")));
					tooltips.Add(new TooltipLine(Mod, "Nm2", Idglib.ColorText(Color.Lime, "Enemy money dropped is increased by 50%")));
					tooltips.Add(new TooltipLine(Mod, "Nm2", Idglib.ColorText(Color.Lime, "There is a 20% chance for enemies to drop double loot")));
					tooltips.Add(new TooltipLine(Mod, "Nm2", Idglib.ColorText(Color.DimGray, "Does not properly support online play, yet")));
					//tooltips.Add(new TooltipLine(mod, "Nm1", "Using this item will enable Nightmare Hardcore, which ups the challenge even further for more Expertise"));


					foreach (TooltipLine line in tooltips)
					{
						if (line.Mod == "Terraria" && line.Name == "ItemName")
						{
							line.OverrideColor = Main.hslToRgb((Main.GlobalTimeWrappedHourly / 3f) % 1f, 0.50f, 0.3f);
						}
					}
				}
				else
				{
					tooltips.Add(new TooltipLine(Mod, "Nmxx", "You're not on an expert mode world Nub! Nightmare Mode NOT enabled"));

				}
			}

		}

		public override void SetDefaults()
		{
			Item.rare = 12;
			Item.maxStack = 1;
			Item.consumable = false;
			Item.width = 24;
			Item.height = 24;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = 4;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.value = 0;
			Item.rare = 12;
			Item.UseSound = SoundID.Item8;
		}

		public override string Texture
		{
			get { return ("Terraria/Extra_19"); }
		}

		public static void drawit(Vector2 where, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI, Matrix zoomitz, Func<float, int, int, float> color2)
		{

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, zoomitz);

			int width = 32; int height = 32;

			Texture2D beam = new Texture2D(Main.graphics.GraphicsDevice, width, height);
			var dataColors = new Color[width * height];


			///


			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					float dist = (new Vector2(x, y) - new Vector2(width / 2, height / 2)).Length();
					if (dist < width / 3)
					{
						//float alg = ((-Main.GlobalTime + ((float)(dist) / 10f)) / 3f);
						float alg = color2(dist, x, y);
						dataColors[x + y * width] = Main.hslToRgb(alg % 1f, 0.75f, 0.5f);
					}
				}
			}


			///


			beam.SetData(0, null, dataColors, 0, width * height);
			spriteBatch.Draw(beam, where + new Vector2(0, 0), null, Color.White, 0, new Vector2(beam.Width / 2, beam.Height / 2), scale * 2f * Main.essScale, SpriteEffects.None, 0f);


			//effect += 0.1f;
			Texture2D inner = SGAmod.ExtraTextures[19];

			for (int i = 0; i < 360; i += 360 / 12)
			{
				Double Azngle = MathHelper.ToRadians(i) + Main.GlobalTimeWrappedHourly;
				Vector2 here = new Vector2((float)Math.Cos(Azngle), (float)Math.Sin(Azngle));

				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, zoomitz);
				spriteBatch.Draw(inner, (where + ((here * 18f) * Main.essScale)), null, Color.White, 0, new Vector2(inner.Width / 2, inner.Height / 2), scale * 0.25f, SpriteEffects.None, 0f);
				Main.spriteBatch.End();
				if (zoomitz == Main.UIScaleMatrix)
					Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
				else
					Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

			}



		}

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			float gg = 0f;
			drawit(position + new Vector2(11, 11), spriteBatch, drawColor, drawColor, ref gg, ref scale, 1, Main.UIScaleMatrix, colorgen);
			return false;
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{


			drawit(Item.Center - Main.screenPosition, spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI, Main.GameViewMatrix.ZoomMatrix, colorgen);
			return false;
		}
	}
	*/
}