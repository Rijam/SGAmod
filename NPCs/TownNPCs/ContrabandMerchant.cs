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
using SGAmod.Items;
using Terraria.GameContent.UI;
using Terraria.GameContent;
using ReLogic.Content;
using Terraria.GameContent.Bestiary;
using System.CodeDom;
// using SGAmod.Items.Consumables.LootBoxes;

namespace SGAmod.NPCs.TownNPCs
{

	[AutoloadHead]
	public class ContrabandMerchant : ModNPC
	{
        private static string Shop1 = "Shop1";

		public int itemRandomizer = 0;
		public static UnifiedRandom randz = new UnifiedRandom();


		public static OverseenCrystalCurrency OverseenCrystalCustomCurrencySystem;
		public static int OverseenCrystalCustomCurrencyID;

		public static AncientClothCurrency AncientClothCurrencyCustomCurrencySystem;
		public static int AncientClothCurrencyCustomCurrencyID;

		public static DesertFossilCurrency DesertFossilCurrencyCustomCurrencySystem;
		public static int DesertFossilCurrencyCustomCurrencyID;

		public static GlowrockCurrency GlowrockCustomCurrencySystem;
		public static int GlowrockCustomCurrencyID;

		public static CrateCurrency CrateCurrencyCustomCurrencySystem;
		public static int CrateCurrencyCustomCurrencyID;

		public override void Load()
		{
			// OverseenCrystalCustomCurrencySystem = new OverseenCrystalCurrency(ModContent.ItemType<OverseenCrystal>(), 999L);
			OverseenCrystalCustomCurrencySystem = new OverseenCrystalCurrency(ItemID.Diamond, 999L); // Temporary
			OverseenCrystalCustomCurrencyID = CustomCurrencyManager.RegisterCurrency(OverseenCrystalCustomCurrencySystem);

			AncientClothCurrencyCustomCurrencySystem = new AncientClothCurrency(ItemID.AncientCloth, 999L);
			AncientClothCurrencyCustomCurrencyID = CustomCurrencyManager.RegisterCurrency(AncientClothCurrencyCustomCurrencySystem);

			DesertFossilCurrencyCustomCurrencySystem = new DesertFossilCurrency(ItemID.FossilOre, 999L);
			DesertFossilCurrencyCustomCurrencyID = CustomCurrencyManager.RegisterCurrency(DesertFossilCurrencyCustomCurrencySystem);

			// GlowrockCustomCurrencySystem = new GlowrockCurrency(ModContent.ItemType<Glowrock>(), 999L);
			GlowrockCustomCurrencySystem = new GlowrockCurrency(ItemID.Meteorite, 999L); // Temporary
			GlowrockCustomCurrencyID = CustomCurrencyManager.RegisterCurrency(GlowrockCustomCurrencySystem);

			// CrateCurrencyCustomCurrencySystem = new CrateCurrency(ModContent.ItemType<TerrariacoCrateBase>(), 999L);
			CrateCurrencyCustomCurrencySystem = new CrateCurrency(ItemID.WoodenCrate, 999L); // Temporary
			CrateCurrencyCustomCurrencyID = CustomCurrencyManager.RegisterCurrency(CrateCurrencyCustomCurrencySystem);
		}

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 25;
			NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
			NPCID.Sets.AttackFrameCount[NPC.type] = 4;
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
			Color c = Main.hslToRgb((float)(Main.GlobalTimeWrappedHourly / 2) % 1f, 0.5f, 0.35f);
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
				new FlavorTextBestiaryInfoElement("A man who shows up near the edge of your town at night to sell you illegal items.")
			});
		}

		public override void HitEffect(NPC.HitInfo hitInfo)
		{
			if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
			{
				if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Head_alt").Type, 1f);
				}
				else
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Head").Type, 1f);
				}
				for (int k = 0; k < 2; k++)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Arm").Type, 1f);
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Leg").Type, 1f);
				}
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs)
		{
			return false;
		}

		public override bool CheckConditions(int left, int right, int top, int bottom)
		{
			return false;
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return ((!Main.dayTime && NPC.CountNPCS(NPC.type) < 1 && spawnInfo.PlayerInTown) ? 1f : 0f);
		}

		public override ITownNPCProfile TownNPCProfile()
		{
			return new ContrabandMerchantProfile();
		}

		public override List<string> SetNPCNameList()
		{
			return new List<string>()
			{
				"'That Guy'", "Shade", "Enigma", "Delphian", "Cloak"
			};
		}

		public static bool IsNpcOnscreen(Vector2 center)
		{
			int w = NPC.sWidth + NPC.safeRangeX * 2;
			int h = NPC.sHeight + NPC.safeRangeY * 2;
			Rectangle npcScreenRect = new Rectangle((int)center.X - w / 2, (int)center.Y - h / 2, w, h);
			foreach (Player player in Main.player)
			{
				// If any player is close enough to the traveling merchant, it will prevent the npc from despawning
				if (player.active && player.getRect().Intersects(npcScreenRect)) return true;
			}
			return false;
		}


		// Consider using this alternate approach to choosing a random thing. Very useful for a variety of use cases.
		// The WeightedRandom class needs "using Terraria.Utilities;" to use
		public override string GetChat()
		{
			WeightedRandom<string> chat = new WeightedRandom<string>();

			int npc = NPC.FindFirstNPC(NPCID.ArmsDealer);
			if (npc >= 0 && Main.rand.NextBool(2))
			{
				chat.Add(Main.npc[npc].GivenName + " thinks he's got the rare stuff, take a look at what I got.");
			}
			/*
			if (ModLoader.GetMod("CalamityMod") != null)
			{
				npc = NPC.FindFirstNPC(ModLoader.GetMod("CalamityMod").Find<ModNPC>("FAP").Type);
				if (npc >= 0 && Main.rand.NextBool(2))
				{
					chat.Add("I coulda swore I saw " + Main.npc[npc].GivenName + " riding some pastel pony, it might have just been those Unicorns around here, but that one had wings too!");
				} 
			}
			*/
			/*
			if (ModLoader.GetMod("StartWithBase") != null)
			{
				chat.Add("I'm not going to make fun of your auto-generated base; if you are anything like me, I hate base building too.");
			}
			*/


			chat.Add("I am dark, mysterious, edgy, and the ideal neckbeard for your party");
			chat.Add("Pretty interesting little settlement you got here.");
			chat.Add("Your pretty brave to be talking to me.");
			chat.Add("Prove your mettle, and I might make it worth your time.");
			chat.Add("I hope you have... Deep pockets.");
			chat.Add("Why yes am I profiting off you, because you can't escape the middleman here.");
			chat.Add("Wrong? Pfff, when that one guy is standing in the way you kill them with rotten eggs! And you think what I do is wrong...");
			chat.Add("Me a fence? With what proof, it's not like guards instantly know where I am when I steal a sweetroll.");
			chat.Add("Sure I may be a recolor but at least I don't use my vanity slots as storage.");
			chat.Add("No, I don't know about this 'Epic Store', stop asking me to sell stuff from it.");
			chat.Add("I'd like to see you try getting what I sell on your own.");
			chat.Add("I like to live on the edge, and part of that includes not living in your crude dwellings.");
			chat.Add("Only I would sell dragon bones, because I know 'he' would protect those creatures.");
			chat.Add("I don't magically get my wares without leaving, if your wondering why I'm not selling anything new after talking to me...", 2.0);
			chat.Add("After you have defeated powerful foes, Check back with me the next time I return as I may have something for you.", 2.0);
			//chat.Add("This message has a weight of 5, meaning it appears 5 times more often.", 5.0);
			//chat.Add("This message has a weight of 0.1, meaning it appears 10 times as rare.", 0.1);

			/*
			if (Main.LocalPlayer.HasItem(ModContent.ItemType<OverseenCrystal>()))
			{
				string[] lines = { "Awe fantastic you got some [i: " + ModContent.ItemType<OverseenCrystal>() + "]! Shhhhh follow me and look inside...",
						"Excellent finds, come see what I got to offer!"};
				chat.Add(lines[Main.rand.Next(lines.Length)], 10);
			}
			*/

			/*
			if (Main.rand.NextBool(3))
			{
				string[] lines = { "Hey if you happen to come across some [i: " + ModContent.ItemType<OverseenCrystal>() + "] be sure to let me know! I... May have under-the-table goods for sale, if you catch my drift",
						"Got any [i: " + ModContent.ItemType<OverseenCrystal>() + "] to trade, quietly? Preferable away from the others?"};
				chat.Add(lines[Main.rand.Next(lines.Length)], 5);
			}
			*/

			if (Main.dayTime) {
				chat = new WeightedRandom<string>();
				chat.Add("Nothing for sale while the sun shines, it's blistering bright glow...");
				chat.Add("If you'll excuse me, I need to pack up and leave.");
				chat.Add("I'll be back another night, but my time is up for now.");
				chat.Add("You're a bit late aren't ya?");
			}

			return chat; // chat is implicitly cast to a string. You can also do "return chat.Get();" if that makes you feel better
		}

		public override void AI()
		{
			if (itemRandomizer == 0)
			{
				UnifiedRandom rando = new UnifiedRandom(Main.worldName.GetHashCode() + NPC.Center.GetHashCode());

				itemRandomizer = rando.Next();
			}
			if ((Main.dayTime) && !ContrabandMerchant.IsNpcOnscreen(NPC.Center))
			{
				if (Main.netMode == NetmodeID.SinglePlayer) Main.NewText(NPC.FullName + " has departed!", 50, 125, 255);
				else ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(NPC.FullName + " has departed!"), new Color(50, 125, 255));
				NPC.active = false;
				NPC.netSkip = -1;
				NPC.life = 0;
			}
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = Language.GetTextValue("LegacyInterface.28");
			if (Main.dayTime)
			{
				button = "Closed";
			}
			else
			{

			}
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop)
		{
			if (firstButton)
			{
				if (!Main.dayTime)
					shop = Shop1;
				else
					Main.npcChatText = "I won't sell anything at this time, come back later.";
			}
		}

        public override void AddShops()
        {
			randz = new UnifiedRandom(itemRandomizer);

            Condition random10LessThanNum(int num) => new("Randomly", () => randz.Next(10) < num);

            NPCShop npcShop = new NPCShop(Type, Shop1)

            //if (Main.LocalPlayer.HasItem(ItemID.AncientCloth))
            //{

                .Add(new Item(ItemID.IronPickaxe) { shopCustomPrice = 30, shopSpecialCurrency = CrateCurrencyCustomCurrencyID })
                .Add(new Item(ItemID.IronPickaxe) { shopCustomPrice = randz.Next(3, 7), shopSpecialCurrency = CrateCurrencyCustomCurrencyID })
            /*
			shop.item[nextSlot].SetDefaults(ItemID.IronPickaxe);
			// shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Accessories.LiquidGambling>());
			shop.item[nextSlot].shopCustomPrice = 30;
			shop.item[nextSlot].shopSpecialCurrency = ContrabandMerchant.CrateCurrencyCustomCurrencyID;
			nextSlot++;

			shop.item[nextSlot].SetDefaults(ItemID.IronPickaxe);
			// shop.item[nextSlot].SetDefaults(ModContent.ItemType<LootBoxVanillaPotions>());
			shop.item[nextSlot].shopCustomPrice = randz.Next(3, 7);
			shop.item[nextSlot].shopSpecialCurrency = ContrabandMerchant.DesertFossilCurrencyCustomCurrencyID;
			nextSlot++;
            */

            /*
			if (NPC.CountNPCS(ModContent.NPCType<Dimensions.NPCs.DungeonPortal>()) > 0)
			{
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<LootBoxDeeperDungeons>());
				shop.item[nextSlot].shopCustomPrice = Main.netMode == NetmodeID.MultiplayerClient ? randz.Next(4, 12) : randz.Next(20, 45);
				shop.item[nextSlot].shopSpecialCurrency = ContrabandMerchant.DesertFossilCurrencyCustomCurrencyID;
				nextSlot++;
			}
			*/

            //if (!Main.hardMode)
			//	return;

                .Add(new Item(ItemID.IronPickaxe) { shopCustomPrice = randz.Next(3, 8), shopSpecialCurrency = AncientClothCurrencyCustomCurrencyID },
                    Condition.Hardmode, random10LessThanNum(8))
                .Add(new Item(ItemID.IronPickaxe) { shopCustomPrice = 1 + randz.Next(1, 3), shopSpecialCurrency = AncientClothCurrencyCustomCurrencyID },
                    Condition.Hardmode, random10LessThanNum(8))
                .Add(new Item(ItemID.IronPickaxe) { shopCustomPrice = 1 + randz.Next(1, 3), shopSpecialCurrency = AncientClothCurrencyCustomCurrencyID },
                    Condition.Hardmode, random10LessThanNum(8))
                .Add(new Item(ItemID.IronPickaxe) { shopCustomPrice = randz.Next(30, 51), shopSpecialCurrency = GlowrockCustomCurrencyID },
                    Condition.Hardmode, random10LessThanNum(8))
                .Add(new Item(ItemID.IronPickaxe) { shopCustomPrice = randz.Next(8, 29), shopSpecialCurrency = GlowrockCustomCurrencyID },
                    Condition.Hardmode, random10LessThanNum(8))
                .Add(new Item(ItemID.IronPickaxe) { shopCustomPrice = randz.Next(8, 29), shopSpecialCurrency = GlowrockCustomCurrencyID },
                    Condition.Hardmode, random10LessThanNum(8))
                .Add(new Item(ItemID.IronPickaxe) { shopCustomPrice = 100, shopSpecialCurrency = OverseenCrystalCustomCurrencyID },
                    Condition.Hardmode, random10LessThanNum(8));
            npcShop.Register();

            /*
			if (randz.Next(10) < 8)
			{
				shop.item[nextSlot].SetDefaults(ItemID.IronPickaxe);
				// shop.item[nextSlot].SetDefaults(ModContent.ItemType<LootBoxAccessories>());
				shop.item[nextSlot].shopCustomPrice = randz.Next(3, 8);
				shop.item[nextSlot].shopSpecialCurrency = ContrabandMerchant.AncientClothCurrencyCustomCurrencyID;
				nextSlot++;
			}
			if (randz.Next(10) < 8)
			{
				shop.item[nextSlot].SetDefaults(ItemID.IronPickaxe);
				// shop.item[nextSlot].SetDefaults(ModContent.ItemType<LootBoxVanillaPotions>());
				shop.item[nextSlot].shopCustomPrice = 1 + randz.Next(1, 3);
				shop.item[nextSlot].shopSpecialCurrency = ContrabandMerchant.AncientClothCurrencyCustomCurrencyID;
				nextSlot++;
			}

			if (randz.Next(10) < 8)
			{
				shop.item[nextSlot].SetDefaults(ItemID.IronPickaxe);
				// shop.item[nextSlot].SetDefaults(ModContent.ItemType<LootBoxVanillaHardmodePotions>());
				shop.item[nextSlot].shopCustomPrice = 1 + randz.Next(1, 3);
				shop.item[nextSlot].shopSpecialCurrency = ContrabandMerchant.AncientClothCurrencyCustomCurrencyID;
				nextSlot++;
			}

			if (randz.Next(10) < 8)
			{
				shop.item[nextSlot].SetDefaults(ItemID.IronPickaxe);
				// shop.item[nextSlot].SetDefaults(ModContent.ItemType<LootBoxAccessories>());
				shop.item[nextSlot].shopCustomPrice = randz.Next(30, 51);
				shop.item[nextSlot].shopSpecialCurrency = ContrabandMerchant.GlowrockCustomCurrencyID;
				nextSlot++;
			}
			if (randz.Next(10) < 8)
			{
				shop.item[nextSlot].SetDefaults(ItemID.IronPickaxe);
				// shop.item[nextSlot].SetDefaults(ModContent.ItemType<LootBoxPotions>());
				shop.item[nextSlot].shopCustomPrice = randz.Next(8, 29);
				shop.item[nextSlot].shopSpecialCurrency = ContrabandMerchant.GlowrockCustomCurrencyID;
				nextSlot++;
			}

			if (randz.Next(10) < 8)
			{
				shop.item[nextSlot].SetDefaults(ItemID.IronPickaxe);
				// shop.item[nextSlot].SetDefaults(ModContent.ItemType<LootBoxVanillaHardmodePotions>());
				shop.item[nextSlot].shopCustomPrice = randz.Next(8, 29);
				shop.item[nextSlot].shopSpecialCurrency = ContrabandMerchant.GlowrockCustomCurrencyID;
				nextSlot++;
			}
            */

            /*
			if (Main.LocalPlayer.HasItem(ModContent.ItemType<OverseenCrystal>()))
			{
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<LootBoxAccessoriesEX>());
				shop.item[nextSlot].shopCustomPrice = 100;
				shop.item[nextSlot].shopSpecialCurrency = ContrabandMerchant.OverseenCrystalCustomCurrencyID;
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
			projType = ModContent.ProjectileType<ContrabandMerchantPoof>();
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 12f;
			randomOffset = 3f;
		}
	}

	public class ContrabandMerchantProfile : ITownNPCProfile
	{
		public string Path => (GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/');

		public int RollVariation() => 0;
		public string GetNameForVariant(NPC npc) => npc.getNewNPCName();

		public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
		{
			if (npc.IsABestiaryIconDummy && !npc.ForcePartyHatOn)
			{
				return ModContent.Request<Texture2D>(Path);
			}

			if (npc.altTexture == 1)
			{
				return ModContent.Request<Texture2D>(Path + "_Alt");
			}

			return ModContent.Request<Texture2D>(Path);
		}

		public int GetHeadTextureIndex(NPC npc)
		{
			return ModContent.GetModHeadSlot(Path + "_Head");
		}
	}

	public class ContrabandMerchantPoof : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Contra poof! Annnndd he's gone! Bummer for you!");
		}

		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 2;
			Projectile.damage = 0;
		}

		public override void AI()
		{
			if (Projectile.ai[0] < 1)
			{
				for (float i = 0f; i < 2f; i += 0.05f)
				{
					Vector2 circle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000));
					circle = circle.SafeNormalize(Vector2.Zero);
					int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, circle.X * i, circle.Y * i);
					Main.dust[dust].scale = Main.rand.NextFloat(1f, 3f);
					Main.dust[dust].noGravity = false;
					Main.dust[dust].alpha = 100;
					Main.dust[dust].velocity = circle * i;
				}
				CombatText.NewText(new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height), Main.DiscoColor, "Oh snap the Fuzz! Gotta run!", true);
			}
			int npcguy = NPC.FindFirstNPC(ModContent.NPCType<ContrabandMerchant>());

			if (npcguy >= 0)
			{
				Main.npc[npcguy].active = false;
				Main.npc[npcguy].type = NPCID.None;
			}
			Projectile.ai[0] += 1;
		}
	}

	/*
	public class DevArmorItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Congrats!");
			Tooltip.SetDefault("You're Winner! Unusual effect: Dev Armor!");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 16;
			Item.height = 16;
			Item.value = 250;
			Item.rare = ItemRarityID.Blue;
		}

        public override bool OnPickup(Player player)
        {
			SGAGlobalItem.AwardSGAmodDevArmor(player);
			return false;
        }

        public override string Texture => "Terraria/Confuse";

    }
	*/

	public class GlowrockCurrency : CustomCurrencySingleCoin
	{
		public Color SGACustomCurrencyTextColor = Color.Cyan;

		public GlowrockCurrency(int coinItemID, long currencyCap) : base(coinItemID, currencyCap)
		{
		}

		public override void GetPriceText(string[] lines, ref int currentLine, long price)
		{
			Color color = SGACustomCurrencyTextColor * ((float)Main.mouseTextColor / 255f);
			SGAPlayer modplayer = Main.LocalPlayer.GetModPlayer<SGAPlayer>();
			lines[currentLine++] = string.Format("[c/{0:X2}{1:X2}{2:X2}:{3} {4} {5}]", new object[]
				{
					color.R,
					color.G,
					color.B,
					Language.GetTextValue("LegacyTooltip.50"),
					price,
					"Glowrock"
				});
		}
	}

	public class CrateCurrency : CustomCurrencySingleCoin
	{
		public Color SGACustomCurrencyTextColor = Color.Goldenrod;

		public CrateCurrency(int coinItemID, long currencyCap) : base(coinItemID, currencyCap)
		{
		}

		public override void GetPriceText(string[] lines, ref int currentLine, long price)
		{
			Color color = SGACustomCurrencyTextColor * ((float)Main.mouseTextColor / 255f);
			SGAPlayer modplayer = Main.LocalPlayer.GetModPlayer<SGAPlayer>();
			lines[currentLine++] = string.Format("[c/{0:X2}{1:X2}{2:X2}:{3} {4} {5}]", new object[]
				{
					color.R,
					color.G,
					color.B,
					Language.GetTextValue("LegacyTooltip.50"),
					price,
					"Terraria Co Supply Crate"
				});
		}
	}

	public class OverseenCrystalCurrency : CustomCurrencySingleCoin
	{
		public Color SGACustomCurrencyTextColor = Color.SkyBlue;

		public OverseenCrystalCurrency(int coinItemID, long currencyCap) : base(coinItemID, currencyCap)
		{
		}

		public override void GetPriceText(string[] lines, ref int currentLine, long price)
		{
			Color color = SGACustomCurrencyTextColor * ((float)Main.mouseTextColor / 255f);
			SGAPlayer modplayer = Main.LocalPlayer.GetModPlayer<SGAPlayer>();
			lines[currentLine++] = string.Format("[c/{0:X2}{1:X2}{2:X2}:{3} {4} {5}]", new object[]
				{
					color.R,
					color.G,
					color.B,
					Language.GetTextValue("LegacyTooltip.50"),
					price,
					"Overseen Crystal"
				});
		}
	}
	public class AncientClothCurrency : CustomCurrencySingleCoin
	{
		public Color SGACustomCurrencyTextColor = Color.LightGoldenrodYellow;

		public AncientClothCurrency(int coinItemID, long currencyCap) : base(coinItemID, currencyCap)
		{
		}

		public override void GetPriceText(string[] lines, ref int currentLine, long price)
		{
			Color color = SGACustomCurrencyTextColor * ((float)Main.mouseTextColor / 255f);
			SGAPlayer modplayer = Main.LocalPlayer.GetModPlayer<SGAPlayer>();
			lines[currentLine++] = string.Format("[c/{0:X2}{1:X2}{2:X2}:{3} {4} {5}]", new object[]
				{
					color.R,
					color.G,
					color.B,
					Language.GetTextValue("LegacyTooltip.50"),
					price,
					"Ancient Cloth"
				});
		}
	}	
	public class DesertFossilCurrency : CustomCurrencySingleCoin
	{
		public Color SGACustomCurrencyTextColor = Color.Orange;

		public DesertFossilCurrency(int coinItemID, long currencyCap) : base(coinItemID, currencyCap)
		{
		}

		public override void GetPriceText(string[] lines, ref int currentLine, long price)
		{
			Color color = SGACustomCurrencyTextColor * ((float)Main.mouseTextColor / 255f);
			SGAPlayer modplayer = Main.LocalPlayer.GetModPlayer<SGAPlayer>();
			lines[currentLine++] = string.Format("[c/{0:X2}{1:X2}{2:X2}:{3} {4} {5}]", new object[]
				{
					color.R,
					color.G,
					color.B,
					Language.GetTextValue("LegacyTooltip.50"),
					price,
					"Sturdy Fossil"
				});
		}
	}
}
