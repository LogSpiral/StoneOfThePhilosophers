using LogSpiralLibrary;
using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing.RenderDrawingContents;
using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing.RenderDrawingEffects;
using LogSpiralLibrary.CodeLibrary.Utilties;
using LogSpiralLibrary.CodeLibrary.Utilties.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StoneOfThePhilosophers.Effects;
using System;
using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace StoneOfThePhilosophers.Contents.Sun;

public class SunAttack : ModProjectile
{
    #region Canvas

    private const string CanvasName = nameof(StoneOfThePhilosophers) + ":" + nameof(StoneOfSun);
    private static readonly AirDistortEffect airDistortEffect = new(32, 1.5f, 0, .5f);
    private static readonly BloomEffect bloomEffect = new(0, 1.05f, 1, 3, true, 2, true);
    private static readonly IRenderEffect[][] renderEffects = [[airDistortEffect], [bloomEffect]];

    public override void Load()
    {
        RenderCanvasSystem.RegisterCanvasFactory(CanvasName, () => new RenderingCanvas(renderEffects));
        base.Load();
    }

    private UltraCanvas ultraCanvas;

    private void DrawSun(float alpha, float distortScaler)
    {
        var origState = Main.graphics.GraphicsDevice.BlendState;
        Main.graphics.GraphicsDevice.BlendState = BlendState.Additive;
        //Main.spriteBatch.End();
        //Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

        #region Shader

        float r = Main.rand.NextFloat();
        Main.instance.GraphicsDevice.Textures[1] = ModAsset.Style_6.Value;
        Main.instance.GraphicsDevice.Textures[2] = ModAsset.HeatMap_4.Value;
        Main.instance.GraphicsDevice.SamplerStates[1] = SamplerState.AnisotropicWrap;
        Main.instance.GraphicsDevice.SamplerStates[2] = SamplerState.AnisotropicClamp;
        HeatMapEffect.HeatMap.Parameters["uTime"].SetValue(Projectile.velocity.SafeNormalize(default).RotatedBy(MathHelper.PiOver2) * Main.GlobalTimeWrappedHourly);
        HeatMapEffect.HeatMap.Parameters["uTransform"].SetValue(Matrix.Identity);
        HeatMapEffect.HeatMap.CurrentTechnique.Passes[0].Apply();

        #endregion Shader

        Main.spriteBatch.Draw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition, null, Color.White * alpha, r + MathHelper.PiOver2, new Vector2(16), new Vector2(8f) * (MathF.Cos(Main.GlobalTimeWrappedHourly * MathHelper.Pi) * .125f + 1f) * distortScaler, 0, 0);

        Main.spriteBatch.Draw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition, null, Color.White * alpha, r, new Vector2(16), new Vector2(8f) * (MathF.Cos(Main.GlobalTimeWrappedHourly * MathHelper.Pi) * .125f + 1f) * distortScaler, 0, 0);
        Main.spriteBatch.spriteEffect.CurrentTechnique.Passes[0].Apply();

        //Main.spriteBatch.End();
        //Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        Main.graphics.GraphicsDevice.BlendState = origState;
    }

    private void DrawExposion(float distortScaler)
    {
        var fac = 1 - Projectile.timeLeft / 21f;
        var fac1 = fac.HillFactor2();
        fac *= distortScaler;
        var origState = Main.graphics.GraphicsDevice.BlendState;
        Main.graphics.GraphicsDevice.BlendState = BlendState.Additive;
        for (int n = 0; n < 3; n++)
            Main.spriteBatch.Draw(ModAsset.ExplosionEffect.Value, Projectile.Center - Main.screenPosition + Main.rand.NextVector2Unit() * 4, new Rectangle(0, 588 - Projectile.timeLeft / 3 * 98, 98, 98), new Color(255, 255, 255) * fac1 * .75f, Projectile.rotation, new Vector2(49), 6f * fac, 0, 0);
        Main.spriteBatch.Draw(ModAsset.FlameOfNuclear_Alpha.Value, Projectile.Center - Main.screenPosition, null, Color.Lerp(Color.Orange, Color.Yellow, fac1) * fac1, Projectile.rotation, new Vector2(128), 3f * fac, 0, 0);
        Main.spriteBatch.Draw(ModAsset.FlameOfNuclear_Alpha.Value, Projectile.Center - Main.screenPosition, null, Color.Lerp(Color.Yellow, Color.White, fac1) * fac1, Projectile.rotation, new Vector2(128), 2f * fac, 0, 0);
        Main.graphics.GraphicsDevice.BlendState = origState;
    }

    private void DrawLaser(float distortScaler)
    {
        var origState = Main.graphics.GraphicsDevice.BlendState;
        Main.graphics.GraphicsDevice.BlendState = BlendState.Additive;

        float alpha = (Projectile.timeLeft / 180f).SmoothSymmetricFactor(1 / 12f);
        float dummySclaer = MathF.Pow(distortScaler, 3.0f);
        var vertexs = Projectile.TailVertexFromProj(default, t => 16 * dummySclaer, t => Color.White * MathF.Pow(t, 2f).WaterDropFactor() * 2 * alpha, .5f);//MathF.Pow(t, 1.5f).WaterDropFactor() *
        DrawingMethods.VertexDraw(vertexs,
            ModAsset.Style_4.Value,
            ModAsset.Style_8.Value,
            ModAsset.HeatMap_4_Alpha.Value,
            new Vector2(-Main.GlobalTimeWrappedHourly * 2, 0), false, null,
            "HeatMap", false, false);//default, 30, .5f, true, Color.Yellow
        DrawingMethods.VertexDraw(vertexs,
            ModAsset.Style_4.Value,
            ModAsset.Style_8.Value,
            ModAsset.HeatMap_4_Alpha.Value,
            new Vector2(-Main.GlobalTimeWrappedHourly * 2, 0), false, null,
            "HeatMap", false, false);//default, 30, .5f, true, Color.Yellow
        var spriteBatch = Main.spriteBatch;
        var starLight = ModAsset.StarLight.Value;
        spriteBatch.spriteEffect.CurrentTechnique.Passes[0].Apply();

        spriteBatch.Draw(starLight, Projectile.Center - Projectile.velocity * 2 - Main.screenPosition, null, Color.Orange * alpha, Projectile.rotation + MathHelper.PiOver2, new Vector2(36), new Vector2(1, 5) * .5f * distortScaler, 0, 0);
        spriteBatch.Draw(starLight, Projectile.Center - Projectile.velocity * 2 - Main.screenPosition, null, Color.Orange * .5f * alpha, Projectile.rotation, new Vector2(36), new Vector2(1, 2) * .5f * distortScaler, 0, 0);
        spriteBatch.Draw(starLight, Projectile.Center - Projectile.velocity * 2 - Main.screenPosition, null, Color.White * alpha, Projectile.rotation + MathHelper.PiOver2, new Vector2(36), new Vector2(1, 5) * .25f * distortScaler, 0, 0);
        spriteBatch.Draw(starLight, Projectile.Center - Projectile.velocity * 2 - Main.screenPosition, null, Color.White * .5f * alpha, Projectile.rotation, new Vector2(36), new Vector2(1, 2) * .25f * distortScaler, 0, 0);

        Main.graphics.GraphicsDevice.BlendState = origState;
    }

    private void DrawSelf(float distortScaler)
    {
        switch (Style)
        {
            case 0:
                {
                    float alpha = (Projectile.timeLeft / 180f).SmoothSymmetricFactor(1 / 12f);
                    DrawSun(alpha, distortScaler);
                    break;
                }
            case 1:
                {
                    DrawExposion(distortScaler);
                    break;
                }
            case 2:
                {
                    DrawLaser(distortScaler);
                    break;
                }
            case 3:
                {
                    float alpha = (Projectile.timeLeft / 1200f).SmoothSymmetricFactor(1 / 12f);
                    DrawSun(alpha, 2 * distortScaler);
                    break;
                }
        }
    }

    private void UpdateCanvas()
    {
        if (ultraCanvas != null)
            ultraCanvas.timeLeft = Projectile.timeLeft;
    }

    public override void OnSpawn(IEntitySource source)
    {
        if (Main.dedServ) return;
        ultraCanvas = UltraCanvas.NewUltraCanvas(CanvasName, Projectile.timeLeft, (canvas, scaler) => DrawSelf(scaler));
        base.OnSpawn(source);
    }

    #endregion Canvas

    /// <summary>
    /// 0太阳 1爆炸 2追踪射线 3头顶太阳
    /// </summary>
    private int Style => (int)Projectile.ai[0];

    public override void SetDefaults()
    {
        Projectile.width = Projectile.height = 32;
        Projectile.timeLeft = 180;
        Projectile.DamageType = DamageClass.Magic;
        Projectile.friendly = true;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.penetrate = -1;
        Projectile.aiStyle = -1;
    }

    public override void AI()
    {
        switch (Style)
        {
            case 0:
                {
                    Projectile.rotation = Projectile.velocity.ToRotation();
                    Dust.NewDustPerfect(Projectile.Center, MyDustId.Fire, new Vector2(0, 0), 0, Color.White).noGravity = true;
                    for (int n = 9; n > 0; n--)
                    {
                        Projectile.oldPos[n] = Projectile.oldPos[n - 1];
                        Projectile.oldRot[n] = Projectile.oldRot[n - 1];
                    }
                    Projectile.oldPos[0] = Projectile.Center;
                    Projectile.oldRot[0] = Projectile.rotation;
                    Projectile.velocity *= .96f;
                    if (Projectile.timeLeft % 5 == 0)
                    {
                        foreach (var npc in Main.npc)
                        {
                            if (npc.active && !npc.friendly)
                            {
                                var distance = Vector2.Distance(npc.Center, Projectile.Center);
                                if (distance < 432)
                                {
                                    var fac = Utils.GetLerpValue(432, 0, distance);
                                    var damage = (int)(MathF.Pow(fac, 0.5f) * Projectile.damage / 2);
                                    int count = (int)(fac * 30);
                                    for (int n = 0; n < count; n++)
                                    {
                                        var unit = (n * MathHelper.TwoPi / count).ToRotationVector2();
                                        Dust.NewDustPerfect(npc.Center, MyDustId.Fire, unit * Main.rand.NextFloat(1, 3), 0, default, Main.rand.NextFloat(0.5f, 1f));
                                    }
                                    if (npc.CanBeChasedBy() || npc.type == NPCID.TargetDummy)
                                    {
                                        Main.player[Projectile.owner].ApplyDamageToNPC(npc, damage, 0, Projectile.direction, false, DamageClass.Magic);
                                        Main.player[Projectile.owner].GetModPlayer<ElementSkillPlayer>().ElementChargeValue[6] += damage / ElementSkillPlayer.Devider;
                                    }
                                    npc.AddBuff(BuffID.Daybreak, 30);
                                    //OnHitNPC(npc, damage, 0, false);
                                }
                            }
                        }
                        //for (int n = 0; n < 30; n++)
                        //{
                        //    var unit = (n / 30f * MathHelper.TwoPi).ToRotationVector2();
                        //    Dust.NewDustPerfect(projectile.Center + unit * 96, MyDustId.Fire, unit * Main.rand.NextFloat(2, 8), 0, default, Main.rand.NextFloat(1f, 1.5f));
                        //}
                    }
                    if (Projectile.timeLeft <= 120)
                    {
                        if (Projectile.timeLeft % 15 == 0 && Projectile.timeLeft != 0)
                        {
                            var unit = (Projectile.timeLeft / 120f * MathHelper.TwoPi).ToRotationVector2();
                            var proj = Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), Projectile.Center + unit * 96, unit * 32, Type, Projectile.damage / 2, Projectile.knockBack * .5f, Projectile.owner, 2);
                            proj.penetrate = 3;
                        }
                    }
                    break;
                }
            case 1:
                {
                    Projectile.friendly = Projectile.frameCounter == 0;
                    break;
                }
            case 2:
                {
                    for (int n = 9; n > 0; n--)
                    {
                        Projectile.oldPos[n] = Projectile.oldPos[n - 1];
                        Projectile.oldRot[n] = Projectile.oldRot[n - 1];
                    }
                    Projectile.oldPos[0] = Projectile.Center;
                    Projectile.oldRot[0] = Projectile.rotation;
                    if (Projectile.timeLeft > 165)
                    {
                        Projectile.velocity = Projectile.velocity.RotatedBy(Utils.GetLerpValue(165, 180, Projectile.timeLeft, true) * MathHelper.Pi / 6);
                    }
                    else if (Projectile.timeLeft < 150 && Projectile.timeLeft > 15)
                    {
                        NPC target = null;
                        float MaxDistance = 1024f;
                        foreach (var npc in Main.npc)
                        {
                            if (!npc.friendly && npc.active && npc.CanBeChasedBy())
                            {
                                float distance = Vector2.Distance(npc.Center, Projectile.Center);
                                if (distance < MaxDistance)
                                {
                                    target = npc;
                                    MaxDistance = distance;
                                }
                            }
                        }
                        if (target != null)
                        {
                            Vector2 targetVec = (target.Center - Projectile.Center).SafeNormalize(default) * 32;
                            Projectile.velocity = Vector2.Lerp(Projectile.velocity, targetVec, MathF.Pow(Utils.GetLerpValue(1024, 32, MaxDistance, true), 0.5f) * 0.2f);
                            Projectile.timeLeft = 30;
                        }
                    }
                    else if (Projectile.timeLeft <= 15)
                    {
                        Projectile.friendly = false;
                    }
                    Projectile.rotation = Projectile.velocity.ToRotation();
                    break;
                }
            case 3:
                {
                    Projectile.Center = Main.player[Projectile.owner].Center - new Vector2(0, 256);
                    for (int n = 9; n > 0; n--)
                    {
                        Projectile.oldPos[n] = Projectile.oldPos[n - 1];
                        Projectile.oldRot[n] = Projectile.oldRot[n - 1];
                    }
                    float alpha = (Projectile.timeLeft / 1200f).SmoothSymmetricFactor(1 / 12f);

                    for (int n = 0; n < 16; n++)
                        Lighting.AddLight(Projectile.Center + (MathHelper.TwoPi / 16 * n).ToRotationVector2() * 320 * alpha, new Vector3(1, 0.95f, 0.85f) * 2 * alpha);
                    Projectile.oldPos[0] = Projectile.Center;
                    Projectile.oldRot[0] = Projectile.rotation;
                    if (Projectile.timeLeft % 3 == 0)
                    {
                        foreach (var npc in Main.npc)
                        {
                            if (npc.active && !npc.friendly)
                            {
                                var distance = Vector2.Distance(npc.Center, Projectile.Center);
                                if (distance < 432)
                                {
                                    var fac = Utils.GetLerpValue(432, 0, distance);
                                    var damage = (int)(MathF.Pow(fac, 0.5f) * Projectile.damage);
                                    int count = (int)(fac * 30);
                                    for (int n = 0; n < count; n++)
                                    {
                                        var unit = (n * MathHelper.TwoPi / count).ToRotationVector2();
                                        Dust.NewDustPerfect(npc.Center, MyDustId.Fire, unit * Main.rand.NextFloat(1, 3), 0, default, Main.rand.NextFloat(0.5f, 1f));
                                    }
                                    if (npc.CanBeChasedBy() || npc.type == NPCID.TargetDummy)
                                    {
                                        Main.player[Projectile.owner].ApplyDamageToNPC(npc, damage, 0, Projectile.direction, false, DamageClass.Magic);
                                        Main.player[Projectile.owner].GetModPlayer<ElementSkillPlayer>().ElementChargeValue[6] += damage / ElementSkillPlayer.Devider;
                                    }
                                    npc.AddBuff(BuffID.Daybreak, 60);
                                    //OnHitNPC(npc, damage, 0, false);
                                }
                            }
                        }
                    }
                    if (Projectile.timeLeft % 30 == 0)
                    {
                        SoundEngine.PlaySound(SoundID.Item74, Projectile.Center);

                        if (Projectile.timeLeft % 180 == 0)
                        {
                            var rand = Main.rand.NextFloat();
                            for (int n = 0; n < 8; n++)
                            {
                                var unit = (n / 8f * MathHelper.TwoPi + rand).ToRotationVector2();
                                var proj = Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), Projectile.Center + unit * 192, unit * 24, Type, Projectile.damage / 2, Projectile.knockBack * .5f, Projectile.owner, 2);
                                proj.penetrate = 3;
                            }
                        }
                        else
                        {
                            var _unit = (Projectile.timeLeft / 120f * MathHelper.TwoPi).ToRotationVector2();
                            NPC target = null;
                            float MaxDistance = 1024;
                            foreach (var npc in Main.npc)
                            {
                                if (npc.CanBeChasedBy() && npc.active && !npc.friendly)
                                {
                                    float distance = Vector2.Distance(npc.Center, Projectile.Center);
                                    if (distance < MaxDistance)
                                    {
                                        MaxDistance = distance;
                                        target = npc;
                                    }
                                }
                            }
                            if (target != null)
                            {
                                _unit = (target.Center - Projectile.Center).SafeNormalize(default).RotatedBy(MathHelper.Pi * 2 / 3);
                            }
                            var proj = Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), Projectile.Center + _unit * 96, _unit * 32, Type, Projectile.damage / 2, Projectile.knockBack * .5f, Projectile.owner, 2);
                            proj.penetrate = 3;
                        }
                    }
                    break;
                }
        }
        UpdateCanvas();
        base.AI();
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.AddBuff(BuffID.Daybreak, 120);
        //target.immune[Projectile.owner] = 0;
        if (Style == 0)
        {
            if (Projectile.active)
            {
                var rand = Main.rand.NextFloat();
                for (int n = 0; n < 8; n++)
                {
                    var unit = (n / 8f * MathHelper.TwoPi + rand).ToRotationVector2();
                    var proj = Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), Projectile.Center + unit * 192, unit * 16, Type, Projectile.damage, Projectile.knockBack * .5f, Projectile.owner, 2);
                    proj.penetrate = 3;
                }
            }

            Projectile.Kill();
        }
        else
        {
            target.immune[Projectile.owner] = 0;
            Projectile.frameCounter++;
            if (Style == 2)
            {
                if (Projectile.penetrate == 1)
                {
                    Projectile.timeLeft = 15;
                    Projectile.penetrate = 2;
                    Projectile.friendly = false;
                }
                for (int n = 0; n < 4 - Projectile.penetrate; n++)
                {
                    for (int k = 0; k < 15; k++)
                    {
                        Dust.NewDustPerfect(target.Center, DustID.SolarFlare, Main.rand.NextVector2Unit() * Main.rand.NextFloat(4) + Projectile.velocity, 0, default, Main.rand.NextFloat(0.5f, 1.5f));
                    }
                }
            }
        }
        if (!target.friendly && target.active && target.CanBeChasedBy())
        {
            Main.player[Projectile.owner].GetModPlayer<ElementSkillPlayer>().ElementChargeValue[6] += damageDone / ElementSkillPlayer.Devider;
        }
    }

    public override void OnKill(int timeLeft)
    {
        switch (Style)
        {
            case 0:
                {
                    for (int n = 0; n < 3; n++)
                    {
                        var unit = (MathHelper.TwoPi / 6 * n + Projectile.rotation).ToRotationVector2();
                        var proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center + unit * 16, unit * 4, Projectile.type, Projectile.damage, 8, Projectile.owner, 1);
                        proj.timeLeft = 21;
                        proj.width = proj.height = 640;
                        proj.penetrate = -1;
                        proj.Center = Projectile.Center + Projectile.velocity;
                        proj.rotation = MathHelper.TwoPi / 6 * n + Projectile.rotation;
                        proj.tileCollide = false;
                    }
                    for (int num431 = 4; num431 < 31; num431++)
                    {
                        float num432 = Projectile.oldVelocity.X * (30f / (float)num431);
                        float num433 = Projectile.oldVelocity.Y * (30f / (float)num431);
                        for (int n = 0; n < 4; n++)
                        {
                            int num434 = Dust.NewDust(new Vector2(Projectile.oldPosition.X - num432, Projectile.oldPosition.Y - num433) + Main.rand.NextVector2Unit() * Main.rand.NextFloat(4) + Projectile.velocity, 8, 8, MyDustId.Fire, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, Color.Orange, 1.2f);
                            Main.dust[num434].noGravity = true;
                            Dust dust = Main.dust[num434];
                            dust.velocity = Projectile.velocity;
                            dust.velocity *= 0.5f;
                        }
                    }
                    SoundEngine.PlaySound(SoundID.Item62, Projectile.Center);
                    break;
                }
        }
    }

    public override bool? CanCutTiles() => Style != 3;

    public override bool PreDraw(ref Color lightColor) => false;
}