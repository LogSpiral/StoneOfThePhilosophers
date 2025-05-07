using Microsoft.Xna.Framework;
using StoneOfThePhilosophers.UI;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace StoneOfThePhilosophers.Contents.Water;
public class StoneOfWaterProj : MagicArea
{
    public override StoneElements Elements => StoneElements.Water;
    public override Color MainColor => Color.Blue;
    public override int Cycle => player.HasBuff<WaterUltra>() ? 4 : 6;//6
    public override void SpecialAttack(Color dustColor, bool trigger)
    {
        switch (specialAttackIndex)
        {
            case 1:
                {
                    //穿石之流
                    if (trigger)
                    {
                        player.AddBuff(ModContent.BuffType<WaterUltra>(), 450);
                    }
                    break;
                }
            case 2:
                {
                    //潮汐领域
                    if (trigger)
                    {
                        var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), Main.MouseWorld, default, ModContent.ProjectileType<WaterAttack>(), Projectile.damage / 3, projectile.knockBack, projectile.owner, 0, 2);
                        proj.timeLeft = 600;
                        proj.tileCollide = false;
                        proj.width = proj.height = 320;
                        proj.Center = Main.MouseWorld;
                        proj.penetrate = -1;
                        proj.usesLocalNPCImmunity = true;
                        proj.localNPCHitCooldown = 20;
                        proj.extraUpdates = 0;
                    }
                    break;
                }
        }

        base.SpecialAttack(dustColor, trigger);
    }
    public override void ShootProj(Vector2 unit, bool dying = false)
    {
        attackCounter++;
        var flag = player.HasBuff<WaterUltra>();

        if (dying)
            for (int n = -2; n < 3; n += 2)
            {
                var rand = Main.rand.NextFloat(-MathHelper.Pi / 12, MathHelper.Pi / 12) * .5f;
                Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center + 64 * unit, unit.RotatedBy(MathHelper.Pi / 12 * n * (flag ? .5f : 1f) + rand) * (flag ? Main.rand.Next(12, 24) : 16), ModContent.ProjectileType<WaterAttack>(), projectile.damage, projectile.knockBack * (flag ? 2 : 1), projectile.owner, flag ? 0 : Main.rand.Next(5), flag ? 1 : 0);
                //for (int k = 0; k < 3; k++)
                //    Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center + (64 + 16 * k) * projectile.velocity, projectile.velocity.RotatedBy(MathHelper.Pi / 12 * n + rand) * (32 + 8 * k) / 3f, ModContent.ProjectileType<WaterAttack>(), projectile.damage, projectile.knockBack, projectile.owner, Main.rand.Next(5));
            }
        else
        {
            if (flag)
            {
                Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center + 64 * unit, unit * Main.rand.Next(12, 24), ModContent.ProjectileType<WaterAttack>(), projectile.damage * 3 / 2, projectile.knockBack * 2, projectile.owner, 0, 1);

            }
            else
            {
                if (Extra)
                {
                    for (int n = -1; n < 2; n++)
                    {
                        Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center + 64 * unit, unit.RotatedBy(MathHelper.Pi / 18 * n) * 16, ModContent.ProjectileType<WaterAttack>(), projectile.damage, projectile.knockBack, projectile.owner, Main.rand.Next(5));
                    }
                }
                else
                {
                    for (int n = -1; n < 2; n += 2)
                    {
                        Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center + 64 * unit, unit.RotatedBy(MathHelper.Pi / 12 * n) * 12, ModContent.ProjectileType<WaterAttack>(), projectile.damage, projectile.knockBack, projectile.owner, Main.rand.Next(5));
                    }
                }
            }


        }

        SoundEngine.PlaySound(SoundID.Item84);

    }
}