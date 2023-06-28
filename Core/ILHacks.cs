using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Reflection;
using Microsoft.Xna.Framework;

namespace SGAmod
{
	public class SGAILHacks
	{
		//Welcome to IDG's collection of vanilla hacking nonsense!
		internal static void Patch()
		{
			//SGAmod.Instance.Logger.Debug("Loading an unhealthy amount of IL patches");
			SGAmod.Instance.Logger.Debug("Loading IL Edits");
			Terraria.IL_Player.StickyMovement += AddCustomWebsCollision;

			//PrivateClassEdits.ApplyPatches();
		}
		internal static void Unpatch()
		{
			//PrivateClassEdits.RemovePatches();
		}

		//Allows custom-tiles for web collisions

		private delegate int CollisionOtherCobwebsDelegate(Player player, int starterNumber);
		private static int CollisionOtherCobwebsMethod(Player player, int starterNumber)
		{
			if (SGAWorld.modtimer > 120)
			{
				int tile = ModContent.TileType<NPCs.Bosses.SpiderQueen.AcidicWebTile>();
				if (starterNumber == tile)
				{
					player.AddBuff(ModContent.BuffType<Buffs.Debuffs.AcidBurn>(), 2);
					starterNumber = TileID.Cobweb;
				}
			}
			return starterNumber;
		}

		private delegate void CollisionAddStickyDelegate(Player player);
		private static void CollisionAddStickyMethod(Player player)
		{
			if (player.GetModPlayer<SGAPlayer>().cobwebRepellent > 0)
				player.stickyBreak += 2;
		}

		private delegate Vector2 ModdedCobwebsHereDelegate(Player player, Vector2 sticky);
		private static Vector2 ModdedCobwebsHereMethod(Player player, Vector2 sticky)
		{
			if (SGAWorld.modtimer > 120)
			{
				int tile = ModContent.TileType<NPCs.Bosses.SpiderQueen.AcidicWebTile>();

				for (int x = 0; x < 2; x++)
				{
					for (int y = 0; y < 2; y++)
					{
						Vector2 here = (player.position / 16f) + new Vector2(x, y);
						// Make sure the tiles are inside the world.
						here.X = Math.Clamp(here.X, 0, Main.maxTilesX);
						here.Y = Math.Clamp(here.Y, 0, Main.maxTilesY);
						if (Main.tile[(int)here.X, (int)here.Y].TileType == tile)
						{
							sticky = here;
							return sticky;
						}
					}
				}
			}

			return sticky;
		}

		private static void AddCustomWebsCollision(ILContext il)
		{
			ILCursor c = new(il);

			if (c.TryGotoNext(MoveType.After, i => i.MatchStloc(3)))
			{
				c.Emit(OpCodes.Ldarg_0); // Loads the argument at index 0 onto the evaluation stack.
				c.Emit(OpCodes.Ldloc, 3); // Loads the local variable at a specific index onto the evaluation stack.
				c.EmitDelegate<ModdedCobwebsHereDelegate>(ModdedCobwebsHereMethod);
				c.Emit(OpCodes.Stloc, 3); // Pops the current value from the top of the evaluation stack and stores it in the local variable list at a specified index.


				if (c.TryGotoNext(MoveType.After, i => i.MatchStloc(6)))
				{
					c.Emit(OpCodes.Ldarg_0);
					c.Emit(OpCodes.Ldloc, 6);
					c.EmitDelegate<CollisionOtherCobwebsDelegate>(CollisionOtherCobwebsMethod);
					c.Emit(OpCodes.Stloc, 6);

					if (c.TryGotoNext(MoveType.After, i => i.MatchStfld<Player>("stickyBreak")))
					{
						c.Emit(OpCodes.Ldarg_0);
						c.EmitDelegate<CollisionAddStickyDelegate>(CollisionAddStickyMethod);
						return;
					}
					SGAmod.Instance.Logger.Error("IL Error: Could not patch AddCustomWebsCollision at 3");
					//throw new Exception("IL Error: Could not patch AddCustomWebsCollision at 3");
				}
				SGAmod.Instance.Logger.Error("IL Error: Could not patch AddCustomWebsCollision at 2");
				//throw new Exception("IL Error: Could not patch AddCustomWebsCollision at 2");
			}
			SGAmod.Instance.Logger.Error("IL Error: Could not patch AddCustomWebsCollision at 1");
			//throw new Exception("IL Error: Could not patch AddCustomWebsCollision at 1");
		}
	}
}