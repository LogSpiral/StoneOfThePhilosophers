using Microsoft.Xna.Framework;
using StoneOfThePhilosophers.UI;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace StoneOfThePhilosophers.Contents.Sun;
public class StoneOfSunProj : MagicArea
{
    public override StoneElements Elements => StoneElements.Solar;
    public override Color MainColor => Color.White;
    public override int Cycle => 60;
    public override void SpecialAttack(Color dustColor, bool trigger)
    {
        if (trigger)
        {
            var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), Main.MouseWorld, default, ModContent.ProjectileType<SunAttack>(), Projectile.damage / 3, projectile.knockBack, projectile.owner, 3);
            proj.timeLeft = 1200;
            proj.tileCollide = false;
            proj.width = proj.height = 320;
            proj.Center = player.Center + new Vector2(0, 256);
            proj.penetrate = -1;
            proj.usesLocalNPCImmunity = true;
            proj.localNPCHitCooldown = 20;
            proj.extraUpdates = 0;
        }
        base.SpecialAttack(dustColor, trigger);
    }
    public override void ShootProj(Vector2 unit, bool dying = false)
    {
        if (dying && projectile.timeLeft % 3 != 0) return;
        attackCounter++;
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
