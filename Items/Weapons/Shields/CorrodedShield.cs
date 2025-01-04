using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using SGAmod.Projectiles;
using Idglibrary;
using System.Linq;
//using AAAAUThrowing;
using Terraria.Graphics.Shaders;
using SGAmod.Buffs;
using Microsoft.Xna.Framework.Audio;
using Terraria.Utilities;
using SGAmod.Buffs.Debuffs;


namespace SGAmod.Items.Weapons.Shields
{
    public class CorrodedShield : ModItem, IShieldItem
    {
        public Projectile GetShieldProj
        {
            get
            {
				Item item = Main.LocalPlayer.HeldItem;
                int typez = SGAPlayer.ShieldTypes.GetValueOrDefault(item.type);

				Projectile proj = null;
                if (Main.LocalPlayer.ownedProjectileCounts[typez] > 0)
                {
                    Projectile[] proj3 = Main.projectile.Where(testprog => testprog.active && testprog.owner == Main.LocalPlayer.whoAmI && testprog.type == typez ).ToArray();
                    if(proj3 != null && proj3.Length > 0) 
                    {
                        proj = proj3[0];
                    }
                }
                else
                {
                    proj = new Projectile();
                    proj.SetDefaults(typez);
                }
                return proj;
            }
        }

        public virtual string ShowPercentText
        {
            get
            {
                Projectile proj = GetShieldProj;
                if (proj != null)
                {
                    CorrodedShieldProj proj2 = proj.ModProjectile as CorrodedShieldProj;
                    if(proj2 != null)
                    {
                        float blockpercent = (proj2.BlockDamagePublic);
                        return "Blocks " + (blockpercent) * 100 + "% of damage ";
                    }
                }
                return "Blocks unknown % of damage ";
            }
        }
        public virtual string DamagePercent => "at a narrow angle";
        public virtual bool CanBlock => true;
        public override void SetDefaults()
        {
            Item.damage = 25;
            Item.crit = 15;
            Item.DamageType = DamageClass.Melee;
            Item.width = 54;
            Item.height = 32;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5;
            Item.noUseGraphic = true;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item7;
            Item.shoot = ModContent.ProjectileType<CorrodedShieldProjDash>();
            Item.shootSpeed = 10f;
            Item.useTurn = false;
            Item.autoReuse = false;
            Item.expert = false;
            Item.noMelee = true;
            ItemID.Sets.Glowsticks[Type] = true;
            
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            damage.Base += player.GetModPlayer<SGAPlayer>().shieldDamageBoost;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return true;
        }

        /*public override void HoldItem(Player player)
        {
            Projectile.NewProjectile(player.GetSource_FromThis(), player.position, Vector2.Zero, ModContent.ProjectileType<CorrodedShieldProj>(), default, 0);
        }*/

        public override int ChoosePrefix(UnifiedRandom rand)
        {
            if(!CanBlock) return base.ChoosePrefix(rand);

            switch (rand.Next(9))
            {
                case 1: return PrefixID.Weak;
                case 2: return PrefixID.Frenzying;
                case 3: return PrefixID.Damaged;
                case 4: return PrefixID.Savage;
                case 5: return PrefixID.Furious;
                case 6: return PrefixID.Terrible;
                    default: return base.ChoosePrefix(rand);
                /*case 7: return ModContent.PrefixType<Busted>(); #TODO
                default: return ModContent.PrefixType<Defensive>();*/
            }
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (ShowPercentText != "none")
            {
                tooltips.Add(new TooltipLine(Mod, "shieldtext", Idglib.ColorText(Color.CornflowerBlue, ShowPercentText + DamagePercent)));
                tooltips.Add(new TooltipLine(Mod, "shieldtext", Idglib.ColorText(Color.CornflowerBlue, "Block at the last second to 'Just Block', taking no damage")));
                tooltips.Add(new TooltipLine(Mod, "shieldtext", Idglib.ColorText(Color.Orange, "Requires 1 Cooldown stack, adds 3 seconds each")));
            }
            tooltips.Add(new TooltipLine(Mod, "shieldtext", Idglib.ColorText(Color.CornflowerBlue, "Can be held out like a torch and used normally by holding shift")));
        }
    }
    public class CorrodedShieldProj : ModProjectile, IDrawAdditive
    {
        public int Blocktimer => blocktimer - Main.player[Projectile.owner].GetModPlayer<SGAPlayer>().shieldBlockTime;
        private int blocktimer = 1;

        protected virtual bool CanBlock => true;
        protected virtual float VisualAngle => 0f;
        protected virtual float BlockAngle => 0.5f;
        protected virtual float BlockDamage => 0.25f;
        protected virtual int BlockPeriod => 30;
        protected virtual float AngleAdjust => 0f;
        protected virtual float BlockAngleAdjust => 0f;
        protected virtual float AlphaFade => 1f;
        protected virtual float HoldingDistance => 10f;

        public virtual float BlockDamagePublic
        {
            get
            {
                float boost = 0;
                if (Main.player[Projectile.owner] != null) 
                {
                    boost += Main.player[Projectile.owner].GetModPlayer<SGAPlayer>().shieldDamageReduce * 1f;
                }
                return BlockDamage + boost;
            }
        }
        public virtual float BlockAnglePublic => BlockAngle;

		public string ItemName => Name.Replace("Proj", "");
        public virtual bool Blocking => true;
        public Player player
        {
            get
            {
                if (Projectile.owner >= 255) return Main.LocalPlayer;
                return Main.player[Projectile.owner];
            }
        }
        
        public virtual void JustBlock(int blocktime, Vector2 where, int damage, int damageSourceIndex) { }
        public virtual void WhileHeld(Player player) { }
        public virtual bool HandleBlock(ref int damage, Player player) { return true; }

        public override void SetDefaults()
        {
            Projectile refProjectile = new Projectile();
            refProjectile.SetDefaults(ProjectileID.Boulder);
            AIType = ProjectileID.Boulder;
            Projectile.friendly = true;
            Projectile.timeLeft = 10;
            Projectile.hostile = false;
            Projectile.penetrate = 10;
            Projectile.light = 0.5f;
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            DrawHeldProjInFrontOfHeldItemAndArms = true;
        }

        public override bool? CanDamage()
        {
            return false;
        }
        public override bool PreAI()
        {
            blocktimer += 1;
            return true;
        }
        public override void AI()
        {
            blocktimer += 1;
            bool heldone = player.HeldItem.type != Mod.Find<ModItem>(ItemName).Type;
            if (Projectile.ai[0] > 0 || ((player.HeldItem == null || heldone) && Projectile.timeLeft <= 10) || player.dead || (player.ownedProjectileCounts[ModContent.ProjectileType<CapShieldToss>()] > 0 && Projectile.GetType() == typeof(CapShieldProj)))
            {
                Projectile.Kill();
            }
            else
            {
                SGAPlayer sgaply = Main.player[Projectile.owner].GetModPlayer<SGAPlayer>();
                sgaply.heldShield = Projectile.whoAmI;
                sgaply.heldShieldReset = 3;

                if(Projectile.timeLeft < 3)
                {
                    Projectile.timeLeft = 3;
                }
                Vector2 mousePos = Main.MouseWorld;

                if(Projectile.owner == Main.myPlayer)
                {
                    Vector2 diff = mousePos - player.MountedCenter;
                    Projectile.velocity = diff.RotatedBy(BlockAngleAdjust);
                    Projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
                    Projectile.netUpdate = true;
                    Projectile.Center = mousePos;
                }
                int dir = Projectile.direction;
                player.ChangeDir(dir);

                Vector2 direction = Projectile.velocity;
                Vector2 directionmeasure = direction;

                player.heldProj = Projectile.whoAmI;

                Projectile.velocity.Normalize();

                player.bodyFrame.Y = player.bodyFrame.Height * 3;
                if (directionmeasure.Y - Math.Abs(directionmeasure.X) > 25) player.bodyFrame.Y = player.bodyFrame.Height * 4;
                if (directionmeasure.Y + Math.Abs(directionmeasure.X) < -25) player.bodyFrame.Y = player.bodyFrame.Height * 2;
                if (directionmeasure.Y + Math.Abs(directionmeasure.X) < -160) player.bodyFrame.Y = player.bodyFrame.Height * 5;
                player.direction = (directionmeasure.X > 0).ToDirectionInt();

                Projectile.Center = player.MountedCenter + (Projectile.velocity * HoldingDistance);
                Projectile.velocity *= 8f;
            }
        }
        protected virtual void DrawAdd()
        {
            if (!CanBlock) return;

            bool facingleft = Projectile.velocity.X > 0;
            SpriteEffects effect = SpriteEffects.None;
            Texture2D texture = ModContent.Request<Texture2D>("SGAmod/items/Weapons/Shields/CorrodedShieldProj").Value;
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f) ;
            float facing = facingleft ? AngleAdjust : -AngleAdjust;

            float alpha = MathHelper.Clamp((30 - Blocktimer) / 8f,0f,1f);
            if (alpha > 0f) Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(), Main.hslToRgb((Main.GlobalTimeWrappedHourly * 3f) % 1f, 1f, 0.85f) * alpha, (Projectile.velocity.ToRotation() + facing) + (facingleft?0:MathHelper.Pi) + VisualAngle, origin, Projectile.scale + 0.25f, facingleft ? effect : SpriteEffects.FlipHorizontally,0);

        }
        public void DrawAdditive(SpriteBatch spriteBatch)
        {
            DrawAdd();
        }
        public virtual void DrawNormal(SpriteBatch spriteBatch, Color drawcolor)
        {
            bool facingleft = Projectile.velocity.X > 0;
            SpriteEffects effect = SpriteEffects.None;
            Texture2D texture = ModContent.Request<Texture2D>("SGAmod/Items/Weapons/Shields/CorrodedShieldProj").Value;
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            float facing = facingleft ? AngleAdjust : -AngleAdjust;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(), drawcolor * AlphaFade * Projectile.Opacity, (Projectile.velocity.ToRotation() + facing) + (facingleft ? 0 : MathHelper.Pi) + VisualAngle, origin, Projectile.scale, facingleft ? effect : SpriteEffects.FlipHorizontally, 0);

        }
        public override bool PreDraw(ref Color lightColor)
        {
            DrawNormal(Main.spriteBatch, lightColor);
            return false;
        }
    }

    public class CorrodedShieldProjDash : ModProjectile, IShieldBashProjectile
    {
        public Player player => Main.player[Projectile.owner];
        public virtual int HoldType => ModContent.ItemType<CorrodedShield>();
        public override void SetDefaults()
        {
            Projectile refProjectile = new Projectile();
            refProjectile.SetDefaults(ProjectileID.Boulder);
            AIType = ProjectileID.Boulder;
            Projectile.friendly = true;
            Projectile.timeLeft = 30;
            Projectile.hostile = false;
            Projectile.penetrate = 5;
            Projectile.light = 0.5f;
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if(GetType() == typeof(CorrodedShieldProjDash)) 
            {
                target.AddBuff(ModContent.BuffType<AcidBurn>(), (int)(60 * 1.5));
            }
        }
        public override void AI()
        {
            bool heldone = player.HeldItem.type != HoldType;
            if (Projectile.ai[0] > 0 || (player.HeldItem == null || heldone)||player.dead)
            {
                Projectile.Kill();
            }
            else
            {
                if (Projectile.ai[1] < 1)
                {
                    int dir = Projectile.direction;
                    player.ChangeDir(dir);
                    player.velocity = Projectile.velocity;
                    player.velocity.Y /= 2f;
                    player.immune = true;
                    player.immuneTime = 30;
                }
                player.velocity.X = Projectile.velocity.X;
                Projectile.Center = player.Center;

                Projectile.ai[1] += 1;
            }
        }
        public override string Texture => "SGAmod/Invisible";



        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}
