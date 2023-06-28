using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SGAmod
{
	/// <summary>
	/// <br>Adapted from Clicker Class Core/BodyGlowmaskPlayer.cs</br>
	/// <br>Usage: In the item's SetStaticDefaults(), Check for !Main.dedServ first, then add:</br>
	/// <br><code>ArmorGlowmaskBody.RegisterData(Item.bodySlot, Color.White);</code></br>
	/// <br>The key value is the slot. Item.bodySlot</br>
	/// <br>The second value can be any color</br>
	/// </summary>
	public class ArmorUseGlowBody : ModPlayer
	{
		//slot, color
		private static Dictionary<int, Color> GlowListBody { get; set; }

		/// <summary>
		/// Register this body piece to have a glow mask.
		/// </summary>
		/// <param name="bodySlot">The key value is the slot. Item.bodySlot</param>
		/// <param name="color">The color is the color to draw</param>
		public static void RegisterData(int bodySlot, Color color)
		{
			if (!GlowListBody.ContainsKey(bodySlot))
			{
				GlowListBody.Add(bodySlot, color);
			}
		}
		public override void Load()
		{
			GlowListBody = new Dictionary<int, Color>();
		}

		public override void Unload()
		{
			GlowListBody.Clear();
			GlowListBody = null;
		}
		public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
		{
			if (!GlowListBody.TryGetValue(drawInfo.drawPlayer.body, out Color color))
			{
				return;
			}
			drawInfo.bodyGlowColor = color * drawInfo.drawPlayer.stealth;
			drawInfo.armGlowColor = color * drawInfo.drawPlayer.stealth;
		}
	}
}