using Microsoft.Xna.Framework;
using SGAmod.Buffs.Debuffs;
<<<<<<< Updated upstream
=======
using SGAmod.Dusts;
using SGAmod.Items.Weapons.Almighty;
>>>>>>> Stashed changes
using System;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod
{
	public class SGAnpcs : GlobalNPC
	{
		public override bool InstancePerEntity => true;

		public bool acidBurn = false;
		public int reducedDefense = 0; // #TODO
<<<<<<< Updated upstream
=======
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
		public bool NoHit = true;
		
>>>>>>> Stashed changes

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
<<<<<<< Updated upstream
=======

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

			if (npc.type == NPCID.CultistBoss)
			{
				if (NPC.CountNPCS(NPCID.CultistBossClone) >= 6 && npc.GetGlobalNPC<SGAnpcs>().NoHit)
				{
					npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<NuclearOption>()));
				}
			}
		}
		

>>>>>>> Stashed changes
	}
}