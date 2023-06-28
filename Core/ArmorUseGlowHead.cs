using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ReLogic.Content;

namespace SGAmod
{
	/// <summary>
	/// <br>Adapted from Clicker Class DrawLayers/HeadLayer.cs</br>
	/// <br>Usage: In the item's SetStaticDefaults(), Check for !Main.dedServ first, then add:</br>
	/// <br><code>ArmorUseGlowHead.RegisterData(Item.headSlot, Texture + "_Head_Glowmask", Color.White);</code></br>
	/// <br>The key value is the slot. Item.headSlot</br>
	/// <br>The string is the texture to draw</br>
	/// <br>The color is the color to draw</br>
	/// </summary>
	public class ArmorUseGlowHead : PlayerDrawLayer
	{
		//slot, string[texture path, r, g, b, special effect]
		private static Dictionary<int, Tuple<string, Color>> GlowListHead { get; set; }

		/// <summary>
		/// Register this head piece to have a glow mask.
		/// </summary>
		/// <param name="headSlot">The key value is the slot. Item.headSlot</param>
		/// <param name="texture">The string is the texture to draw</param>
		/// <param name="color">The color is the color to draw</param>
		public static void RegisterData(int headSlot, string texture, Color color)
		{
			if (!GlowListHead.ContainsKey(headSlot))
			{
				GlowListHead.Add(headSlot, new Tuple<string, Color>(texture, color));
			}
		}

		// Returning true in this property makes this layer appear on the minimap player head icon.
		public override bool IsHeadLayer => false;

		public override void Load()
		{
			GlowListHead = new Dictionary<int, Tuple<string, Color>>();
		}

		public override void Unload()
		{
			GlowListHead.Clear();
			GlowListHead = null;
		}

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			if (drawPlayer.dead || drawPlayer.invis || drawPlayer.head == -1)
			{
				return false;
			}

			return true;
		}

		public override Position GetDefaultPosition()
		{
			return new AfterParent(PlayerDrawLayers.Head);
		}

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;

			if (!GlowListHead.TryGetValue(drawPlayer.head, out Tuple<string, Color> values))
			{
				return;
			}
			Asset<Texture2D> glowmask = ModContent.Request<Texture2D>(values.Item1);

			Vector2 drawPos = drawInfo.Position - Main.screenPosition + new Vector2(drawPlayer.width / 2 - drawPlayer.bodyFrame.Width / 2, drawPlayer.height - drawPlayer.bodyFrame.Height + 4f) + drawPlayer.headPosition;
			Vector2 headVect = drawInfo.headVect;

			Color color = drawPlayer.GetImmuneAlphaPure(values.Item2, drawInfo.shadow);

			DrawData drawData = new(
				glowmask.Value, // The texture to render.
				drawPos.Floor() + headVect, // Position to render at.
				new Rectangle(drawPlayer.bodyFrame.X, drawPlayer.bodyFrame.Y, drawPlayer.bodyFrame.Width, drawPlayer.bodyFrame.Height), // Source rectangle.
				color * drawPlayer.stealth, // Color.
				drawPlayer.headRotation, // Rotation.
				headVect, // Origin. Uses the texture's center.
				1f, // Scale.
				drawInfo.playerEffect, // SpriteEffects.
				0) // 'Layer'. This is always 0 in Terraria.
				{
					shader = drawInfo.cHead //Shader applied aka dyes
				};

			// Queues a drawing of a sprite. Do not use SpriteBatch in drawlayers!
			drawInfo.DrawDataCache.Add(drawData);
		}
	}
}