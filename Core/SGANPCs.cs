using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using SGAmod.Buffs.Debuffs;
using SGAmod.Dusts;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace SGAmod
{
	public class DamageStack
	{
		public int time;
		public int damage;

		public DamageStack(int damage, int time)
		{
			this.damage = damage;
			this.time = time;
		}
		public bool Update()
		{
			time--;
			return time < 1;
		}
	}
	public partial class SGAnpcs : GlobalNPC
	{
		public override bool InstancePerEntity => true;

		//Debuffs
		public bool acidBurn = false;
		public bool gouged = false;
		public int reducedDefense = 0; // #TODO
		public bool MassiveBleeding = false;
		public bool DankSlow = false;

		//TimeSlow
		public float TimeSlow = 0;
		public bool TimeSlowImmune = false;

		//Impale
		public int impaled = 0;
		private int nonStackingImpaled_;
		public int nonStackingImpaled
		{
			get
			{
				return nonStackingImpaled_;
			}
			set
			{
				nonStackingImpaled_ = Math.Max(value, nonStackingImpaled_);
			}
		}
		//Radiation
		internal int IrradiatedAmount_;
		public int IrradiatedAmount
		{
			get
			{
				return IrradiatedAmount_;
			}
			set
			{
				IrradiatedAmount_ = Math.Max(value, IrradiatedAmount_);
			}
		}

		//Other
		public int counter = 0;
		public float damagemul = 1f;
		public List<DamageStack> damageStacks = new List<DamageStack>();
		public int lastHitByItem = 0;
		

		public override void ResetEffects(NPC npc)
		{
			acidBurn = false;
			gouged = false;
			MassiveBleeding = false;
			reducedDefense = 0;
			DankSlow = false;

			damagemul = 1f;
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
			if (MassiveBleeding)
			{
				if(npc.lifeRegen > 0)
					npc.lifeRegen = 0;
				npc.lifeRegen -= 40;
				if (damage < 10)
					damage = 10;
			}
			if (!npc.dontTakeDamage)
			{
				if (damageStacks.Count > 0)
				{
					for (int i = 0; i < damageStacks.Count; i++) 
					{
						impaled += damageStacks[i].damage;
						if (damageStacks[i].Update())
							damageStacks.RemoveAt(i);
					}
				}

				impaled += nonStackingImpaled;
				if (impaled > 0)
				{
					if (npc.lifeRegen > 0) npc.lifeRegen = 0;

					int damageDot = (int)((npc.realLife > 0 ? (impaled * 0.1f) : impaled));

					npc.lifeRegen -= damageDot;
					damage = Math.Max(damageDot / 4, damage);
				}
			}

		}

		public override void DrawEffects(NPC npc, ref Color drawColor)
		{
			if (IrradiatedAmount > 0)
			{
				for (int i = 0; i < Math.Min(12, IrradiatedAmount / 32); i++)
				{
					if (Main.rand.Next(100) < 1)
					{
						int dust126 = Dust.NewDust(npc.Center + Main.rand.NextVector2Circular(npc.width, npc.height), 0, 0, DustID.ScourgeOfTheCorruptor, 0, 0, 140, new Color(30, 30, 30, 20), 1f);
						Main.dust[dust126].noGravity = true;
						Main.dust[dust126].velocity = new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-6f, 1f));
					}
				}
				if (counter % 10 == 0)
				{
					for (int dust654 = 0; dust654 < 1; dust654++)
					{
						Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000));
						randomcircle.Normalize();
						Vector2 ogcircle = randomcircle;
						randomcircle *= (float)(dust654 / 10.00);
						int dust655 = Dust.NewDust(npc.Center + Main.rand.NextVector2Circular(npc.width, npc.height), 0, 0, ModContent.DustType<RadioDust>(), npc.velocity.X + randomcircle.X * 1f, npc.velocity.Y + randomcircle.Y * 1f, 200, Color.Lime, 0.5f);
						Main.dust[dust655].noGravity = true;
					}
				}
			}
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
			if (gouged)
			{
				Vector2 randomCircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000));
				int dust = Dust.NewDust(npc.Center + randomCircle, 0, 0, DustID.Blood, -npc.velocity.X * 0.3f, 4f + (npc.velocity.Y * -0.4f), 30, default, 0.85f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].color = Main.hslToRgb(0f, 0.5f, 0.35f);
			}
			if (MassiveBleeding)
			{
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000));
				randomcircle.Normalize();
				int dust = Dust.NewDust(npc.position + randomcircle * (1.2f * (float)npc.width), npc.width + 4, npc.height + 4, 5, npc.velocity.X * 0.4f, (npc.velocity.Y - 7f) * 0.4f, 30, default, 1.5f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].color = Main.hslToRgb(0f, 0.5f, 0.35f);
			}
		}

        public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
		
            OnHit(npc, player, damageDone, hit.Knockback, hit.Crit, item, null, false);
            
        }
    
        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {

            OnHit(npc, Main.player[projectile.owner], damageDone, hit.Knockback, hit.Crit, null, projectile, true) ;

        }

        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (gouged)
            {
				modifiers.Defense *= 0.5f;
            }
            
        }
        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            
            ModifyDamage(npc, player, ref modifiers.FinalDamage, item, null);

        }
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
			
			ModifyDamage(npc, Main.player[projectile.owner], ref modifiers.FinalDamage, null, projectile);

		}
        public override void PostAI(NPC npc)
        {
			counter++;
			if (TimeSlow > 0 && npc.IsValidEnemy() && !TimeSlowImmune)
			{
				npc.position -= npc.velocity - (npc.velocity / (1 + TimeSlow));
			}
			TimeSlow = 0;
        }
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
			IrradiatedExplosion(npc, IrradiatedAmount);
        }

    }
}