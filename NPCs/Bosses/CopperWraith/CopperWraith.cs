using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using SGAmod.Items;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using log4net.Core;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using SGAmod.Items.Materials.BossDrops;
using SGAmod.Items.Consumables.Other;
using SGAmod.NPCs.Bosses.CobaltWraith;
using Terraria.Chat;

namespace SGAmod.NPCs.Bosses.CopperWraith
{
    public class CopperArmorPiece : ModNPC
    {
        public int armortype = ItemID.CopperChainmail;
        public int attachedID = 0;
        public int CoreID = 0;
        public float friction = 0.75f;
        public float speed = 0.3f;
        public string attachedType = "CopperWraith";
        public bool selfdestruct;
	

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(attachedID);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            attachedID = reader.Read();
        }
        public virtual void ArmorMalfunction()
        {
            selfdestruct = true;
            CopperArmorPiece myself = NPC.ModNPC as CopperArmorPiece;
            NPC.velocity = new Vector2(Main.rand.Next(-5,5), Main.rand.Next(-5,5));
            NPC.SimpleStrikeNPC((int)(NPC.lifeMax * 0.15f), 1, false, 0, null, false, 0, true);
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CopperWraithNotch>(), 8));
        }

        public override bool CheckActive()
        {
            CopperArmorPiece myself = NPC.ModNPC as CopperArmorPiece;
            NPC myowner = Main.npc[myself.attachedID];
            
            return (!myowner.active);
        }
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
			
        }
        public override string Texture => "Terraria/Images/Item_" + armortype;

        public override void SetDefaults()
        {
            NPC.width = 24;
            NPC.height = 24;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 500;
            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath7;
            NPC.value = 0f;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            AnimationType = 0;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
        }

        public override bool PreAI()
        {
            //Later down the line, there is Hellion code to port.
            return true;
        }
        public override void AI()
        {
            int npcType = ModContent.NPCType<CopperWraith>();
            if(NPC.CountNPCS(npcType) > 0)
            {
                NPC myowner = Main.npc[NPC.FindFirstNPC(npcType)];
            }
            else NPC.active = false;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 0f;
            return null;
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if(Main.expertMode)
            {
                double damagemul = 1.0;
                if(projectile.penetrate > 1 || projectile.penetrate < 0)
                {
                    damagemul = 0.5;
                }

                base.OnHitByProjectile(projectile, default, (int)(modifiers.FinalDamage.Base * damagemul));
            }
        }

    }
    public class CopperArmorChainmail : CopperArmorPiece
    {
        public int armortype = ItemID.CopperChainmail;

        public override void SetDefaults()
        {
            NPC.width = 32;
            NPC.height = 32;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 500;
            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath7;
            NPC.value = 0f;
            NPC.aiStyle = -1;
            AIType = -1;
            AnimationType = 0;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
			NPCID.Sets.NeedsExpertScaling[NPC.type] = true;
		}

        public override string Texture => "Terraria/Images/Item_" + (ItemID.CopperChainmail);
        public override void AI()
        {
            CopperArmorPiece myself = NPC.ModNPC as CopperArmorPiece;
            NPC myowner = Main.npc[myself.attachedID];
            if (!myowner.active) myself.ArmorMalfunction();
            else
            {
                NPC.velocity = NPC.velocity + (myowner.Center + new Vector2((float)NPC.ai[1], (float)NPC.ai[2]) - NPC.Center) * (myself.speed);
                NPC.velocity *= myself.friction;
                NPC.rotation = (float)NPC.velocity.X * 0.1f;
                NPC.timeLeft = 999;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Vector2 drawpos = NPC.Center - Main.screenPosition;
            Color lights = drawColor;
            lights.A = (byte)NPC.alpha;
            Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);

            Vector2 drawOffset = new Vector2((float)Math.Sin(Main.GlobalTimeWrappedHourly * 1.61775f + ((float)NPC.whoAmI * 5.734575f)) * 7f, (float)Math.Cos(Main.GlobalTimeWrappedHourly * 1.246f + ((float)NPC.whoAmI) * 5.734575f) * 5f);

            if (GetType() == typeof(CopperArmorChainmail) || GetType() == typeof(CopperArmorGreaves) || GetType() == typeof(CopperArmorHelmet)
                || GetType() == typeof(CobaltArmorChainmail) || GetType() == typeof(CobaltArmorGreaves) || GetType() == typeof(CobaltArmorHelmet)
                ) 
            {
                Rectangle drawrect;
                texture = GetType() == typeof(CobaltArmorGreaves) ? ModContent.Request<Texture2D>("SGAmod/NPCs/Bosses/CobaltWraith/Cobalt_Wraith_resprite_leggys").Value : ModContent.Request<Texture2D>("SGAmod/NPCs/Bosses/CopperWraith/Copper_Wraith_resprite_leggys").Value;
                
                origin = new Vector2((float)texture.Width * 0.5f, -12);
                if (GetType() == typeof(CopperArmorChainmail)
                    || GetType() == typeof(CobaltArmorChainmail)
                    )
                {
                    texture = GetType() == typeof(CobaltArmorChainmail) ? ModContent.Request<Texture2D>("SGAmod/NPCs/Bosses/CobaltWraith/Cobalt_Wraith_resprite_chestplate").Value : ModContent.Request<Texture2D>("SGAmod/NPCs/Bosses/CopperWraith/Copper_Wraith_resprite_chestplate").Value;
             
                    origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);
                }
                if (GetType() == typeof(CopperArmorHelmet)
                    || GetType() == typeof(CobaltArmorHelmet)
                    )
                {
                    texture = GetType() == typeof(CobaltArmorHelmet) ? ModContent.Request<Texture2D>("SGAmod/NPCs/Bosses/CobaltWraith/Cobalt_Wraith_resprite_Helmet").Value : ModContent.Request<Texture2D>("SGAmod/NPCs/Bosses/CopperWraith/Copper_Wraith_resprite_Helmet_1").Value;
                   
                    int offset = (int)(Math.Min(((int)((float)Main.GlobalTimeWrappedHourly * 8f)) % 15f, 5) * ((float)texture.Height / 6f));
                    drawrect = new Rectangle(0, offset, texture.Width, (int)(texture.Height / 6f));
                    origin = new Vector2((float)texture.Width * 0.5f, ((float)(texture.Height / 6f) * 0.5f) + 20);
                }
                else
                {
                    drawrect = new Rectangle(0,0,texture.Width, texture.Height);
                }

                SpriteEffects effect = SpriteEffects.None;
                Player theplayer = Main.LocalPlayer;
                if (theplayer.active && !theplayer.dead)
                {
                    if(theplayer.Center.X < NPC.Center.X)
                    {
                        effect = SpriteEffects.FlipHorizontally;
                    }
                }

                for (float speez = NPC.velocity.Length(); speez > 0f; speez -= 0.2f)
                {
                    Vector2 speedz = (NPC.velocity); speedz.Normalize();
                    spriteBatch.Draw(texture, drawpos + (speedz * speez * -2f) + drawOffset, drawrect, lights * 0.02f, NPC.rotation, origin, Vector2.One, effect, 0f);
                }

                spriteBatch.Draw(texture, drawpos + drawOffset, drawrect, drawColor, NPC.rotation, origin, Vector2.One, effect,0f);
            }
            else
            {
                spriteBatch.Draw(texture, drawpos + (drawOffset / 3f), null, drawColor, NPC.rotation, origin, Vector2.One, SpriteEffects.None, 0f);
            }
            return false;
        }
    }
    public class CopperArmorHelmet : CopperArmorChainmail
    {
        public int armortype = ItemID.CopperHelmet;
        public override void SetDefaults()
        {
            base.SetDefaults();
        }
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
			NPCID.Sets.NeedsExpertScaling[NPC.type] = true;
		}
        public override string Texture => "Terraria/Images/Item_" + (ItemID.CopperHelmet);
    }
    public class CopperArmorGreaves : CopperArmorChainmail
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
        }
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
			NPCID.Sets.NeedsExpertScaling[NPC.type] = true;
		}
    }

    public class CopperArmorBow : CopperArmorPiece
    {
        public int armortype = ItemID.CopperBow;
        public override void SetDefaults()
        {
            NPC.width = 24;
            NPC.height = 24;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 400;
            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath7;
            NPC.value = 0f;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            AIType = -1;
            AnimationType = 0;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
        }
        public override string Texture => "Terraria/Images/Item_" + (ItemID.CopperBow);

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
			NPCID.Sets.NeedsExpertScaling[NPC.type] = true;
		}

        public override void AI()
        {
            float speedmulti = 0.75f;
            if (!Main.expertMode || !Main.masterMode) speedmulti = 0.5f;
            //if (SGAmod.DRMMode) speedmulti = 1f; what
            CopperArmorPiece myself = NPC.ModNPC as CopperArmorPiece;
            int NPCtype = ModContent.NPCType<CopperWraith>();
            NPC myowner = Main.npc[myself.attachedID];
            if (!myowner.active) myself.ArmorMalfunction();
            else
            {
                Player P = Main.player[NPC.target];
                if(NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
                {
                    NPC.TargetClosest(false);
                    P = Main.player[NPC.target];
                    if(!P.active || P.dead)
                    {
                        NPC.active = false;
                        Main.npc[(int)NPC.ai[1]].active = false;
                    }
                }
                else
                {
                    NPC.ai[0] += 1;
                    Vector2 itt = (myowner.Center - NPC.Center + new Vector2(NPC.ai[1] * NPC.spriteDirection, NPC.ai[2]));
                    if (NPC.ai[0] % 1500 > 1250)
                    {
                        itt = (P.position - NPC.position + new Vector2(2f * NPC.ai[1] * NPC.spriteDirection, NPC.ai[2] * 2f));
                    }
                    float locspeed = 0.25f * speedmulti;
                    if (NPC.ai[0] % 900 > 550)
                    {
                        Vector2 cas = new Vector2(NPC.position.X - P.position.X, NPC.position.Y - P.position.Y);
                        double dist = cas.Length();
                        float rotation = (float)Math.Atan2(NPC.position.Y - (P.position.Y - (new Vector2(0, (float)(dist * 0.15f))).Y + (P.height * 0.5f)), NPC.position.X - (P.position.X + (P.width * 0.5f)));
                        NPC.rotation = rotation;
                        NPC.velocity *= 0.86f;
                        if (NPC.ai[0] % 50 == 0 && NPC.ai[0] % 900 > 650)
                        {
                            List<Projectile> one = Idglib.Shattershots(NPC.Center, NPC.Center + new Vector2(-25 * NPC.spriteDirection, 0), Vector2.Zero, Math.Abs(NPC.ai[1]) < 18 ? ProjectileID.DD2BetsyArrow : /*(SGAmod.DRMMode ? ModContent.ProjectileType<UnmanedArrow>() : ProjectileID.WoodenArrowHostile)*/ ProjectileID.WoodenArrowHostile, 7, 12, 0, 1, true, (Main.rand.Next(-100, 100) * 0.000f) - NPC.rotation, true, 300);
                            one[0].hostile = true;
                            one[0].friendly = false;
                            one[0].localAI[0] = P.whoAmI;
                            one[0].netUpdate = true;
                        }
                        NPC.spriteDirection = 1;
                    }
                    else
                    {
                        if (Math.Abs(NPC.velocity.X) > 2) { NPC.spriteDirection = NPC.velocity.X > 0 ? -1 : 1; }
                        NPC.rotation = (float)NPC.velocity.X * 0.09f;
                        locspeed = 0.5f * speedmulti;
                    }
                    NPC.velocity *= 0.96f;
                    itt.Normalize();
                    NPC.velocity += (itt * locspeed);
                    NPC.timeLeft = 999;
                }
            }

        }
    }
    public class CopperArmorSword : CopperArmorPiece
    {
        public int armorType = ItemID.CopperBroadsword;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
			NPCID.Sets.NeedsExpertScaling[NPC.type] = true;
		}
        public override string Texture => "Terraria/Images/Item_" + (ItemID.CopperShortsword);

        public override void SetDefaults()
        {
            NPC.width = 24;
            NPC.height = 24;
            NPC.damage = 5;
            NPC.defense = 0;
            NPC.lifeMax = 400;
            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath7;
            NPC.value = 0f;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            AIType = -1;
            AnimationType = 0;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
        }
        public override void AI()
        {
            float speedmulti = 0.75f;
            if (!Main.expertMode || !Main.masterMode) speedmulti = 0.5f;
            //if (SGAWorld.NightmareHardcore > 0) speedmulti = 1f;
            CopperArmorPiece myself = NPC.ModNPC as CopperArmorPiece;
            int npcType = ModContent.NPCType<CopperWraith>();
            NPC myowner = Main.npc[myself.attachedID];
            if (!myowner.active) myself.ArmorMalfunction();
            else
            {
                Player P = Main.player[myowner.target];
                NPC.ai[0]++;
                NPC.spriteDirection = NPC.velocity.X > 0 ? -1 : 1;
                Vector2 itt = myowner.Center - NPC.Center + new Vector2(NPC.ai[1] * NPC.spriteDirection, NPC.ai[2]);
                float locspeed = 0.25f * speedmulti;
                if (NPC.ai[0] % 600 > 350)
                {
                    NPC.damage = (int)NPC.defDamage * 3;
                    itt = P.position - NPC.position + new Vector2(NPC.ai[1] * NPC.spriteDirection, -8);

                    /*if (NPC.ai[0] % 180 == 0 && SGAWorld.NightmareHardcore > 0)
                    {
                        Vector2 zxx = itt;
                        zxx += P.velocity * 3f;
                        zxx.Normalize();
                        NPC.velocity += zxx * 18;
                    }*/
                    NPC.rotation += (0.65f * NPC.spriteDirection);
                }
                else
                {
                    NPC.damage = (int)NPC.defDamage;
                    if (NPC.ai[0] % 300 < 60)
                    {
                        locspeed = 2.5f;
                        NPC.velocity *= 0.92f;
                    }
                    NPC.rotation = (float)NPC.velocity.X * 0.09f;
                    locspeed = 0.5f * speedmulti;
                }
                NPC.velocity *= 0.96f;
                itt.Normalize();
                NPC.velocity += (itt * locspeed);
                NPC.timeLeft = 999;

            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (Main.expertMode || Main.masterMode || Main.rand.Next(2) == 0) target.AddBuff(BuffID.Bleeding, 60*10, true);
        }
    }

    [AutoloadBossHead]
    public class CopperWraith : ModNPC, ISGABoss
    {
        public int Trophy() => GetType() == typeof(CobaltWraith.CobaltWraith) ? ItemID.CobaltPickaxe : ItemID.IronPickaxe;
        public bool Chance() => Main.rand.Next(0, 10) == 0;
        public int RelicName() => GetType() == typeof(CobaltWraith.CobaltWraith) ? ItemID.CobaltPickaxe : ItemID.IronPickaxe;
        public void NoHitDrops() { }

        public int MasterPet() => GetType() == typeof(CobaltWraith.CobaltWraith) ? ItemID.CobaltPickaxe :ItemID.IronPickaxe;

        public bool PetChance() => Main.rand.Next(0, 4) == 0;

        public int level = 0;
        public Vector2 OffsetPoints = Vector2.Zero;
        public float speed = 0.2f;
        public bool coreOnlyMode = false;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.NeedsExpertScaling[NPC.type] = true;
            NPCID.Sets.MPAllowedEnemies[NPC.type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                CustomTexturePath = "SGAmod/NPCs/Bosses/CopperWraith/CopperWraithLog",
                PortraitScale = 1f,
                PortraitPositionYOverride = 0f,
                
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type,drawModifiers);
        }
        public override void SetDefaults()
        {
            NPC.width = 16;
            NPC.height = 16;
            NPC.damage = 10;
            NPC.defense = 0;
            NPC.lifeMax = 400;
            NPC.HitSound = SoundID.NPCHit5;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.knockBackResist = 0.05f;
            NPC.aiStyle = -1;
            NPC.boss = true;
            Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Copperig");
            AnimationType = 0;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.value = Item.buyPrice(0, 0, 25, 0);
            NPC.friendly = false;
            

        }
        public override string Texture => "SGAmod/NPCs/Bosses/TPD";

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>()
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("Mods." + Mod.Name + ".NPCs." + Name + ".Bestiary")
                
            });
        }

        public override string BossHeadTexture => "SGAmod/NPCs/Bosses/CopperWraith/CopperWraith_Head_Boss";

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.625f * balance);
            NPC.damage = (int)(NPC.damage * 0.6f); 
        }
        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if ((projectile.penetrate < 1 || projectile.penetrate > 1) && projectile.damage > (GetType() == typeof(CobaltWraith.CobaltWraith) ? 20 : 30))
            {
                modifiers.FinalDamage.Base *= 0.25f;
            }
            else
            {
                if (GetType() == typeof(CopperWraith)) modifiers.FinalDamage.Base *= 1.5f;
                if (projectile.maxPenetrate < 2 && projectile.maxPenetrate > -1) modifiers.SetCrit();
            }
        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.LesserHealingPotion;
        }

		public override void OnKill()
		{
			if (!SGAWorld.downedCopperWraith)
			{
				SGAWorld.downedCopperWraith = true;
				Idglib.Chat("You may now craft bars without being attacked", 150, 150, 70);
			}
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            List<int> types = new List<int>();
            
			
            int shardtype = ModContent.ItemType<CopperWraithShard>();
            
            npcLoot.Add(ItemDropRule.ByCondition(new NotWarned(), ModContent.ItemType<TrueCopperWraithNotch>()));

            LeadingConditionRule NotExpert = new LeadingConditionRule(new Conditions.NotExpert());
            NotExpert.OnSuccess(ItemDropRule.Common(shardtype));
            types.Insert(types.Count, SGAmod.WorldOres[0, SGAWorld.oreTypesPreHardmode[0] == TileID.Copper ? 1 : 0]);

            if (shardtype > 0) 
                npcLoot.Add(ItemDropRule.Common(shardtype, 1, Main.expertMode ? 50 : 25, Main.expertMode ? 70 : 35));
            

            
            
        }

        public virtual void SpawnMoreExpert()
        {
            //New Sword 1
            int newguy = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<CopperArmorSword>());
            NPC newguy2 = Main.npc[newguy];
            CopperArmorPiece newguy3 = newguy2.ModNPC as CopperArmorPiece;
            newguy3.attachedID = NPC.whoAmI;
            newguy2.ai[0] = 300f;
            newguy2.ai[1] = -64f;
            newguy2.ai[2] = 48f;
            newguy2.lifeMax = (int)NPC.lifeMax * 1;
            newguy2.life = (int)(NPC.lifeMax * 1f);
            newguy2.knockBackResist = 0.9f;
            newguy2.netUpdate = true;

            //New Sword 2
            newguy = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X - 400, (int)NPC.position.Y, ModContent.NPCType<CopperArmorSword>());
            newguy2 = Main.npc[newguy];
            newguy3 = newguy2.ModNPC as CopperArmorPiece;
            newguy3.attachedID = NPC.whoAmI;
            newguy2.ai[1] = 64f;
            newguy2.ai[2] = 48f;
            newguy2.lifeMax = (int)NPC.lifeMax * 1;
            newguy2.life = (int)NPC.lifeMax * 1;
            newguy2.knockBackResist = 0.9f;
            newguy2.netUpdate = true;

            //New Bow 1
            newguy = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X - 400, (int)NPC.position.Y, ModContent.NPCType<CopperArmorBow>());
            newguy2 = Main.npc[newguy];
            newguy3 = newguy2.ModNPC as CopperArmorPiece;
            newguy3.attachedID = NPC.whoAmI;
            newguy2.ai[0] = 450f;
            newguy2.ai[1] = 16f;
            newguy2.ai[2] = -64f;
            newguy2.lifeMax = (int)NPC.lifeMax * 1;
            newguy2.life = (int)NPC.lifeMax * 1;
            newguy2.knockBackResist = 0.2f;
            newguy2.netUpdate = true;

            //New Bow 2
            newguy = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X + 400, (int)NPC.position.Y, ModContent.NPCType<CopperArmorBow>());
            newguy2 = Main.npc[newguy];
            newguy3 = newguy2.ModNPC as CopperArmorPiece;
            newguy3.attachedID = NPC.whoAmI;
            newguy2.ai[1] = -16f;
            newguy2.ai[2] = -64f;
            newguy2.lifeMax = (int)NPC.lifeMax * 1;
            newguy2.life = (int)NPC.lifeMax * 1;
            newguy2.knockBackResist = 0.2f;
            newguy2.netUpdate = true;

            //New Chainmail

            newguy = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X, (int)NPC.position.Y - 10, level > 0 ? ModContent.NPCType<CobaltArmorChainmail>() : ModContent.NPCType<CopperArmorChainmail>());
            newguy2 = Main.npc[newguy];
            newguy3 = newguy2.ModNPC as CopperArmorPiece;
            newguy3.attachedID = NPC.whoAmI;
            newguy2.ai[0] = 450f;
            //newguy2.ai[1] = 16f;
            //newguy2.ai[2] = -64f;
            newguy2.lifeMax = (int)(NPC.lifeMax * 1.5f);
            newguy2.life = (int)(NPC.lifeMax * 1.5f);
            newguy2.knockBackResist = 1f;
            newguy2.netUpdate = true;
        }
        public override void AI()
        {
            float speedmulti = 0.75f;
            if (GetType() == typeof(CopperWraith))
            {
                if (!Main.expertMode || Main.masterMode) speedmulti = 0.5f;
                //if (SGAWorld.NightmareHardcore > 0) speedmulti = 1f;
            }

            if (GetType() == typeof(CobaltWraith.CobaltWraith))
            {
                speedmulti = 1.25f;
                if (!Main.expertMode || Main.masterMode) speedmulti = 1f;
                //if (SGAWorld.NightmareHardcore > 0) speedmulti = 1.4f;
            }

            Player P = Main.player[NPC.target];
            if (NPC.target < 0 || NPC.target == 255 || P.dead || !P.active)
            {
                NPC.TargetClosest(false);
                P = Main.player[NPC.target];
                if (!P.active || P.dead)
                {
                    NPC.active = false;
                    Main.npc[(int)NPC.ai[1]].active = false;
                }
            }
            else
            {
                int expert = 0;
                if (Main.expertMode || Main.masterMode) expert = 1;
                NPC.ai[0]++;
                if (NPC.type == ModContent.NPCType<CobaltWraith.CobaltWraith>()) level = 1;
                NPC.defense = (int)(((NPC.CountNPCS(ModContent.NPCType<CopperArmorChainmail>())) * 6) + (NPC.CountNPCS(ModContent.NPCType<CopperArmorGreaves>())) * 3 + (NPC.CountNPCS(ModContent.NPCType<CopperArmorHelmet>()) * 4) * ((expert + 1) * 0.4f));
                if (NPC.CountNPCS(ModContent.NPCType<CopperArmorChainmail>()) + NPC.CountNPCS(ModContent.NPCType<CobaltWraith.CobaltArmorChainmail>()) < 1)
                {
                    
                        if (NPC.ai[0] > 50)
                            NPC.ai[0] = -500;

                }
                if(NPC.life < NPC.lifeMax * 0.5f)
                {
                    if (expert > 0)
                    {
                        if (NPC.ai[0] > -1500 && NPC.ai[1] == 0) { NPC.ai[0] = -2000; NPC.ai[1] = 1; }
                        if (NPC.ai[0] == -1850)
                        {
                            
                            List<Projectile> itz = Idglib.Shattershots(NPC.position, NPC.position + new Vector2(0, 200), Vector2.Zero, ProjectileID.DD2BetsyArrow, 10, 5, 360, 20, true, 0, true, 150);
                          
                            for (int f = 0; f < 20; f++)
                            {
                                itz[f].aiStyle = 0;
                                itz[f].rotation = -((float)Math.Atan2(itz[f].velocity.X, itz[f].velocity.Y));
                            }
                            SpawnMoreExpert();
                        }
                        if (NPC.ai[0] == -1800) NPC.ai[0] = 0;
                    }
                }
                if ((NPC.ai[0] == 1 || NPC.ai[0] == -1) && NPC.ai[1] < 1)
                {
                    float mul = (NPC.ai[0] < 0 ? 0.10f : 0.45f);
                    if(NPC.CountNPCS(ModContent.NPCType<CopperArmorChainmail>()) < 1) //oh boy here we go again
                    {
                        int newguy = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y - 10, level > 0 ? ModContent.NPCType<CobaltArmorChainmail>() : ModContent.NPCType<CopperArmorChainmail>());
                        NPC newguy2 = Main.npc[newguy];
                        CopperArmorPiece newguy3 = newguy2.ModNPC as CopperArmorPiece;
                        newguy3.attachedID = NPC.whoAmI;
                        newguy2.lifeMax = (int)NPC.lifeMax * 2;
                        newguy2.life = (int)(NPC.lifeMax * (2 * mul));
                        newguy2.knockBackResist = 0.85f;
                        newguy2.netUpdate = true;
                    }
                    if (NPC.CountNPCS(ModContent.NPCType<CopperArmorSword>()) < 1)
                    {
                        int newguy = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int) NPC.Center.Y - 10, level > 0 ? ModContent.NPCType<CobaltArmorSword>() : ModContent.NPCType<CopperArmorSword>());
                        NPC newguy2 = Main.npc[newguy];
                        CopperArmorPiece newguy3 = newguy2.ModNPC as CopperArmorPiece;
                        newguy3.attachedID = NPC.whoAmI;
                        newguy2.ai[1] = -32f;
                        newguy2.ai[2] = -16f;
                        newguy2.lifeMax = (int)(NPC.lifeMax * 1f);
                        newguy2.life = (int)(NPC.lifeMax * 1f);
                        newguy2.knockBackResist = 0.75f;
                        newguy2.netUpdate = true;
                    }
                    if (NPC.CountNPCS(ModContent.NPCType<CopperArmorHelmet>()) < 1)
                    {
                        int newguy = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y - 10, level > 0 ? ModContent.NPCType<CobaltArmorHelmet>() : ModContent.NPCType<CopperArmorHelmet>());
                        NPC newguy2 = Main.npc[newguy];
                        CopperArmorPiece newguy3 = newguy2.ModNPC as CopperArmorPiece;
                        newguy3.attachedID = NPC.whoAmI;
                        newguy2.ai[2] = -12f;
                        newguy2.lifeMax = (int)(NPC.lifeMax * 1.5f);
                        newguy2.life = (int)(NPC.lifeMax * (1.5f * mul));
                        newguy2.knockBackResist = 0.8f;
                        newguy2.netUpdate = true;
                    }
                    if(NPC.CountNPCS(ModContent.NPCType<CopperArmorGreaves>()) < 1)
                    {
                        int newguy = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y - 10, level > 0 ? ModContent.NPCType<CobaltArmorGreaves>() : ModContent.NPCType<CopperArmorGreaves>());
                        NPC newguy2 = Main.npc[newguy];
                        CopperArmorPiece newguy3 = newguy2.ModNPC as CopperArmorPiece;
                        newguy3.attachedID = NPC.whoAmI;
                        newguy2.ai[2] = 12f;
                        newguy2.lifeMax = (int)(NPC.lifeMax * 1.5f);
                        newguy2.life = (int)(NPC.lifeMax * (1.5f * mul));
                        newguy2.knockBackResist = 0.8f;
                        newguy2.netUpdate = true;

                    }
                    if(NPC.CountNPCS(ModContent.NPCType<CopperArmorBow>()) < 1)
                    {
                        int newguy = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y - 10, level > 0 ? ModContent.NPCType<CobaltArmorSword>() : ModContent.NPCType<CopperArmorBow>());
                        NPC newguy2 = Main.npc[newguy];
                        CopperArmorPiece newguy3 = newguy2.ModNPC as CopperArmorPiece;
                        newguy3.attachedID = NPC.whoAmI;
                        newguy2.ai[1] = 32f;
                        newguy2.ai[2] = -16f;
                        newguy2.lifeMax = (int)(NPC.lifeMax * 1f);
                        newguy2.life = (int)(NPC.lifeMax * 1f);
                        newguy2.knockBackResist = 0.75f;
                        newguy2.netUpdate = true;
                    }
                }
                if (NPC.ai[0] > 1)
                {
                    if (NPC.ai[0] % 600 < 250)
                    {
                        Vector2 itt = ((P.Center + OffsetPoints) - NPC.position); itt.Normalize();
                        NPC.velocity += itt * (speed * speedmulti);
                    }
                    NPC.velocity *= 0.98f;
                }
                if (NPC.ai[0] < 0 && NPC.ai[0] > -2000)
                {
                    if (NPC.ai[0] % 110 < -95)
                    {
                        NPC.velocity = new Vector2(Main.rand.Next(-20, 20), 0);
                        if (NPC.ai[0] % 10 == 0) Idglib.Shattershots(NPC.position, P.position, new Vector2(P.width, P.height), 100, 10, 8, 0, 1, true, 0, true, 300);
                    }
                    else
                    {
                        Vector2 itt = ((P.Center + OffsetPoints + new Vector2(0, -250)) - NPC.position);itt.Normalize();
                        float speedz = (float)level + 0.45f;
                        NPC.velocity += ((itt * speedz) * speedmulti);
                    }
                    float fric = 0.96f + ((float)level * 0.01f);
                    NPC.velocity *= fric;
                }
            }

        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return true;
        }
        private bool CanBeHitByPlayer(Player p)
        {
            return NPC.ai[0] < 0;
        }
        public override bool? CanBeHitByItem(Player player, Item item)
        {
            if (CanBeHitByPlayer(player)) return true;
            else return false;
        }
        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (CanBeHitByPlayer(Main.player[projectile.owner])  &&!(NPC.ai[0] < -1800 && NPC.ai[0] >= -1850) && projectile.type != ProjectileID.DeathLaser) return true;
            else return false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            int npcType = ModContent.NPCType<CopperArmorChainmail>();
            Vector2 drawpos = NPC.Center - Main.screenPosition;
            Color glowingColors1 = Main.hslToRgb((float)drawColor.R * 0.08f, (float)drawColor.G * 0.08f, (float)drawColor.B * 0.08f);
            Texture2D texture = ModContent.Request<Texture2D>("SGAmod/NPCs/Bosses/TPD").Value;
            spriteBatch.Draw(texture, drawpos, null, glowingColors1, NPC.spriteDirection + (NPC.ai[0] * 0.4f), new Vector2(16, 16), new Vector2(Main.rand.Next(1, 20) / 17f, Main.rand.Next(1, 20) / 17f), SpriteEffects.None, 0f);
            if (NPC.ai[0] > 0) return false;
            else
            {
                for (int a = 0; a < 30; a++)
                {
                    spriteBatch.Draw(texture, drawpos, null, glowingColors1, NPC.spriteDirection + (NPC.ai[0] * (1 - (a % 2) * 2)) * 0.4f, new Vector2(16, 16), new Vector2(Main.rand.Next(1, 100) / 17f, Main.rand.Next(1, 20) / 17f), SpriteEffects.None, 0f);
                }
                return true;
            }
        }
    }
}
