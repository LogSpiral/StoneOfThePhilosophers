using LogSpiralLibrary;
using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing;
using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing.RenderDrawingContents;
using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing.RenderDrawingEffects;
using LogSpiralLibrary.CodeLibrary.Utilties.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace StoneOfThePhilosophers.Contents.Moon;

public class MoonAttack : ModProjectile
{
    #region Canvas

    private const string CanvasName = nameof(StoneOfThePhilosophers) + ":" + nameof(StoneOfMoon);
    private static readonly AirDistortEffect airDistortEffect = new(16f, 2f, MathHelper.PiOver2, .05f);
    private static readonly MaskEffect maskEffect = Main.dedServ ? new() : new(LogSpiralLibraryMod.Mask[3].Value, Color.MediumPurple, 0.05f, 0.07f, default, false, false);
    private static readonly BloomEffect bloomEffect = new(0, 1f, 2, 3, true, 0, true);
    private static readonly IRenderEffect[][] renderEffects = [[airDistortEffect], [maskEffect, bloomEffect]];

    public override void Load()
    {
        if (Main.dedServ) return;
        RenderCanvasSystem.RegisterCanvasFactory(CanvasName, () => new(renderEffects));//
        base.Load();
    }

    private UltraCanvas canvas;

    public override void OnSpawn(IEntitySource source)
    {
        if (Projectile.ai[2] == 1)
            canvas = UltraCanvas.NewUltraCanvas(CanvasName, Projectile.timeLeft, (canvas, distortScaler) => DrawLaser(distortScaler, false));
        base.OnSpawn(source);
    }

    private void DrawLaser(float distortScaler, bool normalDrawing)
    {
        SpriteBatch spriteBatch = Main.spriteBatch;
        //Main.NewText(distortScaler);
        //var tex = ModAsset.Style_3.Value;
        //spriteBatch.Draw(tex, Main.ScreenSize.ToVector2() * .5f + Vector2.UnitX * 256, null, Color.White, 0, tex.Size() * .5f, distortScaler * .25f, 0, 0);
        //return;
        var factor = 0.125f * MathHelper.SmoothStep(0, 1, (45 - Projectile.timeLeft) / 15f);
        var origin = boost ? new Vector2(128) : new Vector2(16, 48);
        var scaler = boost ? new Vector2(0.125f, 0.375f * 1.25f) : new Vector2(1, 0.75f);
        distortScaler *= distortScaler;
        var texture = boost ? ModAsset.MoonAttack_Ultra_Alpha.Value : ModAsset.MoonAttack_Alpha.Value;
        float a = Main.rand.NextFloat(0, 1);
        float b = Main.rand.NextFloat(0, 1);
        var color = boost ? (Color.Lerp(Color.Lerp(Color.Blue, Color.Cyan, a), Color.Purple, b)) : Color.Purple;
        var white = Color.White;
        if (normalDrawing)
        {
            color = color with { A = 0 };
            white = white with { A = 0 };
        }
        spriteBatch.Draw(texture, Projectile.Center + dirVec * 0.5f - Main.screenPosition, null, color * factor, Projectile.ai[0], origin, scaler * new Vector2(Projectile.ai[1] / 32f, factor * 1.25f * distortScaler), 0, 0);
        spriteBatch.Draw(texture, Projectile.Center + dirVec * 0.5f - Main.screenPosition, null, white * factor, Projectile.ai[0], origin, scaler * new Vector2(Projectile.ai[1] / 32f, factor * .625f * distortScaler), 0, 0);

        if (Projectile.timeLeft < 30)
        {
            float fac = (30 - Projectile.timeLeft) / 15f;
            fac = MathHelper.Clamp(fac * fac, 0, 1);
            factor = (float)Math.Sin(MathHelper.Pi * Math.Sqrt(1 - Projectile.timeLeft / 30f));
            spriteBatch.Draw(texture, Projectile.Center + dirVec * 0.5f * fac - Main.screenPosition + Main.rand.NextVector2Unit() * factor * 12f, null, color * factor, Projectile.ai[0], origin, scaler * new Vector2(Projectile.ai[1] / 32f * fac, factor * 1.25f * distortScaler), 0, 0);
            spriteBatch.Draw(texture, Projectile.Center + dirVec * 0.5f * fac - Main.screenPosition + Main.rand.NextVector2Unit() * factor * 8f, null, white * factor, Projectile.ai[0], origin, scaler * new Vector2(Projectile.ai[1] / 32f * fac, factor * .625f * distortScaler), 0, 0);
        }
        var randSize = factor * Main.rand.NextFloat(0.5f);

        #region 法阵

        Matrix transform =
            Matrix.CreateScale(2) *
            Matrix.CreateTranslation(-1, -1, -1) *
            new Matrix(Projectile.ai[1] * .5f, 0, 0, 0,
                                            0, (boost ? 64 : 48) * (1 + factor + randSize), 0, 0,
                                            0, 0, (boost ? 64 : 48) * (1 + factor + randSize), 0,
                                            0, 0, 0, 1) *
            Matrix.CreateRotationX(Main.GlobalTimeWrappedHourly * MathHelper.TwoPi / 2f) *
            Matrix.CreateRotationZ(Projectile.ai[0]);
        CustomVertexInfo[] vertexInfos = new CustomVertexInfo[16];
        for (int n = 0; n < 16; n++)
        {
            var vec = new Vector3(n % 2, n / 4 % 2, n / 2 % 2);
            vertexInfos[n].TexCoord = new Vector3(vec.Y, vec.Z, 1);

            vertexInfos[n].Color = color * factor;
            vec = Vector3.Transform(vec, transform);
            if (n == 7)
            {
                randSize = factor * Main.rand.NextFloat(0.75f);

                transform =
                Matrix.CreateScale(2) *
                Matrix.CreateTranslation(-1, -1, -1) *
                new Matrix(Projectile.ai[1] * .5f * 1.05f, 0, 0, 0,
                                        0, 48 * (3 - factor - randSize) * .8f, 0, 0,
                                        0, 0, 48 * (3 - factor - randSize) * .8f, 0,
                                        0, 0, 0, 1) *
                Matrix.CreateRotationX(-Main.GlobalTimeWrappedHourly * MathHelper.TwoPi) *
                Matrix.CreateRotationZ(Projectile.ai[0]);
            }
            vec += new Vector3(Projectile.Center + dirVec * 0.5f - Main.screenPosition - new Vector2(Main.screenWidth, Main.screenHeight) * .5f, 0);
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
        DrawingMethods.VertexDraw(vertexs, boost ? ModAsset.MagicArea_2_Alpha.Value : ModAsset.MagicArea_5_Alpha.Value, TextureAssets.MagicPixel.Value, pass: "OriginColor", autoStart: false);

        #endregion 法阵
    }

    public override bool PreDraw(ref Color lightColor)
    {
        DrawLaser(1f, true);
        return false;
    }

    private void KillCanvas()
    {
        if (canvas != null)
            canvas.timeLeft = 0;
    }

    #endregion Canvas

    private bool boost;
    private Vector2 dirVec;

    public override void SetDefaults()
    {
        Projectile.tileCollide = false;
        Projectile.hostile = false;
        Projectile.friendly = true;
        Projectile.DamageType = DamageClass.Magic;
        Projectile.aiStyle = -1;
        Projectile.width = 1;
        Projectile.height = 1;
        Projectile.timeLeft = 45;
        Projectile.penetrate = -1;
        Projectile.usesLocalNPCImmunity = true;
    }

    public override void AI()
    {
        if (Projectile.timeLeft == 45)
        {
            dirVec = Projectile.ai[0].ToRotationVector2() * Projectile.ai[1];
            boost = Projectile.ai[2] == 1;
            Projectile.localNPCHitCooldown = boost ? 5 : 10;
        }

        if (Projectile.timeLeft == 30)
            SoundEngine.PlaySound(SoundID.Item12, Projectile.Center);
    }

    public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
    {
        if (Projectile.timeLeft > 30)
        {
            return false;
        }
        float point = 0f;
        float factor = Projectile.timeLeft / 15f;
        factor = MathHelper.Clamp(factor * factor, 0, 1);
        if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + dirVec * factor, (boost ? 2 : 1) * 32 * (float)Math.Sin(MathHelper.Pi * Math.Sqrt(1 - Projectile.timeLeft / 30f)), ref point))
        {
            return true;
        }

        return false;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        for (int n = 0; n < 15; n++)
        {
            Dust.NewDustPerfect(target.Center, DustID.PinkTorch, dirVec.SafeNormalize(default).RotatedByRandom(MathHelper.Pi / 6) * Main.rand.NextFloat(-6, 6), 0, default, Main.rand.NextFloat(1, 2)).noGravity = true;
        }
        if (!target.friendly && target.active && target.CanBeChasedBy())
        {
            Main.player[base.Projectile.owner].GetModPlayer<ElementSkillPlayer>().ElementChargeValue[5] += damageDone / ElementSkillPlayer.Devider;
        }
    }

    public override bool ShouldUpdatePosition() => false;

    public override void OnKill(int timeLeft) => KillCanvas();
}