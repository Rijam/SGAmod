using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;

namespace SGAmod.Tiles.Environment
{
	public class NullWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = false;
			DustType = DustID.Smoke;
			if (GetType() == typeof(NullWall))
				AddMapEntry(new Color(0, 0, 0));
		}

		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}

		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override void KillWall(int i, int j, ref bool fail)
		{
			fail = true;
		}
	}
	/*public class NullWallBossArena : NullWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = false;
			DustType = DustID.Smoke;
			AddMapEntry(new Color(200, 0, 0));
		}
		public override string Texture => "SGAmod/Tiles/Environment/NullWall";
	}

	public class CrimsonFakeWall : NullWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = false;
			DustType = DustID.Smoke;
			AddMapEntry(new Color(100, 0, 0));
		}
		public override string Texture => "Terraria/Images/Wall_" + WallID.CrimsonUnsafe4;
	}

	public class CorruptionFakeWall : NullWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = false;
			DustType = DustID.Smoke;
			AddMapEntry(new Color(100, 0, 100));
		}
		public override string Texture => "Terraria/Images/Wall_" + WallID.CorruptionUnsafe4;
	}

	public class UnbreakableIridescentBrick : NullWall, IBreathableWallType
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = false;
			DustType = DustID.Smoke;
			if (GetType() == typeof(UnbreakableIridescentBrick))
			AddMapEntry(new Color(50, 50, 50));
		}

		public override string Texture => "Terraria/Images/Wall_" + WallID.IridescentBrick;
	}
	public class UnbreakableTinPlating : UnbreakableIridescentBrick, IBreathableWallType
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DustType = DustID.Smoke;
			AddMapEntry(new Color(200, 160, 160));
		}
		public override string Texture => "Terraria/Images/Wall_" + WallID.TinPlating;
	}
	public class UnbreakableBubblegumWall : UnbreakableIridescentBrick, IBreathableWallType
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DustType = 16;
			AddMapEntry(new Color(180, 140, 140));
		}
		public override string Texture => "Terraria/Images/Wall_" + WallID.BubblegumBlock;
	}*/
	/*
	public class UnbreakableStainedGlass : UnbreakableIridescentBrick, IBreathableWallType
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DustType = DustID.Smoke;
		}
		public override string Texture => "Terraria/Images/Wall_" + WallID.BubblegumBlock;
		public override bool IsLoadingEnabled(Mod mod) //  tModPorter Suggestion: If you return false for the purposes of manual loading, use the [Autoload(false)] attribute on your class instead
		{
			int[] allGlass = { WallID.BlueStainedGlass, WallID.GreenStainedGlass, WallID.PurpleStainedGlass, WallID.RedStainedGlass, WallID.YellowStainedGlass};
			for (int i = 0; i < allGlass.Length; i += 1)
			{
				//texture = "Terraria/Wall_" + WallID.BlueStainedGlass;
				Mod.AddWall("UnbreakableStainedGlass"+i, new UnbreakableStainedGlass(), "Terraria/Wall_" + allGlass[i]);
			}
			return false;
		}

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Vector2 zerooroffset = Main.drawToScreen ? Vector2.Zero : new Vector2((float)Main.offScreenRange);
			Tile tile = Framing.GetTileSafely(i, j);

			//if (tile.wallColor() > 0)
			//	return true;

			Rectangle rect = new(tile.WallFrameX, tile.WallFrameY, 32, 32);

			spriteBatch.Draw(Main.wallTexture[tile.WallType], zerooroffset + (new Vector2(i, j) * 16) - Main.screenPosition, rect, Lighting.GetColor(i,j,Color.White)*0.75f, 0, new Vector2(rect.Width, rect.Height) / 4f, new Vector2(1f, 1f), SpriteEffects.None, 0f);

			return false;
		}
	}
	*/
}