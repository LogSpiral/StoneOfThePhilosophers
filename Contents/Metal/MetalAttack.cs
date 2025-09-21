using LogSpiralLibrary.CodeLibrary.Utilties.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace StoneOfThePhilosophers.Contents.Metal;

public class MetalAttack : ModProjectile
{
    public bool Extra => Projectile.ai[1] == 1;
    public int TargetIndex = -1;

    public override void SetDefaults()
    {
        Projectile.width = Projectile.height = 32;
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
        if (Projectile.ai[1] == 2)
        {
            float alpha = (Projectile.timeLeft / 120f).SmoothSymmetricFactor(1 / 12f);
            float size = Utils.GetLerpValue(120, 80, Projectile.timeLeft, true);
            for (int n = TargetIndex == -1 ? 9 : 0; n > -1; n--)
                Main.EntitySpriteDraw(ModContent.Request<Texture2D>(Texture + "EX").Value, Projectile.oldPos[n] - Main.screenPosition, new Rectangle((int)(16 * Projectile.ai[0]), 0, 16, 16), lightColor * ((10 - n) * .1f) * alpha * (n == 0 ? 1 : .25f), Projectile.oldRot[n], new Vector2(8), 12f * ((10 - n) * .1f) * size, 0);
            for (int n = TargetIndex == -1 ? 9 : 0; n > -1; n--)
                Main.EntitySpriteDraw(ModContent.Request<Texture2D>(GlowTexture + "EX").Value, Projectile.oldPos[n] - Main.screenPosition, new Rectangle((int)(16 * Projectile.ai[0]), 0, 16, 16), Color.White with { A = 127 } * ((10 - n) * .1f) * alpha * (n == 0 ? 1 : .25f), Projectile.oldRot[n], new Vector2(8), 12f * ((10 - n) * .1f) * size, 0);
        }
        else
        {
            float alpha = (Projectile.timeLeft / 180f).SmoothSymmetricFactor(1 / 12f);
            for (int n = TargetIndex == -1 ? 9 : 0; n > -1; n--)
                Main.EntitySpriteDraw(Extra ? ModContent.Request<Texture2D>(Texture + "EX").Value : TextureAssets.Projectile[Type].Value, Projectile.oldPos[n] - Main.screenPosition, new Rectangle((int)(16 * Projectile.ai[0]), 0, 16, 16), lightColor * ((10 - n) * .1f) * alpha * (n == 0 ? 1 : .25f), Projectile.oldRot[n], new Vector2(8), 3f * ((10 - n) * .1f), 0);
            for (int n = TargetIndex == -1 ? 9 : 0; n > -1; n--)
                Main.EntitySpriteDraw(ModContent.Request<Texture2D>(GlowTexture + (Extra ? "EX" : "")).Value, Projectile.oldPos[n] - Main.screenPosition, new Rectangle((int)(16 * Projectile.ai[0]), 0, 16, 16), Color.White with { A = 127 } * ((10 - n) * .1f) * alpha * (n == 0 ? 1 : .25f), Projectile.oldRot[n], new Vector2(8), 3f * ((10 - n) * .1f), 0);
        }

        return false;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (Projectile.ai[1] == 2)
        {
            if (target.CanBeChasedBy() && target.type != NPCID.WallofFlesh && target.type != NPCID.WallofFleshEye)
                target.velocity += Projectile.velocity * (Projectile.ai[0] == 0 ? 1f : 0.25f) * (hit.Crit ? .6f : .2f);
            for (int n = 0; n < 2; n++)
            {
                if (!Main.rand.NextBool(3))
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity * .5f + Main.rand.NextVector2Unit() * Main.rand.Next(4, 8), Type, Projectile.damage * 3 / 4, Projectile.knockBack / 2, Projectile.owner, Main.rand.Next(4) + 1, 1);
                for (int k = 0; k < 5; k++)
                {
                    Dust.NewDustPerfect(target.Center, DustID.Silver, Main.rand.NextVector2Unit() * Main.rand.NextFloat(4) + Projectile.velocity, 0, default, Main.rand.NextFloat(1, 2));
                }
            }
            if (!target.friendly && target.active && target.CanBeChasedBy())
            {
                Main.player[Projectile.owner].GetModPlayer<ElementSkillPlayer>().ElementChargeValue[3] += damageDone / ElementSkillPlayer.Devider;
            }
        }
        else
        {
            if (target.CanBeChasedBy() && target.type != NPCID.WallofFlesh && target.type != NPCID.WallofFleshEye)
                target.velocity += Projectile.velocity * (Projectile.ai[0] == 0 ? 1f : 0.25f) * (hit.Crit ? .6f : .2f) * .25f;
            for (int n = 0; n < 5 - Projectile.penetrate; n++)
            {
                if (Projectile.ai[0] == 0 && Main.rand.NextBool(2))
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity * .5f + Main.rand.NextVector2Unit() * Main.rand.Next(4, 8), Type, Projectile.damage * 3 / 4, Projectile.knockBack / 2, Projectile.owner, Main.rand.Next(4) + 1, Projectile.ai[1]);
                for (int k = 0; k < 5; k++)
                {
                    Dust.NewDustPerfect(target.Center, Extra ? DustID.Silver : DustID.Copper, Main.rand.NextVector2Unit() * Main.rand.NextFloat(4) + Projectile.velocity, 0, default, Main.rand.NextFloat(1, 2));
                }
            }
            if (!target.friendly && target.active && target.CanBeChasedBy())
            {
                Main.player[Projectile.owner].GetModPlayer<ElementSkillPlayer>().ElementChargeValue[3] += damageDone / ElementSkillPlayer.Devider;
            }
            if ((Main.rand.NextBool(3) || Projectile.penetrate == 1) && Projectile.ai[0] != 0)
            {
                Projectile.penetrate = 2;
                TargetIndex = target.whoAmI;
                offset = Main.rand.NextVector2FromRectangle(new Rectangle(0, 0, target.width * 2 / 3, target.height * 2 / 3)) - new Vector2(target.width, target.height) / 3;
            }
        }
    }

    public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
    {
        if (Projectile.ai[1] == 2)
            return Projectile.timeLeft <= 75 && targetHitbox.Intersects(Utils.CenteredRectangle(projHitbox.Center(), projHitbox.Size() * 3));
        if (Projectile.ai[0] == 0)
            return targetHitbox.Intersects(Utils.CenteredRectangle(projHitbox.Center(), projHitbox.Size() * 3));
        return base.Colliding(projHitbox, targetHitbox);
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        if (Projectile.ai[1] == 2)
        {
            if (Projectile.timeLeft > 45) return false;
        }
        else
        {
            var vec = Projectile.velocity;
            if (vec.X == oldVelocity.X) vec.X = -vec.X;
            if (vec.Y == oldVelocity.Y) vec.Y = -vec.Y;

            if (Projectile.ai[0] == 0)
            {
                for (int n = 0; n < Projectile.penetrate; n++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, -vec + Main.rand.NextVector2Unit() * Main.rand.Next(4, 8), Type, Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner, Main.rand.Next(4) + 1, Projectile.ai[1]);
                }
                for (int k = 0; k < 30; k++)
                {
                    Dust.NewDustPerfect(Projectile.Center + oldVelocity + Main.rand.NextVector2Unit() * Main.rand.NextFloat(0, 32), Extra ? DustID.Silver : DustID.Copper, Main.rand.NextVector2Unit() * Main.rand.NextFloat(4) + oldVelocity * .2f, 0, default, Main.rand.NextFloat(1, 2));
                }
            }

            for (int k = 0; k < 30; k++)
            {
                Dust.NewDustPerfect(Projectile.Center + oldVelocity, Extra ? DustID.Silver : DustID.Copper, Main.rand.NextVector2Unit() * Main.rand.NextFloat(4) + oldVelocity, 0, default, Main.rand.NextFloat(1, 2));
            }
        }

        return base.OnTileCollide(oldVelocity);
    }

    public Vector2 offset;

    public override void OnKill(int timeLeft)
    {
        if (Projectile.ai[1] != 2) return;
        Main.player[Projectile.owner].GetModPlayer<ElementSkillPlayer>().strengthOfShake += 2;
        var vec = Projectile.velocity * .25f;
        //if (vec.X == oldVelocity.X) vec.X = -vec.X;
        //if (vec.Y == oldVelocity.Y) vec.Y = -vec.Y;
        //vec.X *= -1;

        for (int n = 0; n < 16; n++)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + Main.rand.NextVector2Unit() * Main.rand.NextFloat(0, Main.rand.NextFloat(0, 128)), -vec + Main.rand.NextVector2Unit() * Main.rand.Next(4, 8), Type, Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner, Main.rand.Next(4) + 1, 1);
        }
        for (int n = 0; n < 6; n++)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + Main.rand.NextVector2Unit() * Main.rand.NextFloat(0, Main.rand.NextFloat(0, 128)), -vec + Main.rand.NextVector2Unit() * Main.rand.Next(24, 32), Type, Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner, 0, 1);
        }
        for (int k = 0; k < 60; k++)
        {
            Dust.NewDustPerfect(Projectile.Center + Projectile.velocity + Main.rand.NextVector2Unit() * Main.rand.NextFloat(0, Main.rand.NextFloat(0, 128)), DustID.Silver, Main.rand.NextVector2Unit() * Main.rand.NextFloat(4) + Projectile.velocity, 0, default, Main.rand.NextFloat(1, 2));
        }
        base.OnKill(timeLeft);
    }

    public override void AI()
    {
        if (Projectile.ai[1] == 2)
        {
            if (Projectile.timeLeft > 60)
            {
                Projectile.Center = Main.player[Projectile.owner].Center + new Vector2(0, MathHelper.SmoothStep(-64, -256, Utils.GetLerpValue(120, 60, Projectile.timeLeft, true)));
                if (Projectile.timeLeft % 2 == 0)
                {
                    var runit = Main.rand.NextVector2Unit();
                    var gore = Gore.NewGorePerfect(Projectile.Center + runit * 320, -runit * 16, ModContent.GoreType<MetalPiece>(), Main.rand.NextFloat(1, 2f) * 4);
                    gore.Frame = new SpriteFrame(4, 1)
                    {
                        CurrentColumn = (byte)Main.rand.Next(4)
                    };
                    gore.timeLeft = 15;
                }
                Projectile.velocity = default;
            }
            else
            {
                if (Projectile.timeLeft == 60)
                {
                    Projectile.velocity = Main.MouseWorld - Projectile.Center;
                }
                if (Projectile.timeLeft >= 40)
                    Projectile.velocity = Projectile.velocity.SafeNormalize(default) * MathHelper.SmoothStep(0.01f, 64, Utils.GetLerpValue(60, 40, Projectile.timeLeft, true));
                if (Projectile.timeLeft == 30) Projectile.Kill();
            }
            Projectile.rotation += Projectile.rotation * 0.02f;
            for (int n = 9; n > 0; n--)
            {
                Projectile.oldPos[n] = Projectile.oldPos[n - 1];
                Projectile.oldRot[n] = Projectile.oldRot[n - 1];
            }
            Projectile.oldPos[0] = Projectile.Center;
            Projectile.oldRot[0] = Projectile.rotation;
        }
        else
        {
            if (TargetIndex == -1)
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
            }
            else
            {
                var target = Main.npc[TargetIndex];
                if (target.active)
                {
                    Projectile.velocity = default;
                    Projectile.Center = target.Center + offset;
                    if (Projectile.timeLeft % 30 == 0)
                    {
                        var damage = Extra ? 60 : 20;
                        damage = (int)(damage * Main.rand.NextFloat(0.85f, 0.15f));
                        Main.player[Projectile.owner].ApplyDamageToNPC(target, damage, 0, 0);
                        if (!target.friendly && target.active && target.CanBeChasedBy())
                        {
                            Main.player[Projectile.owner].GetModPlayer<ElementSkillPlayer>().ElementChargeValue[3] += damage / ElementSkillPlayer.Devider;
                        }
                    }
                    Projectile.oldPos[0] = Projectile.Center + offset;

                    Projectile.friendly = false;
                }
                else
                {
                    Projectile.Kill();
                }
            }
        }

        base.AI();
    }
}

public class MetalPiece : ModGore
{
    public override void OnSpawn(Gore gore, IEntitySource source)
    {
        base.OnSpawn(gore, source);
    }

    public override Color? GetAlpha(Gore gore, Color lightColor)
    {
        return base.GetAlpha(gore, lightColor);
    }

    public override bool Update(Gore gore)
    {
        gore.alpha = (int)MathHelper.Lerp(255, 5, Utils.GetLerpValue(0, 15, gore.timeLeft, true));
        //gore.timeLeft--;
        //gore.position += gore.position;
        return base.Update(gore);
    }
}