using System.Linq;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Idglibrary;
using Terraria.GameContent;
using ReLogic.Content;
using Terraria.GameContent.Personalities;
using Terraria.GameContent.Bestiary;
/*
using SGAmod.Items.Consumables;
using SGAmod.Items.Pets;
using SGAmod.Items.Armors.Vanity;
*/

namespace SGAmod.NPCs.TownNPCs
{
	[AutoloadHead]
	public class Goat : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Guide];
			NPCID.Sets.ExtraFramesCount[NPC.type] = 10;
			NPCID.Sets.AttackFrameCount[NPC.type] = 5;
			NPCID.Sets.DangerDetectRange[NPC.type] = 700;
			NPCID.Sets.AttackType[NPC.type] = 0;
			NPCID.Sets.AttackTime[NPC.type] = 90;
			NPCID.Sets.AttackAverageChance[NPC.type] = 30;
			NPCID.Sets.HatOffsetY[NPC.type] = 4;

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new(0)
			{
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
				Direction = -1
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			NPC.Happiness
				.SetBiomeAffection<ForestBiome>(AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Dergon>(), AffectionLevel.Love)
			//Princess is automatically set
			; // < Mind the semicolon!
		}

		public override void SetDefaults()
		{
			NPC.townNPC = true;
			NPC.friendly = true;
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = 7;
			NPC.damage = 10;
			NPC.defense = 15;
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;
			AnimationType = NPCID.Guide;
			NPC.homeless = true;
			Color c = Main.hslToRgb((float)(Main.GlobalTimeWrappedHourly/2)%1f, 0.5f, 0.35f);

		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("Draken's great friend.")
			});
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
			{

				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Head").Type, 1f);

				for (int k = 0; k < 2; k++)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Leg").Type, 1f);
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Arm").Type, 1f);
				}
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs)
		{
			return NPC.AnyNPCs(ModContent.NPCType<Dergon>());//&& SGAmod.NightmareUnlocked);
		}

		public override bool CheckConditions(int left, int right, int top, int bottom)
		{
			return true;
		}

		public override ITownNPCProfile TownNPCProfile()
		{
			return new GoatProfile();
		}

		public override List<string> SetNPCNameList()
		{
			return new List<string>()
			{
				"Jubia"
			};
		}

		public override void AI()
        {
			if (!ContrabandMerchant.IsNpcOnscreen(NPC.Center) && !NPC.AnyNPCs(ModContent.NPCType<Dergon>()))
			{
				if (Main.netMode == NetmodeID.SinglePlayer) Main.NewText(NPC.FullName + " has left!", 50, 125, 255);
				else ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(NPC.FullName + " has left!"), new Color(50, 125, 255));
				NPC.active = false;
				NPC.netSkip = -1;
				NPC.life = 0;
			}

			base.AI();
        }


        // Consider using this alternate approach to choosing a random thing. Very useful for a variety of use cases.
        // The WeightedRandom class needs "using Terraria.Utilities;" to use
        public override string GetChat()
		{
			WeightedRandom<string> chat = new WeightedRandom<string>();

			if (!NPC.AnyNPCs(ModContent.NPCType<Dergon>()))
			return "I have nothing to say to you...";

			chat.Add("I am the best goat");
			chat.Add("'Bleat'");
			chat.Add("I love Draken so much");
			chat.Add("Only the best goat for the best derg");
			chat.Add("[i: " + ModContent.ItemType<Items.Consumables.Debug.YellowHeart>() + "] the Derg");
			chat.Add("I never felt true platonic love til I met Draken");
			if (Main.dayTime)
			{
				chat.Add(Main.raining ? "Rain rain, go away, come back another day" : "today is beautiful");
				chat.Add("Thankfully it isn't too hot");
				chat.Add(Main.raining ? "At least it's cool out" : "What a lovely day");
			}
			else
			{
				chat.Add("The moon is beautiful tonight...");
				chat.Add("Some dialog for night time");
				chat.Add("Twinkle twinkle stars");
			}
			return chat;
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = Language.GetTextValue("LegacyInterface.28");

		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (firstButton)
			{
				shop = true;
			}
		}

		public override void SetupShop(Chest shop, ref int nextSlot)
		{
			/*
			shop.item[nextSlot].SetDefaults(Mod.Find<ModItem>("DergPainting").Type);
			shop.item[nextSlot].value = Item.buyPrice(0, 1);
			nextSlot += 1;
			shop.item[nextSlot].SetDefaults(Mod.Find<ModItem>("CalmnessPainting").Type);
			shop.item[nextSlot].value = Item.buyPrice(0, 10);
			nextSlot += 1;
			shop.item[nextSlot].SetDefaults(Mod.Find<ModItem>("MeetingTheSunPainting").Type);
			shop.item[nextSlot].value = Item.buyPrice(0, 10);
			nextSlot += 1;
			shop.item[nextSlot].SetDefaults(Mod.Find<ModItem>("AdventurePainting").Type);
			shop.item[nextSlot].value = Item.buyPrice(1, 0);
			nextSlot += 1;
			shop.item[nextSlot].SetDefaults(Mod.Find<ModItem>("SerenityPainting").Type);
			shop.item[nextSlot].value = Item.buyPrice(1, 0);
			nextSlot += 1;
			shop.item[nextSlot].SetDefaults(Mod.Find<ModItem>("UnderTheWaterfallPainting").Type);
			shop.item[nextSlot].value = Item.buyPrice(1, 0);
			nextSlot += 1;
			if (SGAWorld.NightmareHardcore > 0 && SGAWorld.downedHellion > 1)
			{
				shop.item[nextSlot].SetDefaults(Mod.Find<ModItem>("ParadoxGeneralPainting").Type);
				shop.item[nextSlot].value = Item.buyPrice(1, 0);
				nextSlot += 1;
			}
			shop.item[nextSlot].SetDefaults(Mod.Find<ModItem>("AncientSpaceDiverHelmet").Type);
			nextSlot += 1; 		
			shop.item[nextSlot].SetDefaults(Mod.Find<ModItem>("AncientSpaceDiverChestplate").Type);
			nextSlot += 1; 		
			shop.item[nextSlot].SetDefaults(Mod.Find<ModItem>("AncientSpaceDiverLeggings").Type);
			nextSlot += 1;
			shop.item[nextSlot].SetDefaults(Mod.Find<ModItem>("AncientUnmanedHood").Type);
			nextSlot += 1; 		
			shop.item[nextSlot].SetDefaults(Mod.Find<ModItem>("AncientUnmanedBreastplate").Type);
			nextSlot += 1; 		
			shop.item[nextSlot].SetDefaults(Mod.Find<ModItem>("AncientUnmanedLeggings").Type);
			nextSlot += 1;

			shop.item[nextSlot].SetDefaults(Mod.Find<ModItem>("MasterfullyCraftedHatOfTheDragonGods").Type);
			nextSlot += 1;
			shop.item[nextSlot].SetDefaults(Mod.Find<ModItem>("JoyfulShroom").Type);
			shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 10, 0, 0);
			nextSlot += 1;			
			shop.item[nextSlot].SetDefaults(Mod.Find<ModItem>("AvariceRingWeaker").Type);
			shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 75, 0, 0);
			nextSlot += 1;			
			shop.item[nextSlot].SetDefaults(Mod.Find<ModItem>("NoHitCharmlv1").Type);
			shop.item[nextSlot].shopCustomPrice = Item.buyPrice(1, 0, 0, 0);
			nextSlot += 1;
			shop.item[nextSlot].SetDefaults(Mod.Find<ModItem>("TheWholeExperience").Type);
			shop.item[nextSlot].shopCustomPrice = Item.buyPrice(1, 0, 0, 0);
			nextSlot += 1;
			if (Main.LocalPlayer.HasItem(ModContent.ItemType<CopperTack>()))
            {
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<TinTack>());
				nextSlot++;
			}
			if (Main.LocalPlayer.HasItem(ModContent.ItemType<CobaltTack>()))
			{
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<PalladiumTack>());
				nextSlot++;
			}
			if (Main.LocalPlayer.HasItem(ModContent.ItemType<CopperWraithMask>()))
			{
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<TinWraithMask>());
				nextSlot++;
			}
			if (Main.LocalPlayer.HasItem(ModContent.ItemType<CobaltWraithMask>()))
			{
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<PalladiumWraithMask>());
				nextSlot++;
			}
			if (SGAWorld.downedWraiths >= 1)
			{
				shop.item[nextSlot].SetDefaults(Mod.Find<ModItem>("TinWraithTrophy").Type);
				nextSlot++;
			}
			if (SGAWorld.downedWraiths >= 2)
			{
				shop.item[nextSlot].SetDefaults(Mod.Find<ModItem>("PalladiumWraithTrophy").Type);
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
			projType = ModContent.ProjectileType<Projectiles.Magic.DD2DrakinShotFriendly>();
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 12f;
			randomOffset = 3f;
		}
	}
	public class GoatProfile : ITownNPCProfile
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
