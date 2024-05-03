using Microsoft.Xna.Framework;
using Terraria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Idglibrary;
using SGAmod.Items.Materials.Bars;

namespace SGAmod.Items.Weapons.Shields
{
    public class CapShield : CorrodedShield, IShieldItem, IFormerThrowingItem
    {
        public override string DamagePercent => "at a decent angle";
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 150;
            Item.crit = 15;
            Item.width = 54;
            Item.height = 32;
            Item.useTime = 70;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 60;
            Item.reuseDelay = 80;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5;
            Item.noUseGraphic = true;
            Item.value = Item.sellPrice(0,5,0,0);
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item7;
            Item.shoot = ModContent.ProjectileType<CapShieldProjDash>();
            Item.shootSpeed = 20f;
            Item.useTurn = false;
            Item.autoReuse = false;
            Item.channel = true;
            Item.noMelee = true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2) Item.channel = false;
            else Item.channel = true;
            return player.ownedProjectileCounts[ModContent.ProjectileType<CapShieldProjDash>()] < 1 && player.ownedProjectileCounts[ModContent.ProjectileType<CapShieldToss>()] < 1;
            
        }

        /*public override void HoldItem(Player player)
        {
            Projectile.NewProjectile(player.GetSource_FromThis(), player.position, Vector2.Zero, ModContent.ProjectileType<CapShieldProj>(), default, 0);
        }*/

        public override bool AltFunctionUse(Player player)
        {
            return true ;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if(player.altFunctionUse == 2)
            {
                player.itemAnimation /= 3;
                player.itemTime /= 3;
                damage = (int)(damage);
                type = ModContent.ProjectileType<CapShieldToss>();
                int thisoned = Projectile.NewProjectile(Item.GetSource_FromThis(), position, velocity * player.ThrownVelocity, type, damage, knockback, Main.myPlayer);
                Main.projectile[thisoned].DamageType = DamageClass.Throwing;
                Main.projectile[thisoned].netUpdate = true;
                IdgProjectile.Sync(thisoned);
                return false;
            }
            return true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);
            TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
            if(tt != null)
            {
                string[] thetext = tt.Text.Split(' ');
                string newline = "";
                List<string> valuez = new List<string>();
                foreach (string text2 in thetext)
                {
                    valuez.Add(text2 + " ");
                }
                valuez.RemoveAt(1);
                valuez.Insert(1, "Melee/Ranged ");
                foreach (string text3 in valuez)
                {
                    newline += text3;
                }
                tt.Text = newline;
            }
            tt = tooltips.FirstOrDefault(x => x.Name == "CritChance" && x.Mod == "Terraria");
            if(tt != null)
            {
                string[] thetext = tt.Text.Split(' ');
                string newline = "";
                List<string> valuez = new List<string>();
                int counter = 0;
                foreach (string text2 in thetext)
                {
                    counter ++;
                    if(counter > 1) valuez.Add(text2 + " ");
                }
                int thecrit = (int)(Main.GlobalTimeWrappedHourly % 3f >= 1.5f ? Main.LocalPlayer.GetCritChance(DamageClass.Melee) : Main.LocalPlayer.GetCritChance(DamageClass.Ranged));
                string thecrittype = Main.GlobalTimeWrappedHourly % 3f >= 1.5f ? "Melee " : "Ranged ";
                valuez.Insert(0, thecrit + "% " + thecrittype);
                foreach(string text3 in valuez)
                {
                    newline += text3;
                }
                tt.Text = newline;
            }
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            damage.Base -= player.GetDamage(DamageClass.Melee).Base;
            damage.Base += ((player.GetDamage(DamageClass.Ranged).Base * 1.5f) + player.GetDamage(DamageClass.Melee).Base) / 2f;
            base.ModifyWeaponDamage(player, ref damage);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
         //       .AddIngredient<DankWoodShield>()
                .AddIngredient<PrismalBar>(12)
                .AddRecipeGroup(RecipeGroupID.Fragment, 8)
                .AddIngredient(ItemID.LunarBar)
                .AddIngredient(ItemID.RedDye)
                .AddIngredient(ItemID.SilverDye)
                .AddIngredient(ItemID.BlueDye)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
    public class CapShieldProj : CorrodedShieldProj, IDrawAdditive
    {
        protected override float BlockAngle => 0.4f;
        protected override float BlockDamage => 0.5f;
        public override void JustBlock(int blocktime, Vector2 where, int damage, int damageSourceIndex)
        {
            player.AddBuff(BuffID.ParryDamageBuff, 60 * 3);
        }
        public virtual void DrawAdd(SpriteBatch spriteBatch)
        {
            if (!CanBlock) return;

            bool facingleft = Projectile.velocity.X > 0;
            SpriteEffects effect = SpriteEffects.None;
            Texture2D texture = ModContent.Request<Texture2D>("SGAmod/items/Weapons/Shields/CapShieldProj").Value;
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            float facing = facingleft ? AngleAdjust : -AngleAdjust;

            float alpha = MathHelper.Clamp((30 - Blocktimer) / 8f, 0f, 1f);
            if (alpha > 0f) Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(), Main.hslToRgb((Main.GlobalTimeWrappedHourly * 3f) % 1f, 1f, 0.85f) * alpha, (Projectile.velocity.ToRotation() + facing) + (facingleft ? 0 : MathHelper.Pi) + VisualAngle, origin, Projectile.scale + 0.25f, facingleft ? effect : SpriteEffects.FlipHorizontally, 0);
        }
        public void DrawAdditive(SpriteBatch spriteBatch)
        {
            DrawAdd();
        }
        public override void DrawNormal(SpriteBatch spriteBatch, Color drawcolor)
        {
            bool facingleft = Projectile.velocity.X > 0;
            SpriteEffects effect = SpriteEffects.None;
            Texture2D texture = ModContent.Request<Texture2D>("SGAmod/Items/Weapons/Shields/CapShieldProj").Value;
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
    public class CapShieldProjDash : CorrodedShieldProjDash, IShieldBashProjectile
    {
        public override void SetDefaults()
        {
            Projectile refProjectile = new Projectile();
            refProjectile.SetDefaults(ProjectileID.Boulder);
            AIType = ProjectileID.Boulder;
            Projectile.friendly = true;
            Projectile.timeLeft = 30;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.5f;
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
        public override bool? CanDamage()
        {
            return Projectile.ai[1] > 0 ? true : null;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if ((player.HeldItem == null || player.HeldItem.type != ModContent.ItemType<CapShield>()) || player.dead) Projectile.Kill();
            else
            {
                if (player.channel)
                {
                    if (Projectile.ai[0] < 150) Projectile.ai[0]++;
                    Projectile.timeLeft += 1;
                    for (float new1 = 0; new1 < 360; new1 += 360 / 10f)
                    {
                        if (Projectile.ai[0] * (360/150f) >= new1)
                        {
                            float angle = new1;
                            Vector2 angg = player.velocity.RotatedBy(MathHelper.ToRadians(angle)) + (angle + MathHelper.ToRadians(angle)).ToRotationVector2()*10f;
                            int DustID2 = Dust.NewDust(player.Center - new Vector2(8, 8), 16, 16, DustID.AncientLight, 0, 0, 20, Color.Silver, 1f);
                            Main.dust[DustID2].noGravity = true;
                            Main.dust[DustID2].velocity = new Vector2(angg.X * 0.75f, angg.Y * 0.75f);
                        }
                        Projectile.Center = player.Center;
                    }
                }
                else
                {
                    if (Projectile.ai[1] == 0)
                    {
                        Projectile.damage = (int)(Projectile.damage * (1f + Projectile.ai[0] / 20f));
                        player.itemTime += (int)(Projectile.ai[0] / 5f);
                        player.itemAnimation += (int)(Projectile.ai[0] / 5f);
                        Projectile.timeLeft += (int)(Projectile.ai[0] / 3f);
                    }
                    if (Projectile.ai[1] < (Projectile.ai[0] / 5f) + 3)
                    {
                        if (Projectile.ai[1] % 2 == 0)
                        {
                            Vector2 mousePos = Main.MouseWorld;

                            if(Projectile.owner == Main.myPlayer)
                            {
                                Vector2 diff = mousePos - player.Center;
                                Vector2 velox = Projectile.velocity;
                                Projectile.velocity = diff;
                                Projectile.velocity.Normalize();
                                Projectile.velocity *= velox.Length();
                                Projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
                                Projectile.netUpdate = true;
                            }
                            int dir = Projectile.direction;
                            player.ChangeDir(dir);
                            player.velocity = Projectile.velocity * (1 + (Projectile.ai[0] / 120f));
                            player.immune = true;
                            player.immuneTime = 30;
                        }
                    }
                    else
                    {
                        Projectile.velocity *= 0.98f;
                    }
                    player.velocity.X = Projectile.velocity.X * (1f + (Projectile.ai[0] / 120f));

                    for (float jj = 2; jj < 14; jj += 2)
                    {
                        for (float new1 = -1f; new1 < 2f; new1 += 2f)
                        {
                            float angle = 90;
                            Vector2 velo = player.velocity;
                            velo.Normalize();
                            Vector2 angg = velo.RotatedBy(angle * new1);
                            int DustID2 = Dust.NewDust(Projectile.Center - new Vector2(8, 8), 16, 16, DustID.AncientLight, 0, 0, 20, jj < 5 ? Color.White : new1 < 0 ? Color.Red : Color.Blue, 1f + (14f - jj) / 14);
                            Main.dust[DustID2].velocity = angg * jj;
                            Main.dust[DustID2].noGravity = true;
                        }
                    }

                    Projectile.Center = player.Center + Projectile.velocity;

                    Projectile.ai[1] += 1;
                }
            }
        }
        public override string Texture => "SGAmod/Invisible";

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
    public class CapShieldToss : ModProjectile, IShieldBashProjectile
    {
        List<int> bouncetargets = new List<int>();
        float hittime = 200f;
        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.ignoreWater = true;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 3;
            Projectile.penetrate = 20;
            AIType = 0;
            DrawOriginOffsetX = 8; DrawOriginOffsetY = -8;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.penetrate < 10) return null;
            else return base.CanHitNPC(target);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.velocity *= -1f;
            hittime = 150f;
            Projectile.ai[1] = FindClosestTarget(Projectile.Center, new Vector2(0f, 0f));
            if (Projectile.ai[0] > 30) Projectile.ai[0] -= 30;
        }
        public int FindClosestTarget(Vector2 loc, Vector2 size)
        {
            float num170 = 1000000; // gee IDG good names
            NPC target = null;

            for (int num1722 = 0; num1722 < Main.maxNPCs; num1722++)
            {
                int num172 = num1722;
                if (Main.npc[num172].active && !Main.npc[num172].friendly && !Main.npc[num172].townNPC && Main.npc[num172].CanBeChasedBy() && Projectile.localNPCImmunity[num1722] < 1)
                {
                    float num173 = Main.npc[num172].position.X + (float)(Main.npc[172].width / 2);
                    float num174 = Main.npc[num172].position.Y + (float)(Main.npc[172].height / 2);
                    float num175 = Math.Abs(loc.X + (float)(size.X / 2) - num173) + Math.Abs(loc.Y + (float)(size.Y / 2) - num174);
                    if (Main.npc[num172].active)
                    {
                        if (num175 < num170)
                        {
                            int result = 0;
                            result = bouncetargets.Find(x => x == num172);
                            if (result < 1)
                            {
                                num170 = num175;
                                target = Main.npc[num172];
                            }
                        }
                    }
                }
            }
            if (num170 > 900)
            {
                Projectile.penetrate = 5;
                return -1;
            }
            return target.whoAmI;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.Aquamarine.ToVector3() * 0.5f);
            hittime = Math.Max(1f, hittime / 1.5f);
            float dist2 = 24f;

            for (double num315 = 0;  num315 < Math.PI + 0.04; num315 += Math.PI)
            {
                Vector2 thisloc = new Vector2((float)(Math.Cos((Projectile.rotation + Math.PI / 2) + num315) * dist2), (float)(Math.Sin((Projectile.rotation + Math.PI / 2) + num315) * dist2));
                int dust = Dust.NewDust(new Vector2(Projectile.position.X - 1, Projectile.position.Y) + thisloc, Projectile.width, Projectile.height, DustID.AncientLight, 0f, 0f, 50, num315 < 0.01 ? Color.Blue : Color.Red, 1.5f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = thisloc / 30f;
            }

            Projectile.ai[0]++;
            if (Projectile.ai[0] == 1)
            {
                Projectile.penetrate += (int)((float)Main.player[Projectile.owner].statDefense / 30f);
                Projectile.ai[1] = FindClosestTarget(Projectile.Center, new Vector2(0f, 0f));
            }
            Projectile.velocity.Y += 0.1f;
            if ((Projectile.ai[0] > 90f || (Projectile.penetrate < 10 && Projectile.ai[0] > 20))&& !Main.player[Projectile.owner].dead)
            {
                Vector2 dist = Main.player[Projectile.owner].Center - Projectile.Center;
                Vector2 distnorm = dist; distnorm.Normalize();
                Projectile.velocity += distnorm * (5f + ((float)Projectile.timeLeft / 40f));
                Projectile.velocity /= 1.25f;
                if (dist.Length() < 80) Projectile.Kill();

                Projectile.timeLeft++;
            }
            Projectile.timeLeft++;

            if (Projectile.ai[1] > -1)
            {
                NPC target = Main.npc[(int)Projectile.ai[1]];
                if (target != null && Projectile.penetrate > 9)
                {
                    Projectile.Center += Projectile.DirectionTo(target.Center) * (Projectile.ai[0] > 8f ? (50 * Main.player[Projectile.owner].ThrownVelocity) / hittime : 12f);

                }
            }
            Projectile.rotation += 0.38f;
        }
        public override string Texture => "SGAmod/Items/Weapons/Shields/CapShield";
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("SGAmod/Items/Weapons/Shields/CapShield").Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), Vector2.One, SpriteEffects.None, 0f);
            return false;
        }
    }
}
