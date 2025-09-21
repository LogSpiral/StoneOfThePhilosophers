using LogSpiralLibrary.CodeLibrary.Utilties;
using LogSpiralLibrary.CodeLibrary.Utilties.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Utilities;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace StoneOfThePhilosophers.Contents.Earth;

public class EarthAttack : ModProjectile
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Projectile.width = 24;
        Projectile.height = 24;
        Projectile.aiStyle = -1;
        Projectile.friendly = true;
        Projectile.DamageType = DamageClass.Magic;
        Projectile.tileCollide = true;
        Projectile.penetrate = -1;
        Projectile.timeLeft = 60;
        Projectile.ignoreWater = true;
    }

    public override bool PreDraw(ref Color lightColor)
    {
        switch ((int)Projectile.ai[1])
        {
            case 0:
                {
                    float alpha = (Projectile.timeLeft / 60f).SmoothSymmetricFactor(1 / 12f);
                    for (int n = 9; n > -1; n--)
                        Main.EntitySpriteDraw(ModContent.Request<Texture2D>(Texture + "Piece").Value, Projectile.oldPos[n] - Main.screenPosition,
                            new Rectangle((int)(16 * Projectile.frame), 0, 16, 16), lightColor * ((10 - n) * .1f) * alpha * (n == 0 ? 1 : .25f), Projectile.oldRot[n], new Vector2(8), 3f * ((10 - n) * .1f), 0);
                    break;
                }
            case 1:
                {
                    break;
                }
            case 2:
                {
                    float num287 = 300f;
                    float num288 = Projectile.ai[0];
                    float num289 = MathHelper.Clamp(num288 / 30f, 0f, 1f);
                    if (num288 > num287 - 60f)
                        num289 = MathHelper.Lerp(1f, 0f, (num288 - (num287 - 60f)) / 60f);

                    float num290 = 0.2f;
                    Vector2 top2 = Projectile.Top;
                    Vector2 bottom = Projectile.Bottom;
                    Vector2.Lerp(top2, bottom, 0.5f);
                    Vector2 vector44 = new(0f, bottom.Y - top2.Y);
                    vector44.X = vector44.Y * num290;
                    new Vector2(top2.X - vector44.X / 2f, top2.Y);
                    Texture2D value108 = TextureAssets.Projectile[Projectile.type].Value;
                    Rectangle rectangle19 = value108.Frame();
                    Vector2 origin19 = rectangle19.Size() / 2f;
                    float num291 = -(float)Math.PI / 20f * num288 * (float)((!(Projectile.velocity.X > 0f)) ? 1 : (-1));
                    SpriteEffects effects3 = (Projectile.velocity.X > 0f) ? SpriteEffects.FlipVertically : SpriteEffects.None;
                    bool flag31 = Projectile.velocity.X > 0f;
                    Vector2 spinningpoint5 = Vector2.UnitY.RotatedBy(num288 * 0.14f);
                    float num292 = 0f;
                    float num293 = 5.01f + num288 / 150f * -0.9f;
                    if (num293 < 4.11f)
                        num293 = 4.11f;

                    Color value109 = new(160, 140, 100, 127);
                    Color color69 = new(140, 160, 255, 127);
                    float num294 = num288 % 60f;
                    if (num294 < 30f)
                        color69 *= Utils.GetLerpValue(22f, 30f, num294, clamped: true);
                    else
                        color69 *= Utils.GetLerpValue(38f, 30f, num294, clamped: true);

                    bool flag32 = color69 != Color.Transparent;
                    for (float num295 = (int)bottom.Y; num295 > (float)(int)top2.Y; num295 -= num293)
                    {
                        num292 += num293;
                        float num296 = num292 / vector44.Y;
                        float num297 = num292 * ((float)Math.PI * 2f) / -20f;
                        if (flag31)
                            num297 *= -1f;

                        float num298 = num296 - 0.35f;
                        Vector2 position19 = spinningpoint5.RotatedBy(num297);
                        Vector2 value110 = new(0f, num296 + 1f);
                        value110.X = value110.Y * num290;
                        Color color70 = Color.Lerp(Color.Transparent, value109, num296 * 2f);
                        if (num296 > 0.5f)
                            color70 = Color.Lerp(Color.Transparent, value109, 2f - num296 * 2f);

                        color70.A = (byte)((float)(int)color70.A * 0.5f);
                        color70 *= num289;
                        position19 *= value110 * 100f;
                        position19.Y = 0f;
                        position19.X = 0f;
                        position19 += new Vector2(bottom.X, num295) - Main.screenPosition;
                        if (flag32)
                        {
                            Color color71 = Color.Lerp(Color.Transparent, color69, num296 * 2f);
                            if (num296 > 0.5f)
                                color71 = Color.Lerp(Color.Transparent, color69, 2f - num296 * 2f);

                            color71.A = (byte)((float)(int)color71.A * 0.5f);
                            color71 *= num289;
                            Main.EntitySpriteDraw(value108, position19, rectangle19, color71, num291 + num297, origin19, (1f + num298) * 0.8f, effects3);
                        }

                        Main.EntitySpriteDraw(value108, position19, rectangle19, color70, num291 + num297, origin19, 1f + num298, effects3);
                    }
                    break;
                }
        }
        return false;
    }

    public void SetDefaultStorm()
    {
        Projectile.width = 60;
        Projectile.height = 210;
        Projectile.aiStyle = -1;
        Projectile.friendly = true;
        Projectile.DamageType = DamageClass.Magic;
        Projectile.tileCollide = false;
        Projectile.penetrate = -1;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.timeLeft = 60;
        Projectile.localNPCHitCooldown = -1;
        Projectile.ignoreWater = true;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        //target.immune[Projectile.owner] = 3;
        if (!target.friendly && target.active && target.CanBeChasedBy())
        {
            Main.player[Projectile.owner].GetModPlayer<ElementSkillPlayer>().ElementChargeValue[4] += damageDone / ElementSkillPlayer.Devider;
        }
        base.OnHitNPC(target, hit, damageDone);
    }

    public override void AI()
    {
        switch ((int)Projectile.ai[1])
        {
            case 0:
                {
                    Projectile.velocity += new Vector2(0, 0.2f);
                    Projectile.rotation++;
                    for (int n = 9; n > 0; n--)
                    {
                        Projectile.oldPos[n] = Projectile.oldPos[n - 1];
                        Projectile.oldRot[n] = Projectile.oldRot[n - 1];
                    }
                    Projectile.oldPos[0] = Projectile.Center;
                    Projectile.oldRot[0] = Projectile.rotation;
                    //Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Unit() * 4, MyDustId.BrownDirt, Main.rand.NextVector2Unit() + Projectile.velocity * .25f, 0, Color.White, Main.rand.NextFloat(1.5f, 3f));
                    if (Projectile.timeLeft % 2 == 0 && Main.rand.NextBool(3))
                    {
                        var unit = Projectile.velocity.RotatedByRandom(0.1f);
                        Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), Projectile.Center, unit, Type, Projectile.damage * 2 / 3, Projectile.knockBack, Projectile.owner, 0, 1).tileCollide = false;
                        for (int n = 0; n < 5; n++)
                        {
                            var dust = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Unit() * 4, MyDustId.BrownDirt, unit * .125f + Main.rand.NextVector2Unit() * 4, 0, Color.White with { A = (byte)Main.rand.Next(0, 255) } * Main.rand.NextFloat(0, 1), Main.rand.NextFloat(1.5f, 3f) * .5f);
                            dust.noGravity = true;
                            dust.fadeIn = -1f;
                        }
                    }
                    break;
                }
            case 1:
                {
                    Projectile.velocity *= 0.85f;
                    break;
                }
            case 2:
                {
                    float num = 300f;
                    if (Projectile.soundDelay == 0)
                    {
                        Projectile.soundDelay = -1;
                        Projectile.localAI[1] = SoundEngine.PlayTrackedSound(SoundID.DD2_BookStaffTwisterLoop, Projectile.Center).ToFloat();
                    }

                    SoundEngine.TryGetActiveSound(SlotId.FromFloat(Projectile.localAI[1]), out ActiveSound activeSound);
                    if (activeSound != null)
                    {
                        activeSound.Position = Projectile.Center;
                        activeSound.Volume = 1f - Math.Max(Projectile.ai[0] - (num - 15f), 0f) / 15f;
                    }
                    else
                    {
                        Projectile.localAI[1] = SlotId.Invalid.ToFloat();
                    }

                    if (Projectile.localAI[0] >= 16f && Projectile.ai[0] < num - 15f)
                        Projectile.ai[0] = num - 15f;

                    Projectile.ai[0] += 1f;
                    if (Projectile.ai[0] >= num)
                        Projectile.Kill();

                    Vector2 top = Projectile.Top;
                    Vector2 bottom = Projectile.Bottom;
                    Vector2 value = Vector2.Lerp(top, bottom, 0.5f);
                    Vector2 value2 = new(0f, bottom.Y - top.Y);
                    value2.X = value2.Y * 0.2f;

                    if (Projectile.ai[0] < num - 30f)
                    {
                        for (int j = 0; j < 1; j++)
                        {
                            float value3 = -1f;
                            float value4 = 0.9f;
                            float amount = Main.rand.NextFloat();
                            Vector2 value5 = new(MathHelper.Lerp(0.1f, 1f, Main.rand.NextFloat()), MathHelper.Lerp(value3, value4, amount));
                            value5.X *= MathHelper.Lerp(2.2f, 0.6f, amount);
                            value5.X *= -1f;
                            Vector2 value6 = new(6f, 10f);
                            Vector2 position2 = value + value2 * value5 * 0.5f + value6;
                            Dust dust = Main.dust[Dust.NewDust(position2, 0, 0, DustID.ApprenticeStorm)];
                            dust.position = position2;
                            dust.fadeIn = 1.3f;
                            dust.scale = 0.87f;
                            dust.alpha = 211;
                            if (value5.X > -1.2f)
                                dust.velocity.X = 1f + Main.rand.NextFloat();

                            dust.noGravity = true;
                            dust.velocity.Y = Main.rand.NextFloat() * -0.5f - 1.3f;
                            dust.velocity.X += Projectile.velocity.X * 2.1f;
                            dust.noLight = true;
                        }
                    }

                    Vector2 position3 = Projectile.Bottom + new Vector2(-25f, -25f);
                    for (int k = 0; k < 4; k++)
                    {
                        Dust dust2 = Dust.NewDustDirect(position3, 50, 25, DustID.Smoke, Projectile.velocity.X, -2f, 100);
                        dust2.fadeIn = 1.1f;
                        dust2.noGravity = true;
                    }

                    if (Main.rand.NextBool(3))
                    {
                        var unit = Projectile.velocity.RotatedByRandom(0.1f);
                        var proj = Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), Projectile.Center, unit, Type, Projectile.damage * 2 / 3, Projectile.knockBack, Projectile.owner, 0, 1);
                        proj.tileCollide = false;
                        proj.timeLeft = 180;
                        for (int n = 0; n < 5; n++)
                        {
                            var dust = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Unit() * 4, MyDustId.BrownDirt, unit * .125f + Main.rand.NextVector2Unit() * 8, 0, Color.White with { A = (byte)Main.rand.Next(0, 255) } * Main.rand.NextFloat(0, 1), Main.rand.NextFloat(1.5f, 3f) * .5f);
                            dust.noGravity = true;
                            dust.fadeIn = -2f;
                        }
                    }

                    //for (int l = 0; l < 1; l++)
                    //{
                    //    if (Main.rand.NextBool(5))
                    //    {
                    //        Gore gore = Gore.NewGoreDirect(Projectile.TopLeft + Main.rand.NextVector2Square(0f, 1f) * Projectile.Size, new Vector2(Projectile.velocity.X * 1.5f, (0f - Main.rand.NextFloat()) * 16f), Utils.SelectRandom(Main.rand, 1007, 1008, 1008));
                    //        gore.timeLeft = 60;
                    //        gore.alpha = 50;
                    //        gore.velocity.X += Projectile.velocity.X;
                    //    }
                    //}

                    //for (int m = 0; m < 1; m++)
                    //{
                    //    if (Main.rand.NextBool(7))
                    //    {
                    //        Gore gore2 = Gore.NewGoreDirect(Projectile.TopLeft + Main.rand.NextVector2Square(0f, 1f) * Projectile.Size, new Vector2(Projectile.velocity.X * 1.5f, (0f - Main.rand.NextFloat()) * 16f), Utils.SelectRandom(Main.rand, 1007, 1008, 1008));
                    //        gore2.timeLeft = 0;
                    //        gore2.alpha = 80;
                    //    }
                    //}

                    //for (int n = 0; n < 1; n++)
                    //{
                    //    if (Main.rand.NextBool(7))
                    //    {
                    //        Gore gore3 = Gore.NewGoreDirect(Projectile.TopLeft + Main.rand.NextVector2Square(0f, 1f) * Projectile.Size, new Vector2(Projectile.velocity.X * 1.5f, (0f - Main.rand.NextFloat()) * 16f), Utils.SelectRandom(Main.rand, 1007, 1008, 1008));
                    //        gore3.timeLeft = 0;
                    //        gore3.alpha = 80;
                    //    }
                    //}
                    break;
                }
        }
    }
}

public class EarthSpecialAttack : ModProjectile
{
    public override string Texture => base.Texture.Replace("EarthSpecialAttack", "EarthAttack");
    public bool EarthQuake => Projectile.ai[1] != 0;

    public override bool PreDraw(ref Color lightColor)
    {
        if (!EarthQuake)
        {
            var cen = Projectile.Center - Main.screenPosition;
            var factor1 = ((float)Projectile.timeLeft).SymmetricalFactor(300, 30);
            //factor1 = MathHelper.SmoothStep(0, 1, factor1);
            factor1 *= factor1;
            var factor2 = ((float)Projectile.timeLeft - 15).SymmetricalFactor(285, 30);
            factor2 *= factor2;
            //factor2 = MathHelper.SmoothStep(0, 1, factor2);
            if (ElementSkillPlayer.EarthQuaking)
            {
                float fac3 = Projectile.timeLeft;
                fac3 %= 30;
                fac3 /= 30f;
                fac3 = MathHelper.SmoothStep(0, 1, fac3);
                var color = lightColor with { A = 127 } * fac3;
                var scaler = 1.5f - fac3 * .5f;
                Main.EntitySpriteDraw(ModAsset.crystal_reflection.Value, cen, null, color, 0, new Vector2(40, 96), new Vector2(1, 2) * factor1 * new Vector2(factor1, 1) * scaler, 0);
                Main.EntitySpriteDraw(ModAsset.big_crystal_a.Value, cen - new Vector2(16, 0), null, color, -MathHelper.Pi / 12, new Vector2(20, 64), new Vector2(1.5f, 2) * factor2 * new Vector2(factor2, 1) * scaler, 0);
                Main.EntitySpriteDraw(ModAsset.big_crystal_b.Value, cen + new Vector2(16, 0), null, color, MathHelper.Pi / 12, new Vector2(20, 64), new Vector2(1.5f, 2) * factor2 * new Vector2(factor2, 1) * scaler, 0);
            }
            Main.EntitySpriteDraw(ModAsset.crystal_reflection.Value, cen, null, lightColor, 0, new Vector2(40, 96), new Vector2(1, 2) * factor1 * new Vector2(factor1, 1), 0);
            Main.EntitySpriteDraw(ModAsset.big_crystal_a.Value, cen - new Vector2(16, 0), null, lightColor, -MathHelper.Pi / 12, new Vector2(20, 64), new Vector2(1.5f, 2) * factor2 * new Vector2(factor2, 1), 0);
            Main.EntitySpriteDraw(ModAsset.big_crystal_b.Value, cen + new Vector2(16, 0), null, lightColor, MathHelper.Pi / 12, new Vector2(20, 64), new Vector2(1.5f, 2) * factor2 * new Vector2(factor2, 1), 0);
        }
        return false;
    }

    public override void AI()
    {
        if (Projectile.ai[1] == 1) ElementSkillPlayer.EarthQuaking = true;
        if (EarthQuake)
        {
            if (Projectile.timeLeft % 20 == 0)
                Main.player[Projectile.owner].GetModPlayer<ElementSkillPlayer>().strengthOfShake += 1f;

            if (Projectile.timeLeft % 30 == 0)
            {
                foreach (var npc in Main.npc)
                {
                    if (npc.CanBeChasedBy() && !npc.friendly && npc.active && Math.Abs(npc.velocity.Y) < 0.01f)
                    {
                        bool flag = false;
                        int k = -1;
                        while (k < 5)
                        {
                            var tile = Framing.GetTileSafely(npc.Bottom.ToTileCoordinates() + new Point(0, k));
                            if (tile.HasTile && (Main.tileSolid[tile.TileType] || Main.tileSolidTop[tile.TileType]))
                            {
                                flag = true;
                                break;
                            }
                            k++;
                        }

                        if (flag)
                        {
                            var fac = 1;
                            var damage = (int)(Projectile.damage * Main.rand.NextFloat(0.85f, 1.15f));
                            int count = (int)(fac * 30);
                            for (int n = 0; n < count; n++)
                            {
                                var unit = (n * MathHelper.TwoPi / count).ToRotationVector2();
                                Dust.NewDustPerfect(npc.Bottom, MyDustId.BrownDirt, unit * Main.rand.NextFloat(1, 3), 0, default, Main.rand.NextFloat(0.5f, 1f));
                            }
                            Main.player[Projectile.owner].ApplyDamageToNPC(npc, damage, 0, Projectile.direction);
                            Main.player[Projectile.owner].GetModPlayer<ElementSkillPlayer>().ElementChargeValue[4] += damage / ElementSkillPlayer.Devider;
                        }
                    }
                }
            }
        }
        else
        {
            if (Projectile.timeLeft < 570 && Projectile.timeLeft > 30)
            {
                Rectangle rectangle = Utils.CenteredRectangle(Projectile.Center + new Vector2(0, -80), new Vector2(64, 160));
                foreach (var npc in Main.npc)
                {
                    if (npc.active && !npc.friendly && npc.CanBeChasedBy() && !npc.boss && Vector2.Distance(npc.Center, Projectile.Center) < 200)
                    {
                        npc.velocity = MiscMethods.RectangleCollision(npc.position, npc.velocity, npc.width, npc.height, rectangle);
                    }
                }
                if (ElementSkillPlayer.EarthQuaking && Projectile.timeLeft % 30 == 0)
                {
                    foreach (var npc in Main.npc)
                    {
                        if (npc.CanBeChasedBy() && !npc.friendly && npc.active && Vector2.Distance(npc.Center, Projectile.Center) < 240 && Math.Abs(npc.velocity.Y) < 0.01f)
                        {
                            bool flag = false;
                            int k = -1;
                            while (k < 5)
                            {
                                var tile = Framing.GetTileSafely(npc.Bottom.ToTileCoordinates() + new Point(0, k));
                                if (tile.HasTile && (Main.tileSolid[tile.TileType] || Main.tileSolidTop[tile.TileType]))
                                {
                                    flag = true;
                                    break;
                                }
                                k++;
                            }

                            if (flag)
                            {
                                var fac = 1;
                                var damage = (int)(Projectile.damage * .5f * Main.rand.NextFloat(0.85f, 1.15f));
                                int count = (int)(fac * 15);
                                for (int n = 0; n < count; n++)
                                {
                                    var unit = (n * MathHelper.TwoPi / count).ToRotationVector2();
                                    Dust.NewDustPerfect(npc.Bottom, MyDustId.BrownDirt, unit * Main.rand.NextFloat(1, 3), 0, default, Main.rand.NextFloat(0.5f, 1f));
                                }
                                Main.player[Projectile.owner].ApplyDamageToNPC(npc, damage, 0, Projectile.direction);
                                Main.player[Projectile.owner].GetModPlayer<ElementSkillPlayer>().ElementChargeValue[4] += damage / ElementSkillPlayer.Devider;
                            }
                        }
                    }
                }
            }
        }
        base.AI();
    }

    public override void SetDefaults()
    {
        Projectile.timeLeft = 600;
        Projectile.width = 1;
        Projectile.height = 1;
        Projectile.aiStyle = -1;
        Projectile.friendly = true;
        Projectile.DamageType = DamageClass.Magic;
        Projectile.tileCollide = true;
        Projectile.penetrate = -1;
        Projectile.ignoreWater = true;
        base.SetDefaults();
    }
}