using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing.RenderDrawingContents;
using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing.RenderDrawingEffects;
using LogSpiralLibrary.CodeLibrary.Utilties.Extensions;
using LogSpiralLibrary;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing;

namespace StoneOfThePhilosophers.Contents.Moon;
public class MoonAttack : ModProjectile
{
    public bool boost = false;
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        for (int n = 0; n < 15; n++)
        {
            Dust.NewDustPerfect(target.Center, DustID.PinkTorch, dirVec.SafeNormalize(default).RotatedByRandom(MathHelper.Pi / 6) * Main.rand.NextFloat(-6, 6), 0, default, Main.rand.NextFloat(1, 2)).noGravity = true;
        }
        if (!target.friendly && target.active && target.CanBeChasedBy())
        {
            Main.player[Projectile.owner].GetModPlayer<ElementSkillPlayer>().ElementChargeValue[5] += damageDone / ElementSkillPlayer.Devider;
        }
    }
    //public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
    //{
    //    overWiresUI.Add(index);
    //    projectile.hide = true;
    //}
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
        if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, projectile.Center + dirVec * factor, (boost ? 2 : 1) * 32 * (float)Math.Sin(MathHelper.Pi * Math.Sqrt(1 - projectile.timeLeft / 30f)), ref point))
        {
            return true;
        }

        return false;
    }

    public Vector2 dirVec;
    public void DrawLaser(float distortScaler)
    {
        SpriteBatch spriteBatch = Main.spriteBatch;
        //Main.NewText(distortScaler);
        //var tex = ModAsset.Style_3.Value;
        //spriteBatch.Draw(tex, Main.ScreenSize.ToVector2() * .5f + Vector2.UnitX * 256, null, Color.White, 0, tex.Size() * .5f, distortScaler * .25f, 0, 0);
        //return;
        var factor = 0.125f * MathHelper.SmoothStep(0, 1, (45 - projectile.timeLeft) / 15f);
        var origin = boost ? new Vector2(128) : new Vector2(16, 48);
        var scaler = boost ? new Vector2(0.125f, 0.375f * 1.25f) : new Vector2(1, 0.75f);
        distortScaler *= distortScaler;
        var texture = boost ? ModAsset.MoonAttack_Ultra_Alpha.Value : ModAsset.MoonAttack_Alpha.Value;
        float a = Main.rand.NextFloat(0, 1);
        float b = Main.rand.NextFloat(0, 1);
        var color = boost ? (Color.Lerp(Color.Lerp(Color.Blue, Color.Cyan, a), Color.Purple, b)) : Color.Purple;
        spriteBatch.Draw(texture, projectile.Center + dirVec * 0.5f - Main.screenPosition, null, color * factor, projectile.ai[0], origin, scaler * new Vector2(projectile.ai[1] / 32f, factor * 1.25f * distortScaler), 0, 0);
        spriteBatch.Draw(texture, projectile.Center + dirVec * 0.5f - Main.screenPosition, null, Color.White * factor, projectile.ai[0], origin, scaler * new Vector2(projectile.ai[1] / 32f, factor * .625f * distortScaler), 0, 0);

        if (projectile.timeLeft < 30)
        {
            float fac = (30 - projectile.timeLeft) / 15f;
            fac = MathHelper.Clamp(fac * fac, 0, 1);
            factor = (float)Math.Sin(MathHelper.Pi * Math.Sqrt(1 - projectile.timeLeft / 30f));
            spriteBatch.Draw(texture, projectile.Center + dirVec * 0.5f * fac - Main.screenPosition + Main.rand.NextVector2Unit() * factor * 12f, null, color * factor, projectile.ai[0], origin, scaler * new Vector2(projectile.ai[1] / 32f * fac, factor * 1.25f * distortScaler), 0, 0);
            spriteBatch.Draw(texture, projectile.Center + dirVec * 0.5f * fac - Main.screenPosition + Main.rand.NextVector2Unit() * factor * 8f, null, Color.White * factor, projectile.ai[0], origin, scaler * new Vector2(projectile.ai[1] / 32f * fac, factor * .625f * distortScaler), 0, 0);
        }
        var randSize = factor * Main.rand.NextFloat(0.5f);
        #region 法阵
        Matrix transform =
            Matrix.CreateScale(2) *
            Matrix.CreateTranslation(-1, -1, -1) *
            new Matrix(projectile.ai[1] * .5f, 0, 0, 0,
                                            0, (boost ? 64 : 48) * (1 + factor + randSize), 0, 0,
                                            0, 0, (boost ? 64 : 48) * (1 + factor + randSize), 0,
                                            0, 0, 0, 1) *
            Matrix.CreateRotationX(Main.GlobalTimeWrappedHourly * MathHelper.TwoPi / 2f) *
            Matrix.CreateRotationZ(projectile.ai[0]);
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
        DrawingMethods.VertexDraw(vertexs, boost ? ModAsset.MagicArea_2_Alpha.Value : ModAsset.MagicArea_5_Alpha.Value, TextureAssets.MagicPixel.Value, pass: "OriginColor", autoStart: false);
        #endregion
    }
    public override bool PreDraw(ref Color lightColor)
    {
        //DrawLaser(1f);

        //var vector = Main.screenTarget.Size();
        //Main.spriteBatch.Draw(ModAsset.StarSky_0.Value, new Rectangle(0, 0, (int)vector.X, (int)vector.Y), Color.White);
        //var borderColor = Color.Cyan * .5f;
        //int width = 4;
        //Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, 0, width, (int)vector.Y), borderColor);
        //Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)vector.X - width, 0, width, (int)vector.Y), borderColor);
        //Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, 0, (int)vector.X, width), borderColor);
        //Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, (int)vector.Y- width, (int)vector.X, width), borderColor);
        return false;
    }
    public override bool ShouldUpdatePosition()
    {
        return false;
    }
    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("沉静的月神");
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
    public override void OnSpawn(IEntitySource source)
    {
        canvas = UltraCanvas.NewUltraCanvas(CanvasName, projectile.timeLeft, (canvas, distortScaler) => DrawLaser(distortScaler));
        base.OnSpawn(source);
    }
    const string CanvasName = nameof(StoneOfThePhilosophers) + ":" + nameof(StoneOfMoon);
    readonly IRenderEffect[][] renderEffects = [[new AirDistortEffect(4f, 2f)], [new MaskEffect(LogSpiralLibraryMod.Mask[1].Value, Color.MediumPurple, 0.15f, 0.2f, default, false, false), new BloomEffect(0, 1.25f, 1, 3, true, 2, true)]];
    public override void Load()
    {
        RenderCanvasSystem.RegisterCanvasFactory(CanvasName, () => new RenderingCanvas([[new AirDistortEffect(4f, 2f)], [new MaskEffect(LogSpiralLibraryMod.Mask[1].Value, Color.MediumPurple, 0.05f, 0.07f, default, false, false)]]));//, new BloomEffect(0, 0, 1, 3, true, 2, true)
        base.Load();
    }
    public override void OnKill(int timeLeft)
    {
        if (canvas != null)
            canvas.timeLeft = 0;
        base.OnKill(timeLeft);
    }
    UltraCanvas canvas;
    public override void AI()
    {
        projectile.localNPCHitCooldown = boost ? 5 : 10;
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
