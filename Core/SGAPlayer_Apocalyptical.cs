using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Idglibrary;
using Microsoft.Xna.Framework.Audio;
using System.Linq;
using System;
using SGAmod.Buffs.Debuffs;
using Terraria.WorldBuilding;

namespace SGAmod
{
	public partial class SGAPlayer : ModPlayer
	{
		// Apocalyptical related
		public double[] apocalypticalChance = { 0, 0, 0, 0 };
		public float apocalypticalStrength = 1f;
		public float lifestealentropy = 0f;

		public void Apocalyptical_ResetEffects()
		{
			for (int a = 0; a < apocalypticalChance.Length; a++)
			{
				apocalypticalChance[a] = 0;
			}
			apocalypticalStrength = 1f;
		}
		
		public void Apocalyptical_Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
		{
			if (SGAConfigClient.Instance.EpicApocalypticals)
			{
				Entity target = Player;
				if (damageSource.TryGetCausingEntity(out Entity entity))
				{
					target = entity;
				}
				RippleBoom.MakeShockwave(target, Player, Player.Center, 8f, 1f, 10f, 60, 1f);
				SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Player.Center);
			}
		}
	}

	public class ApocalypticalNPCs : GlobalNPC
	{
		public override bool InstancePerEntity => true;

		public int lastHitByItem = 0; // TODO move later
		public float damagemul = 1f;

		public override void ResetEffects(NPC npc)
		{
			damagemul = 1f;
		}

		public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
		{
			lastHitByItem = item.type;
			DoModifies(npc, player, null, item, ref hit, damageDone);
		}

		public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
		{
			lastHitByItem = -projectile.type;
			Player player = Main.player[projectile.owner];

			if (projectile.friendly && player != null)
			{
				DoModifies(npc, player, projectile, null, ref hit, damageDone);
			}
		}

		/*public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
		{
			lastHitByItem = item.type;
			DoModifies(npc, player, null, item, ref modifiers);
		}

		public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
		{
			lastHitByItem = -projectile.type;
			Player player = Main.player[projectile.owner];

			if (projectile.friendly && player != null)
			{
				DoModifies(npc, player, projectile, null, ref modifiers);
			}
		}*/

		// TODO move later
		public void DoModifies(NPC npc, Player player, Projectile projectile, Item item, ref NPC.HitInfo hit, int damageDone)
		{
			SGAPlayer moddedplayer = player.GetModPlayer<SGAPlayer>();
			int damage = (int)(hit.SourceDamage * damagemul);

			Projectile held = null;
			if (projectile != null)
			{
				/*float resist = npc.GetGlobalNPC<SGAnpcs>().pierceResist;
				if ((projectile.penetrate < 0 || projectile.penetrate > 3) && resist < 1)
				{
					damage = (int)(damage * resist);
					hit.SourceDamage = damage;
				}*/


				/*if (hit.Crit && moddedplayer.molotovLimit > 0 && projectile.CountsAsClass(DamageClass.Throwing))
				{
					hit.Crit = Main.rand.NextBool(10);
				}*/

				if (player != null && player.heldProj >= 0)
					held = Main.projectile[player.heldProj];

				//if (projectile.trap)
					//damage += (int)(hit.SourceDamage * (player.GetModPlayer<SGAPlayer>().TrapDamageMul - 1f));
			}

			DoApoco(npc, projectile, player, item, ref damage, ref hit);

			if (moddedplayer != null)
			{
				/*if (moddedplayer.acidSet.Item1)
				{
					reducedDefense += (npc.poisoned ? 5 : 0) + (npc.venom ? 5 : 0) + (acidburn ? 5 : 0);
				}*/
			}

			//damage += (int)(Math.Min(npc.defense, reducedDefense) / 2);

			/*if (Gourged)
				damage += npc.defense / 4;
			if (Sodden)
				damage += (int)((float)hit.SourceDamage * 0.33f);*/

			if (moddedplayer != null)
			{
				/*if (moddedplayer.PrimordialSkull)
					if (npc.HasBuff(BuffID.OnFire))
						damage += (int)(hit.SourceDamage * 0.25);*/

				if (npc.HasBuff(BuffID.Midas))
					damage += (int)(hit.SourceDamage * 0.15f);
			}

			if (item != null)
			{
				if (item.pick + item.axe + item.hammer > 0)
				{
					/*if (player.HasBuff(ModContent.BuffType<TooltimePotionBuff>()))
					{
						hit.Knockback += 50f;
					}*/
				}
			}

			if (projectile != null)
			{

				SGAProjectile modeproj = projectile.GetGlobalProjectile<SGAProjectile>();
				Player P = Main.player[projectile.owner];
				bool trapdamage = false;
				if (projectile.trap)
					trapdamage = true;

				/*if (trapdamage)
				{
					float totaldamage = 0f;
					//damage += (int)((npc.defense * moddedplayer.TrapDamageAP) / 2.00);
					totaldamage += moddedplayer.TrapDamageAP;
					if (moddedplayer.JaggedWoodenSpike)
					{
						totaldamage += 0.4f;
						//damage += (int)((npc.defense*0.4)/2.00);
					}
					if (moddedplayer.JuryRiggedSpikeBuckler)
					{
						//damage += (int)(damage * 0.1);
						totaldamage += 0.1f;
						//damage += (int)((npc.defense * 0.1) / 2.00);
					}
					totaldamage = Math.Min(totaldamage, 1f);
					if (moddedplayer.GoldenCog)
					{
						npc.life = Math.Max(npc.life - (int)(damage * 0.10), 1);
						if (Main.netMode != NetmodeID.SinglePlayer)
						{
							NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npc.whoAmI, 0f, 0f, 0f, 0, 0, 0);
						}
					}
					damage += (int)((npc.defense * totaldamage) / 2.00);
				}*/

				/*if (moddedplayer.beefield > 0 && (projectile.type == ProjectileID.Bee || projectile.type == ProjectileID.GiantBee))
				{
					damage += (int)(Math.Min(moddedplayer.beedamagemul, 10f + (moddedplayer.beedamagemul / 50f)) * 1.5f);
				}*/

				/*if (modeproj.myplayer != null)
					P = modeproj.myplayer;

				if (P != null)
				{
					if (moddedplayer.CirnoWings == true && projectile.coldDamage)
					{
						damage += (int)((float)hit.SourceDamage * 0.20f);
					}
				}*/

			}

			/*if (moddedplayer.MisterCreeperset && item != null)
			{
				if (item.shoot < ProjectileID.WoodenArrowFriendly && item.CountsAsClass(DamageClass.Melee))
				{
					if (player.velocity.Y > 1)
					{
						hit.Crit = true;
					}
				}
			}*/

			/*if (moddedplayer.Blazewyrmset)
			{
				if (npc.HasBuff(Mod.Find<ModBuff>("ThermalBlaze").Type) && ((item != null && item.CountsAsClass(DamageClass.Melee)) || (projectile != null && projectile.CountsAsClass(DamageClass.Melee))))
				{
					damage += (int)(hit.SourceDamage * 0.20f);
				}
			}*/

			/*if (moddedplayer.alkalescentHeart)
			{
				damage += (int)(hit.SourceDamage * (0f + (npc.HasBuff(ModContent.BuffType<AcidBurn>()) ? 0.15f : (npc.HasBuff(BuffID.Venom) ? 0.10f : (npc.HasBuff(BuffID.Poisoned) ? 0.05f : 0)))));
			}*/

			SGAPlayer sgaply = player.GetModPlayer<SGAPlayer>();

			/*if (sgaply.snakeEyes.Item1)
			{
				bool falsecrit = hit.Crit;
				if (!hit.Crit && Main.rand.NextBool(100))
				{
					CombatText.NewText(npc.Hitbox, Color.Red, "False Crit", false, false);
					falsecrit = true;
				}
				sgaply.snakeEyes.Item2 = falsecrit ? 0 : Math.Min(sgaply.snakeEyes.Item2 + 1, 100);
				damage += (int)(hit.SourceDamage * (0f + (sgaply.snakeEyes.Item2 / 100f)));
			}*/

			/*if ((Main.netMode < NetmodeID.MultiplayerClient || SGAmod.SkillRun > 1) && SGAmod.SkillRun > 0)
			{
				if (sgaply.skillMananger == null)
					return;

				if (item != null)
					sgaply.skillMananger.OnEnemyDamage(ref damage, ref hit.Crit, ref hit.Knockback, item, null);
				if (projectile != null)
					sgaply.skillMananger.OnEnemyDamage(ref damage, ref hit.Crit, ref hit.Knockback, null, projectile);
			}*/

			/*if (petrified)
			{
				if (player != null && (item?.pick > 0 || (projectile != null && player.heldProj >= 0 && player.heldProj == projectile.whoAmI && player.HeldItem.pick > 0)))
				{
					hit.SourceDamage = (int)(hit.SourceDamage * 3f);
					hit.Crit = true;
					SoundEngine.PlaySound(SoundID.Tink.WithPitchOffset(0.25f), npc.Center);
				}
				else
				{
					hit.SourceDamage = (int)(hit.SourceDamage * 0.25f);
				}
			}*/

			if (hit.Crit)
				hit.SourceDamage = (int)(hit.SourceDamage * sgaply.CritDamage);

			//DoModifiesLateEvent?.Invoke(npc, player, projectile, item, ref sourcedamage, ref damage, ref hit.Knockback, ref hit.Crit);

			//LifeSteal(npc, player, ref damage, ref hit.Knockback, ref hit.Crit);
			//OnCrit(npc, projectile, player, item, ref damage, ref hit.Knockback, ref hit.Crit);

			hit.SourceDamage = damage;

		}

		public void DoApoco(NPC npc, Projectile projectile, Player player, Item item, ref int damage, ref NPC.HitInfo hit, int bitBoldedEffects = 7, bool always = false)
		{
			bool effectSound = (bitBoldedEffects & (1 << 1 - 1)) != 0;
			bool effectText = (bitBoldedEffects & (1 << 2 - 1)) != 0;
			bool effectShockwave = (bitBoldedEffects & (1 << 3 - 1)) != 0;

			SGAPlayer moddedplayer = player.GetModPlayer<SGAPlayer>();
			int chance = -1;
			if (projectile != null)
			{
				if (projectile.CountsAsClass(DamageClass.Melee))
					chance = 0;
				if (projectile.CountsAsClass(DamageClass.Ranged))
					chance = 1;
				if (projectile.CountsAsClass(DamageClass.Magic))
					chance = 2;
				if (projectile.CountsAsClass(DamageClass.Throwing))
					chance = 3;
			}
			if (item != null)
			{
				if (item.CountsAsClass(DamageClass.Melee))
					chance = 0;
				if (item.CountsAsClass(DamageClass.Ranged))
					chance = 1;
				if (item.CountsAsClass(DamageClass.Magic))
					chance = 2;
				if (item.CountsAsClass(DamageClass.Throwing))
					chance = 3;

			}
			if (npc != null && (always || chance > -1))
			{

				double chanceboost = 0;
				if (projectile != null)
				{
					chanceboost += projectile.GetGlobalProjectile<SGAProjectile>().extraApocalypticalChance;
				}

				if (always || (hit.Crit && Main.rand.Next(0, 100) < (moddedplayer.apocalypticalChance[chance] + chanceboost)))
				{
					/*if (moddedplayer.HoE && projectile != null)
					{
						float ammount = damage;
						if (moddedplayer.lifestealentropy > 0)
						{
							projectile.vampireHeal((int)((ammount * moddedplayer.apocalypticalStrength)), npc.Center);
							moddedplayer.lifestealentropy -= ammount;
						}
					}*/

					/*if (moddedplayer.ninjaSash > 2)
					{
						for (int i = 0; i < Main.maxProjectiles; i += 1)
						{
							Projectile proj = Main.projectile[i];
							if (proj.active && proj.owner == player.whoAmI)
							{
								if (proj.Throwing().thrown || proj.thrown)
									proj.SGAProj().Embue(projectile);

							}
						}
					}*/

					/*if (moddedplayer.SybariteGem)
					{
						float mul = moddedplayer.apocalypticalStrength * (((float)damage * 3f) / (float)npc.lifeMax);
						int ammount = (int)((float)npc.value * mul);


						Vector2 pos = new Vector2((int)npc.position.X, (int)npc.position.Y);
						pos += new Vector2(Main.rand.Next(npc.width), Main.rand.Next(npc.height));
						SGAUtils.SpawnCoins(pos, ammount, 10f + Math.Min(3f * mul, 20f));
					}*/

					/*if (moddedplayer.dualityshades)
					{
						int ammo = 0;
						for (int i = 0; i < 4; i += 1)
						{
							if (moddedplayer.ammoinboxes[i] > 0)
							{
								int ammox = moddedplayer.ammoinboxes[i];
								Item itemx = new Item();
								itemx.SetDefaults(ammox);
								if (itemx.ammo == AmmoID.Bullet)
								{
									ammo = ammox;
									break;
								}
							}
						}
						if (ammo > 0)
						{
							Item itemy = new Item();
							itemy.SetDefaults(ammo);
							int shootype = itemy.shoot;

							for (int i = 128; i < 260; i += 128)
							{
								Vector2 anglez = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000));
								anglez.Normalize();

								SoundEngine.PlaySound(SoundID.Item25.WithVolumeScale(0.5f).WithPitchOffset(Main.rand.NextFloat(-0.9f, -0.25f)), new Vector2(((npc.Center.X) + (anglez.X * i)), ((npc.Center.Y) + (anglez.Y * i))));

								int thisoned = Projectile.NewProjectile(npc.Center + (anglez * i), anglez * -16f, shootype, (int)(damage * 1.50f * moddedplayer.apocalypticalStrength), 0f, Main.myPlayer);
								Main.projectile[thisoned].ranged = false // tModPorter Suggestion: Remove. See Item.DamageType;


								for (float gg = 4f; gg > 0.25f; gg -= 0.15f)
								{
									int dustIndex = Dust.NewDust(npc.Center + new Vector2(-8, -8) + (anglez * i), 16, 16, DustID.AncientLight, -anglez.X * gg, -anglez.Y * gg, 150, Color.Purple, 3f);
									Dust dust = Main.dust[dustIndex];
									dust.noGravity = true;
								}

								player.ConsumeItemRespectInfiniteAmmoTypes(ammo);
							}
						}
					}*/

					/*if (moddedplayer.RadSuit)
					{
						//IrradiatedAmmount = Math.Max(IrradiatedAmmount, 25);

						IrradiatedExplosion(npc, (int)(damage * 1f * moddedplayer.apocalypticalStrength));

						SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.DD2_DarkMageHealImpact, npc.Center);
						if (sound != null)
							sound.Pitch += 0.525f;

						int proj;

						if (projectile != null)
							proj = Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<RadioactivePool>(), (int)(damage * 0.5f * moddedplayer.apocalypticalStrength), projectile.knockBack, projectile.owner);
						else
							proj = Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<RadioactivePool>(), (int)(damage * 0.5f * moddedplayer.apocalypticalStrength), hit.Knockback, player.whoAmI);

						Main.projectile[proj].width += 80;
						Main.projectile[proj].height += 80;
						Main.projectile[proj].timeLeft += (int)(30 * moddedplayer.apocalypticalStrength);
						Main.projectile[proj].Center -= new Vector2(40, 40);
						Main.projectile[proj].netUpdate = true;
					}*/

					/*if (moddedplayer.CalamityRune)
					{
						SoundEngine.PlaySound(SoundID.Item45, npc.Center);
						int boom = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, Mod.Find<ModProjectile>("BoulderBlast").Type, (int)((damage * 2) * moddedplayer.apocalypticalStrength), hit.Knockback * 2f, player.whoAmI, 0f, 0f);
						Main.projectile[boom].usesLocalNPCImmunity = true;
						Main.projectile[boom].localNPCHitCooldown = -1;
						Main.projectile[boom].netUpdate = true;
						IdgProjectile.AddOnHitBuff(boom, BuffID.Daybreak, (int)(60f * moddedplayer.apocalypticalStrength));
						IdgProjectile.AddOnHitBuff(boom, Mod.Find<ModBuff>("EverlastingSuffering").Type, (int)(400f * moddedplayer.apocalypticalStrength));
					}*/

					damage = (int)(damage * (3f + (moddedplayer.apocalypticalStrength - 1f)));

					/*if (moddedplayer.magatsuSet && npc.HasBuff(ModContent.BuffType<Watched>()))
					{
						Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<Items.Armors.Magatsu.ExplosionDarkSectorEye>(), 0, 0);

						Point location;

						if (projectile != null)
							location = new Point((int)projectile.Center.X, (int)projectile.Center.Y);
						else
							location = new Point((int)npc.Center.X, (int)npc.Center.Y);

						SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.DD2_WyvernScream);
						if (sound != null)
							sound.Pitch = 0.925f;

						foreach (NPC enemy in Main.npc.Where(testby => testby.active && !testby.dontTakeDamage && !testby.friendly && testby != npc && (testby.Center - npc.Center).LengthSquared() < 400 * 400))
						{
							int damazz = Main.DamageVar(damage);
							enemy.StrikeNPC(damazz, 16, -enemy.spriteDirection, true);

							if (Main.netMode != 0)
							{
								NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, npc.whoAmI, damazz, 16f, (float)1, 0, 0, 0);
							}
						}
					}*/

					if (effectText)
					{
						CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), Color.DarkRed, "Apocalyptical!", true, false);
					}

					if (SGAConfigClient.Instance.EpicApocalypticals)
					{
						if (effectShockwave)
						{
							RippleBoom.MakeShockwave(npc, player, npc.Center, 8f, 1f, 10f, 60, 1f);
							if (EffectsSystem.ScreenShake < 32)
							{
								EffectsSystem.AddScreenShake(24f, 1200, player.Center);
							}
						}
						if (effectSound)
						{
							SoundEngine.PlaySound(new SoundStyle("Sounds/Custom/ApocalypticalHit") { Volume = 0.7f, PitchVariance = 0.25f }, npc.Center);
						}
					}
				}
			}
		}
	}

	public class RippleBoom : ModProjectile
	{
		public float rippleSize = 1f;
		public float rippleCount = 1f;
		public float expandRate = 25f;
		public float opacityrate = 1f;
		public float size = 1f;
		int maxtime = 200;
		public override string Texture => "Terraria/Images/Projectile_0";

		public override bool PreDraw(ref Color lightColor)
		{
			return false;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write((double)rippleSize);
			writer.Write((double)rippleCount);
			writer.Write((double)expandRate);
			writer.Write((double)size);
			writer.Write(maxtime);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			rippleSize = (float)reader.ReadDouble();
			rippleCount = (float)reader.ReadDouble();
			expandRate = (float)reader.ReadDouble();
			size = (float)reader.ReadDouble();
			maxtime = reader.ReadInt32();
		}

		public static void MakeShockwave(Entity target, Entity attacker, Vector2 position2, float rippleSize, float rippleCount, float expandRate, int timeleft = 200, float size = 1f, bool important = false)
		{
			if (!Main.dedServ && !Main.gameMenu)
			{
				if (!Filters.Scene["SGAmod:Shockwave"].IsActive() || important)
				{
					int prog = Projectile.NewProjectile(target.GetSource_OnHurt(attacker),position2, Vector2.Zero, ModContent.ProjectileType<RippleBoom>(), 0, 0f);
					Projectile proj = Main.projectile[prog];
					if (proj != null && proj.active)
					{
						RippleBoom modproj = proj.ModProjectile as RippleBoom;
						modproj.rippleSize = rippleSize;
						modproj.rippleCount = rippleCount;
						modproj.expandRate = expandRate;
						modproj.size = size;
						proj.timeLeft = timeleft - 10;
						modproj.maxtime = timeleft;
						proj.netUpdate = true;
						Filters.Scene.Activate("SGAmod:Shockwave", proj.Center, new object[0]).GetShader().UseColor(rippleCount, rippleSize, expandRate).UseTargetPosition(proj.Center);
					}
				}
			}
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ripple Boom");
		}

		public override void SetDefaults()
		{
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.friendly = true;
			Projectile.alpha = 0;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 200;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
		}

		public override void AI()
		{
			//float progress = (maxtime - (float)projectile.timeLeft);
			float progress = ((maxtime - (float)base.Projectile.timeLeft) / 60f) * size;
			Filters.Scene["SGAmod:Shockwave"].GetShader().UseProgress(progress).UseOpacity(100f * ((float)base.Projectile.timeLeft / (float)maxtime));
			Projectile.localAI[1] += 1f;
		}

		public override void Kill(int timeLeft)
		{
			Filters.Scene["SGAmod:Shockwave"].Deactivate(new object[0]);
		}
	}
}