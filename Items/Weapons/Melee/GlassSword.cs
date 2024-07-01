using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Audio;
using SGAmod.Buffs.Debuffs;
using SGAmod.Core;

namespace SGAmod.Items.Weapons.Melee
{
    public class GlassSword : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 6;
            Item.maxStack = Item.CommonMaxStack;
            Item.crit = 0;
            Item.DamageType = DamageClass.Melee;
            Item.width = 54;
            Item.height = 54;
            Item.useTime = 2;
            Item.useAnimation = 22;
            Item.reuseDelay = 11; //Make sure it is exactly half of UseAnimation, else prepare for rotation shenanigans
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 2;
            Item.noUseGraphic = true;
            Item.value = Item.sellPrice(0, 0, 0, 5);
            Item.rare = ItemRarityID.White;
            Item.UseSound = SoundID.Item1;
            Item.useTurn = false;
            Item.autoReuse = true;
        }
        public override bool ConsumeItem(Player player)
        {
            return player.itemAnimation > 0;
        }
        public override bool CanUseItem(Player player)
        {
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.Request<Texture2D>("SGAmod/Items/Weapons/Melee/GlassSword").Value;
                TextureAssets.Item[Item.type] = ModContent.Request<Texture2D>("SGAmod/Items/Weapons/Melee/GlassSword");
            }
            Item.width = 54;
            Item.height = 54;
            Item.knockBack = 2;
            Item.noMelee = false;
            return true;
        }
        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.FinalDamage += target.defense / 2;
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Item.knockBack > 0)
            {
                player.ConsumeItem(Item.type);
                SoundEngine.PlaySound(SoundID.Item27 with {Volume = 0.75f}, player.Center);
                for (int i = 0; i < 80; i += 20)
                {
                    Vector2 position = player.Center;
					Vector2 eree = player.itemRotation.ToRotationVector2().RotatedBy(MathHelper.ToRadians(-45f * player.direction));
                    eree *= player.direction;
                    position += eree * i;
                    int thisoned = Projectile.NewProjectile(player.GetSource_ItemUse(Item), position, new Vector2(eree.X * Main.rand.NextFloat(2.4f, 5f), eree.X * Main.rand.NextFloat(0.5f, 2f)), ModContent.ProjectileType<BrokenGlass>(), damageDone, 0f, Main.myPlayer);
                }
                if (!Main.dedServ)
                {
                    Item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.Request<Texture2D>("SGAmod/Items/Weapons/Melee/GlassSwordBreak").Value;
                }
            }
            else
            {
                target.AddBuff(ModContent.BuffType<Gouged>(), 60 * 12);
            }
            player.itemWidth = 24;
            player.itemHeight = 24;
            if (!Main.dedServ)
            {
                TextureAssets.Item[Item.type] = ModContent.Request<Texture2D>("SGAmod/Items/Weapons/Melee/GlassSwordBreakSmol");
                Item.width = TextureAssets.Item[Item.type].Height();
                Item.height = TextureAssets.Item[Item.type].Width();
            }
            Item.width = 24;
            Item.height = 24;
            Item.knockBack = 0;
        }
        public override void AddRecipes()
        {
           CreateRecipe(20).AddIngredient(ItemID.Glass,4).AddTile(TileID.WorkBenches).Register();
        }
    }
    public class BrokenGlass : ModProjectile
    {
        public virtual bool hitWhileFalling => true;
        public virtual float trans => 1f;
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.ignoreWater = false;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Melee;
            AIType = ProjectileID.WoodenArrowFriendly;

        }
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.RocketII;

        public override bool PreDraw(ref Color lightColor)
        {
            bool facingleft = Projectile.velocity.X > 0f;
            SpriteEffects effect = SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally;
            Texture2D texture = (Texture2D)TextureAssets.Item[ItemID.GlassBowl];
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(), lightColor * trans, Projectile.rotation + (facingleft ? (float)(1f * MathHelper.Pi) : 0f), origin, Projectile.scale, facingleft ? effect : SpriteEffects.None, 0);
            return false;
        }
        public override bool PreKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item50 with { Volume = 0.5f }, Projectile.Center);
            Projectile.type = ProjectileID.Fireball;
            SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
            for (int i = 0; i < 40; i++)
            {
                Vector2 randomCircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomCircle.Normalize();
                randomCircle *= Main.rand.NextFloat(0f,3f);
                int dust = Dust.NewDust(new Vector2(Projectile.position.X - 1, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Ice, 0, 0, 50, Color.Gray, Projectile.scale * 0.5f);
                Main.dust[dust].noGravity = false;
                Main.dust[dust].velocity = randomCircle + Projectile.velocity;
            }
            return true;
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.velocity.Y < 0 && hitWhileFalling)
            {
                return false;
            }
            if (Projectile.ai[1] < 5) return false;
            return base.CanHitNPC(target);
        }
        public override void AI()
        {
            Tile tile = Main.tile[(int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16];
            if (tile != null) if (tile.LiquidAmount > 64) Projectile.Kill();
            Projectile.velocity.Y += 0.1f;
            Projectile.rotation += Projectile.velocity.X * 0.1f;
            Projectile.ai[1] += 1;
        }
    }
}
