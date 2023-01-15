using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.Audio;
using Terraria.GameContent;

namespace StoneOfThePhilosophers.Contents
{
    public class StoneOfMetal : MagicStone
    {
        public override void AddOtherIngredients(Recipe recipe)
        {
            recipe.AddIngredient(ItemID.LargeTopaz);
            base.AddOtherIngredients(recipe);
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("金之石");
            Tooltip.SetDefault("使用金元素魔法程度的能力\n金符「金属疲劳」");
        }
        public override void SetDefaults()
        {
            item.shoot = ModContent.ProjectileType<StoneOfMetalProj>();
            base.SetDefaults();
            item.damage = 20;
        }
    }
    public class StoneOfWood : MagicStone
    {
        public override void AddOtherIngredients(Recipe recipe)
        {
            recipe.AddIngredient(ItemID.LargeEmerald);
            base.AddOtherIngredients(recipe);
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("木之石");
            Tooltip.SetDefault("使用木元素魔法程度的能力\n木符「风灵的角笛」");
        }
        public override void SetDefaults()
        {
            item.shoot = ModContent.ProjectileType<StoneOfWoodProj>();
            base.SetDefaults();
            item.damage = 10;
        }
    }
    public class StoneOfWater : MagicStone
    {
        public override void AddOtherIngredients(Recipe recipe)
        {
            recipe.AddIngredient(ItemID.LargeSapphire);
            base.AddOtherIngredients(recipe);
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("水之石");
            Tooltip.SetDefault("使用水元素魔法程度的能力\n水符「水精公主」");
        }
        public override void SetDefaults()
        {
            item.shoot = ModContent.ProjectileType<StoneOfWaterProj>();
            base.SetDefaults();
            item.damage = 15;
            item.mana = 5;
        }
    }
    public class StoneOfFire : MagicStone
    {
        public override void AddOtherIngredients(Recipe recipe)
        {
            recipe.AddIngredient(ItemID.LargeRuby);
            base.AddOtherIngredients(recipe);
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("火之石");
            Tooltip.SetDefault("使用火元素魔法程度的能力\n火符「火神之光」");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.damage = 25;
            item.shoot = ModContent.ProjectileType<StoneOfFireProj>();

        }
    }
    public class StoneOfEarth : MagicStone
    {
        public override void AddOtherIngredients(Recipe recipe)
        {
            recipe.AddIngredient(ItemID.LargeAmber);
            base.AddOtherIngredients(recipe);
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("土之石");
            Tooltip.SetDefault("使用土元素魔法程度的能力\n土符「慵懒三石塔」");
        }
        public override void SetDefaults()
        {
            item.shoot = ModContent.ProjectileType<StoneOfEarthProj>();
            base.SetDefaults();
        }
    }
    public class StoneOfMoon : MagicStone
    {
        public override void AddOtherIngredients(Recipe recipe)
        {
            recipe.AddIngredient(ItemID.LargeAmethyst);
            recipe.AddIngredient(ItemID.CrystalBall);
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("月之石");
            Tooltip.SetDefault("使用月元素魔法程度的能力\n月符「沉静的月神」");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.shoot = ModContent.ProjectileType<StoneOfMoonProj>();
            item.damage = 30;
            item.mana = 5;
        }
    }
    public class StoneOfSun : MagicStone
    {
        public override void AddOtherIngredients(Recipe recipe)
        {
            recipe.AddIngredient(ItemID.LargeDiamond);
            recipe.AddIngredient(ItemID.CrystalBall);
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("日之石");
            Tooltip.SetDefault("使用日元素魔法程度的能力\n日符「皇家圣焰」");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.shoot = ModContent.ProjectileType<StoneOfSunProj>();
            item.mana = 20;
        }
    }
    public class StoneOfMetalProj : MagicArea
    {
        public override Color MainColor => Color.Yellow;
        public override int Cycle => 48;

        public override void ShootProj(bool dying = false)
        {
            bool flag = dying && projectile.timeLeft % 3 != 0;
            SoundEngine.PlaySound(SoundID.Item69);
            for (int n = 0; n < (flag ? 3 : 1); n++)
                Projectile.NewProjectile(projectile.GetSource_FromThis(), player.Center + (dying ? projectile.velocity.RotatedByRandom(MathHelper.Pi / 3) : projectile.velocity) * 64, projectile.velocity * 32 * (flag ? 1 : 1), ModContent.ProjectileType<MetalAttack>(), (int)(projectile.damage * Main.rand.NextFloat(1.25f, 0.95f)), projectile.knockBack * 3, projectile.owner, flag ? (Main.rand.Next(4) + 1) : 0);
        }
    }
    public class StoneOfWoodProj : MagicArea
    {
        public override Color MainColor => Color.Green;
        public override int Cycle => 24;

        public override void ShootProj(bool dying = false)
        {
            SoundEngine.PlaySound(SoundID.Item74);
            int m = Main.rand.Next(5, 9) - (dying ? 3 : 0);
            float randAngle = Main.rand.NextFloat(-MathHelper.Pi / 12, MathHelper.Pi / 12);
            for (int n = 0; n < m; n++)
                Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center + 64 * projectile.velocity, projectile.velocity.RotatedBy(randAngle + MathHelper.Pi / 3 * (n / (m - 1f) - 0.5f)) * 32,
                    ModContent.ProjectileType<WoodAttack>(), projectile.damage, projectile.knockBack, projectile.owner, Main.rand.Next(Main.rand.Next(5)), Main.rand.NextFloat(24, 48));
        }
    }
    public class StoneOfWaterProj : MagicArea
    {
        public override Color MainColor => Color.Blue;
        public override int Cycle => 6;
        public override void ShootProj(bool dying = false)
        {
            if (dying)
                for (int n = -2; n < 3; n += 2)
                {
                    var rand = Main.rand.NextFloat(-MathHelper.Pi / 12, MathHelper.Pi / 12) * .5f;
                    Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center + 64 * projectile.velocity, projectile.velocity.RotatedBy(MathHelper.Pi / 12 * n + rand) * 16, ModContent.ProjectileType<WaterAttack>(), projectile.damage, projectile.knockBack, projectile.owner, Main.rand.Next(5));
                    //for (int k = 0; k < 3; k++)
                    //    Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center + (64 + 16 * k) * projectile.velocity, projectile.velocity.RotatedBy(MathHelper.Pi / 12 * n + rand) * (32 + 8 * k) / 3f, ModContent.ProjectileType<WaterAttack>(), projectile.damage, projectile.knockBack, projectile.owner, Main.rand.Next(5));
                }
            else
                for (int n = -1; n < 2; n += 2)
                {
                    Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center + 64 * projectile.velocity, projectile.velocity.RotatedBy(MathHelper.Pi / 12 * n) * 16, ModContent.ProjectileType<WaterAttack>(), projectile.damage, projectile.knockBack, projectile.owner, Main.rand.Next(5));
                }
            SoundEngine.PlaySound(SoundID.Item84);

        }
    }
    public class StoneOfFireProj : MagicArea
    {
        public override Color MainColor => Color.Red;
        public override int Cycle => 30;

        public override void ShootProj(bool dying = false)
        {
            if (dying && projectile.timeLeft % 2 == 1) return;
            SoundEngine.PlaySound(SoundID.Item74);

            Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center + 64 * projectile.velocity, projectile.velocity.RotatedByRandom(MathHelper.TwoPi / 48f * (dying ? 2 : 1)) * 32,
                ModContent.ProjectileType<FireAttack>(), projectile.damage, projectile.knockBack, projectile.owner);
        }
    }
    public class StoneOfEarthProj : MagicArea
    {
        public override Color MainColor => Color.Orange;
        public override int Cycle => 6;

        public override void ShootProj(bool dying = false)
        {

        }
    }
    public class StoneOfMoonProj : MagicArea
    {
        public override int Cycle => 12;
        public override Color MainColor => Color.Purple;
        public override void ShootProj(bool dying = false)
        {
            float r = Main.rand.Next(-32, 32) * (dying ? 8f : 1f);
            int randX = Main.rand.Next(-256, 256);//Main.rand.Next(-64, 64);
            var v = new Vector2(randX, -Main.rand.Next(280, 560));
            Projectile.NewProjectile(projectile.GetSource_FromThis(), Main.MouseWorld + new Vector2(r, 0) - v, default, ModContent.ProjectileType<MoonAttack>(), projectile.damage, projectile.knockBack, projectile.owner, v.ToRotation(), v.Length() * 2);
        }
    }
    public class StoneOfSunProj : MagicArea
    {
        public override Color MainColor => Color.White;
        public override int Cycle => 90;

        public override void ShootProj(bool dying = false)
        {
            if (dying && projectile.timeLeft % 3 != 0) return;
            SoundEngine.PlaySound(SoundID.Item74);

            Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center + 64 * projectile.velocity, projectile.velocity.RotatedByRandom(MathHelper.Pi / 48f) * 2,
                ModContent.ProjectileType<SunAttack>(), projectile.damage, projectile.knockBack, projectile.owner);
        }
    }
    public class MetalAttack : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 16;
            Projectile.timeLeft = 180;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 4;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
            Projectile.aiStyle = -1;
        }
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            float alpha = (Projectile.timeLeft / 180f).SmoothSymmetricFactor(1 / 12f);
            for (int n = 9; n > -1; n--)
                Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.oldPos[n] - Main.screenPosition, new Rectangle((int)(16 * Projectile.ai[0]), 0, 16, 16), lightColor * ((10 - n) * .1f) * alpha, Projectile.oldRot[n], new Vector2(8), 3f * ((10 - n) * .1f), 0, 0);
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.CanBeChasedBy() && target.type != NPCID.WallofFlesh && target.type != NPCID.WallofFleshEye)
                target.velocity += Projectile.velocity * (Projectile.ai[0] == 0 ? 1f : 0.25f) * (crit ? .6f : .2f);
            for (int n = 0; n < 5 - Projectile.penetrate; n++)
            {
                if (Projectile.ai[0] == 0 && Main.rand.NextBool(2))
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity * .5f + Main.rand.NextVector2Unit() * Main.rand.Next(4, 8), Type, Projectile.damage * 3 / 4, Projectile.knockBack / 2, Projectile.owner, Main.rand.Next(4) + 1);
                for (int k = 0; k < 15; k++)
                {
                    Dust.NewDustPerfect(target.Center, DustID.Silver, Main.rand.NextVector2Unit() * Main.rand.NextFloat(4) + Projectile.velocity, 0, default, Main.rand.NextFloat(1, 2));
                }
            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (Projectile.ai[0] == 0)
                return targetHitbox.Intersects(Utils.CenteredRectangle(projHitbox.Center(), projHitbox.Size() * 3));
            return base.Colliding(projHitbox, targetHitbox);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            var vec = Projectile.velocity;
            if (vec.X == oldVelocity.X) vec.X = -vec.X;
            if (vec.Y == oldVelocity.Y) vec.Y = -vec.Y;

            if (Projectile.ai[0] == 0)
                for (int n = 0; n < Projectile.penetrate; n++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, -vec + Main.rand.NextVector2Unit() * Main.rand.Next(4, 8), Type, Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner, Main.rand.Next(4) + 1);
                }
            for (int k = 0; k < 30; k++)
            {
                Dust.NewDustPerfect(Projectile.Center + oldVelocity, DustID.Silver, Main.rand.NextVector2Unit() * Main.rand.NextFloat(4) + oldVelocity, 0, default, Main.rand.NextFloat(1, 2));
            }
            return base.OnTileCollide(oldVelocity);
        }
        public override void AI()
        {
            if (Projectile.ai[0] > 0) Projectile.velocity += new Vector2(0, 0.4f);
            Projectile.rotation++;
            for (int n = 9; n > 0; n--)
            {
                Projectile.oldPos[n] = Projectile.oldPos[n - 1];
                Projectile.oldRot[n] = Projectile.oldRot[n - 1];
            }
            Projectile.oldPos[0] = Projectile.Center;
            Projectile.oldRot[0] = Projectile.rotation;
            base.AI();
        }
    }
    public class WoodAttack : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 8;
            Projectile.timeLeft = 180;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
            Projectile.aiStyle = -1;
        }
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            float alpha = (Projectile.timeLeft / 180f).SmoothSymmetricFactor(1 / 12f);

            for (int n = 9; n > -1; n--)
                for (int m = 0; m < 5; m++)
                    Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.oldPos[n] - Main.screenPosition + (m == 0 ? default : Main.rand.NextVector2Unit() * 4), new Rectangle((int)(16 * Projectile.ai[0]), 0, 16, 16), Color.Lerp(lightColor, Color.White, .5f) with { A = 0 } * alpha * ((10 - n) * .1f) * (m == 0 ? 1 : Main.rand.NextFloat(0.25f, 0.5f)), Projectile.oldRot[n], new Vector2(8), 2f * ((10 - n) * .1f), 0, 0);
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int n = 0; n < 4 - Projectile.penetrate; n++)
            {
                for (int k = 0; k < 15; k++)
                {
                    Dust.NewDustPerfect(target.Center, DustID.TerraBlade, Main.rand.NextVector2Unit() * Main.rand.NextFloat(4) + Projectile.velocity, 0, default, Main.rand.NextFloat(0.5f, 1.5f));
                }
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int k = 0; k < 15; k++)
            {
                Dust.NewDustPerfect(Projectile.Center + oldVelocity, DustID.TerraBlade, Main.rand.NextVector2Unit() * Main.rand.NextFloat(4) + oldVelocity, 0, default, Main.rand.NextFloat(0.5f, 1.5f));
            }
            return base.OnTileCollide(oldVelocity);
        }
        public override void AI()
        {
            NPC target = null;
            float maxDistance = 256f;
            foreach (var npc in Main.npc)
            {
                if (!npc.CanBeChasedBy() || npc.friendly) continue;
                var currentDistance = Vector2.Distance(npc.Center, Projectile.Center);
                if (currentDistance < maxDistance)
                {
                    maxDistance = currentDistance;
                    target = npc;
                }
            }
            if (target != null)
            {
                //var fac = MathF.Cos(Main.GameUpdateCount * MathHelper.Pi / 7.5f) * .5f + .5f;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, (target.Center - Projectile.Center).SafeNormalize(default) * Projectile.ai[1], Utils.GetLerpValue(32, 256, maxDistance, true) * 0.25f);
                //Projectile.velocity = Projectile.velocity.SafeNormalize(default) * Projectile.ai[1];
                if (Main.GameUpdateCount % 3 == 0)
                {
                    if (Projectile.timeLeft > 165) Projectile.timeLeft--;
                    if (Projectile.timeLeft < 15) Projectile.timeLeft++;
                }


            }
            //else
            //{
            //    Projectile.alpha = Projectile.timeLeft;
            //}
            Projectile.rotation = Projectile.velocity.ToRotation();
            for (int n = 9; n > 0; n--)
            {
                Projectile.oldPos[n] = Projectile.oldPos[n - 1];
                Projectile.oldRot[n] = Projectile.oldRot[n - 1];
            }
            Projectile.oldPos[0] = Projectile.Center;
            Projectile.oldRot[0] = Projectile.rotation;
        }
    }
    public class WaterAttack : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 8;
            Projectile.timeLeft = 180;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 5;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
            Projectile.aiStyle = -1;
            Projectile.extraUpdates = 2;
        }
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            float alpha = (Projectile.timeLeft / 180f).SmoothSymmetricFactor(1 / 12f);
            for (int n = 9; n > -1; n--)
            {
                for (int m = 0; m < 4; m++)
                    Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.oldPos[n] - Main.screenPosition - (Projectile.velocity + Main.rand.NextVector2Unit() * 4) * MathF.Sqrt(m * .5f), new Rectangle((int)(16 * Projectile.ai[0]), 0, 16, 16), Color.Lerp(lightColor, Color.White, .5f) with { A = 0 } * ((10 - n) * .1f) * alpha, Projectile.oldRot[n] + MathHelper.PiOver2, new Vector2(8), 1f * ((10 - n) * .1f), 0, 0);

            }
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int k = 0; k < 7; k++)
            {
                Dust.NewDustPerfect(Projectile.Center + oldVelocity * .5f, DustID.BlueTorch, Main.rand.NextVector2Unit() * Main.rand.NextFloat(4) + oldVelocity / 8f, 0, default, Main.rand.NextFloat(0.5f, 1.5f));
            }
            for (int k = 0; k < 7; k++)
            {
                Dust.NewDustPerfect(Projectile.Center + oldVelocity * .5f, DustID.BlueTorch, Main.rand.NextVector2Unit() * Main.rand.NextFloat(4), 0, default, Main.rand.NextFloat(0.5f, 1.5f));
            }
            if (Projectile.alpha == 0 && !Main.rand.NextBool(4))
            {

                Projectile.alpha = 1;
                if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X;
                if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y;
            }
            else
            {
                if (Projectile.timeLeft > 3)
                {
                    Projectile.timeLeft = 3;
                    Projectile.velocity = oldVelocity;
                }

            }

            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 2;
            for (int n = 0; n < 6 - Projectile.penetrate; n++)
            {
                for (int k = 0; k < 7; k++)
                {
                    Dust.NewDustPerfect(target.Center, DustID.BlueTorch, Main.rand.NextVector2Unit() * Main.rand.NextFloat(4) + Projectile.velocity / 8f, 0, default, Main.rand.NextFloat(0.5f, 1.5f));
                }
                for (int k = 0; k < 7; k++)
                {
                    Dust.NewDustPerfect(target.Center, DustID.BlueTorch, Main.rand.NextVector2Unit() * Main.rand.NextFloat(4), 0, default, Main.rand.NextFloat(0.5f, 1.5f));
                }
            }
            base.OnHitNPC(target, damage, knockback, crit);
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            for (int n = 9; n > 0; n--)
            {
                Projectile.oldPos[n] = Projectile.oldPos[n - 1];
                Projectile.oldRot[n] = Projectile.oldRot[n - 1];
            }
            Projectile.oldPos[0] = Projectile.Center;
            Projectile.oldRot[0] = Projectile.rotation;
        }
    }

    public class FireAttack : ModProjectile
    {
        BezierCurve<FloatVector2, Vector2> bezierCurve;
        Projectile projectile => Projectile;
        /// <summary>
        /// 0为火球 1大爆炸 2小爆炸
        /// </summary>
        int style => (int)projectile.ai[0];
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 32;
            Projectile.timeLeft = 180;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.aiStyle = -1;
        }
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            switch (style)
            {
                case 0:
                    {
                        float alpha = (Projectile.timeLeft / 180f).SmoothSymmetricFactor(1 / 12f);
                        alpha = 1f;
                        Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition, new Rectangle(0, (int)Main.GameUpdateCount / 2 % 4, 78, 42), Color.White with { A = 0 } * alpha, Projectile.rotation, new Vector2(66, 21), new Vector2(2.5f, 1.75f) * .5f * new Vector2(MathF.Cos(Main.GlobalTimeWrappedHourly * MathHelper.TwoPi) * .25f + 1.75f, 1f), 0, 0);
                        for (int n = 0; n < 8; n++)
                        {
                            Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition + (MathHelper.PiOver4 * n).ToRotationVector2() * 8 + Main.rand.NextVector2Unit() * Main.rand.NextFloat(2, 6), new Rectangle(0, 42 * Main.rand.Next(4), 78, 42), Color.Lerp(Color.White, Color.Red, Main.rand.NextFloat(0, .5f)) with { A = 0 } * 0.125f * alpha, Projectile.rotation, new Vector2(66, 21), new Vector2(2f, 1.5f), 0, 0);
                        }
                        for (int n = 0; n < 8; n++)
                        {
                            Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition + (MathHelper.PiOver4 * n).ToRotationVector2() * 12 + Main.rand.NextVector2Unit() * Main.rand.NextFloat(6, 12), new Rectangle(0, 42 * Main.rand.Next(4), 78, 42), Color.Lerp(Color.Orange, Color.Red, Main.rand.NextFloat(0, .5f)) with { A = 0 } * 0.0625f * alpha, Projectile.rotation, new Vector2(66, 21), new Vector2(2f, 1.5f) * 1.5f, 0, 0);
                        }
                        for (int n = 9; n > -1; n--)
                        {
                            Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.oldPos[n] - Main.screenPosition - (Projectile.velocity + Main.rand.NextVector2Unit() * 4), new Rectangle(0, 42 * Main.rand.Next(4), 78, 42), Color.Lerp(lightColor, Color.White, .5f) with { A = 0 } * ((10 - n) * .1f) * alpha * .25f, Projectile.oldRot[n], new Vector2(66, 21), 1f * ((10 - n) * .1f), 0, 0);
                        }
                        break;
                    }
                case 1:
                    {
                        var fac = 1 - projectile.timeLeft / 21f;
                        for (int n = 0; n < 3; n++)
                            Main.EntitySpriteDraw(ModContent.Request<Texture2D>(Texture.Replace("FireAttack", "ExplosionEffect")).Value, projectile.Center - Main.screenPosition + Main.rand.NextVector2Unit() * 4, new Rectangle(0, 588 - projectile.timeLeft / 3 * 98, 98, 98), new Color(255, 255, 255, 0) * fac.HillFactor2(1) * .75f, projectile.rotation, new Vector2(49), 3f * fac, 0, 0);//new Rectangle(0, projectile.timeLeft / 2, 52, 52)

                        break;
                    }
                case 2:
                    {
                        var fac = 1 - projectile.timeLeft / 21f;
                        Main.EntitySpriteDraw(ModContent.Request<Texture2D>(Texture.Replace("FireAttack", "ExplosionEffect")).Value, projectile.Center - Main.screenPosition, new Rectangle(0, 588 - projectile.timeLeft / 3 * 98, 98, 98), new Color(255, 255, 255, 0) * fac.HillFactor2(1), projectile.rotation, new Vector2(49), 2f * fac, 0, 0);//new Rectangle(0, projectile.timeLeft / 2, 52, 52)
                        break;
                    }
            }

            //IVector<FloatVector2, Vector2>[] vectors = new IVector<FloatVector2, Vector2>[4];
            ////for (int n = 0; n < 6; n++)
            ////{
            ////    vectors[n] = new FloatVector2((MathHelper.TwoPi / 5 * 2 * n).ToRotationVector2() * 128 + new Vector2(960, 560));
            ////}
            //for (int n = 0; n < 4; n++)
            //{
            //    vectors[n] = new FloatVector2((MathHelper.TwoPi / 3 * n).ToRotationVector2() * 128 + new Vector2(960, 560));
            //}
            //if (bezierCurve == null)
            //{
            //    bezierCurve = new BezierCurve<FloatVector2, Vector2>(vectors);
            //    bezierCurve.Recalculate(60);
            //}
            //var m = BezierCurve<FloatVector2, Vector2>.c_Matrixes[1];
            //var str = m.ToString();
            ////var vecs = BezierCurve<FloatVector2, Vector2>.c_Vectors;
            ////if (vecs.Count < 4)
            ////{
            ////    vecs.Add(BezierCurve<FloatVector2, Vector2>.GetVectors(0));
            ////    vecs.Add(BezierCurve<FloatVector2, Vector2>.GetVectors(1));
            ////    vecs.Add(BezierCurve<FloatVector2, Vector2>.GetVectors(2));
            ////    vecs.Add(BezierCurve<FloatVector2, Vector2>.GetVectors(3));
            ////}
            ////var matrix = BezierCurve<FloatVector2, Vector2>.GetMatrix(2);
            ////var str = matrix.ToString();
            ////str += "";
            ////var vec = vecs[1];
            ////var str2 = "(";
            ////for (int i = 0; i < vec.Length; i++)
            ////{
            ////    str2 += $" {vec[i]}";
            ////}
            ////str2 += ")";
            //foreach (var point in bezierCurve.results)
            //{
            //    var vec = point.Value;
            //    Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, vec, new Rectangle(0, 0, 1, 1), Color.Red, 0, new Vector2(.5f), 8, 0, 0);
            //}
            //foreach (var point in vectors)
            //{
            //    var vec = point.Value;
            //    Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, vec, new Rectangle(0, 0, 1, 1), Color.Cyan, 0, new Vector2(.5f), 16, 0, 0);
            //}
            if (bezierCurve == null)
            {
                var wtf = new IVector<FloatVector2, Vector2>[4];
                for (int n = 0; n < 4; n++)
                {
                    wtf[n] = new FloatVector2((MathHelper.TwoPi / 3 * n).ToRotationVector2() * 128 + new Vector2(960, 560));
                }
                bezierCurve = new BezierCurve<FloatVector2, Vector2>(wtf);
                //bezierCurve.Recalculate(60);
            }
            var input = new Vector2[4];
            for (int n = 0; n < 4; n++)
            {
                input[n] = (MathHelper.TwoPi / 3 * n).ToRotationVector2() * 128 + new Vector2(960, 560);
            }
            var vectors = new Vector2[4];
            vectors[0] = input[0];
            vectors[^1] = input[^1];
            Vector2[] array = BezierCurve<FloatVector2, Vector2>.c_Matrixes[1].Apply(input[1..^1], input[0], input[1]);
            for (int m = 0; m < 2; m++)
                vectors[m + 1] = array[m];
            array = new Vector2[60];

            for (int n = 0; n < 60; n++)
            {
                float t = n / (60 - 1f);
                for (int i = 0; i < 4; i++)
                {
                    if (i == 0) array[n] = vectors[0] * MathF.Pow(1 - t, 3);
                    else array[n] = array[n] + vectors[i] * MathF.Pow(1 - t, 3 - i) * MathF.Pow(t, i) * BezierCurve<FloatVector2, Vector2>.c_Vectors[1][i];
                }
            }
            #region MyRegion
            //MatrixEX matrix = new MatrixEX(4, (i, j) => i switch
            //{
            //    0 => j switch
            //    {
            //        0 => 2,
            //        1 => 3,
            //        2 => 3,
            //        3 or _ => 3
            //    },
            //    1 => j switch
            //    {
            //        0 => 1,
            //        1 => 1,
            //        2 => 4,
            //        3 or _ => 5
            //    },
            //    2 => j switch
            //    {
            //        0 => 5,
            //        1 => 1,
            //        2 => 4,
            //        3 or _ => 0
            //    },
            //    3 or _ => j switch
            //    {
            //        0 => 2,
            //        1 => 7,
            //        2 => 1,
            //        3 or _ => 8
            //    }
            //});
            //float[] values = matrix.Apply(new float[] { 1, 2, 3, 4 });
            //var str = "(";
            //for (int n = 0; n < 4; n++) 
            //{
            //    str += $" {values[n]},";
            //}
            //str += ")";
            //str += "";
            #endregion

            foreach (var vec in array)
            {
                Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, vec, new Rectangle(0, 0, 1, 1), Color.Red, 0, new Vector2(.5f), 8, 0, 0);
            }
            foreach (var vec in input)
            {
                Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, vec, new Rectangle(0, 0, 1, 1), Color.Cyan, 0, new Vector2(.5f), 16, 0, 0);
            }
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (style == 0)
            {
                Projectile.timeLeft = 15;
                Projectile.friendly = false;
                Projectile.tileCollide = false;
                Projectile.velocity = oldVelocity * .375f;
                for (int n = 0; n < 3; n++)
                {
                    var unit = (MathHelper.TwoPi / 6 * n + projectile.rotation).ToRotationVector2();
                    var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center + unit * 16, unit * 4, projectile.type, projectile.damage, 8, projectile.owner, 1);
                    proj.timeLeft = 21;
                    proj.width = proj.height = 160;
                    proj.penetrate = -1;
                    proj.Center = projectile.Center + projectile.velocity;
                    proj.rotation = MathHelper.TwoPi / 6 * n + projectile.rotation;
                    proj.tileCollide = false;
                }
                for (int num431 = 4; num431 < 31; num431++)
                {
                    float num432 = projectile.oldVelocity.X * (30f / (float)num431);
                    float num433 = projectile.oldVelocity.Y * (30f / (float)num431);
                    for (int n = 0; n < 4; n++)
                    {
                        int num434 = Dust.NewDust(new Vector2(projectile.oldPosition.X - num432, projectile.oldPosition.Y - num433) + Main.rand.NextVector2Unit() * Main.rand.NextFloat(4) + projectile.velocity, 8, 8, MyDustId.Fire, projectile.oldVelocity.X, projectile.oldVelocity.Y, 100, Color.Orange, 1.2f);
                        Main.dust[num434].noGravity = true;
                        Dust dust = Main.dust[num434];
                        dust.velocity = projectile.velocity;
                        dust.velocity *= 0.5f;
                    }
                }
                SoundEngine.PlaySound(SoundID.Item62);
            }
            return false;
        }
        public override void Kill(int timeLeft)
        {
            //switch (style)
            //{
            //    case 0:
            //        {
            //            for (int n = 0; n < 3; n++)
            //            {
            //                var unit = (MathHelper.TwoPi / 6 * n + projectile.rotation).ToRotationVector2();
            //                var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center + unit * 16, unit * 4, projectile.type, projectile.damage, 8, projectile.owner, 1);
            //                proj.timeLeft = 21;
            //                proj.width = proj.height = 160;
            //                proj.penetrate = -1;
            //                proj.Center = projectile.Center + projectile.velocity;
            //                proj.rotation = MathHelper.TwoPi / 6 * n + projectile.rotation;
            //                proj.tileCollide = false;
            //            }
            //            for (int num431 = 4; num431 < 31; num431++)
            //            {
            //                float num432 = projectile.oldVelocity.X * (30f / (float)num431);
            //                float num433 = projectile.oldVelocity.Y * (30f / (float)num431);
            //                for (int n = 0; n < 4; n++)
            //                {
            //                    int num434 = Dust.NewDust(new Vector2(projectile.oldPosition.X - num432, projectile.oldPosition.Y - num433) + Main.rand.NextVector2Unit() * Main.rand.NextFloat(4) + projectile.velocity, 8, 8, MyDustId.Fire, projectile.oldVelocity.X, projectile.oldVelocity.Y, 100, Color.Orange, 1.2f);
            //                    Main.dust[num434].noGravity = true;
            //                    Dust dust = Main.dust[num434];
            //                    dust.velocity = projectile.velocity;
            //                    dust.velocity *= 0.5f;
            //                }

            //            }
            //            SoundEngine.PlaySound(SoundID.Item62);
            //            break;
            //        }
            //}
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(24, 300);
            //target.immune[Projectile.owner] = 0;
            if (style == 0)
            {
                Projectile.timeLeft = 15;
                Projectile.friendly = false;
                for (int n = 0; n < 3; n++)
                {
                    var unit = (MathHelper.TwoPi / 6 * n + projectile.rotation).ToRotationVector2();
                    var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center + unit * 16, unit * 4, projectile.type, projectile.damage, 8, projectile.owner, 1);
                    proj.timeLeft = 21;
                    proj.width = proj.height = 160;
                    proj.penetrate = -1;
                    proj.Center = projectile.Center + projectile.velocity;
                    proj.rotation = MathHelper.TwoPi / 6 * n + projectile.rotation;
                    proj.tileCollide = false;
                }
                for (int num431 = 4; num431 < 31; num431++)
                {
                    float num432 = projectile.oldVelocity.X * (30f / (float)num431);
                    float num433 = projectile.oldVelocity.Y * (30f / (float)num431);
                    for (int n = 0; n < 4; n++)
                    {
                        int num434 = Dust.NewDust(new Vector2(projectile.oldPosition.X - num432, projectile.oldPosition.Y - num433) + Main.rand.NextVector2Unit() * Main.rand.NextFloat(4) + projectile.velocity, 8, 8, MyDustId.Fire, projectile.oldVelocity.X, projectile.oldVelocity.Y, 100, Color.Orange, 1.2f);
                        Main.dust[num434].noGravity = true;
                        Dust dust = Main.dust[num434];
                        dust.velocity = projectile.velocity;
                        dust.velocity *= 0.5f;
                    }

                }
                SoundEngine.PlaySound(SoundID.Item62);
            }
            else
            {
                target.immune[projectile.owner] = 0;
                projectile.frameCounter++;
            }
        }
        public override void AI()
        {
            switch (style)
            {
                case 0:
                    {
                        Projectile.rotation = Projectile.velocity.ToRotation();
                        Dust.NewDustPerfect(projectile.Center, MyDustId.Fire, new Vector2(0, 0), 0, Color.White, 1f).noGravity = true;
                        for (int n = 9; n > 0; n--)
                        {
                            Projectile.oldPos[n] = Projectile.oldPos[n - 1];
                            Projectile.oldRot[n] = Projectile.oldRot[n - 1];
                        }
                        Projectile.oldPos[0] = Projectile.Center;
                        Projectile.oldRot[0] = Projectile.rotation;
                        break;
                    }
                case 1:
                case 2:
                    {
                        projectile.friendly = projectile.frameCounter == 0;
                        break;
                    }
            }
            if (style == 1 && projectile.timeLeft == 10)
            {
                for (int n = 0; n < 3; n++)
                {
                    var unit = Main.rand.NextVector2Unit();
                    var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center + unit * Main.rand.NextFloat(16, 32f) * 16, unit * Main.rand.NextFloat(2, 8), projectile.type, projectile.damage / 2, 8, projectile.owner, 2);
                    proj.timeLeft = 21;
                    proj.width = proj.height = 80;
                    proj.penetrate = -1;
                    proj.Center = projectile.Center + projectile.velocity;
                    proj.rotation = MathHelper.TwoPi / 6 * n + projectile.rotation;
                    proj.tileCollide = false;
                    for (int m = 0; m < 8; m++)
                    {
                        Dust.NewDustPerfect(proj.Center, MyDustId.Fire, Main.rand.NextVector2Unit() * Main.rand.NextFloat(2, 8), 0, default, Main.rand.NextFloat(0.5f, 2f));
                    }
                }
                SoundEngine.PlaySound(SoundID.Item62);
            }
            base.AI();
        }
    }
    public class EarthAttack : ModProjectile
    {
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return base.OnTileCollide(oldVelocity);
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
        }
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return base.PreDraw(ref lightColor);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }
        public override void AI()
        {
            base.AI();
        }
    }
    public class MoonAttack : ModProjectile
    {
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int n = 0; n < 15; n++)
            {
                Dust.NewDustPerfect(target.Center, DustID.PinkTorch, dirVec.SafeNormalize(default).RotatedByRandom(MathHelper.Pi / 6) * Main.rand.NextFloat(-6, 6), 0, default, Main.rand.NextFloat(1, 2)).noGravity = true;
            }
        }
        public Projectile projectile => Projectile;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (projectile.timeLeft > 30)
            {
                return false;
            }
            float point = 0f;
            float factor = projectile.timeLeft / 15f;
            factor = MathHelper.Clamp(factor * factor, 0, 1);
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, projectile.Center + dirVec * factor, 8 * (float)Math.Sin(MathHelper.Pi * Math.Sqrt(1 - projectile.timeLeft / 30f)), ref point))
            {
                return true;
            }

            return false;
        }

        public Vector2 dirVec;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            var factor = 0.125f * MathHelper.SmoothStep(0, 1, (45 - projectile.timeLeft) / 15f);

            spriteBatch.Draw(TextureAssets.Projectile[Type].Value, projectile.Center + dirVec * 0.5f - Main.screenPosition, null, Color.Purple with { A = 0 } * factor, projectile.ai[0], new Vector2(16, 48), new Vector2(projectile.ai[1] / 32f, factor * 1.25f), 0, 0);
            spriteBatch.Draw(TextureAssets.Projectile[Type].Value, projectile.Center + dirVec * 0.5f - Main.screenPosition, null, Color.White with { A = 0 } * factor, projectile.ai[0], new Vector2(16, 48), new Vector2(projectile.ai[1] / 32f, factor * .625f), 0, 0);

            if (projectile.timeLeft < 30)
            {
                float fac = (30 - projectile.timeLeft) / 15f;
                fac = MathHelper.Clamp(fac * fac, 0, 1);
                factor = (float)Math.Sin(MathHelper.Pi * Math.Sqrt(1 - projectile.timeLeft / 30f));
                spriteBatch.Draw(TextureAssets.Projectile[Type].Value, projectile.Center + dirVec * 0.5f * fac - Main.screenPosition + Main.rand.NextVector2Unit() * factor * 12f, null, Color.Purple with { A = 0 } * factor, projectile.ai[0], new Vector2(16, 48), new Vector2(projectile.ai[1] / 32f * fac, factor * 1.25f), 0, 0);
                spriteBatch.Draw(TextureAssets.Projectile[Type].Value, projectile.Center + dirVec * 0.5f * fac - Main.screenPosition + Main.rand.NextVector2Unit() * factor * 8f, null, Color.White with { A = 0 } * factor, projectile.ai[0], new Vector2(16, 48), new Vector2(projectile.ai[1] / 32f * fac, factor * .625f), 0, 0);
            }
            var randSize = factor * Main.rand.NextFloat(0.5f);
            #region 法阵
            Matrix transform =
                Matrix.CreateScale(2) *
                Matrix.CreateTranslation(-1, -1, -1) *
                new Matrix(projectile.ai[1] * .5f, 0, 0, 0,
                                                0, 48 * (1 + factor + randSize), 0, 0,
                                                0, 0, 48 * (1 + factor + randSize), 0,
                                                0, 0, 0, 1) *
                Matrix.CreateRotationX(Main.GlobalTimeWrappedHourly * MathHelper.TwoPi / 2f) *
                Matrix.CreateRotationZ(projectile.ai[0]);
            CustomVertexInfo[] vertexInfos = new CustomVertexInfo[16];
            for (int n = 0; n < 16; n++)
            {
                var vec = new Vector3(n % 2, n / 4 % 2, n / 2 % 2);
                vertexInfos[n].TexCoord = new Vector3(vec.Y, vec.Z, 1);

                vertexInfos[n].Color = Color.Purple * factor;
                vec = Vector3.Transform(vec, transform);
                if (n == 7)
                {
                    randSize = factor * Main.rand.NextFloat(0.75f);

                    transform =
                    Matrix.CreateScale(2) *
                    Matrix.CreateTranslation(-1, -1, -1) *
                    new Matrix(projectile.ai[1] * .5f * 1.05f, 0, 0, 0,
                                            0, 48 * (3 - factor - randSize) * .8f, 0, 0,
                                            0, 0, 48 * (3 - factor - randSize) * .8f, 0,
                                            0, 0, 0, 1) *
                    Matrix.CreateRotationX(-Main.GlobalTimeWrappedHourly * MathHelper.TwoPi) *
                    Matrix.CreateRotationZ(projectile.ai[0]);
                }
                vec += new Vector3(projectile.Center + dirVec * 0.5f - Main.screenPosition - new Vector2(Main.screenWidth, Main.screenHeight) * .5f, 0);
                vec.Z = (2000 - vec.Z) / 2000f;
                vec /= vec.Z;
                vertexInfos[n].Position = new Vector2(vec.X, vec.Y) + Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) * .5f;
            }
            CustomVertexInfo[] vertexs = new CustomVertexInfo[24];
            for (int n = 0; n < 24; n++)
            {
                int index = (n % 12) switch
                {
                    0 => 0,
                    1 => 4,
                    2 => 6,
                    3 => 0,
                    4 => 6,
                    5 => 2,
                    6 => 1,
                    7 => 5,
                    8 => 7,
                    9 => 1,
                    10 => 7,
                    11 or _ => 3,
                };
                if (n > 11) index += 8;
                vertexs[n] = vertexInfos[index];
            }
            StoneOfThePhilosophersHelper.VertexDraw(vertexs, ModContent.Request<Texture2D>("StoneOfThePhilosophers/MagicArea_2").Value, TextureAssets.MagicPixel.Value);
            #endregion

            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("沉静的月神");
        }
        public override void SetDefaults()
        {
            projectile.tileCollide = false;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.DamageType = DamageClass.Magic;
            projectile.aiStyle = -1;
            projectile.width = 1;
            projectile.height = 1;
            projectile.timeLeft = 45;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
        }
        public override void AI()
        {
            if (projectile.timeLeft == 45)
            {
                dirVec = projectile.ai[0].ToRotationVector2() * projectile.ai[1];
            }

            if (projectile.timeLeft == 30)
            {
                SoundEngine.PlaySound(SoundID.Item12, projectile.Center);
            }
            //if (owner.type != ModContent.NPCType<ErchiusHorror>() || !owner.active || owner.ai[3] != 2) projectile.Kill();
        }
    }
    public class SunAttack : ModProjectile
    {
        Projectile projectile => Projectile;
        /// <summary>
        /// 0为火球 1大爆炸 2小爆炸
        /// </summary>
        int style => (int)projectile.ai[0];
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 32;
            Projectile.timeLeft = 180;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.aiStyle = -1;
        }
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            switch (style)
            {
                case 0:
                    {
                        float alpha = (Projectile.timeLeft / 180f).SmoothSymmetricFactor(1 / 12f);
                        BlendState state = new BlendState();
                        state.ColorBlendFunction = BlendFunction.Add;
                        state.ColorDestinationBlend = Blend.InverseSourceAlpha;
                        state.AlphaDestinationBlend = Blend.Zero;
                        state.ColorSourceBlend = Blend.InverseDestinationColor;
                        state.AlphaSourceBlend = Blend.One;
                        Main.spriteBatch.End();
                        Main.spriteBatch.Begin(SpriteSortMode.Deferred, state, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                        //alpha = 1f;
                        //Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, 0, 960, 1120), Color.White);
                        Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition, null, Color.White * alpha, Projectile.rotation, new Vector2(16), new Vector2(2.5f, 1.75f) * .25f * (MathF.Cos(Main.GlobalTimeWrappedHourly * MathHelper.Pi) * .5f + 1.5f) * 16, 0, 0);
                        //for (int n = 0; n < 8; n++)
                        //{
                        //    Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition + (MathHelper.PiOver4 * n).ToRotationVector2() * 8 + Main.rand.NextVector2Unit() * Main.rand.NextFloat(2, 6), null, Color.Lerp(Color.White, Color.Red, Main.rand.NextFloat(0, .5f)) * 0.125f * alpha, Projectile.rotation, new Vector2(16), new Vector2(2f, 1.5f), 0, 0);
                        //}
                        //for (int n = 0; n < 8; n++)
                        //{
                        //    Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition + (MathHelper.PiOver4 * n).ToRotationVector2() * 12 + Main.rand.NextVector2Unit() * Main.rand.NextFloat(6, 12), null, Color.Lerp(Color.Orange, Color.Red, Main.rand.NextFloat(0, .5f)) * 0.0625f * alpha, Projectile.rotation, new Vector2(16), new Vector2(2f, 1.5f) * 1.5f, 0, 0);
                        //}
                        //for (int n = 9; n > -1; n--)
                        //{
                        //    Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.oldPos[n] - Main.screenPosition - (Projectile.velocity + Main.rand.NextVector2Unit() * 4), null, Color.Lerp(lightColor, Color.White, .5f) * ((10 - n) * .1f) * alpha * .25f * 10, Projectile.oldRot[n], new Vector2(16), 1f * ((10 - n) * .1f), 0, 0);
                        //}
                        Main.spriteBatch.End();
                        state.Dispose();
                        Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                        break;
                    }
                case 1:
                    {
                        var fac = 1 - projectile.timeLeft / 21f;
                        var fac1 = fac.HillFactor2(1);

                        for (int n = 0; n < 3; n++)
                            Main.EntitySpriteDraw(ModContent.Request<Texture2D>(Texture.Replace("SunAttack", "ExplosionEffect")).Value, projectile.Center - Main.screenPosition + Main.rand.NextVector2Unit() * 4, new Rectangle(0, 588 - projectile.timeLeft / 3 * 98, 98, 98), new Color(255, 255, 255, 0) * fac1 * .75f, projectile.rotation, new Vector2(49), 3f * fac, 0, 0);//new Rectangle(0, projectile.timeLeft / 2, 52, 52)
                        Main.EntitySpriteDraw(ModContent.Request<Texture2D>(Texture.Replace("SunAttack", "FlameOfNuclear")).Value, projectile.Center - Main.screenPosition, null, Color.Lerp(Color.Orange, Color.Yellow, fac1) with { A = 0 } * fac1, projectile.rotation, new Vector2(128), 1.5f * fac, 0, 0);
                        Main.EntitySpriteDraw(ModContent.Request<Texture2D>(Texture.Replace("SunAttack", "FlameOfNuclear")).Value, projectile.Center - Main.screenPosition, null, Color.Lerp(Color.Yellow, Color.White, fac1) * fac1, projectile.rotation, new Vector2(128), 1f * fac, 0, 0);

                        break;
                    }
                case 2:
                    {
                        var fac = 1 - projectile.timeLeft / 21f;
                        var fac1 = fac.HillFactor2(1);
                        Main.EntitySpriteDraw(ModContent.Request<Texture2D>(Texture.Replace("SunAttack", "ExplosionEffect")).Value, projectile.Center - Main.screenPosition, new Rectangle(0, 588 - projectile.timeLeft / 3 * 98, 98, 98), new Color(255, 255, 255, 0) * fac1, projectile.rotation, new Vector2(49), 2f * fac, 0, 0);//new Rectangle(0, projectile.timeLeft / 2, 52, 52)
                        Main.EntitySpriteDraw(ModContent.Request<Texture2D>(Texture.Replace("SunAttack", "FlameOfNuclear")).Value, projectile.Center - Main.screenPosition, null, Color.Lerp(Color.Orange, Color.Yellow, fac1) with { A = 0 } * fac1, projectile.rotation, new Vector2(128), 1f * fac, 0, 0);//new Rectangle(0, projectile.timeLeft / 2, 52, 52)
                        Main.EntitySpriteDraw(ModContent.Request<Texture2D>(Texture.Replace("SunAttack", "FlameOfNuclear")).Value, projectile.Center - Main.screenPosition, null, Color.Lerp(Color.Yellow, Color.White, fac1) with { A = 0 } * fac1, projectile.rotation, new Vector2(128), .5f * fac, 0, 0);

                        break;
                    }
            }

            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return base.OnTileCollide(oldVelocity);
        }
        public override void Kill(int timeLeft)
        {
            switch (style)
            {
                case 0:
                    {
                        for (int n = 0; n < 3; n++)
                        {
                            var unit = (MathHelper.TwoPi / 6 * n + projectile.rotation).ToRotationVector2();
                            var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center + unit * 16, unit * 4, projectile.type, projectile.damage, 8, projectile.owner, 1);
                            proj.timeLeft = 21;
                            proj.width = proj.height = 160;
                            proj.penetrate = -1;
                            proj.Center = projectile.Center + projectile.velocity;
                            proj.rotation = MathHelper.TwoPi / 6 * n + projectile.rotation;
                            proj.tileCollide = false;
                        }
                        for (int num431 = 4; num431 < 31; num431++)
                        {
                            float num432 = projectile.oldVelocity.X * (30f / (float)num431);
                            float num433 = projectile.oldVelocity.Y * (30f / (float)num431);
                            for (int n = 0; n < 4; n++)
                            {
                                int num434 = Dust.NewDust(new Vector2(projectile.oldPosition.X - num432, projectile.oldPosition.Y - num433) + Main.rand.NextVector2Unit() * Main.rand.NextFloat(4) + projectile.velocity, 8, 8, MyDustId.Fire, projectile.oldVelocity.X, projectile.oldVelocity.Y, 100, Color.Orange, 1.2f);
                                Main.dust[num434].noGravity = true;
                                Dust dust = Main.dust[num434];
                                dust.velocity = projectile.velocity;
                                dust.velocity *= 0.5f;
                            }

                        }
                        SoundEngine.PlaySound(SoundID.Item62);
                        break;
                    }
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(24, 300);
            //target.immune[Projectile.owner] = 0;
            if (style == 0) Projectile.Kill();
            else
            {
                target.immune[projectile.owner] = 0;
                projectile.frameCounter++;
            }
        }
        public override void AI()
        {
            switch (style)
            {
                case 0:
                    {
                        Projectile.rotation = Projectile.velocity.ToRotation();
                        Dust.NewDustPerfect(projectile.Center, MyDustId.Fire, new Vector2(0, 0), 0, Color.White, 1f).noGravity = true;
                        for (int n = 9; n > 0; n--)
                        {
                            Projectile.oldPos[n] = Projectile.oldPos[n - 1];
                            Projectile.oldRot[n] = Projectile.oldRot[n - 1];
                        }
                        Projectile.oldPos[0] = Projectile.Center;
                        Projectile.oldRot[0] = Projectile.rotation;
                        break;
                    }
                case 1:
                case 2:
                    {
                        projectile.friendly = projectile.frameCounter == 0;
                        break;
                    }
            }
            //if (style == 1 && projectile.timeLeft == 10)
            //{
            //    for (int n = 0; n < 3; n++)
            //    {
            //        var unit = Main.rand.NextVector2Unit();
            //        var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center + unit * Main.rand.NextFloat(16, 32f) * 16, unit * Main.rand.NextFloat(2, 8), projectile.type, projectile.damage / 2, 8, projectile.owner, 2);
            //        proj.timeLeft = 21;
            //        proj.width = proj.height = 80;
            //        proj.penetrate = -1;
            //        proj.Center = projectile.Center + projectile.velocity;
            //        proj.rotation = MathHelper.TwoPi / 6 * n + projectile.rotation;
            //        proj.tileCollide = false;
            //        for (int m = 0; m < 8; m++)
            //        {
            //            Dust.NewDustPerfect(proj.Center, MyDustId.Fire, Main.rand.NextVector2Unit() * Main.rand.NextFloat(2, 8), 0, default, Main.rand.NextFloat(0.5f, 2f));
            //        }
            //    }
            //    SoundEngine.PlaySound(SoundID.Item62);
            //}
            base.AI();
        }
    }
}