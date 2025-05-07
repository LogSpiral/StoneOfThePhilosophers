using LogSpiralLibrary.CodeLibrary.Utilties.Extensions;
using LogSpiralLibrary.CodeLibrary.Utilties;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace StoneOfThePhilosophers.Contents.Sun;
public class SunAttack : ModProjectile
{
    Projectile projectile => Projectile;
    /// <summary>
    /// 0为火球 1大爆炸 2小爆炸 3大太阳
    /// </summary>
    int style => (int)projectile.ai[0];
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
    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();
    }
    public override bool? CanCutTiles()
    {
        return style != 3;
    }
    public override bool PreDraw(ref Color lightColor)
    {
        switch (style)
        {
            case 0:
                {
                    float alpha = (Projectile.timeLeft / 180f).SmoothSymmetricFactor(1 / 12f);
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    #region Shader
                    float r = Main.rand.NextFloat();
                    Main.instance.GraphicsDevice.Textures[1] = ModAsset.Style_6.Value;
                    Main.instance.GraphicsDevice.Textures[2] = ModAsset.HeatMap_4.Value;
                    Main.instance.GraphicsDevice.SamplerStates[1] = SamplerState.AnisotropicWrap;
                    Main.instance.GraphicsDevice.SamplerStates[2] = SamplerState.AnisotropicClamp;
                    StoneOfThePhilosophers.HeatMap.Parameters["uTime"].SetValue(projectile.velocity.SafeNormalize(default).RotatedBy(MathHelper.PiOver2) * Main.GlobalTimeWrappedHourly);
                    StoneOfThePhilosophers.HeatMap.Parameters["uTransform"].SetValue(Matrix.Identity);
                    StoneOfThePhilosophers.HeatMap.CurrentTechnique.Passes[0].Apply();

                    #endregion
                    Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition, null, Color.White * alpha, r + MathHelper.PiOver2, new Vector2(16), new Vector2(8f) * (MathF.Cos(Main.GlobalTimeWrappedHourly * MathHelper.Pi) * .125f + 1f), 0, 0);//

                    Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition, null, Color.White * alpha, r, new Vector2(16), new Vector2(8f) * (MathF.Cos(Main.GlobalTimeWrappedHourly * MathHelper.Pi) * .125f + 1f), 0, 0);//
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
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                    break;
                }
            case 1:
                {
                    var fac = 1 - projectile.timeLeft / 21f;
                    var fac1 = fac.HillFactor2(1);

                    for (int n = 0; n < 3; n++)
                        Main.EntitySpriteDraw(ModAsset.ExplosionEffect.Value, projectile.Center - Main.screenPosition + Main.rand.NextVector2Unit() * 4, new Rectangle(0, 588 - projectile.timeLeft / 3 * 98, 98, 98), new Color(255, 255, 255, 0) * fac1 * .75f, projectile.rotation, new Vector2(49), 6f * fac, 0, 0);//new Rectangle(0, projectile.timeLeft / 2, 52, 52)
                    Main.EntitySpriteDraw(ModAsset.FlameOfNuclear.Value, projectile.Center - Main.screenPosition, null, Color.Lerp(Color.Orange, Color.Yellow, fac1) with { A = 0 } * fac1, projectile.rotation, new Vector2(128), 3f * fac, 0, 0);
                    Main.EntitySpriteDraw(ModAsset.FlameOfNuclear.Value, projectile.Center - Main.screenPosition, null, Color.Lerp(Color.Yellow, Color.White, fac1) * fac1, projectile.rotation, new Vector2(128), 2f * fac, 0, 0);

                    break;
                }
            case 2:
                {
                    float alpha = (Projectile.timeLeft / 180f).SmoothSymmetricFactor(1 / 12f);

                    var vertexs = projectile.TailVertexFromProj(default, t => 16, t => Color.White * MathF.Pow(t, 2f).WaterDropFactor() * 2 * alpha, .5f);//MathF.Pow(t, 1.5f).WaterDropFactor() * 
                    DrawingMethods.VertexDraw(vertexs,
                        ModAsset.Style_4.Value,
                        ModAsset.Style_8.Value,
                        ModAsset.HeatMap_4.Value,
                        new Vector2(-Main.GlobalTimeWrappedHourly * 2, 0), false, null,
                        "HeatMap", true, true);//default, 30, .5f, true, Color.Yellow
                    DrawingMethods.VertexDraw(vertexs,
                        ModAsset.Style_4.Value,
                        ModAsset.Style_8.Value,
                        ModAsset.HeatMap_4.Value,
                        new Vector2(-Main.GlobalTimeWrappedHourly * 2, 0), false, null,
                        "HeatMap", true, true);//default, 30, .5f, true, Color.Yellow
                    var spriteBatch = Main.spriteBatch;
                    var starLight = ModAsset.StarLight.Value;
                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    spriteBatch.Draw(starLight, projectile.Center - projectile.velocity * 2 - Main.screenPosition, null, Color.Orange * alpha, projectile.rotation + MathHelper.PiOver2, new Vector2(36), new Vector2(1, 5) * .5f, 0, 0);
                    spriteBatch.Draw(starLight, projectile.Center - projectile.velocity * 2 - Main.screenPosition, null, Color.Orange * .5f * alpha, projectile.rotation, new Vector2(36), new Vector2(1, 2) * .5f, 0, 0);
                    spriteBatch.Draw(starLight, projectile.Center - projectile.velocity * 2 - Main.screenPosition, null, Color.White * alpha, projectile.rotation + MathHelper.PiOver2, new Vector2(36), new Vector2(1, 5) * .25f, 0, 0);
                    spriteBatch.Draw(starLight, projectile.Center - projectile.velocity * 2 - Main.screenPosition, null, Color.White * .5f * alpha, projectile.rotation, new Vector2(36), new Vector2(1, 2) * .25f, 0, 0);
                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    break;
                }
            case 3:
                {
                    float alpha = (Projectile.timeLeft / 1200f).SmoothSymmetricFactor(1 / 12f);
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    #region Shader
                    float r = Main.rand.NextFloat();
                    Main.instance.GraphicsDevice.Textures[1] = ModAsset.Style_6.Value;
                    Main.instance.GraphicsDevice.Textures[2] = ModAsset.HeatMap_4.Value;
                    Main.instance.GraphicsDevice.SamplerStates[1] = SamplerState.AnisotropicWrap;
                    Main.instance.GraphicsDevice.SamplerStates[2] = SamplerState.AnisotropicClamp;
                    StoneOfThePhilosophers.HeatMap.Parameters["uTime"].SetValue(projectile.velocity.SafeNormalize(default).RotatedBy(MathHelper.PiOver2) * Main.GlobalTimeWrappedHourly);
                    StoneOfThePhilosophers.HeatMap.Parameters["uTransform"].SetValue(Matrix.Identity);
                    StoneOfThePhilosophers.HeatMap.CurrentTechnique.Passes[0].Apply();

                    #endregion
                    Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition, null, Color.White * alpha, r + MathHelper.PiOver2, new Vector2(16), new Vector2(16f) * (MathF.Cos(Main.GlobalTimeWrappedHourly * MathHelper.Pi) * .125f + 1f), 0, 0);//

                    Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition, null, Color.White * alpha, r, new Vector2(16), new Vector2(16f) * (MathF.Cos(Main.GlobalTimeWrappedHourly * MathHelper.Pi) * .125f + 1f), 0, 0);//
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    break;
                }
        }

        return false;
    }
    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        return base.OnTileCollide(oldVelocity);
    }
    public override void OnKill(int timeLeft)
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
                        proj.width = proj.height = 640;
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
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.AddBuff(BuffID.Daybreak, 120);
        //target.immune[Projectile.owner] = 0;
        if (style == 0)
        {
            if (Projectile.active)
            {
                var rand = Main.rand.NextFloat();
                for (int n = 0; n < 8; n++)
                {
                    var unit = (n / 8f * MathHelper.TwoPi + rand).ToRotationVector2();
                    var proj = Projectile.NewProjectileDirect(projectile.GetProjectileSource_FromThis(), projectile.Center + unit * 192, unit * 16, Type, Projectile.damage / 4, projectile.knockBack * .5f, projectile.owner, 2);
                    proj.penetrate = 3;

                }
            }

            Projectile.Kill();
        }
        else
        {
            target.immune[projectile.owner] = 0;
            projectile.frameCounter++;
            if (style == 2)
            {
                if (projectile.penetrate == 1)
                {
                    projectile.timeLeft = 15;
                    projectile.penetrate = 2;
                    projectile.friendly = false;
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
                    projectile.velocity *= .96f;
                    if (projectile.timeLeft % 15 == 0)
                    {
                        foreach (var npc in Main.npc)
                        {
                            if (npc.active && !npc.friendly)
                            {
                                var distance = Vector2.Distance(npc.Center, projectile.Center);
                                if (distance < 432)
                                {
                                    var fac = Utils.GetLerpValue(432, 0, distance);
                                    var damage = (int)(MathF.Pow(fac, 0.5f) * projectile.damage / 4);
                                    int count = (int)(fac * 30);
                                    for (int n = 0; n < count; n++)
                                    {
                                        var unit = (n * MathHelper.TwoPi / count).ToRotationVector2();
                                        Dust.NewDustPerfect(npc.Center, MyDustId.Fire, unit * Main.rand.NextFloat(1, 3), 0, default, Main.rand.NextFloat(0.5f, 1f));
                                    }
                                    if (npc.CanBeChasedBy() || npc.type == NPCID.TargetDummy)
                                    {
                                        Main.player[projectile.owner].ApplyDamageToNPC(npc, damage, 0, projectile.direction, false);
                                        Main.player[Projectile.owner].GetModPlayer<ElementSkillPlayer>().ElementChargeValue[6] += damage / ElementSkillPlayer.Devider;
                                    }
                                    npc.AddBuff(BuffID.Daybreak, 30);
                                    //OnHitNPC(npc, damage, 0, false);
                                }
                            }
                        }
                        for (int n = 0; n < 30; n++)
                        {
                            var unit = (n / 30f * MathHelper.TwoPi).ToRotationVector2();
                            Dust.NewDustPerfect(projectile.Center + unit * 96, MyDustId.Fire, unit * Main.rand.NextFloat(2, 8), 0, default, Main.rand.NextFloat(1f, 1.5f));
                        }
                    }
                    if (projectile.timeLeft <= 120)
                    {
                        if (projectile.timeLeft % 15 == 0 && projectile.timeLeft != 0)
                        {
                            var unit = (projectile.timeLeft / 120f * MathHelper.TwoPi).ToRotationVector2();
                            var proj = Projectile.NewProjectileDirect(projectile.GetProjectileSource_FromThis(), projectile.Center + unit * 96, unit * 32, Type, Projectile.damage / 2, projectile.knockBack * .5f, projectile.owner, 2);
                            proj.penetrate = 3;
                        }
                    }
                    break;
                }
            case 1:
                {
                    projectile.friendly = projectile.frameCounter == 0;
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
                    if (projectile.timeLeft > 165)
                    {
                        projectile.velocity = projectile.velocity.RotatedBy(Utils.GetLerpValue(165, 180, projectile.timeLeft, true) * MathHelper.Pi / 6);
                    }
                    else if (projectile.timeLeft < 150 && projectile.timeLeft > 15)
                    {
                        NPC target = null;
                        float MaxDistance = 1024f;
                        foreach (var npc in Main.npc)
                        {
                            if (!npc.friendly && npc.active && npc.CanBeChasedBy())
                            {
                                float distance = Vector2.Distance(npc.Center, projectile.Center);
                                if (distance < MaxDistance)
                                {
                                    target = npc;
                                    MaxDistance = distance;
                                }
                            }
                        }
                        if (target != null)
                        {
                            Vector2 targetVec = (target.Center - projectile.Center).SafeNormalize(default) * 32;
                            projectile.velocity = Vector2.Lerp(projectile.velocity, targetVec, MathF.Pow(Utils.GetLerpValue(1024, 32, MaxDistance, true), 0.5f) * 0.2f);
                            projectile.timeLeft = 30;
                        }
                    }
                    else if (projectile.timeLeft <= 15)
                    {
                        projectile.friendly = false;
                    }
                    projectile.rotation = projectile.velocity.ToRotation();
                    break;
                }
            case 3:
                {
                    projectile.Center = Main.player[projectile.owner].Center - new Vector2(0, 256);
                    for (int n = 9; n > 0; n--)
                    {
                        Projectile.oldPos[n] = Projectile.oldPos[n - 1];
                        Projectile.oldRot[n] = Projectile.oldRot[n - 1];
                    }
                    float alpha = (Projectile.timeLeft / 1200f).SmoothSymmetricFactor(1 / 12f);

                    for (int n = 0; n < 16; n++)
                        Lighting.AddLight(projectile.Center + (MathHelper.TwoPi / 16 * n).ToRotationVector2() * 320 * alpha, new Vector3(1, 0.95f, 0.85f) * 2 * alpha);
                    Projectile.oldPos[0] = Projectile.Center;
                    Projectile.oldRot[0] = Projectile.rotation;
                    if (projectile.timeLeft % 15 == 0)
                    {
                        foreach (var npc in Main.npc)
                        {
                            if (npc.active && !npc.friendly)
                            {
                                var distance = Vector2.Distance(npc.Center, projectile.Center);
                                if (distance < 432)
                                {
                                    var fac = Utils.GetLerpValue(432, 0, distance);
                                    var damage = (int)(MathF.Pow(fac, 0.5f) * projectile.damage / 4);
                                    int count = (int)(fac * 30);
                                    for (int n = 0; n < count; n++)
                                    {
                                        var unit = (n * MathHelper.TwoPi / count).ToRotationVector2();
                                        Dust.NewDustPerfect(npc.Center, MyDustId.Fire, unit * Main.rand.NextFloat(1, 3), 0, default, Main.rand.NextFloat(0.5f, 1f));
                                    }
                                    if (npc.CanBeChasedBy() || npc.type == NPCID.TargetDummy)
                                    {
                                        Main.player[projectile.owner].ApplyDamageToNPC(npc, damage, 0, projectile.direction, false);
                                        Main.player[Projectile.owner].GetModPlayer<ElementSkillPlayer>().ElementChargeValue[6] += damage / ElementSkillPlayer.Devider;
                                    }
                                    npc.AddBuff(BuffID.Daybreak, 30);
                                    //OnHitNPC(npc, damage, 0, false);
                                }
                            }
                        }

                    }
                    if (projectile.timeLeft % 30 == 0)
                    {
                        SoundEngine.PlaySound(SoundID.Item74);

                        if (projectile.timeLeft % 180 == 0)
                        {
                            var rand = Main.rand.NextFloat();
                            for (int n = 0; n < 8; n++)
                            {
                                var unit = (n / 8f * MathHelper.TwoPi + rand).ToRotationVector2();
                                var proj = Projectile.NewProjectileDirect(projectile.GetProjectileSource_FromThis(), projectile.Center + unit * 192, unit * 24, Type, Projectile.damage / 2, projectile.knockBack * .5f, projectile.owner, 2);
                                proj.penetrate = 3;

                            }

                        }
                        else
                        {
                            var _unit = (projectile.timeLeft / 120f * MathHelper.TwoPi).ToRotationVector2();
                            NPC target = null;
                            float MaxDistance = 1024;
                            foreach (var npc in Main.npc)
                            {
                                if (npc.CanBeChasedBy() && npc.active && !npc.friendly)
                                {
                                    float distance = Vector2.Distance(npc.Center, projectile.Center);
                                    if (distance < MaxDistance)
                                    {
                                        MaxDistance = distance;
                                        target = npc;
                                    }
                                }
                            }
                            if (target != null)
                            {
                                _unit = (target.Center - projectile.Center).SafeNormalize(default).RotatedBy(MathHelper.Pi * 2 / 3);
                            }
                            var proj = Projectile.NewProjectileDirect(projectile.GetProjectileSource_FromThis(), projectile.Center + _unit * 96, _unit * 32, Type, Projectile.damage / 2, projectile.knockBack * .5f, projectile.owner, 2);
                            proj.penetrate = 3;
                        }

                    }
                    break;
                }
        }
        base.AI();
    }
}
