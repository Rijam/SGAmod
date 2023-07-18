using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using ReLogic.Content;

namespace SGAmod
{
	/// <summary>
	/// <br>Adapted from Clicker Class DrawLayers/HeadLayer.cs</br>
	/// <br>Usage: In the item's SetStaticDefaults(), Check for !Main.dedServ first, then add:</br>
	/// <br><code>ArmorUseGlowLegs.RegisterData(Item.legSlot, Texture + "_Legs_Glow", Color.White);</code></br>
	/// <br>The key value is the slot. Item.legSlot</br>
	/// <br>The string is the texture to draw</br>
	/// <br>The color is the color to draw</br>
	/// </summary>
	public class ArmorUseGlowLegs : PlayerDrawLayer
	{
		private static Dictionary<int, Tuple<string, Color>> GlowListLegs { get; set; }

		/// <summary>
		/// Register this leg piece to have a glow mask. No texture needs to be passed.
		/// </summary>
		/// <param name="legSlot">The key value is the slot. Item.legSlot</param>
		/// <param name="texture">The string is the texture to draw</param>
		/// <param name="color">The color is the color to draw</param>
		public static void RegisterData(int legSlot, string texture, Color color)
		{
			if (!GlowListLegs.ContainsKey(legSlot))
			{
				GlowListLegs.Add(legSlot, new Tuple<string, Color>(texture, color));
			}
		}

		public override void Load()
		{
			GlowListLegs = new Dictionary<int, Tuple<string, Color>>();
		}

		public override void Unload()
		{
			GlowListLegs.Clear();
			GlowListLegs = null;
		}

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			if (drawPlayer.dead || drawPlayer.invis || drawPlayer.legs == -1)
			{
				return false;
			}

			return true;
		}

		public override Position GetDefaultPosition()
		{
			return new AfterParent(PlayerDrawLayers.Leggings);
		}

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;

			if (!GlowListLegs.TryGetValue(drawPlayer.legs, out Tuple<string, Color> values))
			{
				return;
			}
			Asset<Texture2D> glowmask = ModContent.Request<Texture2D>(values.Item1);

			Vector2 drawPos = drawInfo.Position - Main.screenPosition + new Vector2(drawPlayer.width / 2 - drawPlayer.legFrame.Width / 2, drawPlayer.height - drawPlayer.legFrame.Height + 4f) + drawPlayer.legPosition;
			Vector2 legsOffset = drawInfo.legsOffset;


			Color color = drawPlayer.GetImmuneAlphaPure(values.Item2, drawInfo.shadow);

			DrawData drawData = new(
				glowmask.Value, // The texture to render.
				drawPos.Floor() + legsOffset, // Position to render at.
				drawPlayer.legFrame, // Source rectangle.
				color * drawPlayer.stealth, // Color.
				drawPlayer.legRotation, // Rotation.
				legsOffset, // Origin. Uses the texture's center.
				1f, // Scale.
				drawInfo.playerEffect, // SpriteEffects.
				0) // 'Layer'. This is always 0 in Terraria.
			{
				shader = drawInfo.cLegs //Shader applied aka dyes
			};

			// Queues a drawing of a sprite. Do not use SpriteBatch in drawlayers!
			drawInfo.DrawDataCache.Add(drawData);
		}
	}
}