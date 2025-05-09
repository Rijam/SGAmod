using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using System.Collections.Generic;
using Terraria.ModLoader;
using System.IO;
// using SGAmod.NPCs.SpiderQueen;
// using SGAmod.Dimensions;
using Terraria.GameContent.Events;
using System.Linq;
using Steamworks;
using Terraria.ModLoader.Engine;
using System.Reflection;
// using Terraria.ModLoader.Audio;

namespace SGAmod.Items.Consumables.Debug
{

    public class YellowHeart : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Oh hey, you found an Easter Egg!");
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 14;
            Item.maxStack = 30;
            Item.rare = ItemRarityID.Yellow;
            Item.value = 1000;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item9;
            Item.consumable = true;
        }
        public override bool? UseItem(Player player)
        {
            /*
            SGAPlayer.centerOverrideTimerIsActive = 300;
            player.SGAPly().centerOverrideTimer = 300;
            */
            //PrivateClassEdits.CrashPatch();
            return true;
        }
        public override string Texture
        {
            get { return "Terraria/Images/Heart2"; }
        }

    }

    /*
    public class Debug13 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Debug-For testing purposes only");
            Tooltip.SetDefault("By having this item you are clearly only testing things");
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 14;
            Item.rare = ItemRarityID.Yellow;
            Item.value = 1000;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useAnimation = 8;
            Item.useTime = 8;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item9;
        }
        public override string Texture
        {
            get { return "Terraria/Images/UI/Camera_5"; }
        }

        public override void UpdateInventory(Player player)
        {
            if (player.inventory[49].type == Item.type && (SGAmod.TotalCheating) && player.SGAPly().Sequence)
            {
                SGAmod.cheating = false;
                SGAWorld.cheating = false;
                var snd = SoundEngine.PlaySound(SoundID.PlayerKilled);
                if (snd != null)
                {
                    snd.Pitch = -0.80f;
                }
            }
        }
    }
    */

    /*
    public class Debug12 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Debug-Hard Reset");
            Tooltip.SetDefault("Does something questionable... Don't use this item, seriously!");
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 14;
            Item.rare = ItemRarityID.Yellow;
            Item.value = 1000;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useAnimation = 8;
            Item.useTime = 8;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item9;
        }
        public override string Texture
        {
            get { return "Terraria/Images/UI/Camera_5"; }
        }

        public override bool? UseItem(Player player)
        {
            Point who = new(player.whoAmI,0);
            Vector2 whereWereWe = new Vector2(player.position.X, player.position.Y);
            Player newguy =  new Player(true);//(Player)player.Clone();//
            newguy.position = whereWereWe;
            newguy.fallStart = (int)whereWereWe.Y;
            newguy.fallStart2 = (int)whereWereWe.Y;
            newguy.name = "REBOOTED";
            newguy.immune = false;
            newguy.active = true;
            newguy.hurtCooldowns[0] = -6;
                newguy.hurtCooldowns[1] = -6;


            Main.player[0] = newguy;

            //Main.myPlayer = 1;
            return true;
        }
    }
    */

    
    public class Debug11 : ModItem
    {
        public override void SetStaticDefaults()
        {
            
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 14;
            Item.rare = ItemRarityID.Yellow;
            Item.value = 1000;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useAnimation = 8;
            Item.useTime = 8;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item9;
        }
        public override string Texture
        {
            get { return "Terraria/Images/Xmas_0"; }
        }

        public override bool? UseItem(Player player)
        {
            Credits.CreditsManager.queuedCredits = true;
            return true;
        }
    }
    


    public class Debug10 : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Debug-Cataclysmic Mothron Head to Unlimited Brit Power of supreme Manifest Destiny");
        }

        public override void SetDefaults()
        {
            Item.width = 666;
            Item.height = 666;
            Item.rare = ItemRarityID.LightPurple;
            Item.value = 66666;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item9;
        }
        public override string Texture
        {
            get { return "Terraria/Images/Gore_687"; }
        }
    }
   

    /*
    public class Debug9 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Debug-Disable Armor/Accesories in inventory");
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 14;
            Item.rare = ItemRarityID.Yellow;
            Item.value = 1000;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item9;
        }
        public override void UpdateInventory(Player player)
        {
            player.SGAPly().disabledAccessories = Math.Max(player.SGAPly().disabledAccessories,600);
        }
        public override string Texture
        {
            get { return "Terraria/Images/Item_" + BuffID.NoBuilding; }
        }
    }
    */

    /*
    public class Debug8 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Debug-Clear Cooldown Stacks");
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 14;
            Item.rare = ItemRarityID.Yellow;
            Item.value = 1000;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useAnimation = 8;
            Item.useTime = 8;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item9;
        }
        public override bool? UseItem(Player player)
        {
            foreach(ActionCooldownStack stack in player.SGAPly().CooldownStacks)
            {
                stack.timeleft = 1;
            }

            return false;

        }
        public override string Texture
        {
            get { return "Terraria/Images/Item_" + BuffID.Titan; }
        }
    }
    */

    /*
    public class Debug7 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Debug-Music Test");
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 14;
            Item.maxStack = 30;
            Item.rare = ItemRarityID.Yellow;
            Item.value = 1000;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item9;
            Item.consumable = true;
        }
        public override bool? UseItem(Player player)
        {
            //Main.NewText(SGAmod.musicTest != null);
            SGAmod.musicTest = new MusicStreamingOGGPlus("tmod:SGAmod/Sounds/Music/creepy.ogg");
            //if (!SGAmod.musicTest.IsPlaying)
            //{
            SGAmod.musicTest.Reset();
            SGAmod.musicTest.Play();
                SGAmod.musicTest.SetVariable("Pitch", -0.95f);
                SGAmod.musicTest.SetVariable("Volume", 1f);
            //}

            return false;

        }
        public override string Texture
        {
            get { return "Terraria/Images/Item_" + ItemID.Harp; }
        }
    }
    */

    /*
    public class Debug6 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Debug-Steamworks test");
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 14;
            Item.maxStack = 30;
            Item.rare = ItemRarityID.Yellow;
            Item.value = 1000;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item9;
            Item.consumable = true;
        }
        public override bool CanUseItem(Player player)
        {
            return SGAmod.SteamID == "76561198080218537";
        }
        public override bool? UseItem(Player player)
        {
            int count = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagAll);
            Main.NewText("Hello... "+SteamFriends.GetPersonaName());
            //Main.NewText("I see you have friends:");
            for (int friend = 0; friend < Math.Min(count,10); friend += 1)
            {
                CSteamID steamid = SteamFriends.GetFriendByIndex(friend, EFriendFlags.k_EFriendFlagAll);
                Main.NewText(SteamFriends.GetFriendPersonaName(steamid));

            }
            SteamMusicRemote.EnableShuffled(true);
            SteamMusicRemote.UpdateShuffled(true);
                SteamMusic.PlayNext();

            return false;

        }
        public override string Texture
        {
            get { return "Terraria/Item_" + ItemID.SteampunkBoiler; }
        }
    }
    */

    /*
    public class Debug5 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Debug-Draken Party Attempt");
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 14;
            Item.maxStack = 30;
            Item.rare = ItemRarityID.Yellow;
            Item.value = 1000;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item9;
            Item.consumable = true;
        }
        public override bool? UseItem(Player player)
        {
            for(int i=0;i<200; i+=1)
            {
                BirthdayParty.PartyDaysOnCooldown = 0;
                BirthdayParty.CheckMorning();
                if (BirthdayParty.CelebratingNPCs.FirstOrDefault(type => type == ModContent.NPCType<NPCs.TownNPCs.Draken>()) == default)
                    break;
            }
            return true;
        }
        public override string Texture
        {
            get { return "Terraria/Images/Item_" + ItemID.PartyPresent; }
        }

    }
    public class Debug3 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Debug-Bring up the Custom UI");
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 14;
            Item.maxStack = 30;
            Item.rare = ItemRarityID.Yellow;
            Item.value = 1000;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item9;
            Item.consumable = true;
        }
        public override bool? UseItem(Player player)
        {
            SGAmod.TryToggleUI(null);
            return true;
        }
        public override string Texture
        {
            get { return "Terraria/Images/Item_" + ItemID.PurpleSolution; }
        }
    }
    */

    public class Debug2 : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Debug-Gain 100 Expertise");
            // Tooltip.SetDefault("Right click to remove 100 Expertise");
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 14;
            Item.maxStack = 30;
            Item.rare = ItemRarityID.Yellow;
            Item.value = 0;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item9;
            Item.consumable = true;
        }
        public override bool? UseItem(Player player)
        {
            SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();
            sgaplayer.ExpertiseCollected += 100;
            sgaplayer.ExpertiseCollectedTotal += 100;
            Main.NewText("Expertise: "+ sgaplayer.ExpertiseCollected+": Max: "+ sgaplayer.ExpertiseCollectedTotal);
            return true;
        }
        public override bool CanRightClick()
        {
            return true;
        }
        public override void RightClick(Player player)
        {
            SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();
            if (sgaplayer.ExpertiseCollected - 100 < 0)
            {
                sgaplayer.ExpertiseCollected = 0;
            }
            else
            {
                sgaplayer.ExpertiseCollected -= 100;
            }
            if (sgaplayer.ExpertiseCollectedTotal - 100 < 0)
            {
                sgaplayer.ExpertiseCollectedTotal = 0;
            }
            else
            {
                sgaplayer.ExpertiseCollectedTotal -= 100;
            }
            Main.NewText("Expertise: " + sgaplayer.ExpertiseCollected + ": Max: " + sgaplayer.ExpertiseCollectedTotal);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "CurrentState", "ExpertiseCollected == " + Main.LocalPlayer.GetModPlayer<SGAPlayer>().ExpertiseCollected.ToString()));
            tooltips.Add(new TooltipLine(Mod, "CurrentState", "ExpertiseCollectedTotal == " + Main.LocalPlayer.GetModPlayer<SGAPlayer>().ExpertiseCollectedTotal.ToString()));
        }

        public override string Texture
        {
            get { return "Terraria/Images/Item_" + ItemID.DarkBlueSolution; }
        }
    }

    public class Debug1 : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Debug-reset SGA Player Save Data");
            // Tooltip.SetDefault("Use this item if your getting 'null List' errors on killing enemies\nHolding this item activates a trippy Shader");
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 14;
            Item.maxStack = 30;
            Item.rare = ItemRarityID.Yellow;
            Item.value = 1000;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item9;
            Item.consumable = true;
        }
        public override bool? UseItem(Player player)
        {
            SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();
            sgaplayer.ExpertiseCollected = 0;
            sgaplayer.ExpertiseCollectedTotal = 0;
            /*sgaplayer.Redmanastar = 0;
            sgaplayer.Electicpermboost = 0;
            sgaplayer.gothellion = false;
            sgaplayer.Drakenshopunlock = false;
            sgaplayer.dragonFriend = false;
            sgaplayer.benchGodFavor = false;
            sgaplayer.GenerateNewBossList();
            for (int x = 0; x < SGAWorld.questvars.Length; x++)
            {
                SGAWorld.questvars[x] = 0;
            }
            */
            return true;
        }
        public override string Texture
        {
            get { return "Terraria/Images/CoolDown"; }
        }

    }

    /*
    public class Debug4 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Debug-Dark Sector Awakener");
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 14;
            Item.maxStack = 30;
            Item.rare = ItemRarityID.Yellow;
            Item.value = 1000;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item9;
            Item.consumable = true;
        }
        public override bool? UseItem(Player player)
        {
            new DarkSector((int)player.Center.X/16, (int)player.Center.Y / 16);
            return true;
        }
        public override string Texture
        {
            get { return "Terraria/Images/Item_" + ItemID.Darkness; }
        }

    }
    */
}