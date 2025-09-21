using LogSpiralLibrary.CodeLibrary.Utilties;
using LogSpiralLibrary.CodeLibrary.Utilties.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StoneOfThePhilosophers.Effects;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace StoneOfThePhilosophers.Contents.Water;

public class WaterAttack : ModProjectile
{
    public override bool? CanCutTiles()
    {
        return (int)Projectile.ai[1] != 2;
    }

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
        switch ((int)Projectile.ai[1])
        {
            case 0:
                {
                    float alpha = (Projectile.timeLeft / 180f).SmoothSymmetricFactor(1 / 12f);
                    for (int n = 9; n > -1; n--)
                    {
                        for (int m = 0; m < 4; m++)
                            Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.oldPos[n] - Main.screenPosition - (Projectile.velocity + Main.rand.NextVector2Unit() * 4) * MathF.Sqrt(m * .5f), new Rectangle((int)(16 * Projectile.ai[0]), 0, 16, 16), Color.Lerp(lightColor, Color.White, .5f) with { A = 0 } * ((10 - n) * .1f) * alpha, Projectile.oldRot[n] + MathHelper.PiOver2, new Vector2(8), 1f * ((10 - n) * .1f), 0);
                    }
                    break;
                }
            case 1:
                {
                    float alpha = (Projectile.timeLeft / 180f).SmoothSymmetricFactor(1 / 12f);
                    var tex = ModAsset.WaterAttackUltra.Value;
                    Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, new Rectangle(0, (int)Main.GameUpdateCount / 2 % 4, 78, 42), Color.White with { A = 0 } * alpha, Projectile.rotation, new Vector2(66, 21), new Vector2(2.5f, 1.75f) * .5f * new Vector2(MathF.Cos(Main.GlobalTimeWrappedHourly * MathHelper.TwoPi) * .25f + 1.75f, 1f), 0);
                    for (int n = 0; n < 4; n++)
                    {
                        Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition + (MathHelper.PiOver4 * n).ToRotationVector2() * 8 + Main.rand.NextVector2Unit() * Main.rand.NextFloat(2, 6), new Rectangle(0, 42 * Main.rand.Next(4), 78, 42), Color.Lerp(Color.White, Color.Red, Main.rand.NextFloat(0, .5f)) with { A = 0 } * 0.125f * alpha, Projectile.rotation, new Vector2(66, 21), new Vector2(2f, 1.5f), 0);
                    }
                    for (int n = 0; n < 4; n++)
                    {
                        Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition + (MathHelper.PiOver4 * n).ToRotationVector2() * 12 + Main.rand.NextVector2Unit() * Main.rand.NextFloat(6, 12), new Rectangle(0, 42 * Main.rand.Next(4), 78, 42), Color.Lerp(Color.Orange, Color.Red, Main.rand.NextFloat(0, .5f)) with { A = 0 } * 0.0625f * alpha, Projectile.rotation, new Vector2(66, 21), new Vector2(2f, 1.5f), 0);
                    }
                    for (int n = 9; n > -1; n--)
                    {
                        Main.EntitySpriteDraw(tex, Projectile.oldPos[n] - Main.screenPosition - (Projectile.velocity + Main.rand.NextVector2Unit() * 4), new Rectangle(0, 42 * Main.rand.Next(4), 78, 42), Color.Lerp(lightColor, Color.White, .5f) with { A = 0 } * ((10 - n) * .1f) * alpha * .25f, Projectile.oldRot[n], new Vector2(66, 21), 1f * ((10 - n) * .1f), 0);
                    }
                    break;
                }
            case 2:
                {
                    float alpha = (Projectile.timeLeft / 600f).SmoothSymmetricFactor(1 / 12f);
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                    #region Shader

                    float r = Main.GlobalTimeWrappedHourly * 2f;
                    Main.instance.GraphicsDevice.Textures[1] = ModAsset.MagicArea_4.Value;
                    Main.instance.GraphicsDevice.Textures[2] = ModAsset.HeatMap_1.Value;
                    Main.instance.GraphicsDevice.SamplerStates[1] = SamplerState.AnisotropicWrap;
                    Main.instance.GraphicsDevice.SamplerStates[2] = SamplerState.AnisotropicClamp;
                    HeatMapEffect.HeatMap.Parameters["uTime"].SetValue(Projectile.velocity.SafeNormalize(default).RotatedBy(MathHelper.PiOver2) * Main.GlobalTimeWrappedHourly);
                    var matrix =
                        Matrix.CreateTranslation(-0.5f, -0.5f, 0) *
                        Matrix.CreateScale(1, 1.5f, 1) *

                        Matrix.CreateRotationZ(-2 * r) *
                        Matrix.CreateTranslation(0.5f, 0.5f, 0);
                    HeatMapEffect.HeatMap.Parameters["uTransform"].SetValue(matrix);
                    HeatMapEffect.HeatMap.CurrentTechnique.Passes[0].Apply();

                    #endregion Shader

                    Main.EntitySpriteDraw(ModAsset.SunAttack.Value, Projectile.Center - Main.screenPosition, null, Color.White * alpha, 0, new Vector2(16), new Vector2(16f) * (-MathF.Cos(Main.GlobalTimeWrappedHourly * MathHelper.Pi) * .25f + 1.25f), 0);//

                    matrix =
                        Matrix.CreateTranslation(-0.5f, -0.5f, 0) *
                        Matrix.CreateScale(1, 1.5f, 1) *

                        Matrix.CreateRotationZ(r) *
                        Matrix.CreateTranslation(0.5f, 0.5f, 0);
                    HeatMapEffect.HeatMap.Parameters["uTransform"].SetValue(matrix);
                    HeatMapEffect.HeatMap.CurrentTechnique.Passes[0].Apply();

                    Main.EntitySpriteDraw(ModAsset.SunAttack.Value, Projectile.Center - Main.screenPosition, null, Color.White * alpha, 0, new Vector2(16), new Vector2(20f) * (MathF.Cos(Main.GlobalTimeWrappedHourly * MathHelper.Pi) * .125f + 1f), 0);//
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    break;
                }
        }

        return false;
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        switch ((int)Projectile.ai[1])
        {
            case 0:
            case 1:
                {
                    for (int k = 0; k < 7; k++)
                    {
                        Dust.NewDustPerfect(Projectile.Center + oldVelocity * .5f, DustID.BlueTorch, Main.rand.NextVector2Unit() * Main.rand.NextFloat(4) + oldVelocity / 8f, 0, default, Main.rand.NextFloat(0.5f, 1.5f));
                    }
                    for (int k = 0; k < 7; k++)
                    {
                        Dust.NewDustPerfect(Projectile.Center + oldVelocity * .5f, DustID.BlueTorch, Main.rand.NextVector2Unit() * Main.rand.NextFloat(4), 0, default, Main.rand.NextFloat(0.5f, 1.5f));
                    }
                    if ((int)Projectile.ai[1] == 1)
                    {
                        if (Projectile.alpha < 3 && !Main.rand.NextBool(5))
                        {
                            Projectile.alpha++;
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
                    }
                    else
                    {
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
                    }
                    break;
                }
            case 2:
                {
                    break;
                }
        }

        return false;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        switch ((int)Projectile.ai[1])
        {
            case 0:
            case 1:
                {
                    target.immune[Projectile.owner] = 0;
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
                    if (!target.friendly && target.active && target.CanBeChasedBy())
                    {
                        Main.player[Projectile.owner].GetModPlayer<ElementSkillPlayer>().ElementChargeValue[1] += damageDone / ElementSkillPlayer.Devider;
                    }
                    if ((int)Projectile.ai[1] == 1)
                    {
                        var projectile = Projectile;
                        foreach (var npc in Main.npc)
                        {
                            if (npc.active && !npc.friendly)
                            {
                                var distance = Vector2.Distance(npc.Center, projectile.Center);
                                if (distance < 80)
                                {
                                    var fac = Utils.GetLerpValue(80, 0, distance);
                                    var _damage = (int)(MathF.Pow(fac, 0.5f) * projectile.damage * 3 / 4);
                                    int count = (int)(fac * 10);
                                    for (int n = 0; n < count; n++)
                                    {
                                        var unit = (n * MathHelper.TwoPi / count).ToRotationVector2();
                                        Dust.NewDustPerfect(npc.Center, MyDustId.Water, unit * Main.rand.NextFloat(1, 3), 0, default, Main.rand.NextFloat(0.5f, 1f));
                                    }
                                    if (npc.CanBeChasedBy())
                                    {
                                        Main.player[projectile.owner].ApplyDamageToNPC(npc, _damage, 0, projectile.direction);
                                        Main.player[Projectile.owner].GetModPlayer<ElementSkillPlayer>().ElementChargeValue[1] += _damage / ElementSkillPlayer.Devider;
                                    }
                                }
                            }
                        }
                        if (target.CanBeChasedBy() && target.type != NPCID.WallofFlesh && target.type != NPCID.WallofFleshEye)
                            target.velocity += Projectile.velocity * (hit.Crit ? .6f : .2f) * .25f;
                    }
                    break;
                }
            case 2:
                {
                    break;
                }
        }
        base.OnHitNPC(target, hit, damageDone);
    }

    public override void AI()
    {
        switch ((int)Projectile.ai[1])
        {
            case 0:
            case 1:
                {
                    Projectile.rotation = Projectile.velocity.ToRotation();
                    for (int n = 9; n > 0; n--)
                    {
                        Projectile.oldPos[n] = Projectile.oldPos[n - 1];
                        Projectile.oldRot[n] = Projectile.oldRot[n - 1];
                    }
                    Projectile.oldPos[0] = Projectile.Center;
                    Projectile.oldRot[0] = Projectile.rotation;
                    break;
                }
            case 2:
                {
                    Projectile.Center = Main.player[Projectile.owner].Center;
                    var projectile = Projectile;
                    if (projectile.timeLeft % 15 == 0)
                    {
                        foreach (var npc in Main.npc)
                        {
                            if (npc.active && !npc.friendly)
                            {
                                var distance = Vector2.Distance(npc.Center, projectile.Center);
                                if (distance < 320)
                                {
                                    var fac = Utils.GetLerpValue(320, 0, distance);
                                    var damage = (int)(MathF.Pow(fac, 0.5f) * projectile.damage / 4);
                                    int count = (int)(fac * 30);
                                    for (int n = 0; n < count; n++)
                                    {
                                        var unit = (n * MathHelper.TwoPi / count).ToRotationVector2();
                                        Dust.NewDustPerfect(npc.Center, MyDustId.Water, unit * Main.rand.NextFloat(1, 3), 0, default, Main.rand.NextFloat(0.5f, 1f));
                                    }
                                    if (npc.CanBeChasedBy() || npc.type == NPCID.TargetDummy)
                                    {
                                        Main.player[projectile.owner].ApplyDamageToNPC(npc, damage, 0, projectile.direction);
                                        Main.player[Projectile.owner].GetModPlayer<ElementSkillPlayer>().ElementChargeValue[1] += damage / ElementSkillPlayer.Devider;
                                    }
                                    npc.AddBuff(ModContent.BuffType<TidalErosion>(), 600);
                                    //OnHitNPC(npc, damage, 0, false);
                                }
                            }
                        }
                        for (int n = 0; n < 30; n++)
                        {
                            var unit = (n / 30f * MathHelper.TwoPi).ToRotationVector2();
                            Dust.NewDustPerfect(projectile.Center + unit * 96, MyDustId.Water, unit * Main.rand.NextFloat(2, 8), 0, default, Main.rand.NextFloat(1f, 1.5f));
                        }
                    }
                    break;
                }
        }
    }
}

public class WaterUltra : ModBuff
{
}