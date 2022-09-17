using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Tiles.Bars
{
    public class NovusOreTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            TileID.Sets.Ore[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileShine[Type] = 800;
            Main.tileShine2[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileOreFinderPriority[Type] = 200;
            TileID.Sets.Ore[Type] = true;
            HitSound = SoundID.Tink;
            DustType = 0;
            ItemDrop = ModContent.ItemType<Items.Materials.Bars.NovusOre>();
            MinPick = 55;
            MineResist = 1.25f;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Novus Ore");
            AddMapEntry(new Color(70, 0, 40), name);
        }

        /*
        public override bool CanExplode(int i, int j)
        {
            return SGAWorld.downedWraiths > 0;
        }
        */
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)  
        {
            r = 0.5f;
            g = 0f;
            b = 0.2f;
        }
    }
    public class NoviteOreTile : NovusOreTile
    {
        public override void SetStaticDefaults()
        {
            TileID.Sets.Ore[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileShine[Type] = 1200;
            Main.tileShine2[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileOreFinderPriority[Type] = 200;
            TileID.Sets.Ore[Type] = true;
            HitSound = SoundID.Tink;
            DustType = DustID.GoldCoin;
            ItemDrop = ModContent.ItemType<Items.Materials.Bars.NoviteOre>();
            MinPick = 55;
            MineResist = 1.25f;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Novite Ore");
            AddMapEntry(new Color(240, 221, 168), name);
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.5f;
            g = 0.5f;
            b = 0.15f;
        }
    }
}