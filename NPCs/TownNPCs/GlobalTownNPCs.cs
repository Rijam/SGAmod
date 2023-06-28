using Terraria;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.NPCs.TownNPCs
{
	public class GlobalTownNPCs : GlobalNPC
	{
		public override void SetStaticDefaults()
		{
			// Happiness
			int draken = ModContent.NPCType<Dergon>(); // Get NPC's type
			int goat = ModContent.NPCType<Goat>();
			//int contrabandMerchant = ModContent.NPCType<ContrabandMerchant>();

			var drakenHappiness = NPCHappiness.Get(draken);
			var goatHappiness = NPCHappiness.Get(goat);

			var guide = NPCHappiness.Get(NPCID.Guide); // Get the key into the NPC's happiness
			var zoologist = NPCHappiness.Get(NPCID.BestiaryGirl); // Get the key into the NPC's happiness
			var taxCollector = NPCHappiness.Get(NPCID.TaxCollector); // Get the key into the NPC's happiness

			zoologist.SetNPCAffection(draken, AffectionLevel.Love);
			guide.SetNPCAffection(draken, AffectionLevel.Like);
			taxCollector.SetNPCAffection(draken, AffectionLevel.Hate);

			if (ModLoader.TryGetMod("BossesAsNPCs", out Mod bossesAsNPCs))
			{
				if (bossesAsNPCs.TryFind<ModNPC>("Betsy", out ModNPC betsy))
				{
					var betsyHappiness = NPCHappiness.Get(betsy.Type);

					betsyHappiness.SetNPCAffection(draken, AffectionLevel.Like);
					drakenHappiness.SetNPCAffection(betsy.Type, AffectionLevel.Like);
				}
			}
		}

		public override void GetChat(NPC npc, ref string chat)
		{
			int draken = NPC.CountNPCS(ModContent.NPCType<Dergon>());

			switch (npc.type)
			{
				case NPCID.Guide:
					if (Main.rand.NextBool(3) && draken > 0)
					{
						string[] lines = { "A dragon is the last person I'd expect to move in to be honest.",
						Main.npc[draken].GivenName + " seems upset over his past, I feel sorry for his past."};
						chat = lines[Main.rand.Next(lines.Length)];
					}
					else
					{
						if (Main.rand.NextBool(3) && draken <= 0)
						{
							chat = "I think I see something flying above, maybe if you clear the area of powerful monsters, it might land...";
							if (Main.rand.NextBool(2))
							{
								chat = "What is that up there... It looks like a dragon...";
							}
						}

					}
					break;
				case NPCID.ArmsDealer:
					if (Main.rand.NextBool(5))
					{
						chat = "Somewhere along the way I got all these Starfish and Shark Teeth, now if only you could find guns that use them I could sell them to you";
						return;

					}
					if (Main.rand.NextBool(3) && draken > 0)
					{
						string[] lines = { "I'm sure the dragon is worth a lot on the black market, just need to find the right person",
						"How much do you think he could get for selling the dragon? People would pay well for beasts like him."};
						chat = lines[Main.rand.Next(lines.Length)];
					}
					break;
				case NPCID.PartyGirl:
					if (Main.rand.NextBool(3) && draken > 0)
					{
						string[] lines = { "I tried coloring " + Main.npc[draken].GivenName + " pink but he didn't seem to like it, strange.",
						"Working on a way to way a party hat fit on the derg, though I might just need 2! Twice the party!"};
						chat = lines[Main.rand.Next(lines.Length)];
					}
					break;
				case NPCID.Merchant:
					if (Main.rand.NextBool(3) && draken > 0)
					{
						string[] lines = { "I found it odd " + Main.npc[draken].GivenName + " was asking me about apples even though you know I don't sell those, don't dragons eat meat?",
						"Those scales on " + Main.npc[draken].GivenName + " might be worth quite a bit, might peel a few off later when he's sleeping."};
						chat = lines[Main.rand.Next(lines.Length)];
					}
					break;
				case NPCID.TravellingMerchant:
					if (Main.rand.NextBool(3) && draken > 0)
					{
						string[] lines = { "Oh a tamed dragon! Are you selling it by any chance?",
						"What do you mean the dragon isn't for sale? I'll offer you top dollar for it!"};
						chat = lines[Main.rand.Next(lines.Length)];
					}
					break;
				case NPCID.TaxCollector:
					if (Main.rand.NextBool(3) && draken > 0)
					{
						string[] lines = { "I don't expect for one second that scaled lizard is hiding his hoard, tax evasion I say!",
						"I'll find that dragon's hoard sooner or later, he can't keep lying forever."};
						chat = lines[Main.rand.Next(lines.Length)];
					}
					break;

					/*
					case NPCID.Nurse:

					if (Main.rand.Next(3) == 0 && Main.LocalPlayer.statLife<Main.LocalPlayer.statLifeMax2)
                    {
						chat = Main.rand.NextBool() ? "I can heal your wounds, but surgery doesn't happen over night... not all the time anyways" : ("Healing takes time, "+Main.rand.NextBool() ? "stay around why don't you?" : "but that's just life");
					}
					*/

					/*
					if (Hellion.GetHellion() != null)
					{
						chat = "I see you're busy with that other girl... what do you want me for?";
					}
					break;
					*/
			}
		}
	}
}