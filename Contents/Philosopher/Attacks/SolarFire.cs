using LogSpiralLibrary;
using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing;
using LogSpiralLibrary.CodeLibrary.Utilties;
using LogSpiralLibrary.CodeLibrary.Utilties.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StoneOfThePhilosophers.Contents.Fire;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace StoneOfThePhilosophers.Contents.Philosopher.Attacks;

public class SolarFireZone : ModProjectile
{
    private Player Owner => Main.player[Projectile.owner];
    public override string Texture => $"Terraria/Images/Item_{ItemID.LivingFireBlock}";

    public override void SetDefaults()
    {
        Projectile.timeLeft = 900;
        Projectile.width = Projectile.height = 1;
        Projectile.friendly = false;
        Projectile.hostile = false;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.hide = true;
        Projectile.aiStyle = -1;
        base.SetDefaults();
    }

    public override void AI()
    {
        Projectile.Center = Owner.MountedCenter;
        if (Projectile.timeLeft > 300)
        {
            if (Projectile.timeLeft % 60 == 0)
            {
                for (int n = 0; n < 5; n++)
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, default, ModContent.ProjectileType<SolarFireBall>(),
                        Projectile.damage, Projectile.knockBack, Projectile.owner, .2f * n);
            }
        }
        else
        {
            if (Projectile.timeLeft % 60 == 0)
            {
                for (int n = 0; n < 5; n++)
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, default, ModContent.ProjectileType<SolarFireLaser>(),
                        Projectile.damage, Projectile.knockBack, Projectile.owner, .2f * n, Main.MouseWorld.X, Main.MouseWorld.Y);
            }
        }
        base.AI();
    }
}

public class SolarFireBall : ModProjectile
{
    public override string Texture => $"Terraria/Images/Item_{ItemID.LivingFireBlock}";

    public override void SetDefaults()
    {
        Projectile.timeLeft = 120;
        Projectile.width = Projectile.height = 16;
        Projectile.friendly = true;
        Projectile.penetrate = 1;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.DamageType = DamageClass.Magic;
        Projectile.aiStyle = -1;
        base.SetDefaults();
    }

    public override void AI()
    {
        if (Projectile.timeLeft > 105)
        {
            float t = 120 - Projectile.timeLeft;
            t /= 15f;

            Projectile.Center = Main.player[Projectile.owner].Center +
                ((MathF.Sqrt(t * t + 1) - 1 + Projectile.ai[0]) * MathHelper.TwoPi).ToRotationVector2() * 64 * MathF.Pow(t, .5f);

            Projectile.velocity = Vector2.UnitY;
        }
        else if (Projectile.timeLeft == 105)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.netUpdate = true;
                Projectile.velocity = (Main.MouseWorld - Projectile.Center).SafeNormalize(default) * 32;
            }
            if (Projectile.velocity != default)
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            SoundEngine.PlaySound(SoundID.Item62, Projectile.Center);

            for (int n = 0; n < 20; n++)
                Dust.NewDustPerfect(Projectile.Center, DustID.SolarFlare, Main.rand.NextVector2Unit() * Main.rand.NextFloat(0, 4) - Projectile.velocity * .1f, 0, default, .5f);

            for (int n = 0; n < 10; n++)
                Dust.NewDustPerfect(Projectile.Center, DustID.SolarFlare, Main.rand.NextVector2Unit() * Main.rand.NextFloat(0, 8) - Projectile.velocity * .5f, 0, default, .5f);
        }
        else if (Projectile.timeLeft == 90)
            Projectile.tileCollide = true;
        if (Main.rand.NextBool(3))
            Dust.NewDustPerfect(Projectile.Center, DustID.SolarFlare).noGravity = true;
        base.AI();
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        for (int n = 0; n < 10; n++)
            Dust.NewDust(Projectile.position, 16, 16, DustID.SolarFlare);
        target.AddBuff(BuffID.OnFire, 360);
        target.AddBuff(BuffID.Daybreak, 120);

        base.OnHitNPC(target, hit, damageDone);
    }

    public override void OnKill(int timeLeft)
    {
        SoundEngine.PlaySound(SoundID.Item62, Projectile.Center);

        var proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center + Main.rand.NextFloat(Main.rand.NextFloat(64)) * Main.rand.NextVector2Unit(), default, ModContent.ProjectileType<FireAttack>(), Projectile.damage * 3 / 4, 8, Projectile.owner, 3);
        proj.timeLeft = 21;
        proj.width = proj.height = 160;
        proj.penetrate = -1;
        proj.Center = Projectile.Center + Projectile.velocity;
        proj.tileCollide = false;
        proj.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
        base.OnKill(timeLeft);
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        for (int n = 0; n < 10; n++)
            Dust.NewDust(Projectile.position, 16, 16, DustID.SolarFlare);
        return true;
    }

    public override bool PreDraw(ref Color lightColor)
    {
        var spriteBatch = Main.spriteBatch;
        var texture = TextureAssets.Projectile[Type].Value;
        var center = Projectile.Center - Main.screenPosition;
        var alpha = (Projectile.timeLeft / 120f).SymmetricalFactor(0.5f, 0.2f);
        var color = Color.White * alpha;
        var origin = texture.Size() * .5f;
        spriteBatch.Draw(texture, center, null, color, Projectile.rotation, origin, 1, 0, 0);
        float scaler = 1f;
        spriteBatch.Draw(texture, center, null, color with { A = 0 } * .5f, Projectile.rotation, origin, 1.5f, 0, 0);
        for (int n = 0; n < 10; n++)
        {
            spriteBatch.Draw(texture, center + Main.rand.NextVector2Unit() * Main.rand.NextFloat(0, 8) * scaler, null, color with { A = 0 } * Main.rand.NextFloat(0, 0.5f),
                Projectile.rotation, origin, Main.rand.NextFloat(1, 1.25f) * scaler, 0, 0);
            center -= Projectile.velocity * .25f;
            scaler *= .9f;
        }
        spriteBatch.DrawQuadraticLaser_PassHeatMap(Projectile.Center, -Projectile.velocity.SafeNormalize(default), LogSpiralLibraryMod.HeatMap[15].Value, LogSpiralLibraryMod.AniTex[8].Value,
            80f, 16f, 0, alpha);
        spriteBatch.DrawQuadraticLaser_PassHeatMap(Projectile.Center + Projectile.velocity, -Projectile.velocity.SafeNormalize(default), LogSpiralLibraryMod.HeatMap[15].Value, LogSpiralLibraryMod.AniTex[8].Value,
    64f, 64f, 0, alpha * .75f);
        return false;
    }

    public override bool ShouldUpdatePosition() => Projectile.timeLeft < 105;
}

public class SolarFireLaser : ModProjectile
{
    public override string Texture => $"Terraria/Images/Item_{ItemID.LivingFireBlock}";

    public override void SetDefaults()
    {
        Projectile.timeLeft = 45;
        Projectile.width = Projectile.height = 1;
        Projectile.friendly = true;
        Projectile.penetrate = -1;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.DamageType = DamageClass.Magic;
        Projectile.aiStyle = -1;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 10;
        base.SetDefaults();
    }

    public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
    {
        float fac = (Projectile.timeLeft / 60f + Projectile.ai[0]);
        if (fac % 1 < .5f)
        {
            Projectile.hide = true;
            overWiresUI.Add(index);
        }
        else
            Projectile.hide = false;
        base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
    }

    public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
    {
        if (Projectile.timeLeft > 38) return false;

        float point = 0f;
        var factor = (45f - Projectile.timeLeft).HillFactor2(45);
        return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.velocity * factor * 1600f, 32 * factor, ref point);
    }

    public override bool ShouldUpdatePosition() => false;

    public override void AI()
    {
        var ownerCenter = Main.player[Projectile.owner].Center;
        float angle = (Projectile.timeLeft / 60f + Projectile.ai[0]) * MathHelper.TwoPi;
        var targetVec = (new Vector2(Projectile.ai[1], Projectile.ai[2]) - ownerCenter).SafeNormalize(default);
        var normalVec = new Vector2(-targetVec.Y, targetVec.X);
        Projectile.Center = ownerCenter - targetVec * 32 + MathF.Cos(angle) * targetVec * 16 + MathF.Sin(angle) * normalVec * 48;
        Projectile.velocity = (new Vector2(Projectile.ai[1], Projectile.ai[2]) + angle.ToRotationVector2() * 16 - Projectile.Center).SafeNormalize(default);
        if (Projectile.velocity != default)
            Projectile.rotation = Projectile.velocity.ToRotation();
        if (Projectile.timeLeft == 45)
            SoundEngine.PlaySound(MySoundID.LaserBeam with { MaxInstances = -1 }, Projectile.Center);
        base.AI();
    }

    public override bool PreDraw(ref Color lightColor)
    {
        var factor = (45f - Projectile.timeLeft).HillFactor2(45);
        var spriteBatch = Main.spriteBatch;

        #region 顶点准备

        int MagicFieldCount = 1;
        int vertexCount = 4 * MagicFieldCount;
        CustomVertexInfoEX[] vertexInfos = new CustomVertexInfoEX[vertexCount];//这里是最基本的顶点们
        for (int n = 0; n < vertexCount; n++)
        {
            var vec = new Vector4(n % 2, n / 2 % 2, 0, 1);
            vertexInfos[n].TexCoord = new Vector3(vec.X, vec.Y, factor);
            vertexInfos[n].Color = Color.Orange;
            //vertexInfos[n].Color = n < 4 ? Color.Red : Color.Cyan;
            vertexInfos[n].Position = new Vector4(n % 2, n / 2 % 2, 0, 1);
        }

        #endregion 顶点准备

        #region 顶点连接

        vertexCount = 6 * MagicFieldCount;
        CustomVertexInfoEX[] vertexs = new CustomVertexInfoEX[vertexCount];//三角形会共用顶点对吧，所以我就不得不准备个大一点的数组然后给所有的三角形安排上自己的顶点
        for (int n = 0; n < vertexCount; n++)
        {
            int index = (n % 6) switch
            {
                0 => 0,
                1 => 2,
                2 => 1,
                3 => 1,
                4 => 2,
                5 or _ => 3,
            };
            index += n / 6 * 4;
            vertexs[n] = vertexInfos[index];
        }

        #endregion 顶点连接

        #region 矩阵生成与绘制

        float height = 2000f;
        Vector3 offset = new(Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) * .5f, 0);
        for (int n = 0; n < MagicFieldCount; n++)
        {
            string pass = "VertexColor";
            string path = "Images/MagicArea_4";
            Vector2 scaler = n switch
            {
                0 => new Vector2(-0.09f, 0.6f),
                1 or _ => new Vector2(-0.15f, 1f)
            };
            Matrix translation = n switch
            {
                0 or 1 or _ => Matrix.CreateTranslation(-Vector3.One)
            };
            float theta = (float)GlobalTimeSystem.GlobalTime * .03f;
            float k = 1 + MathF.Pow(1 - factor, 2);
            Vector3 scale = n switch
            {
                0 => new Vector3(k, k, 1f),
                1 or _ => new Vector3(1.5f, 1.5f, 2f)
            };

            //声明矩阵transform
            //缩放为原来的两倍
            //平移至以原点为中心
            //最重要的缩放矩阵
            //旋转量一号，这个是法阵旋转动画，去掉了就是静止的3d法阵
            //旋转量二号，这个是最重要的之一，把朝向你的法阵逐渐转到朝向角色前方
            //旋转量三号，这个让法阵能跟着鼠标走
            //平移，确保投影中心正确
            //投影
            //平移回去
            //非常ez啊
            Matrix transform =
            Matrix.CreateScale(2) *
            translation *
            Matrix.CreateScale(scale * new Vector3(32, 32, -16)) *
            Matrix.CreateRotationZ(theta) *
            Matrix.CreateRotationY(MathHelper.Pi * 5 / 12f) *
            Matrix.CreateRotationZ(Projectile.rotation) *
            Matrix.CreateTranslation(new Vector3(Projectile.Center, 0) - offset) *
            new Matrix(height, 0, 0, 0,
                            0, height, 0, 0,
                            0, 0, 0, -1,
                            0, 0, 0, height) *
            Matrix.CreateTranslation(offset);
            DrawingMethods.VertexDrawEX(vertexs[(6 * n)..(6 * n + 6)],
                ModContent.Request<Texture2D>($"StoneOfThePhilosophers/{path}").Value,
                ModAsset.Style_4.Value, null,
                new Vector2(Main.GameUpdateCount, Main.GlobalTimeWrappedHourly) * scaler, false, transform, pass, n == 0, n == 1);
        }

        #endregion 矩阵生成与绘制

        spriteBatch.DrawQuadraticLaser_PassHeatMap(Projectile.Center, Projectile.velocity, LogSpiralLibraryMod.HeatMap[15].Value, LogSpiralLibraryMod.AniTex[8].Value, 1600f, factor * 64f);

        return false;
    }
}