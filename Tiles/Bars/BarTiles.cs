using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using SGAmod.Items.Materials.Bars;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;

namespace SGAmod.Tiles.Bars
{
    public class BarTiles : ModTile
    {
        public override void SetStaticDefaults()
        {
            // TileID.Sets.Ore[Type] = true;
            Main.tileShine[Type] = 2100;
            Main.tileSolid[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLighted[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Metal Bar");
            AddMapEntry(Color.White, name); // TODO: add the different colors for the bars (even though vanilla's is just one color)
        }

		public override bool Drop(int i, int j)
		{
			Tile t = Main.tile[i, j];
			int style = t.TileFrameX / 18;
			var item = style switch
			{
				0 => ItemType<PhotosyteBar>(),
				1 => ItemType<NoviteBar>(),
				2 => ItemType<NovusBar>(),
				3 => ItemType<CryostalBar>(),
				4 => ItemType<VirulentBar>(),
				5 => ItemType<WovenEntrophite>(),
				6 => ItemType<PrismalBar>(),
				7 => ItemType<StarMetalBar>(),
				8 => ItemType<VibraniumBar>(),
				9 => ItemType<DrakeniteBar>(),
				_ => 0,
			};
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, item);

			return base.Drop(i, j);
		}

		/*
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (Main.tile[i,j].TileType == Mod.Find<ModTile>("DrakeniteBarTile").Type)
            Dimensions.Tiles.Fabric.DrawStatic(this, i, j, spriteBatch,drakenite: true);
        }
        */

	}
}