using Idglibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Core.Utils;
using SGAmod.Buffs.Debuffs;
using SGAmod.Items.Materials.BossDrops;
using SGAmod.NPCs.Bosses.CopperWraith;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.NPCs.Bosses.CobaltWraith
{
    public class CobaltArmorChainmail : CopperArmorChainmail
    {
        public int armortype = ItemID.CobaltBreastplate;
        public int mode = 0;

        public override void SetDefaults()
        {
            NPC.width = 32;
            NPC.height = 32;
            NPC.damage = 0;
            NPC.defense = 5;
            NPC.lifeMax = 500;
            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath7;
            NPC.value = 0f;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            AIType = -1;
            AnimationType = 0;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            CopperArmorPiece myself = NPC.ModNPC as CopperArmorPiece;
            myself.friction /= 8;
            myself.speed = 0.54f;
            NPC.buffImmune[BuffID.Daybreak] = true;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CobaltWraithNotch>(), 5));
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

        public virtual void CobaltFloat(NPC myowner)
        {
            NPC.velocity = NPC.velocity + (myowner.Center + new Vector2((float)NPC.ai[1], (float)NPC.ai[2]) - NPC.Center) * speed;
            NPC.velocity *= 0.5f;
            NPC.rotation = (float)NPC.velocity.X * 0.1f;
            NPC.timeLeft = 999;
        }
        public override string Texture => "Terraria/Images/Item_" + armortype;

        public override void AI()
        {
            CopperArmorPiece myself = NPC.ModNPC as CopperArmorPiece;
            NPC myowner = Main.npc[myself.attachedID];
            if (myowner.active == false)
            {
                myself.ArmorMalfunction();
            }
            else
            {
                CobaltFloat(myowner);
            }
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (Main.expertMode || Main.masterMode)
            {
                double damagemul = 1.0;
                if (projectile.penetrate > 1)
                    damagemul = 0.8;
                if (projectile.penetrate > 2 || projectile.penetrate < 0)
                    damagemul = 0.6;
                modifiers.FinalDamage.Base *= (float)damagemul;
            }
        }
        
    }
    public class CobaltArmorHelmet : CobaltArmorChainmail
    {
        public int armortype = ItemID.CobaltHelmet;

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
    }
    public class CobaltArmorGreaves : CobaltArmorChainmail
    {
        public int armortype = ItemID.CobaltHelmet;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }
        public override string Texture =>"Terraria/Images/Item_" + armortype ;

        
    }
    public class CobaltArmorBricks : CobaltArmorChainmail
    {
        public int armortype = ItemID.CobaltBrickWall;

        public override void SetDefaults()
        {
            base.SetDefaults();
            NPC.width = 48;
            NPC.height = 48;
            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath7;
            CopperArmorPiece myself = NPC.ModNPC as CopperArmorPiece;
            myself.friction = 0.95f;
            myself.speed = 0.04f;
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

    } 
    public class CobaltBrickArmorNexus : CobaltArmorChainmail
    {
        public int armortype = ItemID.CobaltBrick;
        public int madeturrets = 0;
        public int laserblast = -1000;

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
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(mode);
            
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            mode = reader.ReadInt32();
        }
        public override string Texture => "Terraria/Images/Item_" + armortype;
        public override void CobaltFloat(NPC myowner)
        {
            if (mode == 0)
            {
                base.CobaltFloat(myowner);
                return;
            }
            NPC ownerz = Main.npc[NPC.FindFirstNPC(ModContent.NPCType<CobaltWraith>())];
            Player p = Main.player[ownerz.target];

            NPC.velocity = NPC.velocity + (new Vector2(myowner.Center.X + (mode * 48f), p.Center.Y - 480) - NPC.Center) * speed;
            NPC.velocity /= 2f;
            NPC.rotation += Math.Sign(mode) * 0.1f;
            NPC.timeLeft = 999;
        }
        public override void AI()
        {
            base.AI();
            madeturrets++;
            if (laserblast == -1000)
                laserblast = (int)Main.rand.Next(0, 50) - 300;
            int findthem = NPC.FindFirstNPC(ModContent.NPCType<CobaltWraith>());
            if (findthem >= 0 && findthem < Main.maxNPCs)
            {
                NPC ownerz = Main.npc[findthem];
                if((ownerz.ModNPC as CobaltWraith).raged == true)
                {
                    laserblast++;
                    Player p = Main.player[ownerz.target];
                    if (laserblast % 20 == 0 && laserblast % 400 > 200 && laserblast > 0 && p != null && (Main.expertMode || Main.masterMode) && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        if(mode == 0)
                        {

                        }
                        else
                        {
                            Idglib.Shattershots(NPC.Center, NPC.Center + Vector2.UnitY, Vector2.Zero, ProjectileID.WaterBolt, 20, 30, 0, 1, true, 0, true, 100);
                        }
                    }
                }
            }
            if (madeturrets == 5)
            {
                int nexus = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X, (int)NPC.position.Y - 24, Main.rand.Next(0, 100) < 50 && mode == 0 ? ModContent.NPCType<CobaltArmorBow>() : ModContent.NPCType<CobaltChainSawPiece>());
                NPC armpiece = Main.npc[nexus];
                CopperArmorPiece newguy3 = armpiece.ModNPC as CopperArmorPiece;
                newguy3.attachedID = NPC.whoAmI;

                armpiece.lifeMax = (int)(NPC.lifeMax * 0.75f);
                armpiece.ai[1] = -16;
                armpiece.ai[1] = -0f;
                armpiece.life = (int)(NPC.lifeMax * (0.75f));
                armpiece.knockBackResist = 1f;
                armpiece.netUpdate = true;
            }
        }
    }
    public class CobaltArmorBow : CobaltArmorChainmail
    {
        public int armortype  = ItemID.CobaltRepeater;

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
            base.SetDefaults();
            NPC.width = 24;
            NPC.height = 24;
            NPC.damage = 0;
            NPC.defense = 5;
            NPC.lifeMax = 500;
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
            CopperArmorPiece myself = NPC.ModNPC as CopperArmorPiece;
            NPC myowner = Main.npc[myself.attachedID];
            if(myowner.active == false) 
            {
                myself.ArmorMalfunction();
            }
            else
            {
                Player p = Main.player[NPC.target];
                if (NPC.target < 0 || NPC.target == 255 || p.dead || !p.active)
                {
                    NPC.TargetClosest(false);
                    Main.npc[(int)NPC.ai[1]].active = false;
                }
                else
                {
                    NPC.ai[0]++;
                    Vector2 itt = (myowner.Center - NPC.Center + new Vector2(NPC.ai[1] * NPC.spriteDirection, NPC.ai[2]));
                    if (NPC.ai[0] % 1500 > 1250)
                        itt = p.position - NPC.position + new Vector2(3f * NPC.ai[1] * NPC.spriteDirection, NPC.ai[2] * 2f);
                    float locspeed = 0.25f;
                    if (NPC.ai[0] % 900 > 550)
                    {
                        Vector2 cas = new Vector2(NPC.position.X - p.position.X, NPC.position.Y - p.position.Y);
                        double dist = cas.Length();
                        float rotation = (float)Math.Atan2(NPC.position.Y - p.position.Y - (dist * 0.05f) + (p.height * 0.5f) , NPC.position.X - p.position.X + (p.width * 0.5f));
                        NPC.rotation = rotation;
                        NPC.velocity = NPC.velocity * 0.86f;
                        if (NPC.ai[0] % 20 == 0 && NPC.ai[0] % 900 > 650)
                        {
                            int arrowtype = ProjectileID.WoodenArrowHostile;
                            List<Projectile> one = Idglib.Shattershots(NPC.Center, NPC.Center + new Vector2(-15 * NPC.spriteDirection, 0), Vector2.Zero, arrowtype, 20,20, 0, 1, true, (Main.rand.Next(-100,100) * 0.000f) - NPC.rotation, true, 300);
                            one[0].hostile = true;
                            one[0].friendly = false;
                            one[0].localAI[0] = p.whoAmI;
                            one[0].netUpdate = true;
                        }
                        NPC.spriteDirection = 1;
                    }
                    else
                    {
                        if (Math.Abs(NPC.velocity.X) > 2) NPC.spriteDirection = NPC.velocity.X > 0 ? -1 : 1;
                        NPC.rotation = (float)NPC.velocity.X * 0.09f;
                        locspeed = 0.5f;
                    }
                    NPC.velocity *= 0.96f;
                    itt.Normalize();
                    NPC.velocity = NPC.velocity + (itt * locspeed);
                    NPC.timeLeft = 999;
                }
            }
        }
    }
    public class CobaltChainSawPiece : CobaltArmorChainmail
    {
        public int armortype = ItemID.CobaltChainsaw;

        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
            Main.npcFrameCount[NPC.type] = 1;
        }
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.CobaltChainsaw;
        public override void SetDefaults()
        {
            base.SetDefaults();
            NPC.width = 24;
            NPC.height = 24;
            NPC.damage = 26;
            NPC.defDamage = 6;
            NPC.defense = 5;
            NPC.lifeMax = 500;
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
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return NPC.ai[0] % 300 > 150;
        }
        public override void AI()
        {
            CopperArmorPiece myself = NPC.ModNPC as CopperArmorPiece;
            NPC myowner = Main.npc[myself.attachedID];
            if (myowner.active == false)
                myself.ArmorMalfunction();
            else
            {
                Player p = Main.player[NPC.target];
                if (NPC.target < 0 || NPC.target == 255 || p.dead || !p.active)
                {
                    NPC.TargetClosest(false);
                    p = Main.player[NPC.target];
                    if (!p.active || p.dead)
                    {
                        NPC.active = false;
                        Main.npc[(int)NPC.ai[1]].active = false;
                    }
                    
                }
                else
                {
                    NPC.ai[0] = NPC.ai[0] + 1;
                    if (NPC.ai[0] % 300 < 150)
                        NPC.velocity = NPC.velocity + (myowner.Center + new Vector2((float)NPC.ai[1], (float)NPC.ai[2]) - NPC.Center) / 50;
                    else
                        NPC.velocity = NPC.velocity + (p.Center - NPC.Center) * 0.02f;
                    NPC.velocity *= 0.5f;
                    Vector2 diff = NPC.Center - myowner.Center;
                    diff.Normalize();
                    NPC.position = NPC.position + (diff * 5);

                    NPC.rotation = (float)Math.Atan2(NPC.position.Y - myowner.position.Y + (myowner.height * 0.5f), NPC.position.X - myowner.position.X + (myowner.width * 0.5f)) + 90f;

                    NPC.timeLeft = 999;
                }
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if ((Main.expertMode || Main.masterMode) || Main.rand.NextBool(2))
                target.AddBuff(BuffID.Bleeding, 60 * 10, true);
            if ((Main.expertMode || Main.masterMode) || Main.rand.NextBool(4))
                target.AddBuff(ModContent.BuffType<MassiveBleeding>(), 60 * 4, true);
        }
    }

    public class CobaltArmorSword : CopperArmorChainmail
    {
        public int armortype = ItemID.CobaltSword;

        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }
        public override string Texture => "Terraria/Images/Item_" + armortype;
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CobaltWraithNotch>(), 8));
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            NPC.width = 24;
            NPC.height = 24;
            NPC.damage = 20;
            NPC.defDamage = 20;
            NPC.lifeMax = 500;
            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath7;
            NPC.value = 0f;
            NPC.knockBackResist = -0.5f;
            NPC.aiStyle = -1;
            AIType = -1;
            AnimationType = 0;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
        }
        public override void AI()
        {
            CopperArmorPiece myself = NPC.ModNPC as CopperArmorPiece;
            NPC myowner = Main.npc[myself.attachedID];
            if (myowner.active == false)
                myself.ArmorMalfunction();
            else
            {
                Player p = Main.player[NPC.target];
                NPC.ai[0]++;
                NPC.spriteDirection = NPC.velocity.X > 0 ? -1 : 1;
                Vector2 itt = myowner.Center - NPC.Center + new Vector2(NPC.ai[1] * NPC.spriteDirection, NPC.ai[2]);
                float locspeed = 0.25f;
                if (NPC.ai[0] % 600 > 350)
                {
                    NPC.damage = (int)NPC.defDamage * 3;
                    itt = itt = p.position - NPC.position + new Vector2(NPC.ai[1] * -NPC.spriteDirection, -8);
                    /*if (NPC.ai[0] % 160 == 0 && SGAmod.DRMMode)
               {
                   Vector2 zxx = itt;
                   itt += p.velocity * 3f;
                   zxx.Normalize();
                   NPC.velocity += (zxx) * 18f;
               }*/
                    NPC.rotation = NPC.rotation + (0.65f * NPC.spriteDirection);
                    locspeed = 0.5f;
                }
                else
                {
                    NPC.damage = (int)NPC.defDamage;
                    if (NPC.ai[0] % 300 < 60)
                    {
                        locspeed = 1.0f;
                        NPC.velocity *= 0.92f;
                    }
                    NPC.rotation = (float)NPC.velocity.X * 0.09f;
                    locspeed = 0.7f;
                }
                NPC.velocity *= 0.96f;
                itt.Normalize();
                NPC.velocity = NPC.velocity + (itt * locspeed);
                NPC.timeLeft = 999;


            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if ((Main.expertMode || Main.masterMode) || Main.rand.Next(2) == 0)
            {
                target.AddBuff(BuffID.Bleeding, 60 * 10, true);
            }
            if (Main.expertMode || Main.masterMode || Main.rand.Next(4) == 0) 
            {
                target.AddBuff(BuffID.BrokenArmor, 60 * 8, true);
            }
        }
    }
    [AutoloadBossHead]
    public class CobaltWraith : CopperWraith.CopperWraith, ISGABoss
    {
        bool madearmor = false;
        public bool raged = false;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.NeedsExpertScaling[NPC.type] = true;
            NPCID.Sets.MPAllowedEnemies[NPC.type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                CustomTexturePath = "SGAmod/NPCs/Bosses/CobaltWraith/CobaltWraithLog",
                PortraitScale = 1f,

            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }
        public override void SetDefaults()
        {
            NPC.width = 16;
            NPC.height = 16;
            NPC.damage = 40;
            NPC.defense = 0;
            NPC.lifeMax = 2000;
            NPC.HitSound = SoundID.NPCHit5;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.knockBackResist = 0.05f;
            NPC.aiStyle = -1;
            NPC.boss = true;
            Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Copperig");
            AnimationType = 0;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            CopperWraith.CopperWraith myself = NPC.ModNPC as CopperWraith.CopperWraith;
            myself.level = 1;
            NPC.buffImmune[BuffID.Daybreak] = true;
            NPC.value = Item.buyPrice(0, 1, 0, 0);
        }
        public override string Texture => "SGAmod/NPCs/Bosses/TPD";

        public override string BossHeadTexture => "SGAmod/NPCs/Bosses/CobaltWraith/CobaltWraith_Head_Boss";

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.625f * balance);
            NPC.damage = (int)(NPC.damage * 0.6f);
        }
        public override void SpawnMoreExpert()
        {
            Idglib.Chat("Turn.... baacckk...", 5, 5, 60);
            int newguy = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X + 400, (int)NPC.position.Y, level > 0 ? ModContent.NPCType<CobaltArmorChainmail>() : ModContent.NPCType<CopperArmorChainmail>());
            NPC newguy2 = Main.npc[newguy];
            CopperArmorPiece newguy3 = newguy2.ModNPC as CopperArmorPiece;
            newguy3.attachedID = NPC.whoAmI;
            newguy2.lifeMax = (int)(NPC.lifeMax * 2f);
            newguy2.life = (int)(NPC.lifeMax * 2f);
            NPC.knockBackResist = 1f;
            Main.npc[newguy].width += 16;
            newguy2.height += 16;
            newguy2.netUpdate = true;
            raged = true;
            createarmorthings();
        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.HealingPotion;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CobaltWraithShard>(), 1, Main.expertMode ? 25 : 10, Main.expertMode ? 25 : 10));
        }

        public void createarmorthings()
        {
            for (float fx = -14f; fx < 15f; fx += 28)
            {
                int guy = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X + 0, (int)NPC.position.Y, ModContent.NPCType<CobaltArmorBricks>());
                NPC arm = Main.npc[guy];
                CopperArmorPiece guy3 = arm.ModNPC as CopperArmorPiece;
                guy3.attachedID = NPC.whoAmI;
                arm.lifeMax = (int)(NPC.lifeMax * 1.25f);
                arm.ai[1] = -fx;
                arm.ai[2] = 0;
                arm.life = (int)(NPC.lifeMax * 1.25f);
                arm.knockBackResist = 1f;
                int lastone = guy;
                arm.netUpdate = true;

                for (int countz = 1;  countz < 4; countz++) 
                {
                    int nexus = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<CobaltBrickArmorNexus>());
                    NPC armpiece = Main.npc[nexus];
                    guy3 = armpiece.ModNPC as CopperArmorPiece;
                    guy3.attachedID = NPC.whoAmI;
                    armpiece.lifeMax = (int)(NPC.lifeMax * 0.75f);
                    armpiece.ai[1] = -186 * (fx > 0 ? 1 : -1) / 2f;
                    armpiece.ai[2] = -24f;
                    armpiece.life = (int)(NPC.lifeMax * 0.75f);
                    armpiece.knockBackResist = 1f;
                    (armpiece.ModNPC as CobaltArmorChainmail).mode = (int)(fx * (countz+1));
                    armpiece.netUpdate = true;
                }

                for (float fx2 = 5f; fx2 < 35f; fx2 += 12f)
                {
                    int leg = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<CobaltArmorBricks>());
                    NPC legpiece = legpiece = Main.npc[leg];
                    guy3 = legpiece.ModNPC as CopperArmorPiece;
                    guy3.attachedID = lastone;
                    guy3.speed /= (1 + (fx2 / 300));
                    lastone = leg;
                    legpiece.lifeMax = (int)(NPC.lifeMax * 0.75f);
                    legpiece.ai[1] = Main.rand.Next(-80, 80);
                    legpiece.ai[2] = Main.rand.Next(-80, 80);
                    legpiece.life = (int)(NPC.lifeMax * 0.75f);
                    legpiece.knockBackResist = 1f;
                    legpiece.netUpdate = true;

                    int nexus = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<CobaltBrickArmorNexus>());
                    legpiece = Main.npc[nexus];
                    guy3 = legpiece.ModNPC as CopperArmorPiece;
                    guy3.speed /= (1 + (fx2 / 200));
                    guy3.attachedID = leg;
                    legpiece.lifeMax = (int)(NPC.lifeMax * 0.75f);
                    legpiece.ai[1] = Main.rand.Next(-80, 80);
                    legpiece.ai[2] = Main.rand.Next(-80, 80);
                    legpiece.life = (int)(NPC.lifeMax * 0.75f);
                    legpiece.knockBackResist = 1f;
                    legpiece.netUpdate = true;
                }
            }
             
        }
        public override void AI()
        {
            Player p = Main.player[NPC.target];
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest(false);
                p = Main.player[NPC.target];
                if(!p.active || p.dead)
                {
                    NPC.active = false;
                    Main.npc[(int)NPC.ai[1]].active = false;
                }
            }
            else
            {
                if ((p.Center - NPC.Center).Length() < 700)
                    NPC.timeLeft = Math.Max(NPC.timeLeft, 500);
                base.AI();
                if (NPC.ai[0] > 10) NPC.ai[0]++;
                if (NPC.ai[0] == 1)
                {
                    if (madearmor == false)
                    {
                        madearmor = true;
                        for (float fx = -14f; fx < 15f; fx += 28)
                        {
                            int guy = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<CobaltArmorBricks>());
                            NPC arm = Main.npc[guy];
                            CopperArmorPiece guy3 = arm.ModNPC as CopperArmorPiece;
                            guy3.attachedID = NPC.whoAmI;
                            arm.lifeMax = (int)(NPC.lifeMax * 1.25f);
                            arm.ai[1] = -fx;
                            arm.ai[2] = 0f;
                            arm.life = (int)(NPC.lifeMax * 1.25f);
                            arm.knockBackResist = 1f;
                            arm.netUpdate = true;
                            int lastone = guy;

                            int nexus = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<CobaltBrickArmorNexus>());
                            NPC piece = Main.npc[nexus];
                            guy3 = piece.ModNPC as CopperArmorPiece;
                            guy3.attachedID = guy;
                            piece.lifeMax = (int)(NPC.lifeMax * 0.75f);
                            piece.ai[1] = -16 * (fx > 0 ? 1 : -1) / 2 ;
                            piece.ai[2] = -24f;
                            piece.life = (int)(NPC.lifeMax * 0.75f);
                            piece.knockBackResist = 1f;
                            piece.netUpdate = true;

                            for (float fx2 = 20f; fx2 < 75f; fx2 += 18f) 
                            {
                                int leg = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<CobaltArmorBricks>());
                                piece = Main.npc[leg];
                                guy3 = piece.ModNPC as CopperArmorPiece;
                                guy3.attachedID = lastone;
                                guy3.speed /= (1f + (fx2 / 300));
                                lastone = leg;
                                piece.lifeMax = (int)(NPC.lifeMax * 0.75f);
                                piece.ai[1] = Main.rand.Next(-80, 80);
                                piece.ai[2] = Main.rand.Next(-80, 80);
                                piece.life = (int)(NPC.lifeMax * 0.75f);
                                piece.knockBackResist = 1f;
                                piece.netUpdate = true ;

                                nexus = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<CobaltBrickArmorNexus>());
                                piece = Main.npc[nexus];
                                guy3 = piece.ModNPC as CopperArmorPiece;
                                guy3.speed /= (1 + (fx2 / 200));
                                guy3.attachedID = leg;
                                piece.lifeMax = (int)(NPC.lifeMax * 0.75f);
                                piece.ai[1] = Main.rand.Next(-80, 80);
                                piece.ai[2] = Main.rand.Next(-80, 80);
                                piece.life = (int)(NPC.lifeMax * 0.75f);
                                piece.knockBackResist = 1f;
                                piece.netUpdate = true ;
                            }
                        }
                    }
                }
            }
            
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            int npctype = ModContent.NPCType<CopperArmorChainmail>();
            Vector2 drawpos = NPC.Center - Main.screenPosition;
            Color glowcolor = Main.hslToRgb((int)drawColor.R * 0.08f, (float)drawColor.G * 0.08f, (float)drawColor.B * 0.08f);
            Texture2D texture = ModContent.Request<Texture2D>("SGAmod/NPCs/Bosses/TPD").Value;
            spriteBatch.Draw(texture, drawpos, null, glowcolor, NPC.spriteDirection + (NPC.ai[0] * 0.4f), new Vector2(16, 16), new Vector2(Main.rand.Next(1, 20) / 17f, Main.rand.Next(1, 20) / 17f), SpriteEffects.None, 0f);
            if (NPC.ai[0] > 0) return false;
            else
            {
                for (int a = 0; a < 30;  a++)
                {
                    spriteBatch.Draw(texture, drawpos, null, glowcolor, NPC.spriteDirection + (NPC.ai[0] * (1 - (a % 2) * 2)) * 0.4f, new Vector2(16, 16), new Vector2(Main.rand.Next(1, 100) / 17f, Main.rand.Next(1, 20) / 17f), SpriteEffects.None, 0f);
                }
                return true;
            }
        }
    }
}
