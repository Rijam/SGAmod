using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace SGAmod.Items.Materials.Misc
{
    public class StygianCore : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = 50000;
            Item.maxStack = 10;
            Item.rare = ItemRarityID.Red;
        }
        public override string Texture => "Terraria/Images/Sun";

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Magenta * 0.5f;
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D inner = TextureAssets.Item[ModContent.ItemType<AssemblyStar>()].Value;

            Vector2 slotSize = new Vector2(52f, 52f) * scale;
			//position -= slotSize *  Main.inventoryScale / 2f - frame.Size() * scale / 2f;
            Vector2 drawPos = position /*+ slotSize * Main.inventoryScale / 2f*/;
			Vector2 textureOrigin = new Vector2(inner.Width/2, inner.Height/2);

            for(float i = 0; i < 1f; i += 0.1f)
            {
                spriteBatch.Draw(inner, drawPos, null, (Color.DarkMagenta * (1f - ((i + (Main.GlobalTimeWrappedHourly / 2f)) % 1f)) * 0.5f) * 0.5f, i * MathHelper.TwoPi, textureOrigin, Main.inventoryScale * (0.5f + 1.75f * (((Main.GlobalTimeWrappedHourly / 2f) + i) % 1f)), SpriteEffects.None, 0f);

            }
            return true;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D inner = TextureAssets.Item[ModContent.ItemType<AssemblyStar>()].Value;

            Vector2 slotSize = new Vector2(52f, 52f);
            Vector2 position = Item.Center - Main.screenPosition;

            Vector2 textureOrigin = new Vector2(inner.Width/2, inner.Height / 2);

            for (float i = 0; i < 1f; i += 0.1f)
            {
                spriteBatch.Draw(inner, position, null, (Color.DarkMagenta * (1f - ((i + (Main.GlobalTimeWrappedHourly / 2f)) % 1f)) * 0.5f) * 0.5f, i * MathHelper.TwoPi, textureOrigin, 1f * (0.5f + 1.75f * (((Main.GlobalTimeWrappedHourly / 2f) + i) % 1f)), SpriteEffects.None, 0f);

            }
            spriteBatch.Draw(TextureAssets.Item[Item.type].Value, position, null, alphaColor, rotation, TextureAssets.Item[Item.type].Size() / 2f, 128f / 256f, SpriteEffects.None, 0f);

            return false;
        }
    }
}
