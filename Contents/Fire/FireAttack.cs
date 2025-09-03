using LogSpiralLibrary.CodeLibrary.Utilties;
using LogSpiralLibrary.CodeLibrary.Utilties.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StoneOfThePhilosophers.Effects;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace StoneOfThePhilosophers.Contents.Fire;

public class FireAttack : ModProjectile
{
    /// <summary>
    /// 0为火球 1真火球 2大爆炸 3小爆炸 4凤凰 5追踪真火 6火之领域
    /// </summary>
    private int Style => (int)Projectile.ai[0];

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

    public override void AI()
    {
        switch (Style)
        {
            case 0:
            case 1:
            case 4:
            case 5:
                {
                    Projectile.rotation = Projectile.velocity.ToRotation();
                    Dust.NewDustPerfect(Projectile.Center, MyDustId.Fire, new Vector2(0, 0), 0, Color.White, 1f).noGravity = true;
                    Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Unit() * 4, MyDustId.Fire, Main.rand.NextVector2Unit() + Projectile.velocity * .25f, 0, Color.White, Main.rand.NextFloat(1.5f, 3f)).noGravity = true;

                    for (int n = 9; n > 0; n--)
                    {
                        Projectile.oldPos[n] = Projectile.oldPos[n - 1];
                        Projectile.oldRot[n] = Projectile.oldRot[n - 1];
                    }
                    Projectile.oldPos[0] = Projectile.Center;
                    Projectile.oldRot[0] = Projectile.rotation;
                    if (Style == 4)
                    {
                        if (Projectile.timeLeft == 15)
                        {
                            Projectile.friendly = false;
                            for (int n = 0; n < 5; n++)
                            {
                                var unit = (MathHelper.TwoPi / 6 * n + Projectile.rotation + Main.rand.NextFloat()).ToRotationVector2();
                                var proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, unit * 32, Projectile.type, Projectile.damage / 2, 8, Projectile.owner, 5);
                                proj.rotation = MathHelper.TwoPi / 6 * n + Projectile.rotation;
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
                            SoundEngine.PlaySound(SoundID.Item62);
                        }
                    }
                    if (Style == 5)
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
                            Projectile.velocity = Vector2.Lerp(Projectile.velocity, (target.Center - Projectile.Center).SafeNormalize(default) * 32, Utils.GetLerpValue(32, 256, maxDistance, true) * 0.25f);
                        }
                    }
                    break;
                }
            case 2:
            case 3:
                {
                    Projectile.friendly = Projectile.frameCounter == 0;
                    break;
                }
            case 6:
                {
                    if (Projectile.timeLeft % 15 == 0)
                    {
                        foreach (var npc in Main.npc)
                        {
                            if (npc.active && !npc.friendly)
                            {
                                var distance = Vector2.Distance(npc.Center, Projectile.Center);
                                if (distance < 320)
                                {
                                    var fac = Utils.GetLerpValue(320, 0, distance);
                                    var damage = (int)(MathF.Pow(fac, 0.5f) * Projectile.damage / 4);
                                    int count = (int)(fac * 30);
                                    for (int n = 0; n < count; n++)
                                    {
                                        var unit = (n * MathHelper.TwoPi / count).ToRotationVector2();
                                        Dust.NewDustPerfect(npc.Center, MyDustId.Fire, unit * Main.rand.NextFloat(1, 3), 0, default, Main.rand.NextFloat(0.5f, 1f));
                                    }
                                    if (npc.CanBeChasedBy() || npc.type == NPCID.TargetDummy)
                                    {
                                        Main.player[Projectile.owner].ApplyDamageToNPC(npc, damage, 0, Projectile.direction, false);
                                        Main.player[Projectile.owner].GetModPlayer<ElementSkillPlayer>().ElementChargeValue[0] += damage / ElementSkillPlayer.Devider;
                                    }
                                    npc.AddBuff(BuffID.Daybreak, 30);
                                    //OnHitNPC(npc, damage, 0, false);
                                }
                            }
                        }
                        for (int n = 0; n < 30; n++)
                        {
                            var unit = (n / 30f * MathHelper.TwoPi).ToRotationVector2();
                            Dust.NewDustPerfect(Projectile.Center + unit * 96, MyDustId.Fire, unit * Main.rand.NextFloat(2, 8), 0, default, Main.rand.NextFloat(1f, 1.5f));
                        }
                    }
                    break;
                }
        }
        if (Style == 2 && Projectile.timeLeft == 10)
        {
            for (int n = 0; n < 3; n++)
            {
                var unit = Main.rand.NextVector2Unit();
                var proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center + unit * Main.rand.NextFloat(16, 32f) * 16, unit * Main.rand.NextFloat(2, 8), Projectile.type, Projectile.damage / 3, 8, Projectile.owner, 3);
                proj.timeLeft = 21;
                proj.width = proj.height = 80;
                proj.penetrate = -1;
                proj.Center = Projectile.Center + Projectile.velocity;
                proj.rotation = MathHelper.TwoPi / 6 * n + Projectile.rotation;
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

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.AddBuff(BuffID.OnFire, 300);
        if (Style == 0 || Style == 1 || Style == 5)
        {
            Projectile.timeLeft = 15;
            Projectile.friendly = false;
            for (int n = 0; n < 3; n++)
            {
                var unit = (MathHelper.TwoPi / 6 * n + Projectile.rotation).ToRotationVector2();
                var proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center + unit * 16, unit * 4, Projectile.type, Projectile.damage * 3 / 4, 8, Projectile.owner, Style == 0 ? 3 : 2);
                proj.timeLeft = 21;
                proj.width = proj.height = 160;
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
            target.immune[Projectile.owner] = 2;
        }
        else if (Style == 4)
        {
            for (int n = 0; n < 2; n++)
            {
                if (!Main.rand.NextBool(4)) continue;
                var unit = (MathHelper.TwoPi / 6 * n + Projectile.rotation).ToRotationVector2();
                var proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center + unit * 16, unit * 4, Projectile.type, Projectile.damage / 6, 8, Projectile.owner, 3);
                proj.timeLeft = 21;
                proj.width = proj.height = 120;
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
            var soundEff = SoundID.Item62;
            soundEff.Volume *= .5f;
            SoundEngine.PlaySound(soundEff);
            target.immune[Projectile.owner] = 2;
        }
        else if (Style == 6)
        {
            target.immune[Projectile.owner] = 0;
            target.AddBuff(BuffID.Daybreak, 300);
        }
        else
        {
            Projectile.frameCounter++;
            target.immune[Projectile.owner] = 0;
        }
        if (!target.friendly && target.active && target.CanBeChasedBy())
        {
            Main.player[Projectile.owner].GetModPlayer<ElementSkillPlayer>().ElementChargeValue[0] += damageDone / ElementSkillPlayer.Devider;
        }
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        if (Style == 0 || Style == 1 || Style == 5)
        {
            Projectile.timeLeft = 15;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.velocity = oldVelocity * .375f;
            for (int n = 0; n < 3; n++)
            {
                var unit = (MathHelper.TwoPi / 6 * n + Projectile.rotation).ToRotationVector2();
                var proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center + unit * 16, unit * 4, Projectile.type, Projectile.damage, 8, Projectile.owner, Style == 0 ? 3 : 2);
                proj.timeLeft = 21;
                proj.width = proj.height = 160;
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
        }
        return false;
    }

    public override bool? CanCutTiles()
    {
        return Style != 6;
    }

    public override bool PreDraw(ref Color lightColor)
    {
        switch (Style)
        {
            case 0:
                {
                    float alpha = (Projectile.timeLeft / 180f).SmoothSymmetricFactor(1 / 12f);
                    Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition, new Rectangle(0, (int)Main.GameUpdateCount / 2 % 4, 78, 42), Color.White with { A = 0 } * alpha, Projectile.rotation, new Vector2(66, 21), new Vector2(2.5f, 1.75f) * .5f * new Vector2(MathF.Cos(Main.GlobalTimeWrappedHourly * MathHelper.TwoPi) * .25f + 1.75f, 1f), 0, 0);
                    for (int n = 0; n < 4; n++)
                    {
                        Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition + (MathHelper.PiOver4 * n).ToRotationVector2() * 8 + Main.rand.NextVector2Unit() * Main.rand.NextFloat(2, 6), new Rectangle(0, 42 * Main.rand.Next(4), 78, 42), Color.Lerp(Color.White, Color.Red, Main.rand.NextFloat(0, .5f)) with { A = 0 } * 0.125f * alpha, Projectile.rotation, new Vector2(66, 21), new Vector2(2f, 1.5f), 0, 0);
                    }
                    for (int n = 0; n < 4; n++)
                    {
                        Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition + (MathHelper.PiOver4 * n).ToRotationVector2() * 12 + Main.rand.NextVector2Unit() * Main.rand.NextFloat(6, 12), new Rectangle(0, 42 * Main.rand.Next(4), 78, 42), Color.Lerp(Color.Orange, Color.Red, Main.rand.NextFloat(0, .5f)) with { A = 0 } * 0.0625f * alpha, Projectile.rotation, new Vector2(66, 21), new Vector2(2f, 1.5f), 0, 0);
                    }
                    for (int n = 9; n > -1; n--)
                    {
                        Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.oldPos[n] - Main.screenPosition - (Projectile.velocity + Main.rand.NextVector2Unit() * 4), new Rectangle(0, 42 * Main.rand.Next(4), 78, 42), Color.Lerp(lightColor, Color.White, .5f) with { A = 0 } * ((10 - n) * .1f) * alpha * .25f, Projectile.oldRot[n], new Vector2(66, 21), 1f * ((10 - n) * .1f), 0, 0);
                    }
                    break;
                }
            case 1:
            case 5:
                {
                    float alpha = (Projectile.timeLeft / 180f).SmoothSymmetricFactor(1 / 12f);
                    Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition, new Rectangle(0, (int)Main.GameUpdateCount / 2 % 4, 78, 42), Color.White with { A = 0 } * alpha, Projectile.rotation, new Vector2(66, 21), new Vector2(2.5f, 1.75f) * .5f * new Vector2(MathF.Cos(Main.GlobalTimeWrappedHourly * MathHelper.TwoPi) * .25f + 1.75f, 1f), 0, 0);
                    for (int n = 0; n < 8; n++)
                    {
                        Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition + (MathHelper.PiOver4 * n).ToRotationVector2() * 8 + Main.rand.NextVector2Unit() * Main.rand.NextFloat(2, 6), new Rectangle(0, 42 * Main.rand.Next(4), 78, 42), Color.Lerp(Color.White, Color.Red, Main.rand.NextFloat(0, .5f)) with { A = 0 } * 0.125f * alpha, Projectile.rotation, new Vector2(66, 21), new Vector2(2f, 1.5f), 0, 0);
                    }
                    for (int n = 0; n < 8; n++)
                    {
                        Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition + (MathHelper.PiOver4 * n).ToRotationVector2() * 12 + Main.rand.NextVector2Unit() * Main.rand.NextFloat(6, 12), new Rectangle(0, 42 * Main.rand.Next(4), 78, 42), Color.Lerp(Color.Orange, Color.Red, Main.rand.NextFloat(0, .5f)) with { A = 0 } * 0.0625f * alpha, Projectile.rotation, new Vector2(66, 21), new Vector2(2f, 1.5f) * 1.5f, 0, 0);
                    }
                    DrawingMethods.VertexDraw(Projectile.TailVertexFromProj(default, 30, .5f, false, Color.Yellow * alpha),
                        ModAsset.Style_4.Value,
                        ModAsset.Style_8.Value,
                        ModAsset.HeatMap_0.Value,
                        new Vector2(-Main.GlobalTimeWrappedHourly * 2, 0), false, null,
                        "HeatMap");
                    break;
                }
            case 2:
                {
                    var fac = 1 - Projectile.timeLeft / 21f;
                    for (int n = 0; n < 3; n++)
                        Main.EntitySpriteDraw(ModAsset.ExplosionEffect.Value, Projectile.Center - Main.screenPosition + Main.rand.NextVector2Unit() * 4, new Rectangle(0, 588 - Projectile.timeLeft / 3 * 98, 98, 98), new Color(255, 255, 255, 0) * fac.HillFactor2(1) * .75f, Projectile.rotation, new Vector2(49), 3f * fac, 0, 0);//new Rectangle(0, projectile.timeLeft / 2, 52, 52)

                    break;
                }
            case 3:
                {
                    var fac = 1 - Projectile.timeLeft / 21f;
                    Main.EntitySpriteDraw(ModAsset.ExplosionEffect.Value, Projectile.Center - Main.screenPosition, new Rectangle(0, 588 - Projectile.timeLeft / 3 * 98, 98, 98), new Color(255, 255, 255, 0) * fac.HillFactor2(1), Projectile.rotation, new Vector2(49), 2f * fac, 0, 0);//new Rectangle(0, projectile.timeLeft / 2, 52, 52)
                    break;
                }
            case 4:
                {
                    float alpha = (Projectile.timeLeft / 180f).SmoothSymmetricFactor(1 / 12f);
                    var tex = ModAsset.FirePhoenix.Value;
                    var rect = new Rectangle(0, (int)Main.GameUpdateCount / 2 % 8 * 76, 72, 76);
                    SpriteEffects spriteEffects = Projectile.velocity.X > 0 ? 0 : SpriteEffects.FlipVertically;
                    //var origin = new Vector2(projectile.velocity.X > 0 ? 58 : 14, 42);
                    var origin = new Vector2(58, Projectile.velocity.X > 0 ? 42 : 34);

                    Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, rect, Color.White with { A = 0 } * alpha, Projectile.rotation, origin, new Vector2(2.5f, 1.75f) * .5f * new Vector2(MathF.Cos(Main.GlobalTimeWrappedHourly * MathHelper.TwoPi) * .25f + 1.75f, 1f), spriteEffects, 0);
                    for (int n = 0; n < 8; n++)
                    {
                        Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition + (MathHelper.PiOver4 * n).ToRotationVector2() * 8 + Main.rand.NextVector2Unit() * Main.rand.NextFloat(2, 6)
                            , rect, Color.Lerp(Color.White, Color.Red, Main.rand.NextFloat(0, .5f)) with { A = 0 } * 0.125f * alpha, Projectile.rotation, origin, new Vector2(3f, 1.5f), spriteEffects, 0);
                    }
                    for (int n = 0; n < 8; n++)
                    {
                        Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition + (MathHelper.PiOver4 * n).ToRotationVector2() * 12 + Main.rand.NextVector2Unit() * Main.rand.NextFloat(6, 12)
                            , rect, Color.Lerp(Color.Orange, Color.Red, Main.rand.NextFloat(0, .5f)) with { A = 0 } * 0.0625f * alpha, Projectile.rotation, origin, new Vector2(4f, 1.5f) * 1.5f, spriteEffects, 0);
                    }
                    //for (int n = 9; n > -1; n--)
                    //{
                    //    Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.oldPos[n] - Main.screenPosition - (Projectile.velocity + Main.rand.NextVector2Unit() * 4), new Rectangle(0, 42 * Main.rand.Next(4), 78, 42), Color.Lerp(lightColor, Color.White, .5f) with { A = 0 } * ((10 - n) * .1f) * alpha * .25f, Projectile.oldRot[n], new Vector2(66, 21), 1f * ((10 - n) * .1f), 0, 0);
                    //}
                    DrawingMethods.VertexDraw(Projectile.TailVertexFromProj(default, t => MathHelper.SmoothStep(30, 0, t), t => Color.Yellow * t.WaterDropFactor(), .5f),
                        ModAsset.Style_4.Value,
                        ModAsset.Style_8.Value,
                        ModAsset.HeatMap_0.Value,
                        new Vector2(-Main.GlobalTimeWrappedHourly * 2, 0), false, null,
                        "HeatMap");//default, 30, .5f * alpha, false, Color.Yellow
                    break;
                }
            case 6:
                {
                    float alpha = (Projectile.timeLeft / 300f).SmoothSymmetricFactor(1 / 12f);
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                    #region Shader

                    float r = Main.GlobalTimeWrappedHourly * 2f;
                    Main.instance.GraphicsDevice.Textures[1] = ModAsset.MagicArea_4.Value;
                    Main.instance.GraphicsDevice.Textures[2] = ModAsset.HeatMap_0.Value;
                    Main.instance.GraphicsDevice.SamplerStates[1] = SamplerState.AnisotropicWrap;
                    Main.instance.GraphicsDevice.SamplerStates[2] = SamplerState.AnisotropicClamp;
                    HeatMapEffect.HeatMap.Parameters["uTime"].SetValue(Projectile.velocity.SafeNormalize(default).RotatedBy(MathHelper.PiOver2) * Main.GlobalTimeWrappedHourly);
                    HeatMapEffect.HeatMap.Parameters["uTransform"].SetValue(Matrix.Identity);

                    HeatMapEffect.HeatMap.CurrentTechnique.Passes[0].Apply();

                    #endregion Shader

                    Main.EntitySpriteDraw(ModAsset.SunAttack.Value, Projectile.Center - Main.screenPosition, null, Color.White * alpha, -2 * r, new Vector2(16), new Vector2(12f) * (-MathF.Cos(Main.GlobalTimeWrappedHourly * MathHelper.Pi) * .25f + 1.25f), 0, 0);//

                    Main.EntitySpriteDraw(ModAsset.SunAttack.Value, Projectile.Center - Main.screenPosition, null, Color.White * alpha, r, new Vector2(16), new Vector2(16f) * (MathF.Cos(Main.GlobalTimeWrappedHourly * MathHelper.Pi) * .125f + 1f), 0, 0);//
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    break;
                }
        }
        return false;
    }
}