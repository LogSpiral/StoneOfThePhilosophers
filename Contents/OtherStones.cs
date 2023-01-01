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
            item.shoot = ModContent.ProjectileType<StoneOfFireProj>();
            base.SetDefaults();
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
            item.damage = 40;
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
            item.shoot = ModContent.ProjectileType<StoneOfSunProj>();
            base.SetDefaults();
        }
    }
    public class StoneOfMetalProj : MagicArea
    {
        public override Color MainColor => Color.Yellow;
        public override void ShootProj(bool dying = false)
        {

        }
    }
    public class StoneOfWoodProj : MagicArea
    {
        public override Color MainColor => Color.Green;
        public override void ShootProj(bool dying = false)
        {

        }
    }
    public class StoneOfWaterProj : MagicArea
    {
        public override Color MainColor => Color.Blue;
        public override void ShootProj(bool dying = false)
        {

        }
    }
    public class StoneOfFireProj : MagicArea
    {
        public override Color MainColor => Color.Red;
        public override void ShootProj(bool dying = false)
        {
            Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, (Main.MouseWorld - projectile.Center).SafeNormalize(default).RotatedBy(Main.rand.NextFloat(-MathHelper.TwoPi / 48, MathHelper.TwoPi / 48) * (dying ? 2 : 1)) * 16,
                ModContent.ProjectileType<FireAttack>(), projectile.damage, projectile.knockBack, projectile.owner);
        }
    }
    public class StoneOfEarthProj : MagicArea
    {
        public override Color MainColor => Color.Orange;
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
            if (dying && projectile.timeLeft % 2 == 0) return;
            float r = Main.rand.Next(-32, 32) * (dying ? 8f : 1f);
            int randX = Main.rand.Next(-256, 256);//Main.rand.Next(-64, 64);
            var v = new Vector2(randX, -Main.rand.Next(280, 560));
            Projectile.NewProjectile(projectile.GetSource_FromThis(), Main.MouseWorld + new Vector2(r, 0) - v, default, ModContent.ProjectileType<MoonAttack>(), projectile.damage, projectile.knockBack, projectile.owner, v.ToRotation(), v.Length() * 2);
        }
    }
    public class StoneOfSunProj : MagicArea
    {
        public override Color MainColor => Color.White;
        public override void ShootProj(bool dying = false)
        {

        }
    }
    public class MetalAttack : ModProjectile
    {
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
    public class WoodAttack : ModProjectile
    {
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
    public class WaterAttack : ModProjectile
    {
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
    public class FireAttack : ModProjectile
    {
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
            if (Projectile.width == 160)
                return false;
            Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition, new Rectangle(0, (int)Main.GameUpdateCount / 2 % 4, 78, 42), Color.White with { A = 0 }, Projectile.rotation, new Vector2(66, 21), 1f, 0, 0);
            for (int n = 0; n < 8; n++)
            {
                Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition + (MathHelper.PiOver4 * n).ToRotationVector2() * 4 + Main.rand.NextVector2Unit(), new Rectangle(0, 42 * Main.rand.Next(4), 78, 42), Color.White with { A = 0 } * 0.125f, Projectile.rotation, new Vector2(66, 21), 1f, 0, 0);
            }
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(24, 300);
            Projectile.width = Projectile.height = 160;
            if (Projectile.timeLeft > 2)
                Projectile.timeLeft = 2;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            base.AI();
        }
    }
    public class EarthAttack : ModProjectile
    {
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
                spriteBatch.Draw(TextureAssets.Projectile[Type].Value, projectile.Center + dirVec * 0.5f * fac - Main.screenPosition, null, Color.Purple with { A = 0 } * factor, projectile.ai[0], new Vector2(16, 48), new Vector2(projectile.ai[1] / 32f * fac, factor * 1.25f), 0, 0);
                spriteBatch.Draw(TextureAssets.Projectile[Type].Value, projectile.Center + dirVec * 0.5f * fac - Main.screenPosition, null, Color.White with { A = 0 } * factor, projectile.ai[0], new Vector2(16, 48), new Vector2(projectile.ai[1] / 32f * fac, factor * .625f), 0, 0);
            }
            Matrix transform =
            Matrix.CreateScale(2) *
            Matrix.CreateTranslation(-1, -1, -1) *
            new Matrix(projectile.ai[1] * .5f, 0, 0, 0,
                                            0, 48 * (1 + factor), 0, 0,
                                            0, 0, 48 * (1 + factor), 0,
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
                    transform =
                    Matrix.CreateScale(2) *
                    Matrix.CreateTranslation(-1, -1, -1) *
                    new Matrix(projectile.ai[1] * .5f * 1.05f, 0, 0, 0,
                                            0, 48 * (3 - factor) * .8f, 0, 0,
                                            0, 0, 48 * (3 - factor) * .8f, 0,
                                            0, 0, 0, 1) *
                    Matrix.CreateRotationX(-Main.GlobalTimeWrappedHourly * MathHelper.TwoPi) *
                    Matrix.CreateRotationZ(projectile.ai[0]);
                }
                vec += new Vector3(projectile.Center + dirVec * 0.5f - Main.screenPosition - new Vector2(Main.screenWidth, Main.screenHeight) * .5f, 0);
                vec.Z = (1600 - vec.Z) / 1600f;
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
}