using Microsoft.Xna.Framework;
using SGAmod.Buffs.Debuffs;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod
{
	public class SGAnpcs : GlobalNPC
	{
		public override bool InstancePerEntity => true;

		public bool acidBurn = false;
		public int reducedDefense = 0; // #TODO

		public override void ResetEffects(NPC npc)
		{
			acidBurn = false;
			reducedDefense = 0;
		}

		public override void UpdateLifeRegen(NPC npc, ref int damage)
		{
			if (acidBurn)
			{
				int tier = 2;
				//if (npc.HasBuff(ModContent.BuffType<RustBurn>()) && RustBurn.IsInorganic(npc))
				//	tier = 3;
				npc.lifeRegen -= 20 + Math.Min(tier * 150, npc.defense * tier);
				if (damage < 5)
					damage = 5;
			}
		}

		public override void DrawEffects(NPC npc, ref Color drawColor)
		{
			if (acidBurn)
			{
				if (Main.rand.Next(5) < 4)
				{
					int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, ModContent.DustType<Dusts.AcidDust>(), npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default, 1f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1.8f;
					Main.dust[dust].velocity.Y -= 0.5f;
					if (Main.rand.NextBool(4))
					{
						Main.dust[dust].noGravity = false;
						Main.dust[dust].scale *= 0.5f;
					}
				}
				drawColor.R = (byte)(drawColor.R * 0.2f);
				drawColor.G = (byte)(drawColor.G * 0.8f);
				drawColor.B = (byte)(drawColor.B * 0.2f);
			}
		}
	}
}