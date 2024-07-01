using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameInput;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;
using Idglibrary;
using Terraria.ModLoader.IO;
using Terraria.Graphics.Shaders;
using SGAmod.NPCs;
using Terraria.Utilities;

namespace SGAmod
{
    public partial class SGAPlayer : ModPlayer
    {
        protected void Expertise_SaveExpertise(ref TagCompound tag)
        {
            if (ExpertisePointsFromBosses != null)
            {
                tag["enemyvaluesTotal"] = ExpertisePointsFromBosses.Count;
                for (int i = 0; i < ExpertisePointsFromBosses.Count; i += 1)
                {
                    int value = ExpertisePointsFromBosses[i];
                    string tagname = "enemyvalues" + ((string)i.ToString());
                    tag[tagname] = value;
                    string tagname2 = "enemyvaluesPoints" + ((string)i.ToString());
                    tag[tagname2] = ExpertisePointsFromBossesPoints[i];
                    string tagname3 = "enemyvaluesModded" + ((string)i.ToString());
                    tag[tagname3] = ExpertisePointsFromBossesModded[i];
                }
            }
        }

        protected void Expertise_LoadExpertise(TagCompound tag)
        {

            ExpertiseCollected = tag.GetAsLong("ZZZExpertiseCollectedZZZ");
            long maybeExpertiseCollected = tag.GetAsLong("ZZZExpertiseCollectedTotalZZZ");
            ExpertiseCollectedTotal = maybeExpertiseCollected;

            ExpertisePointsFromBosses = new List<int>();
            ExpertisePointsFromBossesPoints = new List<int>();
            ExpertisePointsFromBossesModded = new List<string>();

            if (maybeExpertiseCollected < 1 || (!tag.ContainsKey("resetver")))
            {
                Expertise_GenerateNewBossList();
            }
            else
            {
                int maxx = tag.GetInt("enemyvaluesTotal");
                if (maxx < 1)
                {
                    Expertise_GenerateNewBossList();
                }
                else
                {
                    for (int i = 0; i < maxx; i += 1)
                    {
                        int v1 = tag.GetInt("enemyvalues" + ((string)i.ToString()));
                        int v2 = tag.GetInt("enemyvaluesPoints" + ((string)i.ToString()));
                        string v3 = tag.GetString("enemyvaluesModded" + ((string)i.ToString()));

                        ExpertisePointsFromBosses.Add(v1);
                        ExpertisePointsFromBossesPoints.Add(v2);
                        ExpertisePointsFromBossesModded.Add(v3);
                    }
                }

            }

        }

        public int? Expertise_FindBossEXP(int npcid, NPC npc)
        {
            int? found = -1;
            // int? foundpre = -1;

            if (npc != null)
            {
                if (npc.ModNPC != null)
                {
                    string thisName = npc.ModNPC.GetType().Name;

                    /*
                    if (npc.ModNPC.GetType() == typeof(SPinkyTrue))
                    {
                        thisName = typeof(SPinky).Name;
                    }

                    foundpre = ExpertisePointsFromBossesModded.FindIndex(x => (x == thisName));
                    //Main.NewText(foundpre);
                    //Main.NewText(npc.modNPC.GetType().Name);
                    if (foundpre != null && foundpre > -1)
                    {
                        bool condition = (npc.ModNPC.GetType() != typeof(SPinky) || !Main.expertMode);
                        if (condition)
                        {
                            return foundpre;
                        }
                    }
                    */
                }
            }


            if (npcid == NPCID.EaterofWorldsHead || npcid == NPCID.EaterofWorldsBody || npcid == NPCID.EaterofWorldsTail)
            {
                found = ExpertisePointsFromBosses.FindIndex(x => (x == NPCID.EaterofWorldsHead));
                goto gohere;
            }
            if (npcid == NPCID.DD2DarkMageT1 || npcid == NPCID.DD2DarkMageT3)
            {
                found = ExpertisePointsFromBosses.FindIndex(x => (x == NPCID.DD2DarkMageT1));
                goto gohere;
            }
            if (npcid == NPCID.BigMimicCorruption || npcid == NPCID.BigMimicCrimson ) // || npcid == ModContent.NPCType<NPCs.Dank.SwampBigMimic>()) TODO
            {
                found = ExpertisePointsFromBosses.FindIndex(x => (x == NPCID.BigMimicCorruption));
                goto gohere;
            }
            if (npcid == NPCID.DD2OgreT2 || npcid == NPCID.DD2OgreT3)
            {
                found = ExpertisePointsFromBosses.FindIndex(x => (x == NPCID.DD2OgreT2));
                goto gohere;
            }
            if (npcid == NPCID.GoblinSorcerer || npcid == NPCID.GoblinPeon || npcid == NPCID.GoblinThief || npcid == NPCID.GoblinWarrior || npcid == NPCID.GoblinArcher)
            {
                found = ExpertisePointsFromBosses.FindIndex(x => (x == NPCID.GoblinPeon));
                goto gohere;
            }

            found = ExpertisePointsFromBosses.FindIndex(x => x == npcid);

        gohere:

            return found;
        }

        public void Expertise_DoExpertiseCheck(NPC npc, bool tempc = false)
        {
            if (tempc == false)
            {
                if (npc == null)
                    return;
                if (!npc.active)
                    return;
                if (npc.lifeMax < 100)
                    return;
            }
            if (ExpertisePointsFromBosses == null)
            {
                Main.NewText("SGAmod: The Expertise enemy list was somehow null... HOW?!");
                return;
            }

            if (ExpertisePointsFromBosses.Count < 1)
                return;

            int npcid = npc.type;

            int? found = Expertise_FindBossEXP(npcid, npc);

            if (found != null && found > -1)
            {
                int collected = ExpertisePointsFromBossesPoints[(int)found];
                if (Main.expertMode)
                {
                    /*
                    if (SGAWorld.NightmareHardcore > 0)
                        collected = (int)(collected * ((SGAWorld.NightmareHardcore == 1 ? 1.25f : 1.40f) + (NoHitCharm ? 1.25f : 0)));
                    */
                }
                else
                {
                    collected = (int)(collected * 0.80);
                }

                Expertise_AddExpertise(collected);

                ExpertisePointsFromBosses.RemoveAt((int)found);
                ExpertisePointsFromBossesPoints.RemoveAt((int)found);
                ExpertisePointsFromBossesModded.RemoveAt((int)found);

                int? findagain = Expertise_FindBossEXP(npcid, npc);

                if (findagain == null || findagain < 0)
                {
                    if (Main.myPlayer == Player.whoAmI)
                    {
                        Main.NewText("You have gained Expertise! (You now have " + ExpertiseCollected + ")");
                    }
                }
            }
        }

        public void Expertise_AddExpertise(int ammount)
        {
            ExpertiseCollected += ammount;
            ExpertiseCollectedTotal += ammount;

            CombatText.NewText(new Rectangle((int)Player.position.X, (int)Player.position.Y, Player.width, Player.height), Color.LimeGreen, "+" + ammount + " Expertise", false, false);
        }

        public void Expertise_AddToList(int value, int s2ndvalue)
        {
            ExpertisePointsFromBosses.Add(value);
            ExpertisePointsFromBossesPoints.Add(s2ndvalue);
            ExpertisePointsFromBossesModded.Add("");
        }
        public void Expertise_AddToListModded(string value, int s2ndvalue)
        {
            ExpertisePointsFromBosses.Add(-1);
            ExpertisePointsFromBossesPoints.Add(s2ndvalue);
            ExpertisePointsFromBossesModded.Add(value);
        }



        public void Expertise_GenerateNewBossList()
        {

            //Prehardmode Bosses

            Expertise_AddToListModded("CopperWraith", 100);

            Expertise_AddToList(NPCID.KingSlime, 100);

            Expertise_AddToList(NPCID.EyeofCthulhu, 100);

            // addtolistmodded("CaliburnGuardian", 75);

            for (int i = 0; i < 50; i += 1)
            {
                Expertise_AddToList(NPCID.EaterofWorldsHead, 3);
            }

            // addtolistmodded("CaliburnGuardian", 100);

            Expertise_AddToList(NPCID.BrainofCthulhu, 150);

            Expertise_AddToList(NPCID.QueenBee, 150);

            Expertise_AddToListModded("SpiderQueen", 250);

            // addtolistmodded("CaliburnGuardian", 125);

            Expertise_AddToList(NPCID.SkeletronHead, 200);

            Expertise_AddToList(NPCID.Deerclops, 200);

            // addtolistmodded("BossFlyMiniboss1", 200);

            // addtolistmodded("Murk", 300);

            Expertise_AddToList(NPCID.WallofFlesh, 500);


            //Hardmode Bosses

            Expertise_AddToListModded("CobaltWraith", 300);
            // addtolistmodded("Cirno", 300);
            Expertise_AddToList(NPCID.QueenSlimeBoss, 300);
            Expertise_AddToList(NPCID.TheDestroyer, 300);
            Expertise_AddToList(NPCID.SkeletronPrime, 300);
            Expertise_AddToList(NPCID.Spazmatism, 150);
            Expertise_AddToList(NPCID.Retinazer, 150);
            // addtolistmodded("SharkvernHead", 500);
            Expertise_AddToList(NPCID.Plantera, 600);//2600
            // addtolistmodded("Cratrosity", 700);
            Expertise_AddToList(NPCID.Golem, 400);
            Expertise_AddToList(NPCID.HallowBoss, 600);
            Expertise_AddToList(NPCID.DukeFishron, 600);
            Expertise_AddToList(NPCID.DD2Betsy, 700);
            Expertise_AddToList(NPCID.CultistBoss, 500);//5000
            // addtolistmodded("TPD", 800);
            // addtolistmodded("SpaceBoss", 700);
            Expertise_AddToList(NPCID.LunarTowerNebula, 200);
            Expertise_AddToList(NPCID.LunarTowerSolar, 200);
            Expertise_AddToList(NPCID.LunarTowerStardust, 200);
            Expertise_AddToList(NPCID.LunarTowerVortex, 200);
            Expertise_AddToList(NPCID.MoonLordCore, 1000);//8500

            //Post-moonlord Bosses

            // addtolistmodded("LuminiteWraith", 1500);
            // addtolistmodded("SPinky", 1500);
            // addtolistmodded("Cratrogeddon", 1500);
            // addtolistmodded("Hellion", 3000);

            //Not bosses
            for (int i = 0; i < 75; i += 1)
            {
                Expertise_AddToList(NPCID.GoblinPeon, 2);
            }

            Expertise_AddToList(NPCID.Pinky, 25);
            Expertise_AddToList(NPCID.Tim, 50);
            Expertise_AddToList(NPCID.DoctorBones, 50);
            Expertise_AddToList(NPCID.Nymph, 50);
            Expertise_AddToList(NPCID.TheGroom, 25);
            Expertise_AddToList(NPCID.TheBride, 25);
            Expertise_AddToList(NPCID.DD2DarkMageT1, 75);
            // addtolistmodded("TidalElemental", 75);

            //Not bosses: Hardmode (+3000 total)
            for (int i = 0; i < 2; i += 1)//1050
            {
                Expertise_AddToList(NPCID.GoblinSummoner, 25);
                Expertise_AddToList(NPCID.Mothron, 50);
                Expertise_AddToList(NPCID.Mimic, 25);
                Expertise_AddToList(NPCID.Medusa, 50);
                Expertise_AddToList(NPCID.IceGolem, 50);
                Expertise_AddToList(NPCID.SandElemental, 50);
                Expertise_AddToList(NPCID.MartianSaucerCore, 150);
                Expertise_AddToList(NPCID.PirateShip, 75);
                Expertise_AddToList(NPCID.PirateCaptain, 50);
            }

            Expertise_AddToList(NPCID.BigMimicCorruption, 100);
            Expertise_AddToList(NPCID.BigMimicHallow, 100);
            Expertise_AddToList(NPCID.BigMimicJungle, 100);
            Expertise_AddToList(NPCID.Clown, 25);
            Expertise_AddToList(NPCID.RainbowSlime, 25);
            // addtolistmodded("EliteBat", 50);
            Expertise_AddToList(NPCID.PresentMimic, 25);
            Expertise_AddToList(NPCID.RuneWizard, 50);
            Expertise_AddToList(NPCID.Moth, 50);
            Expertise_AddToList(NPCID.DD2OgreT2, 50);
            // addtolistmodded("PrismBanshee", 300);

            for (int i = 0; i < 3; i += 1)
            {
                Expertise_AddToList(NPCID.MourningWood, 50);
                Expertise_AddToList(NPCID.Pumpking, 100);
                Expertise_AddToList(NPCID.Everscream, 50);
                Expertise_AddToList(NPCID.SantaNK1, 75);
                Expertise_AddToList(NPCID.IceQueen, 125);
            }


            for (int i = 0; i < 100; i += 1)
            {
                // ignore this, it's filler to keep the list from running out
                Expertise_AddToList(NPCID.CultistArcherWhite, 1);
            }
        }
    }
}