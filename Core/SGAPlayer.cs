using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;
using Idglibrary;
using Terraria.ModLoader.IO;
using Terraria.Graphics.Shaders;
using Terraria.Utilities;

namespace SGAmod
{
    public partial class SGAPlayer : ModPlayer
    {

        // Apocalyptical related
        public double[] apocalypticalChance = { 0, 0, 0, 0 };
        public float apocalypticalStrength = 1f;
        public float lifestealentropy = 0f;

        public bool dragonFriend = false;

        public List<int> ExpertisePointsFromBosses;
        public List<string> ExpertisePointsFromBossesModded;
        public List<int> ExpertisePointsFromBossesPoints;
        public long ExpertiseCollected = 0;
        public long ExpertiseCollectedTotal = 0;

        public override void ResetEffects()
        {
            Player.breathMax = 200;
        }

        public override void CopyClientState(ModPlayer targetCopy)
        {
            SGAPlayer sgaplayer = targetCopy as SGAPlayer;
            sgaplayer.ExpertiseCollected = ExpertiseCollected;
            sgaplayer.ExpertiseCollectedTotal = ExpertiseCollectedTotal;
            sgaplayer.dragonFriend = dragonFriend;
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            bool mismatch = false;
            SGAPlayer sgaplayer = clientPlayer as SGAPlayer;

            if (sgaplayer.ExpertiseCollected != ExpertiseCollected || 
                sgaplayer.ExpertiseCollectedTotal != ExpertiseCollectedTotal || 
                sgaplayer.dragonFriend != dragonFriend
                )
            {
                mismatch = true;
            }

            if (mismatch)
            {
                SendClientChangesPacket();
            }
        }


        private void SendClientChangesPacket()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {

            }
        }

        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            return new[] {
                new Item(ModContent.ItemType<Items.Consumables.GrabBags.StartingBag>())
            };
        }

        public override void SaveData(TagCompound tag)
        {
            tag["ZZZExpertiseCollectedZZZ"] = ExpertiseCollected;
            tag["ZZZExpertiseCollectedTotalZZZ"] = ExpertiseCollectedTotal;
            tag["dragonFriend"] = dragonFriend;

            SaveExpertise(ref tag);
        }

        public override void LoadData(TagCompound tag)
        {
            dragonFriend = tag.GetBool("dragonFriend");

            LoadExpertise(tag);
        }
    }
}