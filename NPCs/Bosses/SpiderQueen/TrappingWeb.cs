using System;
using System.Collections.Generic;
using System.Linq;
using Idglibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;


namespace SGAmod.NPCs.Bosses.SpiderQueen
{
	public class TrappingWeb : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Trapping Web");
		}
		
		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Boulder);
			AIType = ProjectileID.Boulder;	  
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.penetrate = 10;
			Projectile.light = 0.5f;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.tileCollide = true;
			Projectile.penetrate = 1;
		}

		public override string Texture => "Terraria/Images/Item_" + ItemID.Cobweb;

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.penetrate--;
			if (Projectile.penetrate <= 0)
			{
				Projectile.Kill();
			}
			return false;
		}

		public override bool PreKill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item45, Projectile.Center);

			int i = (int)Projectile.Center.X/16;
			int j = (int)Projectile.Center.Y/16;
			for (int x = -8; x <= 8; x++)
			{
				for (int y = -8; y <= 8; y++)
				{
					//WorldGen.Convert(i + x, j + y, 0, 0);
					Tile tile = Main.tile[i + x, j + y];
					if (!tile.HasTile && Main.rand.NextFloat(0,(new Vector2(x, y)).Length()) < 1)
					{
						if (Main.netMode == NetmodeID.SinglePlayer || Main.dedServ)
						{
							AcidicWebTile.checkForWebRemoval = true;
							AcidicWebTile.websToRemove.Add(new Point(i + x, j + y));
						}
						//tile.type = (ushort)ModContent.TileType<AcidicWebTile>();
						WorldGen.PlaceTile(i + x, j + y,(ushort)ModContent.TileType<AcidicWebTile>());
						tile.HasTile = true;
						NetMessage.SendTileSquare(Main.myPlayer, i + x, j + y, 1, 1);
					}
				}
			}

			if (Main.netMode == NetmodeID.SinglePlayer || Main.dedServ)
			{
				AcidicWebTile.websToRemove = AcidicWebTile.websToRemove.Distinct().ToList();
			}

			for (int k = 0; k < 80; k++)
			{
				Vector2 randomcircle = new(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				randomcircle *= Main.rand.NextFloat(0f, 6f);
				int dust = Dust.NewDust(new Vector2(Projectile.position.X - 1, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Web, 0, 0, 50, Color.Lime, Projectile.scale * 2f);
				Main.dust[dust].noGravity = false;
				Main.dust[dust].velocity = new Vector2(randomcircle.X, randomcircle.Y);
			}

			return true;
		}

		
		public override void AI()
		{
			Projectile.rotation = ((float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f)+MathHelper.ToRadians(90);

			int DustID2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height,DustID.Web, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 20, Color.Lime, 1.5f);
			Main.dust[DustID2].noGravity = true;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, null, Color.Lime, 0f, TextureAssets.Projectile[Projectile.type].Value.Size()/2f, 1, SpriteEffects.None, 0f);

			return false;
		}
	}


	public class AcidicWebTile : ModTile
	{
		public static List<Point> websToRemove = new();
		public static bool checkForWebRemoval = false;

		public static void RemoveWebs()
		{
			if (!checkForWebRemoval)
				return;

			if (NPC.CountNPCS(ModContent.NPCType<SpiderQueen>()) > 0)
				return;

			if (Main.netMode == NetmodeID.SinglePlayer || Main.dedServ)
			{
				for (int i = 0; i < 1 + (websToRemove.Count / 50); i += 1)
				{

					websToRemove = websToRemove.OrderBy(testby => Main.rand.Next()).ToList();

					AcidicWebTile.checkForWebRemoval = true;

					Point tilecoord = websToRemove[0];

					Tile tile = Framing.GetTileSafely(tilecoord.X, tilecoord.Y);

					if (tile.TileType == ModContent.TileType<AcidicWebTile>())
					{
						WorldGen.KillTile(tilecoord.X, tilecoord.Y);

						NetMessage.SendTileSquare(Main.myPlayer, tilecoord.X, tilecoord.Y, 1, 1);
					}

					websToRemove.RemoveAt(0);
				}

				if (websToRemove.Count < 1)
				{
					AcidicWebTile.checkForWebRemoval = false;
				}
			}
		}

		public override bool IsLoadingEnabled(Mod mod)
		{
			SGAmodSystem.PostUpdateEverythingEvent += RemoveWebs;
			return true;
		}
		public override void SetStaticDefaults()
		{
			TileID.Sets.NotReallySolid[Type] = true;
			Main.tileCut[Type] = true;
			Main.tileNoFail[Type] = true;
			Main.tileSolid[Type] = false;
			HitSound = SoundID.Grass;
			MineResist = 1f;
			DustType = DustID.Web;
			TileID.Sets.CanBeClearedDuringGeneration[Type] = true;
			AddMapEntry(Color.Lime, CreateMapEntryName());
		}

		public override string Texture => "Terraria/Images/Tiles_" + TileID.Cobweb;

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Vector2 zerooroffset = Main.drawToScreen ? Vector2.Zero : new Vector2((float)Main.offScreenRange);
			Vector2 location = (new Vector2(i, j) * 16)+zerooroffset;
			Tile tile = Main.tile[i, j];

			NoiseGenerator Noisegen = new(WorldGen._lastSeed)
			{
				Amplitude = 0.50f
			};
			Noisegen.Frequency *= 1.00;

			spriteBatch.Draw(TextureAssets.Tile[tile.TileType].Value,
				location - Main.screenPosition,
				new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16),
				Color.Lerp(Color.DarkGreen * 0.250f, Color.Lime, 0.50f + (float)Noisegen.Noise((int)(location.X + Main.GlobalTimeWrappedHourly * 10f),
				(int)(location.Y+Main.GlobalTimeWrappedHourly * 10f))),
				0f, Vector2.Zero, 1, SpriteEffects.None, 0f);

			return false;
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if (Main.tile[i, j].HasTile)
			{
				g = Color.Lime.G * 0.25f;
			}
		}
	}
}