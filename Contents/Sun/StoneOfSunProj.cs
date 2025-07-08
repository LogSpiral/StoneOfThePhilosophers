using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using System;

namespace StoneOfThePhilosophers.Contents.Sun;
public class StoneOfSunProj : MagicArea
{
    protected override StoneElements Elements => StoneElements.Solar;
    protected override int Cycle => 45;
    public override void SpecialAttack(bool trigger) => SpecialAttackStatic(Projectile, trigger, SpecialAttackIndex);
    public override void ShootProj(Vector2 unit, bool dying = false) => ShootProjStatic(projectile, unit, dying, AttackCounter, Extra);

    public static void SpecialAttackStatic(Projectile projectile, bool trigger, int SpecialAttackIndex)
    {
        if (!trigger) return;
        foreach (var target in Main.projectile)
        {
            if (target.active && target.type == ModContent.ProjectileType<SunAttack>() && target.ai[0] == 3)
            {
                target.timeLeft = 1120;
                return;
            }
        }
        var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), Main.MouseWorld, default, ModContent.ProjectileType<SunAttack>(), projectile.damage, projectile.knockBack, projectile.owner, 3);
        proj.timeLeft = 1200;
        proj.tileCollide = false;
        proj.width = proj.height = 320;
        proj.Center = Main.player[projectile.owner].Center + new Vector2(0, 256);
        proj.penetrate = -1;
        proj.usesLocalNPCImmunity = true;
        proj.localNPCHitCooldown = 20;
        proj.extraUpdates = 0;
    }
    public static void ShootProjStatic(Projectile projectile, Vector2 unit, bool dying, int AttackCounter, bool Extra)
    {
        if (dying && projectile.timeLeft % 3 != 0) return;
        SoundEngine.PlaySound(SoundID.Item74);
        if (dying)
        {
            unit = unit.RotatedBy((projectile.timeLeft / 3f - 2) * MathHelper.Pi / 8);
            Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center + 64 * unit, unit * 32,
ModContent.ProjectileType<SunAttack>(), projectile.damage, projectile.knockBack, projectile.owner);
        }
        else
        {
            Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center + 64 * unit, unit.RotatedByRandom(MathHelper.Pi / 48f) * 32,
ModContent.ProjectileType<SunAttack>(), projectile.damage, projectile.knockBack, projectile.owner);
        }
    }
}
