using System.Linq;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Idglibrary;
using System.IO;
using Steamworks;
using ReLogic.Content;
using Terraria.GameContent.Personalities;
using Terraria.GameContent.Bestiary;
//using SGAmod.Dimensions;

namespace SGAmod.NPCs.TownNPCs
{
	[AutoloadHead]
	public class Dergon : ModNPC
	{

		float walkframe = 0f;
		int confort = 0;
		public int PartyVictum => BirthdayParty.CelebratingNPCs.FirstOrDefault(type => type == NPC.whoAmI);

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 7;
			NPCID.Sets.ExtraFramesCount[NPC.type] = 0;
			NPCID.Sets.AttackFrameCount[NPC.type] = 0;
			NPCID.Sets.DangerDetectRange[NPC.type] = 700;
			NPCID.Sets.AttackType[NPC.type] = 0;
			NPCID.Sets.AttackTime[NPC.type] = 90;
			NPCID.Sets.AttackAverageChance[NPC.type] = 30;
			NPCID.Sets.HatOffsetY[NPC.type] = 4;

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new(0)
			{
				CustomTexturePath = Mod.Name + "/NPCs/TownNPCs/Dergon", // If the NPC is multiple parts like a worm, a custom texture for the Bestiary is encouraged.
				// Position = new Vector2(40f, 24f),
				// PortraitPositionXOverride = 0f,
				// PortraitPositionYOverride = 0f,
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
				Direction = 1
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			NPC.Happiness
				.SetBiomeAffection<ForestBiome>(AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Goat>(), AffectionLevel.Love)
			//Princess is automatically set
			; // < Mind the semicolon!
		}

		public override void SetDefaults()
		{
			NPC.townNPC = true;
			NPC.friendly = true;
			NPC.width = 32;
			NPC.height = 40;
			NPC.aiStyle = 7;
			NPC.damage = 10;
			NPC.defense = 15;
			NPC.lifeMax = 1000;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;
			AnimationType = NPCID.Guide;
			NPC.homeless = true;
			Color c = Main.hslToRgb((float)(Main.GlobalTimeWrappedHourly / 2) % 1f, 0.5f, 0.35f);
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("A dragon with a troubled past.")
			});
		}

		public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(confort);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
			confort = reader.ReadInt32();
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
			{
				for (int i = 0; i < 2; i++)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Wing").Type, 1f);
					Gore.NewGore(NPC.GetSource_Death(), NPC.Center + new Vector2(NPC.spriteDirection * -6f, 3f), NPC.velocity, Mod.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Leg").Type, 1f);
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.Center + new Vector2(NPC.spriteDirection * 8f, -2f), NPC.velocity, Mod.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Head").Type, 1f);
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs)
		{
			for (int gg = 0; gg < Main.maxPlayers; gg += 1)
			{

				if (Main.player[gg].active)
				{
					if (Main.player[gg].GetModPlayer<SGAPlayer>().ExpertiseCollectedTotal > 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		public override bool CheckConditions(int left, int right, int top, int bottom)
		{
			return true;
		}

		public override ITownNPCProfile TownNPCProfile()
		{
			return new DergonProfile();
		}

		public override List<string> SetNPCNameList()
		{
			return new List<string>()
			{
				"Draken"
			};
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (Math.Abs(NPC.velocity.Y) < 1)
			{
				Texture2D tex = TextureAssets.Npc[NPC.type].Value;
				Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 7) / 2f;
				Vector2 drawPos = ((NPC.Center - Main.screenPosition)) + new Vector2(0f, -12f);
				Color color = drawColor;
				int timing = (int)(walkframe);
				timing %= 6;

				if (Math.Abs(NPC.velocity.X) < 0.25f)
				{
					timing = 0;
					//drawPos.Y += 2f;
				}
				else
				{
					timing += 1;
					//if (timing > 0 && timing < 4)
						//drawPos.Y += 2f;
				}

				timing *= ((tex.Height) / 7);
				spriteBatch.Draw(tex, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height - 1) / 7), color, 0, drawOrigin, NPC.scale, NPC.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			}
			else
			{
				Texture2D tex = ModContent.Request<Texture2D>(Mod.Name + "/NPCs/TownNPCs/DrakenFly").Value;
				Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 4) / 2f;
				Vector2 drawPos = ((NPC.Center - Main.screenPosition)) + new Vector2(0f, -12f);
				Color color = drawColor;
				int timing = (int)(Main.GlobalTimeWrappedHourly * 10f);
				timing %= 4;

				if (timing == 0)
				{
					drawPos.Y -= 8f;
				}

				timing *= ((tex.Height) / 4);
				spriteBatch.Draw(tex, drawPos, new Rectangle(0, timing + 2, tex.Width, (tex.Height - 1) / 4), color, 0, drawOrigin, NPC.scale, NPC.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			}

			if (NPCID.Sets.NPCBestiaryDrawOffset.TryGetValue(Type, out NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers))
			{
				drawModifiers.Frame++;
				if (drawModifiers.Frame >= 7)
				{
					drawModifiers.Frame = 0;
				}
				// Replace the existing NPCBestiaryDrawModifiers with our new one with an adjusted rotation
				NPCID.Sets.NPCBestiaryDrawOffset.Remove(Type);
				NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
			}

			return false;
		}

		public override void AI()
		{
			if (Math.Abs(NPC.velocity.X) < 0.25f)
				walkframe = 0f;
			else
				walkframe += Math.Abs(NPC.velocity.X) * 0.15f;
		}

		public override string GetChat()
		{
			WeightedRandom<string> chat = new();

			SGAPlayer modplayer = Main.LocalPlayer.GetModPlayer<SGAPlayer>();
			long expgathered = Main.LocalPlayer.GetModPlayer<SGAPlayer>().ExpertiseCollectedTotal;

			/*
			if (SGAmod.TotalCheating && SGAmod.PlayingPercent >= 1f)
            {
				chat.Add("YOU KNOW WHAT YOU DID");
				chat.Add("REPENT");
				return chat.Get();
            }
			*/

			/*
			if (SGAPocketDim.WhereAmI == typeof(LimboDim))
			{
				chat.Add("Rk erb, hqfubswhg whaw!");
				chat.Add("VGhpcyBpcyBvbmx5IGhhbGYgdGhlIHN0ZXBzIHJlcXVpcmVk");
				chat.Add("WmtiIGR1aCBicngga2h1aD8=");
				chat.Add("EaIwnlOKMJWgnJkfnJ8fVUEbLKDtMzSaM290VUImMKZtpTIipTkyVTkyMaDtLJ5xVUWcM2u0");
				chat.Add("dWdnY2Y6Ly92enRoZS5wYnovbi8wZzdnaFFz");
				chat.Add("N Lrne bs Vaqrcraqrapr Qnl");
				chat.Add("Vg lzw eslz, xaymjw gml eq qwsj gx tajlz");
				chat.Add("emxsaGs6Ly9oc2tsd3RhZi51Z2UvMGxvNDdLdUg=");
				return chat.Get();
			}
			*/

			if (NPC.life < NPC.lifeMax * 0.5)
			{
				chat.Add("Please help! I don't want to die!", 15.0);
				chat.Add("It hurts, so much...", 15.0);
				chat.Add("(Loud Whimpering)", 15.0);
			}
			else
			{
				chat.Add("Um... Hi...");
				chat.Add("(Quiet Whimpering)");
				chat.Add("...");
				chat.Add("Is it safe out now?");

				if (expgathered < 200)
				{
					chat.Add("Hi... I'm Draken, I hope your friendly.");
				}
				if (expgathered > 800)
				{
					chat.Add("I feel safer with someone like you around.");
				}
				if (expgathered > 2000)
				{
					chat.Add("I hope all those you've slain were meant to harm us... I can't bear idea of innocents dying.");
				}

				int Tnpc1 = NPC.FindFirstNPC(NPCID.Dryad);
				if (Tnpc1 >= 0)
				{
					chat.Add("I feel aside from you, " + Main.npc[Tnpc1].GivenName + " is the only one who cares about me.");
					chat.Add("There's a growing connection between " + Main.npc[Tnpc1].GivenName + " and I, she understands me better than the others and it gives me comfort.");
				}
				int Tnpc2 = NPC.FindFirstNPC(ModContent.NPCType<ContrabandMerchant>());
				if (Tnpc2 >= 0)
				{
					chat.Add("There's a strange man hanging around here, he's creeping me out.");
					chat.Add("I heard of someone called " + Main.npc[Tnpc2].GivenName + " who has been hanging out in the back alleyways at night, selling strange items.");
				}
				/*if (ModLoader.GetMod("CalamityMod") != null)
				{
					npc = NPC.FindFirstNPC(ModLoader.GetMod("CalamityMod").NPCType("FAP"));
					if (npc >= 0 && Main.rand.Next(2) == 0)
					{
						chat.Add("" + Main.npc[npc].GivenName + " often watches me and sharpens that blade of his when no one else is around... I feel threatened by him.");
					}
				}*/

				if (NPC.AnyNPCs(ModContent.NPCType<Goat>()))
				{
					chat.Add("Jubia! They're here! Thank you [i: " + ItemID.LifeCrystal + "]",2);
					chat.Add("Jubia! I [i: " + ItemID.LifeFruit + "] the Goat",2);
					chat.Add("Jubia is the only person who ever truly helped me... Except for you of course", 2);
					chat.Add("I want to [i: " + ItemID.BetsyWings + "] Hug my goat", 2);
                }
                else
                {
					chat.Add("Where's my lovely Goat? :(");
				}

				/*
				if (SGAmod.SteamID != "" && !SGAmod.isGoG)
                {
					int friends = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate);
					if (friends>0)
					chat.Add("I wish I had "+ friends+" friends..."+(Main.rand.NextBool() ? "Your so lucky, sigh." : ""),1);
					friends = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagIgnored);
					if (friends > 0)
						chat.Add("Those "+ friends + " people also abused me too you know.",1);
					friends = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagFriendshipRequested);
					if (friends > 0)
						chat.Add("Those " + friends + " people want to be your friend, I wish I had, real friends...", 1);
				}
				*/

				chat.Add("When I overheard the last group of people talking about a bounty, I ran away, and kept flying as far as I could.");
				chat.Add("The last group of people I thought were my friends were going to sell me off as a bounty, I escaped when they were distracted. I just want to be treated like anyone else.");
				chat.Add("I'm not sure what to think about all this...");
				chat.Add("I hope I won't be abandoned like the last few times...");
				chat.Add("They're out there, somewhere.");
				chat.Add("I hope my parents are alive; I will find them.");
				chat.Add("I have not done what Dragons are hated for, but I am judged the same...");
				chat.Add("It seems like everywhere I go people are always trying to attack me.");
				chat.Add("The others... are giving me strange looks.");
				chat.Add("It's been uneasy, but I'll manage.");
				chat.Add("I am not weak...");
				chat.Add("People are wrong about our kind...");
				chat.Add("Where did my keys go?");
				chat.Add("Croteam has released Serious Sam 4, my Human thinks its alright, kinda underwhelming and kept like a filler title");
				chat.Add("Please don't point those blades at me...");
				chat.Add("'Rawr <3'");
				chat.Add("I cannot roar, I just make a cute whining sound.");
				chat.Add("I have uneasy thoughts about my flesh dissolving down gullets.");
				chat.Add("What is it?");
				chat.Add("My glowing flank? Even I don't know how I came to have this, I've had it for as long as I can remember.");
				chat.Add("Is there no true peace for any of us?");
				chat.Add("Do you know what it's like to be hunted? Despite doing nothing wrong at all?");
				chat.Add("I do not wish to be slain, please don't let anyone kill me :(");
				chat.Add("I've heard stories of how Dragons slept on massive piles of gold, that both sounds very uncomfortable and I would never steal from anyone.");
				chat.Add("I may be a dragon, but I feel... Different. I don't understand why our most of kind is so greedy and selfish. Worse yet, I'm judged no different...");
				chat.Add("I was often called a 'Dergon' by my former friends, I still don't get what it means.");
				chat.Add("Please stop trying to climb me, I'm not a mount.");
				chat.Add("Stop touching my tail, though the rubbing on my scales feels good... Rawr <3");
				chat.Add("I could use hugs. Maybe some belly rubs too.");
				chat.Add("Sometimes I can still hear the voices inside my head, talking in a deep directive voice...");
				chat.Add("I don't feel up to talking much right now.");
				chat.Add("What can I do to make people not hate me for what I am?");
				chat.Add("Raw venison tastes flavorless, is this how our kind always ate?");
				chat.Add("I hope you are not afraid, please don't be afraid of me.");
				chat.Add("Please protect me, I feel the others want to mount my head like a trophy.");
				chat.Add("All those trophies you have, of monsters... Am I monster?");
				chat.Add("Why are you looking at me like that? Is there something on my snout?");
				chat.Add("You think I look nice? Well from what I heard: my 'sprites' were a paid commission, and they weren't cheap. Thank you either way!");
				chat.Add("Often I feel timid but then I talk about things that I can only relate as Meta, it's very strange.");
				chat.Add("I remember a time, when all everyone would say, the sounds would echo: 'Button 2'");
				chat.Add("I often look at my claws; I was clearly meant to hurt and kill, but I don't want to be like the stories...");
				chat.Add("My human would like to thank you for 100,000 downloads, you have no idea how much it means to them, literally the world [i: " + ItemID.LifeFruit + "]");
				chat.Add("Do you have any idea what it's like, to put your faith in people and then just... Have it mean nothing to them? I guess I would know what it feels like to have the world against me, sigh.");
				if (BirthdayParty.PartyIsUp)
                {
					chat.Add("I still don't have a party hat :(",3);
					chat.Add("The cake is sweet, thank you for these feelings.",3);
					chat.Add("The last time I heard of a party, it was in celebration at one of my kind's deaths...",3);
					chat.Add("No, your not going to play 'pin the tail on my tail'", 3);

					int index = PartyVictum;
					/*
					if (index != default && !Main.LocalPlayer.SGAPly().dragonFriend)
					{
						List<int> guys = new List<int>();
						guys.AddRange(BirthdayParty.CelebratingNPCs);
						guys.Remove(index);

						chat.Add("They're throwing a party... for me, a monster?", 500);
						chat.Add("I'm scared "+Main.LocalPlayer.name, 500);
						chat.Add("Don't let them hurt me, please :(", 500);
						chat.Add("It's going to happen, isn't it?", 500);
						chat.Add("They're going to slay me!", 500);

						if (guys.Count > 0)
						{
							NPC him = new NPC();
							him.SetDefaults(Main.npc[guys[0]].type);
							chat.Add("I don't trust the "+ him.GetGivenOrTypeNetName()+ "; they're clearly planning something...", 500);
							chat.Add(him.GetGivenOrTypeNetName() + " is scaring me.", 5);
							chat.Add("It's going to take a good deal of convincing that this isn't some kind of setup to corner me", 500);

							if (guys.Count > 1)
							{
								NPC him2 = new NPC();
								him.SetDefaults(Main.npc[guys[1]].type);
								chat.Add("I want you to come to my party, not because of fun, but because I'm afraid "+him.GetGivenOrTypeNetName() + " and " + him2.GetGivenOrTypeNetName() +" are planning to slay me in public...", 500);
							}
						}
					}
					*/
				}
				if (!NPC.homeless)
				{
					chat.Add("The dwelling you made is far better than being forced to sleep outside.");
					chat.Add("I wondered away from the last group of people and suddenly I'm here, and you have a house for me too.");
					chat.Add("The shelter you provided it far better than what the town of Torch made for me.");
					chat.Add("While I am grateful for the place to stay, I often sleep on the floor as these beds were not made for someone my size; it's not real problem though.");
				}
				if (ModLoader.Mods.Length > 30)
				{
					chat.Add("I think you might be running too many mods, tone back the hot sauce, yeah? I know I don't like sauce.");
				}
				if (ModLoader.TryGetMod("BossChecklist", out Mod _))
				{
					chat.Add("Oh, I see you brought your notepad, nice!");
					chat.Add("Need to keep a checklist handy? It's always good to be planned.");
				}
				/*
				if (ModLoader.GetMod("CalamityMod") != null)
				{
					chat.Add("I have no idea what this 'Calamity' your talking about is.");
					chat.Add("I have no idea what this 'Yharim' your talking about is.");
					chat.Add("I keep hearing about another dragon in the jungle, but all I saw was a large feathered chicken.");
					chat.Add("I have no clue about this whole 'when do we get Yharim it has been a long time since the last major boss' thing is, can you please stop talking about it...")
				}
				*/
				/*
				if (modplayer.gottf2)
				{
					chat.Add("Terraria Co? Supply Crates? What are those?");
					chat.Add("What is a 'TF2' ?");
					if (SGAWorld.downedCratrosity)
					{
						chat.Add("Greed huh? That alone is what powered that thing? I guess the desire for a quick buck matters more than our lives...", 2.0);
					}
				}
				*/
				if (Main.LocalPlayer.wingTimeMax > 0)
				{
					chat.Add("Being blessed with flight does not mean you are ever free, I would know");
					chat.Add("Please tell me you did not rip those off some creature, don't hurt my wings...");
					chat.Add("Where did you get wings from?");
				}
				/*
				if (SGAWorld.downedMurk > 1)
				{
					if (SGAWorld.GennedVirulent)
					{
						chat.Add("The Very essence of the Murk has seeped into the Jungle, but yet, it is only a fraction of the power I've sensed here...", 2.0);
					}
					else
					{
						chat.Add("What Terrible secrets was that jungle-slime-creature hiding? An army of killer flies at its command...", 2.0);
					}	
				}
				if (SGAWorld.downedSpiderQueen)
				{
					chat.Add("You did a good favor to the world by ridding that giant spider, I can only feel remorse for those who became her dinner... Eaten alive...", 2.0);
				}
				if (SGAWorld.downedSharkvern)
				{
					chat.Add("Half Wyvern, Half Shark, completely brutal and sadly, undeserving of mercy; there was no other way", 2.0);
				}
				if (SGAWorld.downedSPinky)
				{
					chat.Add("What... Even was that slime? And what happened to the world when it was present?? Such Power...", 2.0);
				}
				if (DD2Event.DownedInvasionT3)
				{
					chat.Add("You had no other choice... Did you? I saw you kill that other dragon.", 2.0);
				}
				if (NPC.downedMartians)
				{
					chat.Add("More creatures from other worlds, thankfully at least they were not those I was afraid of...", 1.0);
				}
				if (SGAWorld.downedHarbinger)
				{
					chat.Add("Those eyes you defeated were nothing more than watchers for something far more terrible, I can only hope what is coming will not be our end.", 2.0);
				}
				if (NPC.downedMoonlord)
				{
					chat.Add("While I feel safer that you struck down that cosmic abomination, I sense of fear this, was only the beginner...", 2.0);
				}
				if (SGAWorld.downedHellion > 1)
				{
					chat.Add("My god... You've done, you beat her... I don't know what to say other than thank you <3", 2.0);
					chat.Add("So is this it? Am I finally free of Hellion's wrath? I don't hear her voice anymore", 2.0);
					if (!Main.LocalPlayer.HasItem(ModContent.ItemType<Items.Weapons.DragonCommanderStaff>()))
					chat.Add("Hey! Bring a [i: " + ModContent.ItemType<Items.CosmicFragment>() + "] to me, I have something for you!", 5.0);
				}
				else
				{
					if (SGAWorld.downedWraiths > 0)
					{
						if (SGAWorld.downedWraiths < 2)
							chat.Add("So they are called Wraiths? That first one was weak but I'm feeling... something, terrible...", 2.0);
						else if (SGAWorld.downedWraiths < 3)
							chat.Add("An entire array of animated armor! I don't like this! (whining noises)", 2.0);
						if (SGAWorld.downedWraiths > 3)
						{
							chat.Add("It's getting stronger, and closer, and whatever it is... It's not good! That last Wraith said something about a master and I'm very worried...", 2.0);
							chat.Add("This is very concerning, these powerful foes were merely messengers to their so called master, could their master be my enslaver? Please no!", 2.0);
						}
					}
				}
				if (SGAWorld.downedCaliburnGuardians > 0)
				{
					chat.Add("These Shrines are strange, they are so old and forgotten, yet yield a relic you now possess. It makes wonder what their purpose is...", 2.0);
					if (SGAWorld.downedCaliburnGuardians > 1)
						if (SGAWorld.downedCaliburnGuardians < 3)
							chat.Add("Another shrine, only more questions...", 2.0);
					if (SGAWorld.downedCaliburnGuardians > 2)
						chat.Add("I am sensing no other Shrines left uncovered on this planet, you are already a powerful friend. But I must wonder... These... Swords, weapons. I don't want to think whoever they belonged to, were meant to kill our kind long ago...", 2.0);
				}
				*/
				chat.Add("One day, they might find us... I surely hope not.");
				if (!Main.dayTime)
				{
					chat.Add("At least I have somewhere nice to stay during the night.");
					chat.Add("zzzz... Oh, do you need something?");
					chat.Add("'no, what is happening! no!' OH! I'm sorry, just these same nightmares again.");
					chat.Add("Every night I dream of some women destroying entire planets, and I'm flying away, watching entire worlds burn...");
					chat.Add("I'm trying to sleep please...");
					chat.Add("Do you even sleep at all? I never see you get into bed, only just touch it and hear a pop up sound.");
				}
				chat.Add("Could you help make this land a little safer? I can offer you what I found on my previous adventures.", 2.0);
				chat.Add("Hello...", 2.0);
				chat.Add("Hold 'Shift' to see your current and total Expertise, as well as see what's next on the target list.", 6.00);
			}
			//chat.Add("This message has a weight of 5, meaning it appears 5 times more often.", 5.0);
			//chat.Add("This message has a weight of 0.1, meaning it appears 10 times as rare.", 0.1);

			/*
			if (SGAWorld.downedSPinky && SGAWorld.downedCratrosityPML && SGAWorld.downedWraiths > 3)
			{
				if (SGAWorld.downedHellion == 0)
				{
					if (!Main.LocalPlayer.GetModPlayer<SGAPlayer>().gothellion && Main.expertMode)
					{
						Main.LocalPlayer.QuickSpawnItem(Mod.Find<ModItem>("HellionSummon").Type);
						Main.LocalPlayer.GetModPlayer<SGAPlayer>().gothellion = true;
						return Main.LocalPlayer.name + "! Something bad happened! Something REALLY bad! She found us! I was in my place when suddenly this appeared... This is just what I saw manifest itself outside planets before they burned." + "\n" + Main.LocalPlayer.name +
							" you need to stop her! You need to stop Hellion before she destroys you all and enslaves me again!";
					}

				}
			}
			*/
			return chat; // chat is implicitly cast to a string. You can also do "return chat.Get();" if that makes you feel better
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{
			//button = Language.GetTextValue("LegacyInterface.28");
			button = "Spend Expertise";
			button2 = "What's Next?";
			if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
			{
				button = "Check Expertise";
				/*
				if (Main.LocalPlayer.SGAPly().dragonFriend)
				{
					button2 = "Hug";
				}
				*/
			}
			/*
			if (BirthdayParty.PartyIsUp && BirthdayParty.GenuineParty && PartyVictum != default && !Main.LocalPlayer.SGAPly().dragonFriend)
			{
				switch (confort)
				{
					case 0:
						button2 = "What's Wrong?";
						break;
					case 1:
						button2 = "But that isn't true";
						break;
					case 2:
						button2 = "Why?";
						break;
					case 3:
						button2 = "(try to comfort him)";
						break;
					case 4:
						button2 = "You are not weak";
						break;
					case 5:
						button2 = "We will";
						break;
					case 6:
						button2 = "Enjoy the party";
						break;
				}
				return;
			}
			*/
		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (!firstButton)
            {
				/*
				if (BirthdayParty.PartyIsUp && BirthdayParty.GenuineParty && PartyVictum != default && !Main.LocalPlayer.SGAPly().dragonFriend)
                {
					switch (confort)
					{
						case 0:
							Main.npcChatText = "I've had... Nightmares. Terrible nightmares, I can remember being in this lab, I remember hearing voices. They talked about how they were making a weapon, to kill people. Even after I escaped I learned of the cruelty our kind had brought to these worlds, we killed innocent lives, we were no different than the monsters you slay...";
							confort = 1;
							break;
						case 1:
							Main.npcChatText = "Is it? I've had a few people try to take my life, just to claim they slain a dragon, a monster, and became heroes because our kind is destined to die. How else am I not suppose to think I'm some kind of monster when your own town wants to kill me, hang my head in the town hall, and then tanner my scaled hide!";
							confort = 2;
							break;
						case 2:
							Main.npcChatText = "I.. I don't know ok? I'm just, afraid ok?? I'm afraid of dying without ever finding out the truth, the party, the balloons, all of it... It just brings back memories of... of people cheering while a dragon is barely grasping for life, bleeding out and fading from existence; worse yet, they did nothing wrong. We died because we existed. I can't trust them, not after having already been betrayed by people I thought were my friends!";
							confort = 3;
							break;
						case 3:
							Main.npcChatText = "Thank you, I appreciate the hug, but I'm still afraid. People think we're big, strong, and can take on armies, but I'm just too weak...";
							confort = 4;
							break;
						case 4:
							Main.npcChatText = "I don't want to test that, I don't want to fight or harm anyone, I can't even hunt my own dinner... I Just want to survive and find out what happened to my parents.";
							confort = 5;
							break;
						case 5:
							Main.npcChatText = "Hmmm, thank you for the hope, thank you for being a real friend, for everything.";
							confort = 6;
							break;
						case 6:
							Main.npcChatText = "I think I will now... Thank you so much for helping me overcome my paranoia "+Main.LocalPlayer.name;
                            confort = 7;
							Main.LocalPlayer.SGAPly().dragonFriend = true;
							NPC.AddBuff(BuffID.Lovestruck, 120);
							break;
					}
					return;
                }
				*/
			}

			if (firstButton)
			{
				if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
				{
					SGAPlayer modplayer = Main.LocalPlayer.GetModPlayer<SGAPlayer>();

					NPC him2;
					string adder = "";

					if (modplayer.ExpertisePointsFromBosses is not null && modplayer.ExpertisePointsFromBosses.Count > 0) // Added null safety check
					{
						him2 = new NPC();
						if (modplayer.ExpertisePointsFromBossesModded[0] != "")
						{
							him2.SetDefaults(Mod.Find<ModNPC>(modplayer.ExpertisePointsFromBossesModded[0]).Type);
						}
                        else
						{
							List<int> stringa = new List<int>(modplayer.ExpertisePointsFromBosses);
							while ((WorldGen.crimson && (stringa[0] == NPCID.EaterofWorldsBody || stringa[0] == NPCID.EaterofWorldsTail || stringa[0] == NPCID.EaterofWorldsHead)) ||
								(!WorldGen.crimson && stringa[0] == NPCID.BrainofCthulhu))
								stringa.RemoveAt(0);


							him2.SetDefaults(stringa[0]);
						}

						if (him2 != null)
						{
							adder = " The very next target is a(n) " + him2.FullName+". "+ GetNextItem();
							if (modplayer.ExpertisePointsFromBosses[0] == NPCID.CultistArcherWhite)
								adder = " You got them all!! " + GetNextItem();
						}
						else
						{
							adder = " The very next target is... ugh.... (ERROR) 0_0";
						}
					}
					Main.npcChatText = "You have " + modplayer.ExpertiseCollected + " Expertise, out of a total of " + modplayer.ExpertiseCollectedTotal + "." + adder;
				}
				else
				{
					shop = true;
				}
			} 
			else 
			{

				if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
				{
					/*
					if (Main.LocalPlayer.SGAPly().dragonFriend)
					{
						if (!NPC.HasBuff(BuffID.RapidHealing))
						{
							Main.npcChatText = "Rawr! <3 Thank you, (wing hugs you back)";
							Main.LocalPlayer.AddBuff(BuffID.Lovestruck, 60 * 2);
							Main.LocalPlayer.AddBuff(BuffID.SoulDrain, 60 * 100, false);

							NPC.AddBuff(BuffID.Lovestruck, 60 * 2);
							NPC.AddBuff(BuffID.RapidHealing, 60 * 300, false);
                        }
                        else
                        {
							Main.npcChatText = "(Draken is full of love, and currently cannot accept more)";
						}
						return;
					}
					*/
				}


				string chat = "There is still more trouble out there, please, protect us";
				if (Main.rand.Next(0, 2) == 0)
					chat = "More trouble looms from overhead, and below. Keep up the good work " + Main.LocalPlayer.name + "!";

				/*
				if (SGAWorld.downedHellion<2 && SGAWorld.downedSPinky && SGAWorld.downedWraiths>3 && SGAWorld.downedCratrosityPML && Main.LocalPlayer.SGAPly().gothellion)
				{
					chat = "What are you doing right now!? use the [i:" + Mod.Find<ModItem>("HellionSummon").Type + "] and stop Hellion!";
					if (Main.rand.Next(0,2)==0)
						chat = "They're going to recapture me and force me into slavery, you need to use the [i:" + Mod.Find<ModItem>("HellionSummon").Type + "] and stop them!";
				}

				if (!SGAWorld.downedSPinky && SGAWorld.downedWraiths > 3)
				{
					chat = "That pinky slime is mad, like REALLY mad and it distresses and worries me, I cannot imagine what would happen if you made it made it envious too, such as something like a  [i:" + Mod.Find<ModItem>("Prettygel").Type + "]";
				}

				if (!SGAWorld.downedCratrosityPML && SGAWorld.downedWraiths > 3)
				{
					chat = "That being calling itself Cthulhu's brother dropped... a [i:" + Mod.Find<ModItem>("SalvagedCrate").Type + "]? It mentions a special kind of key, the same key used to enter the Temple it seems, at nighttime...";
				}

				if (SGAWorld.downedWraiths < 4 && NPC.downedAncientCultist)
					chat = "Yet Another creature has stolen knowledge from you, this time the Ancient Manipulator AND made you lose your knowledge to craft Luminite Bars, yes I know this is getting old but this is the last one, you can fight it by using a [i:" + Mod.Find<ModItem>("WraithCoreFragment3").Type + "] made with lunar fragments and the previous summoning item. Rematch will unlock Luminite bars but require defeating Moon Lord first.";

				if (!SGAWorld.downedHarbinger && NPC.downedGolemBoss)
				{
					chat = "We're being watched, I don't know by what, but I do know the dungeon isn't occupied by anyone, despite what everyone else is talking about. Maybe those alien-like Probes and those Etheria monsters are behind this?";
					if (DD2Event.DownedInvasionT3 && NPC.downedMartians)
						chat = "After dispatching those previous foes I figured it out: we're being watched, by something from outside this world and I think I know how to get rid of it. I think you could mix some ectoplasm with one of those bloody eyes to create a [i:" + Mod.Find<ModItem>("TruelySusEye").Type + "] and see what happens";
				}

				if (!SGAWorld.downedTPD && NPC.downedGolemBoss)
					chat = "In the night skies I sense those mechanical creations again, but worse... I have only seen a glimpse of their appearance and the best I could describe them as is a [i:" + Mod.Find<ModItem>("Mechacluskerf").Type + "]";

				if (!SGAWorld.downedCratrosity && NPC.downedPlantBoss)
				{
					if (SGAWorld.tf2cratedrops)
					{
						chat = "The town seems obsessed with these [i:" + Mod.Find<ModItem>("TerrariacoCrateBase").Type + "] of late every night. It is written on them to not use other keys, but maybe if you tried to use those golden keys on it, what would happen?";
					}
					if (SGAWorld.tf2cratedrops)
					{
						if (NPC.CountNPCS(NPCID.Merchant) > 0)
							chat = Main.npc[NPC.FindFirstNPC(NPCID.Merchant)].GivenName + "is selling something... Fishy, a [i:" + Mod.Find<ModItem>("PremiumUpgrade").Type + "]?, maybe should check it out I don't like the sound of it.";
						else
							chat = "I heard of something called a [i:" + Mod.Find<ModItem>("PremiumUpgrade").Type + "] but the merchant is no where to be found";
					}
				}

				if (!SGAWorld.downedSharkvern && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
					chat = "The skies above the oceans have been more turbulent than usual lately from my flights over them as if, something had recently been roused from its slumber. Go there and check it out, You'd need something to sound it from the depths, something like a [i:" + Mod.Find<ModItem>("ConchHorn").Type + "] made with sea materials and that new green stuff in the underground jungle";

				if (SGAWorld.downedWraiths == 1 && Main.hardMode)
					chat = "Hmmm... Another strange creation has done something worse this time, they have stolen your knowledge to make a hardmode anvil, you can fight it by using a [i:" + Mod.Find<ModItem>("WraithCoreFragment2").Type + "] which is as made from tier 1 hardmode ores and the previous summoning item";
				if (!SGAWorld.GennedVirulent && Main.rand.Next(0, 2) == 0 && Main.hardMode)
					chat = "Murk has returned, more powerful than ever, but at the same time that very power is seeping out of his gelatin body. Perhaps it could be released into the jungle... Take a [i:" + Mod.Find<ModItem>("RoilingSludge").Type + "] to the jungle and find out";
				if (!SGAWorld.downedCirno && Main.rand.Next(0, 2) == 0 && Main.hardMode)
					chat = "The snowy lands are colder than usually and I can feel ice flakes forming on my own wings, let alone yours, a strong being of ice is hampering our ability to fly, craft a core of its power, a [i:" + Mod.Find<ModItem>("Nineball").Type + "] with Ice Fairy dusts and souls and take it to the frosty plains during the day";

				if (SGAWorld.downedMurk<2)
					chat = "The dank structures' walls are protected by a strong, fly swarming, roiling creature in the jungle, perhaps a [i:" + Mod.Find<ModItem>("RoilingSludge").Type + "] may attract its fly swamps, and its wrath";
				if (!SGAWorld.downedSpiderQueen)
					chat = "A highly voracious creature lurks below the surface, I fear it might eat a small dragon like me whole, go and find it please, I think an [i:" + Mod.Find<ModItem>("AcidicEgg").Type + "] will lure it from its feasting to confront you";
				if (SGAWorld.downedCaliburnGuardians<3 && Main.rand.Next(0,3)==0 && Main.LocalPlayer.SGAPly().ExpertiseCollectedTotal>=300)
					chat = "I've noticed 3 structures below the surface that warrant investigation, I have found a [i: " + Mod.Find<ModItem>("CaliburnCompess").Type + "] that may help you with that.";

				if (SGAWorld.downedWraiths == 0)
					chat = "I'd be careful using the furnace if I were you, something is watching... Perhaps you can lure it out beforehand with a [i:" + Mod.Find<ModItem>("WraithCoreFragment").Type + "] before it catches you off guard, I heard these can be made with copper/tin ore and fallen stars";

				if (SGAWorld.downedHellion > 1)
					chat = "You've done it, thank you [i: " + ItemID.Heart + "]!";
				*/

				Main.npcChatText = chat + " --- You can Hold 'Shift' and click 'Check Expertise' to see your current and total Expertise, as well as see what's next on the target list.";
				//Main.CloseNPCChatOrSign();
				//SGAmod.TryToggleUI(null);
			}
		}

		public int[,] itemsinshop = new int[14, 2];
		public string GetNextItem()
		{
			itemsinshop = new[,]
			{
				/*
				{ ModContent.ItemType<Items.EmptyCharm>(),50 },
				{ ModContent.ItemType<Items.AssemblyStar>(),200 },
				{ ModContent.ItemType<Items.Tools.UniversalBait>(),250 },

				{ ModContent.ItemType<Items.Consumables.CaliburnCompess>(),300 },
				{ ModContent.ItemType<Items.Accessories.GrippingGloves>(),400 },
				{ ModContent.ItemType<Items.Consumables.RedManaStar>(),500 },
				{ ModContent.ItemType<Items.Weapons.ThievesThrow>(),750 },
				*/
				{ ItemID.Terragrim, 1000 },
				/*
				{ ModContent.ItemType<Items.Consumables.SoulJar>(),1250 },
				{ ItemID.RodofDiscord,2000 },
				{ ModContent.ItemType<Items.Consumables.Gong>(),2500 },
				//{ SGAmod.Instance.ItemType("EntropyTransmuter"),4000 },
				{ ModContent.ItemType<Items.Accessories.PrimordialSkull>(),5000 },
				{ ModContent.ItemType<Items.Accessories.MVMUpgrade>(),6000 },
				*/
				{ ItemID.AviatorSunglasses, 10000 },
				{ ItemID.RedPotion, 1000000 },
				{ ItemID.RedPotion, 1000000 },
				{ ItemID.RedPotion, 1000000 },
			};

			SGAPlayer modplayer = Main.LocalPlayer.GetModPlayer<SGAPlayer>();
			int index = 0;
			long expmax = modplayer.ExpertiseCollectedTotal;
			//int offset = 0;
			while (index < itemsinshop.Length && expmax > itemsinshop[index, 1])
			{
				//expmax -= itemsinshop[index, 1];
				index += 1;
			}
			long math = itemsinshop[index, 1] - modplayer.ExpertiseCollectedTotal;
			string str;
            if (math == 0)
            {
                index++;
                math = itemsinshop[index, 1] - modplayer.ExpertiseCollectedTotal;
            }
            if (math >= 50000)
			{
				str = "You unlocked everything so far, stay safe friend <3";
			}
			else
			{
				Item itm = new();
				itm.SetDefaults(itemsinshop[index, 0]);
				str = "You are " + (math) + " away from the next item unlocking: "+ itm.Name+".";
			}
			return str;
		}

		public override void SetupShop(Chest shop, ref int nextSlot)
		{
			// Draken's shop items

			SGAPlayer modplayer = Main.LocalPlayer.GetModPlayer<SGAPlayer>();

			
			if (Main.hardMode)
			{
				shop.item[nextSlot].SetDefaults(ItemID.PlatinumCoin);
				shop.item[nextSlot].shopCustomPrice = 30;
				shop.item[nextSlot].shopSpecialCurrency = SGAmod.ExpertiseCustomCurrencyID;
				nextSlot++;
			}
			else
			{
				shop.item[nextSlot].SetDefaults(ItemID.GoldCoin);
				shop.item[nextSlot].shopCustomPrice = 1;
				shop.item[nextSlot].shopSpecialCurrency = SGAmod.ExpertiseCustomCurrencyID;
				nextSlot++;
			}
            /*
			shop.item[nextSlot].SetDefaults(Mod.Find<ModItem>("BossHints").Type);
			shop.item[nextSlot].shopCustomPrice = 1;
			shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
			nextSlot++;

			if (modplayer.Drakenshopunlock)
			{
				shop.item[nextSlot].SetDefaults(Mod.Find<ModItem>("IDGStartBag").Type);
				shop.item[nextSlot].shopCustomPrice = 10;
				shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
				nextSlot++;
			}
			if (modplayer.ExpertiseCollectedTotal >= 50)
			{
				shop.item[nextSlot].SetDefaults(Mod.Find<ModItem>("EmptyCharm").Type);
				shop.item[nextSlot].shopCustomPrice = 10;
				shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
				nextSlot++;
			}
			if (modplayer.ExpertiseCollectedTotal >= 200)
			{
				shop.item[nextSlot].SetDefaults(Mod.Find<ModItem>("AssemblyStar").Type);
				shop.item[nextSlot].shopCustomPrice = 15;
				shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
				nextSlot++;
			}		
			if (modplayer.ExpertiseCollectedTotal >= 250 && NPC.CountNPCS(NPCID.Angler)>0)
			{
				shop.item[nextSlot].SetDefaults(Mod.Find<ModItem>("UniversalBait").Type);
				shop.item[nextSlot].shopCustomPrice = 3;
				shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
				nextSlot++;
			}				
			if (modplayer.ExpertiseCollectedTotal >= 300)
			{
				shop.item[nextSlot].SetDefaults(Mod.Find<ModItem>("CaliburnCompess").Type);
				shop.item[nextSlot].shopCustomPrice = 30;
				shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
				nextSlot++;
			}
			if (modplayer.ExpertiseCollectedTotal >= 400)
			{
				shop.item[nextSlot].SetDefaults(Mod.Find<ModItem>("GrippingGloves").Type);
				shop.item[nextSlot].shopCustomPrice = 30;
				shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
				nextSlot++;
			}
			if (modplayer.ExpertiseCollectedTotal >= 500)
			{
				shop.item[nextSlot].SetDefaults(Mod.Find<ModItem>("RedManaStar").Type);
				shop.item[nextSlot].shopCustomPrice = 50;
				shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
				nextSlot++;
			}		
			if (modplayer.ExpertiseCollectedTotal >= 750)
			{
				shop.item[nextSlot].SetDefaults(Mod.Find<ModItem>("ThievesThrow").Type);
				shop.item[nextSlot].shopCustomPrice = 30;
				shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
				nextSlot++;
			}
            */
			if (modplayer.ExpertiseCollectedTotal >= 1000)
			{
				shop.item[nextSlot].SetDefaults(ItemID.Terragrim);
				shop.item[nextSlot].shopCustomPrice = 75;
				shop.item[nextSlot].shopSpecialCurrency = SGAmod.ExpertiseCustomCurrencyID;
				nextSlot++;
			}
            /*
			if (modplayer.ExpertiseCollectedTotal >= 1250)
			{
				shop.item[nextSlot].SetDefaults(Mod.Find<ModItem>("SoulJar").Type);
				shop.item[nextSlot].shopCustomPrice = 25;
				shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
				nextSlot++;
			}
			if (modplayer.ExpertiseCollectedTotal >= 2000)
			{
				shop.item[nextSlot].SetDefaults(Main.dayTime ? ModContent.ItemType<Items.Tools.RodOfTeleportation>() : ItemID.RodofDiscord);
				shop.item[nextSlot].shopCustomPrice = 100;
				shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
				nextSlot++;
			}
			if (modplayer.dragonFriend)
			{
				shop.item[nextSlot].SetDefaults(Mod.Find<ModItem>("Fieryheart").Type);
				shop.item[nextSlot].shopCustomPrice = 125;
				shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
				nextSlot++;
			}
			if (modplayer.ExpertiseCollectedTotal >= 2500)
			{
				shop.item[nextSlot].SetDefaults(Main.dayTime ? Mod.Find<ModItem>("Gong").Type : Mod.Find<ModItem>("BoneBucket").Type);
				shop.item[nextSlot].shopCustomPrice = 150;
				shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
				nextSlot++;
			}			
			if (modplayer.ExpertiseCollectedTotal >= 5000)
			{
				shop.item[nextSlot].SetDefaults(Mod.Find<ModItem>("PrimordialSkull").Type);
				shop.item[nextSlot].shopCustomPrice = 100;
				shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
				nextSlot++;
			}
			if (modplayer.ExpertiseCollectedTotal >= 5000 && SGAWorld.downedCratrosity)
			{
				shop.item[nextSlot].SetDefaults(Mod.Find<ModItem>("SOATT").Type);
				shop.item[nextSlot].shopCustomPrice = 125;
				shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
				nextSlot++;
			}
			if (modplayer.ExpertiseCollectedTotal >= 6000)
			{
				shop.item[nextSlot].SetDefaults(Mod.Find<ModItem>("MVMUpgrade").Type);
				shop.item[nextSlot].shopCustomPrice = 150;
				shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
				nextSlot++;
			}
            */
			if (modplayer.ExpertiseCollectedTotal >= 10000)
			{
				shop.item[nextSlot].SetDefaults(ItemID.AviatorSunglasses);
				shop.item[nextSlot].shopCustomPrice = 200;
				shop.item[nextSlot].shopSpecialCurrency = SGAmod.ExpertiseCustomCurrencyID;
				nextSlot++;
			}
            /*
			if (modplayer.ExpertiseCollectedTotal >= 12000 && SGAWorld.downedHellion>1 && Main.LocalPlayer.HasItem(ModContent.ItemType<Items.CosmicFragment>()))
			{
				shop.item[nextSlot].SetDefaults(Mod.Find<ModItem>("DragonCommanderStaff").Type);
				shop.item[nextSlot].shopCustomPrice = 500;
				shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
				nextSlot++;
			}
			if (!Main.expertMode && NPC.downedMoonlord)
			{
				shop.item[nextSlot].SetDefaults(Mod.Find<ModItem>("VenerableCatharsis").Type);
				shop.item[nextSlot].shopCustomPrice = 500;
				shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
				nextSlot++;
			}
			*/
		}

		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			damage = 35;
			knockback = 4f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			cooldown = 5;
			randExtraCooldown = 10;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
		{
			projType = ProjectileID.Flames;
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 12f;
			randomOffset = 3f;
		}
	}

	public class DergonProfile : ITownNPCProfile
	{
		public string Path => (GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/');

		public int RollVariation() => 0;
		public string GetNameForVariant(NPC npc) => npc.getNewNPCName();

		public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
		{
			return ModContent.Request<Texture2D>(Path);
		}

		public int GetHeadTextureIndex(NPC npc)
		{
			return ModContent.GetModHeadSlot(Path + "_Head");
		}
	}
}
