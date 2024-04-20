//From Qwerty's random content mod, used with permission, thank you qwerty!
//https://github.com/qwerty3-14/QwertysRandomContent/blob/master/ItemUseGlow.cs github source
//Has been modifed by IDGCaptainRussia94


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SGAmod.Core
{
    public class ItemUseGlow : GlobalItem
    {
        public Texture2D glowTexture = null;
        public int glowOffsetY = 0;
        public int glowOffsetX = 0;
        public float angleAdd = 0f;
        public override bool InstancePerEntity => true;
        protected override bool CloneNewInstances =>true;

        public Func<Item, Player, Color> GlowColor = delegate (Item item, Player player)
        {
            return Color.White;
        };
        public Action<Item, PlayerDrawSet, Vector2, float, Color> CustomDraw = delegate (Item item, PlayerDrawSet drawSet, Vector2 position, float angle, Color glowColor)
        {

        };
    }
    public class PlayerUseGlow : ModPlayer
    {
        //public static readonly PlayerDrawLayer ItemUseGlow = new PlayerDrawLayer("");
    }
}
