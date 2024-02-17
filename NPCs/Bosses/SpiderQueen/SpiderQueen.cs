using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using SGAmod.Items;
using Terraria.GameContent.Bestiary;
using SGAmod.Dusts;
using SGAmod.Buffs.Debuffs;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;

namespace SGAmod.NPCs.Bosses.SpiderQueen
{
	[AutoloadBossHead]
	public class SpiderQueen : ModNPC, ISGABoss
	{
		public int Trophy() => ItemID.IronPickaxe;
		public bool Chance() => Main.rand.NextBool(10);
		public int RelicName() => ItemID.IronPickaxe;
		public void NoHitDrops() { }
		public int MasterPet() => ItemID.IronPickaxe;
		public bool PetChance() => Main.rand.NextBool(4);

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Spider Queen");

			// Add this in for bosses that have a summon item, requires corresponding code in the item (See MinionBossSummonItem.cs)
			NPCID.Sets.MPAllowedEnemies[Type] = true;
			// Automatically group with other bosses
			NPCID.Sets.BossBestiaryPriority.Add(Type);

			// Specify the debuffs it is immune to
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<AcidBurn>()] = true;

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new()
			{
				CustomTexturePath = GetType().Namespace.Replace('.', '/') + "/SpiderQueenLog",
				PortraitScale = 0.6f, // Portrait refers to the full picture when clicking on the icon in the bestiary
				PortraitPositionYOverride = -20f, // Full picture
				Position = new Vector2(0, -50f), // Small picture
				Scale = 0.75f, // Small picture
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}
		public override void SetDefaults()
		{
			NPC.width = 48;
			NPC.height = 48;
			NPC.damage = 70;
			NPC.defense = 5;
			NPC.boss = true;
			NPC.lifeMax = 5000;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0f;
			NPC.aiStyle = -1;
			AIType = 0;
			AnimationType = 0;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			NPC.value = 25000f;
			NPC.npcSlots = 10f; // Take up open spawn slots, preventing random NPCs from spawning during the fight
			// The following code assigns a music track to the boss in a simple way.
			if (!Main.dedServ)
			{
				Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/SpiderQueen");
			}
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			// Sets the description of this NPC that is listed in the bestiary
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
				new FlavorTextBestiaryInfoElement("Mods." + Mod.Name + ".NPCs." + Name + ".Bestiary")
			});
		}

		public override void OnKill()
		{
			NPC.SetEventFlagCleared(ref SGAWorld.downedSpiderQueen, -1);
			// Achivements.SGAAchivements.UnlockAchivement("Spider Queen", Main.LocalPlayer);
			/*if (Main.expertMode)
			{
				NPC.DropBossBags();
				return;
			}
			else
			{
				for (int i = 0; i <= Main.rand.Next(25, 45); i++)
				{
					Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height,ModContent.ItemType<VialofAcid>());
				}
				if (Main.rand.Next(0, 3) == 0)
					Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<Items.Weapons.Shields.CorrodedShield>());

				if (Main.rand.Next(7) == 0)
				{
					Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<Items.Armors.Vanity.SpiderQueenMask>());
				}
		
				if (Main.rand.Next(0, 3) == 0)
					Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<Items.Accessories.AmberGlowSkull>());
			}
			*/
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			// Add the treasure bag using ItemDropRule.BossBag (automatically checks for expert mode)
			npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<Items.Consumables.GrabBags.TreasureBagSpiderQueen>()));

			// #TODO Add Trophy, Relic, Pet

			// All our drops here are based on "not expert", meaning we use .OnSuccess() to add them into the rule, which then gets added
			LeadingConditionRule notExpertRule = new(new Conditions.NotExpert());

			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Armor.Vanity.BossMasks.SpiderQueenMask>(), 7));
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Materials.BossDrops.VialOfAcid>(), 1, 25, 45));
			// notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Shields.CorrodedShield>(), 3)); #TODO
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Accessories.Defense.CorrodedSkull>(), 3));
		}

		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * 0.625f * balance);
			NPC.damage = (int)(NPC.damage * 0.6f);
		}

		public void GetAngleDifferenceBlushiMagic(Vector2 targetPos, out float angle1, out float angle2)
		{

			Vector2 offset = targetPos - NPC.Center;
			float rotation = MathHelper.PiOver2;
			if (offset != Vector2.Zero)
			{
				rotation = offset.ToRotation();
			}
			targetPos = Main.player[(int)NPC.target].Center;
			offset = targetPos - NPC.Center;
			float newRotation = MathHelper.PiOver2;
			if (offset != Vector2.Zero)
			{
				newRotation = offset.ToRotation();
			}
			if (newRotation < rotation - MathHelper.Pi)
			{
				newRotation += MathHelper.TwoPi;
			}
			else if (newRotation > rotation + MathHelper.Pi)
			{
				newRotation -= MathHelper.TwoPi;
			}

			angle1 = rotation;
			angle2 = newRotation;
		}

		public int Phase
		{
			get
			{
				return (int)NPC.ai[1];
			}
			set
			{
				NPC.ai[1] = value;
			}
		}

		public void DoPhase(int phasetype)
		{
			if (phasetype > 0)
			{
				if (Phase == 1)
				{
					if (NPC.life < NPC.lifeMax * (Main.expertMode ? 0.5f : 0.33f))
					{
						NPC.ai[0] = 10000;
						Phase = 2;
						return;
					}
				}
				if (Phase == 0)
				{
					NPC.ai[0] = 10000;
					Phase = 1;
					return;
				}


				//Phase 2-Charging
				if (NPC.ai[0] > 1999 && NPC.ai[0] < 3000)
				{

					if (NPC.ai[0] > 2998)
					{
						NPC.ai[0] = 0;
						return;
					}
					if (NPC.ai[0] % 210 < 90 && NPC.ai[0] % 210 > 25)
					{
						NPC.rotation = NPC.rotation.AngleLerp((P.Center - NPC.Center).ToRotation(), 0.15f);
						// #New: spawn some dusts while shooting the web spit
						Dust webDust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Web, NPC.velocity.X, NPC.velocity.Y, 0, default, 1f);
						webDust.noGravity = true;
						if (NPC.ai[0] % 20 == 0 && Main.expertMode)
						{
							// Web spit shots
							Idglib.Shattershots(NPC.Center + NPC.rotation.ToRotationVector2() * 32, NPC.Center + NPC.rotation.ToRotationVector2() * 200, new Vector2(0, 0), ProjectileID.WebSpit, 15, 20, 35, 1, true, 0, false, 1600);
							SoundEngine.PlaySound(SoundID.Item102.WithVolumeScale(0.75f).WithPitchOffset(-0.25f), NPC.Center);
						}

					}
					if (NPC.ai[0] % 210 > 100 && NPC.ai[0] % 210 < 200)
					{
						charge = true;
						if (NPC.ai[0] % 210 == 105)
							// Charge at the player
							SoundEngine.PlaySound(SoundID.Roar.WithPitchOffset(0.25f), NPC.Center);
						NPC.velocity += NPC.rotation.ToRotationVector2() * 2f;

						if ((NPC.ai[0] % (Main.expertMode ? 15 : 20)) == 0 && Phase > 1)
						{
							// Shoot out to the side
							Idglib.Shattershots(NPC.Center, NPC.Center + (NPC.rotation + MathHelper.Pi/2f).ToRotationVector2() * 64, new Vector2(0, 0), Mod.Find<ModProjectile>("SpiderVenom").Type, 10, 7, 35, 1, true, 0, true, 1600);
							Idglib.Shattershots(NPC.Center, NPC.Center + (NPC.rotation - MathHelper.Pi/2f).ToRotationVector2() * 64, new Vector2(0, 0), Mod.Find<ModProjectile>("SpiderVenom").Type, 10, 7, 35, 1, true, 0, true, 1600);
							SoundEngine.PlaySound(SoundID.Item102.WithVolumeScale(0.75f).WithPitchOffset(-0.25f), NPC.Center);
						}

						if (NPC.velocity.Length() > 96f)
						{
							NPC.velocity.Normalize();
							NPC.velocity *= 96f;
						}
					}
					NPC.localAI[0] += NPC.velocity.Length() / 3f;
					NPC.velocity /= 1.15f;
				}
			}

			//Spinning Trap Webs
			if (NPC.ai[0] > 2999 && NPC.ai[0] < 4000) {
				if (NPC.ai[0] == 3005)
					SoundEngine.PlaySound(SoundID.NPCHit56.WithVolumeScale(0.75f).WithPitchOffset(-0.25f), NPC.Center);

				if (NPC.ai[0] > 3100 && NPC.ai[0] < 3300)
				{

					legdists = 72;
					GetAngleDifferenceBlushiMagic(new Vector2(NPC.localAI[1], NPC.localAI[2]), out float angle1, out float angle2);
					float rotSpeed = angle2 > angle1 ? 0.05f : -0.05f;
					rotSpeed *= 1f + ((float)(angle2 - angle1) * 0.2f);

					NPC.rotation += rotSpeed;
					if (NPC.ai[0] % 10 == 0)
					{
						//AcidicWebTile
						int type = ModContent.ProjectileType<TrappingWeb>();
						Idglib.Shattershots(NPC.Center + NPC.rotation.ToRotationVector2() * 32, NPC.Center + NPC.rotation.ToRotationVector2() * 200, new Vector2(0, 0), type, 15, 7, 35, 1, true, 0, true, 1600);
						SoundEngine.PlaySound(SoundID.Item102.WithVolumeScale(0.75f).WithPitchOffset(-0.25f), NPC.Center);
					}

					if (NPC.ai[0] % 150 == 31)
					{
						NPC.localAI[1] = P.Center.X;
						NPC.localAI[2] = P.Center.Y;
					}
				}

				if (NPC.ai[0] > 3350)
				{
					NPC.ai[0] = Main.rand.Next(2400, 2700);
					NPC.netUpdate = true;
				}

				NPC.velocity *= 0.96f;
			}


			//Wounded
			if (NPC.ai[0] > 9999)
			{
				NPC.velocity /= 1.25f;
				if (NPC.ai[0] == 10001)
					SoundEngine.PlaySound(SoundID.NPCHit51.WithPitchOffset(0.25f), NPC.Center);
				NPC.rotation += Main.rand.NextFloat(1f, -1f) * 0.08f;


				if (NPC.ai[0] > 10100)
				{
					if (Phase == 1)
					{
						NPC.ai[0] = 2000;
					}
				}
				if (NPC.ai[0] > 10100)
				{
					if (Phase == 2)
					{
						NPC.ai[0] = 3000;
					}
				}
			}
		}

		public Player P;
		int legdists = 100;
		bool charge = false;

		public override void AI()
		{
			LegsMethod();
			charge = false;
			legdists = 128;
			NPC.direction = NPC.velocity.X > 0 ? -1 : 1;
			P = Main.player[NPC.target];
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			{
				NPC.TargetClosest(false);
				P = Main.player[NPC.target];
				if (!P.active || P.dead)
				{
					float speed = ((-10f));
					NPC.velocity = new Vector2(NPC.velocity.X, NPC.velocity.Y + speed);
					NPC.active = false;
				}

			}
			else
			{
				//if (SGAmod.DRMMode)
				//	phase = 2;
				NPC.dontTakeDamage = false;
				bool sighttoplayer = (Collision.CanHitLine(new Vector2(NPC.Center.X, NPC.Center.Y), 6, 6, new Vector2(P.Center.X, P.Center.Y), 6, 6));
				bool underground = Items.Consumables.BossSummons.AcidicEgg.Underground((int)NPC.Center.Y);//(int)((double)((npc.position.Y + (float)npc.height) * 2f / 16f) - Main.worldSurface * 2.0) > 0;
				if (!underground)
				{
					NPC.dontTakeDamage = true;
				}
				NPC.ai[3] -= 1;
				if (NPC.ai[3] < 1)
					NPC.ai[0] += 1;

				Vector2 playerangledif = P.Center - NPC.Center;
				float playerdist = playerangledif.Length();
				float maxspeed = 3f;
				if (Main.expertMode && !sighttoplayer)
					maxspeed += 3f;

				float maxrotate = 0.05f;
				playerangledif.Normalize();

				if (NPC.ai[0] < 1201)//Standard Attacks
				{
					if (Phase == 0)
					{
						if (NPC.life < NPC.lifeMax * 0.75)
						{
							DoPhase(1);
							return;
						}
					}

					NPC.ai[0] %= 1200;
					if (NPC.ai[0] % 1200 < 600)
					{

						if (NPC.ai[0] == 10)
						{
							if (Phase > 0)
								NPC.ai[0] = Main.rand.Next(50, 400);
							NPC.netUpdate = true;
						}

						NPC.localAI[0] += 1f;
						NPC.localAI[1] = P.Center.X;
						NPC.localAI[2] = P.Center.Y;
						if ((NPC.ai[0] + 26) % (Main.expertMode ? 60 : 90) == 0)//Chase after the player and Squirt
						{
							// Standard Shots while moving forward
							Idglib.Shattershots(NPC.Center + NPC.rotation.ToRotationVector2() * 32, NPC.Center + NPC.rotation.ToRotationVector2() * 200, new Vector2(0, 0), ModContent.ProjectileType<SpiderVenom>(), 15, 8, 60 + Phase * 10, Phase + 1, true, 0, true, 1600);
							SoundEngine.PlaySound(SoundID.Item102.WithVolumeScale(0.75f).WithPitchOffset(-0.25f), NPC.Center);
						}
						NPC.rotation = NPC.rotation.AngleLerp((P.Center - NPC.Center).ToRotation(), maxrotate);
						NPC.velocity += NPC.rotation.ToRotationVector2() * 0.4f;
						NPC.velocity += playerangledif * 0.075f;
						if (NPC.velocity.Length() > maxspeed)
						{
							NPC.velocity.Normalize(); NPC.velocity *= maxspeed;
						}
					}
					else
					{
						//Acid Spin Attack
						if (NPC.ai[0] % 1200 == 601)
						{
							NPC.ai[0] += 1;
							NPC.ai[3] = 60;
							SoundEngine.PlaySound(SoundID.NPCHit37.WithVolumeScale(0.75f).WithPitchOffset(-0.25f), NPC.Center);

							// #New: Spawn a circle of dusts.
							for (int i = 0; i < 30; i++)
							{
								Vector2 circle = Main.rand.NextVector2CircularEdge(1f, 1f);
								Dust dust = Dust.NewDustPerfect(NPC.Center + circle * 20f, ModContent.DustType<AcidDust>(), circle, 250, default, 1f);
								dust.noGravity = true;
								dust.noLightEmittence = true;
								dust.noLight = true;
							}
						}
						if (NPC.ai[0] % 1200 > 602)
						{
							GetAngleDifferenceBlushiMagic(new Vector2(NPC.localAI[1], NPC.localAI[2]), out float angle1, out float angle2);
							float rotSpeed = angle2 > angle1 ? 0.05f : -0.05f;
							rotSpeed *= 1f + ((float)(angle2 - angle1) * 0.2f);

							legdists = 72;

							if (NPC.ai[0] % 150 < 60)
							{
								NPC.rotation += rotSpeed;
								if (NPC.ai[0] % (Main.expertMode ? 5 : 10) == 0 && NPC.ai[3] < 1)
								{
									int type = ModContent.ProjectileType<SpiderVenom>();
									Idglib.Shattershots(NPC.Center + NPC.rotation.ToRotationVector2() * 32, NPC.Center + NPC.rotation.ToRotationVector2() * 200, new Vector2(0, 0), type, 15, 7, 35 + (Phase * 15), 1 + Phase, true, 0, true, 1600);
									SoundEngine.PlaySound(SoundID.Item102.WithVolumeScale(0.75f).WithPitchOffset(-0.25f), NPC.Center);
								}

								if (NPC.ai[0] % 150 == 61)
								{
									NPC.localAI[1] = P.Center.X;
									NPC.localAI[2] = P.Center.Y;
								}
							}
						}


						NPC.velocity *= 0.96f;

					}
					if (NPC.ai[0] == 1195 && Phase > 0)
					{
						NPC.ai[0] = 2100;
					}

				}//Standard Attacks Over

				DoPhase(Phase);


				if (sighttoplayer)
				{
					if (NPC.ai[2] > 1500)
					{
						NPC.ai[2] = 0;
						NPC.ai[0] = 3000;
						NPC.netUpdate = true;
					}

					if (Phase > 1 && NPC.ai[0] < 3000)
						NPC.ai[2] += 1;
				}
			}
		}

		List<SpiderLeg> legs;

		public void LegsMethod()
		{
			if (legs == null)
			{
				legs = new List<SpiderLeg>();
				Vector2[] legbody = { new Vector2(-10, -12), new Vector2(0, -12), new Vector2(10, -12), new Vector2(20, -8) };
				Vector2[] legbodyExtended = { new Vector2(-12, -64), new Vector2(32, -84), new Vector2(72, -84), new Vector2(100, -80) };

				for (int xx = -1; xx < 2; xx += 2)
				{
					for (int i = 0; i < legbodyExtended.Length; i += 1)
					{
						legs.Add(new SpiderLeg(new Vector2(legbodyExtended[i].X, legbodyExtended[i].Y*xx), new Vector2(legbody[i].X, legbody[i].Y * xx),xx));
					}
				}
			}
			else
			{
				for (int i = 0; i < legs.Count; i += 1)
				{
					legs[i].LegUpdate(NPC.Center, NPC.rotation, legdists,NPC.velocity, charge);
				}
			}
		}

		internal readonly Texture2D texBody = ModContent.Request<Texture2D>("SGAmod/NPCs/Bosses/SpiderQueen/SpiderQueen").Value;
		internal readonly Texture2D texBodyGlow = ModContent.Request<Texture2D>("SGAmod/NPCs/Bosses/SpiderQueen/SpiderQueen").Value;
		internal readonly Texture2D texSkull = ModContent.Request<Texture2D>("SGAmod/NPCs/Bosses/SpiderQueen/SpiderQueenCorrodedSkull").Value;
		internal readonly Texture2D texBodyOverlay = ModContent.Request<Texture2D>("SGAmod/NPCs/Bosses/SpiderQueen/SpiderQueenOverlay").Value;

		internal readonly Texture2D texLeg1 = ModContent.Request<Texture2D>("SGAmod/NPCs/Bosses/SpiderQueen/SpiderLeg").Value;
		internal readonly Texture2D texLeg2 = ModContent.Request<Texture2D>("SGAmod/NPCs/Bosses/SpiderQueen/SpiderClaw").Value;
		internal readonly Texture2D texLeg2Glow = ModContent.Request<Texture2D>("SGAmod/NPCs/Bosses/SpiderQueen/SpiderClaw_Glow").Value;

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Vector2 drawOriginBody = new(texBody.Width, texBody.Height / 2);
			Vector2 drawPos = ((NPC.Center - Main.screenPosition)) + NPC.rotation.ToRotationVector2() * -46f;
			Vector2 drawPosHead = ((NPC.Center - Main.screenPosition)) + NPC.rotation.ToRotationVector2() * 38f;

			if (legs != null)
			{
				for (int i = 0; i < legs.Count; i += 1)
				{
					legs[i].Draw(this, NPC.Center, NPC.rotation,false, NPC.velocity, spriteBatch);
				}
			}

			Vector2 floatypos = new((float)Math.Cos(Main.GlobalTimeWrappedHourly / 1f) * 6f, (float)Math.Sin(Main.GlobalTimeWrappedHourly / 1.37f) * 3f);
			spriteBatch.Draw(texBody, drawPosHead, null, drawColor, NPC.rotation, drawOriginBody, NPC.scale, SpriteEffects.None, 0f);
			spriteBatch.Draw(texBodyGlow, drawPosHead, null, Color.White, NPC.rotation, drawOriginBody, NPC.scale, SpriteEffects.None, 0f);
			spriteBatch.Draw(texSkull, drawPos+ floatypos.RotatedBy(NPC.rotation), null, Color.White * 0.75f, NPC.rotation + (NPC.whoAmI * 0.753f), new Vector2(texSkull.Width / 2f, texSkull.Height / 2f), NPC.scale, SpriteEffects.None, 0f);
			spriteBatch.Draw(texBodyOverlay, drawPosHead, null, drawColor, NPC.rotation, drawOriginBody, NPC.scale, SpriteEffects.None, 0f);

			return false;
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (NPC.life < 1 && Main.netMode != NetmodeID.Server)
			{
				for (int i = 0; i < 6; i += 1)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity + new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2)), Mod.Find<ModGore>("SpiderBody").Type);
				}
				for (int i = 0; i < 2; i += 1)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.Center + (NPC.rotation.ToRotationVector2() * 24f), NPC.velocity + new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2)), Mod.Find<ModGore>("SpiderManible").Type);
				}
				for (int i = 0; i < 80; i++)
				{
					Vector2 randomcircle = new Vector2(Main.rand.Next(-58, -14), Main.rand.Next(-10, 14)).RotatedBy(NPC.rotation);
					int dust = Dust.NewDust(NPC.Center + new Vector2(randomcircle.X, randomcircle.Y), 0, 0, ModContent.DustType<AcidDust>());
					Main.dust[dust].scale = 2f;
					Main.dust[dust].noGravity = false;
					Main.dust[dust].velocity = -NPC.velocity * (float)(Main.rand.Next(20, 120) * 0.01f) + (Main.rand.NextFloat(0, 360).ToRotationVector2() * Main.rand.NextFloat(1f, 6f));
				}
				if (legs != null)
				{
					for (int i = 0; i < legs.Count; i += 1)
					{
						legs[i].Draw(this, NPC.Center, NPC.rotation, true, NPC.velocity);
					}
				}
			}
		}

		public class SpiderLeg
		{
			Vector2 LegPos;
			Vector2 PreviousLegPos;
			Vector2 CurrentLegPos;
			float lerpvalue = 1;
			//float maxdistance;
			Vector2 desirsedLegPos;
			Vector2 BodyLoc;
			int side;
			public SpiderLeg(Vector2 Startleg, Vector2 BodyLoc,int side)
			{
				LegPos = Startleg;
				PreviousLegPos = Startleg;
				CurrentLegPos = Startleg;
				desirsedLegPos = Startleg;
				this.BodyLoc = BodyLoc;
				this.side = side;
			}

			public void LegUpdate(Vector2 SpiderLoc, float SpiderAngle, float maxdistance, Vector2 SpiderVel, bool charge)
			{
				bool spin = maxdistance < 94;
				//this.maxdistance = maxdistance;
				float dev = charge ? 2f : 5f;
				float forward = Math.Abs((SpiderVel.Length() - 4f) * 8f)* (desirsedLegPos.X>-0 ? desirsedLegPos.X/100f : 1f);
				if (spin)
					forward -= (125f - desirsedLegPos.X);
				Vector2 leghere = SpiderLoc+(new Vector2(forward,0f) + desirsedLegPos).RotatedBy(SpiderAngle);
				lerpvalue += (1f - lerpvalue) / dev;
				LegPos = Vector2.Lerp(PreviousLegPos, CurrentLegPos, lerpvalue);

				if ((LegPos - leghere).Length() > (maxdistance+((dev-4f)*16f))+ (charge ? 74 : 0))
				{
					PreviousLegPos = LegPos;
					CurrentLegPos = leghere+new Vector2(Main.rand.Next(-24,24), Main.rand.Next(-24, 24));
					lerpvalue = 0f;
				}
			}

			public void Draw(SpiderQueen bossInstance, Vector2 SpiderLoc, float SpiderAngle, bool gibs, Vector2 velo, SpriteBatch spriteBatch = null)
			{

				int length1 = 58;//First Leg
				int length2 = 74;//Second Leg

				Vector2 start = SpiderLoc + BodyLoc.RotatedBy(SpiderAngle);

				Vector2 middle = LegPos - start;

				float angleleg1 = (LegPos - start).ToRotation() + (MathHelper.Clamp((MathHelper.PiOver2) - MathHelper.ToRadians(middle.Length() / 1.6f), MathHelper.Pi / 12f, MathHelper.PiOver2) * side);

				Vector2 legdist = angleleg1.ToRotationVector2();
				legdist.Normalize();
				Vector2 halfway1 = legdist;
				legdist *= length1 - 8;

				Vector2 leg2 = (SpiderLoc + BodyLoc.RotatedBy(SpiderAngle)) + legdist;

				float angleleg2 = (LegPos - leg2).ToRotation();

				halfway1 *= length1 / 2;
				Vector2 halfway2 = leg2 + (angleleg2.ToRotationVector2() * length2 / 2);
				if (!gibs)
				{
					Color lighting = Lighting.GetColor((int)((start.X + halfway1.X) / 16f), (int)((start.Y + halfway1.Y) / 16f));
					spriteBatch.Draw(bossInstance.texLeg1, start - Main.screenPosition, null, lighting, angleleg1, new Vector2(4, bossInstance.texLeg1.Height / 2f), 1f, angleleg1.ToRotationVector2().X > 0 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
					lighting = Lighting.GetColor((int)(halfway2.X / 16f), (int)(halfway2.Y / 16f));
					spriteBatch.Draw(bossInstance.texLeg2, leg2 - Main.screenPosition, null, lighting, angleleg2, new Vector2(4, bossInstance.texLeg2.Height / 2f), 1f, angleleg2.ToRotationVector2().X > 0 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
					spriteBatch.Draw(bossInstance.texLeg2Glow, leg2 - Main.screenPosition, null, Color.White, angleleg2, new Vector2(4, bossInstance.texLeg2.Height / 2f), 1f, angleleg2.ToRotationVector2().X > 0 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
				}
				else
				{
					Gore.NewGore(bossInstance.NPC.GetSource_Death(), halfway1, velo + new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2)), SGAmod.Instance.Find<ModGore>("SpiderLeg1").Type);
					Gore.NewGore(bossInstance.NPC.GetSource_Death(), halfway2, velo + new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2)), SGAmod.Instance.Find<ModGore>("SpiderLeg2").Type);
				}
			}
		}
	}
}

