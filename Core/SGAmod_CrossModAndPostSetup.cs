using SGAmod.NPCs.Bosses.CopperWraith;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using SGAmod.Items.Consumables.BossSummons;
using SGAmod.Items.Materials.BossDrops;
using Terraria.Localization;
using SGAmod.Items.Consumables.Other;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SGAmod.Items.Accessories.Expert;
using SGAmod.Items.Weapons.Shields;
using SGAmod.NPCs.Bosses.SpiderQueen;
using SGAmod.NPCs.Bosses.CobaltWraith;

namespace SGAmod
{
	public partial class SGAmodSystem : ModSystem
	{
		public override void PostSetupContent()
		{
			
			if (ModLoader.TryGetMod("BossChecklist", out Mod bossList))
			{
				bossList.Call("LogBoss",
					Mod,
					nameof(CopperWraith),
					0.05f,
					() => SGAWorld.downedCopperWraith,
					ModContent.NPCType<CopperWraith>(),
					new Dictionary<string, object>()
					{
						["spawnInfo"] = Language.GetOrRegister("Mods.SGAmod.NPCs.CopperWraith.SpawnInfo"),
						["spawnItems"] = ModContent.ItemType<WraithCoreFragment>(),
						["collectibles"] = ModContent.ItemType<TrueCopperWraithNotch>(),
						["customPortrait"] = (SpriteBatch spriteBatch, Rectangle rect, Color color) => 
						{ 
							Texture2D texture = ModContent.Request<Texture2D>("SGAmod/NPCs/Bosses/CopperWraith/CopperWraithLog").Value;
							Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
							spriteBatch.Draw(texture, centered, color);
						},
						["despawnMessage"] = Language.GetOrRegister("Mods.SGAmod.NPCs.CopperWraith.DespawnMessage")
					} /*0.05f, ModContent.NPCType<CopperWraith>(), this, "Copper Wraith", (Func<bool>)(() => (SGAWorld.downedWraiths > 0)), ModContent.ItemType<WraithCoreFragment>(), new List<int>() { }, new List<int>() { ModContent.ItemType<CopperWraithShard>(), ModContent.ItemType<TinWraithShard>(), ItemID.CopperOre, ItemID.TinOre, ItemID.IronOre, ItemID.LeadOre, ItemID.SilverOre, ItemID.TungstenOre, ItemID.GoldOre, ItemID.PlatinumOre }, "Use a [i:" + ModContent.ItemType<WraithCoreFragment>() + "] at anytime, will also spawn should you craft too many bars at a furnace before beating it", "Copper Wraith makes a hasty retreat", "SGAmod/NPCs/Wraiths/CopperWraithLog", "SGAmod/NPCs/Wraiths/CopperWraith_Head_Boss"*/
				);

				//bossList.Call("AddBoss", 3.5f, ModContent.NPCType<SpiderQueen>(), this, "Spider Queen", (Func<bool>)(() => SGAWorld.downedSpiderQueen), new List<int>() { ModContent.ItemType<AcidicEgg>() }, new List<int>() { }, new List<int>() { ModContent.ItemType<Items.Materials.BossDrops.VialOfAcid>(), ModContent.ItemType<AmberGlowSkull>(), ModContent.ItemType<CorrodedShield>(), ModContent.ItemType<AlkalescentHeart>() }, "Use a [i: " + ItemType("AcidicEgg") + "] underground, anytime", "The Spider Queen will be feasting quite well tonight", "SGAmod/NPCs/SpiderQueen/SpiderQueenLog");
				bossList.Call("LogBoss",
					Mod,
					nameof(SpiderQueen),
					3.5f,
					() => SGAWorld.downedSpiderQueen,
					ModContent.NPCType<SpiderQueen>(),
					new Dictionary<string, object>()
					{
						["spawnInfo"] = Language.GetOrRegister("Mods.SGAmod.NPCs.SpiderQueen.SpawnInfo"),
						["spawnItems"] = ModContent.ItemType<AcidicEgg>(),
						["customPortrait"] = (SpriteBatch spritebatch,  Rectangle rect, Color color) =>
						{
							Texture2D texture = ModContent.Request<Texture2D>("SGAmod/NPCs/Bosses/SpiderQueen/SpiderQueenLog").Value;
							Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
							spritebatch.Draw(texture, centered, color);
						},
						["despawnMessage"] = Language.GetOrRegister("Mods.SGAmod.NPCs.SpiderQueen.DespawnMessage")
					}
				);
				//bossList.Call("AddBoss", 6.4f, ModContent.NPCType<CobaltWraith>(), this, "Cobalt Wraith", (Func<bool>)(() => (SGAWorld.downedWraiths > 1)), ModContent.ItemType<WraithCoreFragment2>(), new List<int>() { }, new List<int>() { ModContent.ItemType<WraithFragment4>(), ItemID.Hellstone, ItemID.SoulofLight, ItemID.SoulofNight, ItemID.PalladiumOre, ItemID.CobaltOre, ItemID.MythrilOre, ItemID.OrichalcumOre, ItemID.AdamantiteOre, ItemID.TitaniumOre }, "Use a [i:" + ItemType("WraithCoreFragment2") + "] at anytime, defeat this boss to unlock crafting a hardmode forge, as well as anything crafted at one", "Cobalt Wraith completes its mission", "SGAmod/NPCs/Wraiths/CobaltWraithLog", "SGAmod/NPCs/Wraiths/CobaltWraith_Head_Boss");
				bossList.Call("LogBoss",
					Mod,
					nameof(CobaltWraith),
					7.4f,
					() => SGAWorld.downedCobaltWraith,
					ModContent.NPCType<CobaltWraith>(),
					new Dictionary<string, object>()
					{
						["spawnInfo"] = Language.GetOrRegister("Mods.SGAmod.NPCs.CobaltWraith.SpawnInfo"),
						["spawnItems"] = ModContent.ItemType<EmpoweredWraithCoreFragment>(),
						["customPortrait"] = (SpriteBatch spritebatch, Rectangle rect, Color color) =>
						{
							Texture2D texture = ModContent.Request<Texture2D>("SGAmod/NPCs/Bosses/CobaltWraith/CobaltWraithLog").Value;
							Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
							spritebatch.Draw(texture, centered, color);
						},
						["despawnMessage"] = Language.GetOrRegister("Mods.SGAmod.NPCs.CobaltWraith.DespawnMessage")
					});
			}
		}
	}
}
