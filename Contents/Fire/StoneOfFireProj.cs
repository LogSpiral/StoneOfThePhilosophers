using Microsoft.Xna.Framework;
using StoneOfThePhilosophers.UI;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace StoneOfThePhilosophers.Contents.Fire;

public class StoneOfFireProj : MagicArea
{
    public override StoneElements Elements => StoneElements.Fire;
    public override Color MainColor => Color.Red;
    public override int Cycle => 30;
    public override void ShootProj(Vector2 unit, bool dying = false)
    {
        if (dying && projectile.timeLeft % 2 == 1) return;
        SoundEngine.PlaySound(SoundID.Item74);
        attackCounter++;
        if (attackCounter % 5 == 0 && Extra)
        {
            var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center + 64 * unit, unit.RotatedByRandom(MathHelper.TwoPi / 48f * (dying ? 2 : 0)) * 32, ModContent.ProjectileType<FireAttack>(), (int)(projectile.damage * 1.25f), projectile.knockBack, projectile.owner, 4);
            proj.timeLeft = 45;
            proj.tileCollide = false;
        }
        else
        {
            Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center + 64 * unit, unit.RotatedByRandom(MathHelper.TwoPi / 48f * (dying ? 2 : 1)) * 32,
ModContent.ProjectileType<FireAttack>(), projectile.damage, projectile.knockBack, projectile.owner, Extra ? 1 : 0);
        }

    }
    public override void SpecialAttack(Color dustColor, bool trigger)
    {
        if (trigger)
        {
            var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), Main.MouseWorld, default, ModContent.ProjectileType<FireAttack>(), projectile.damage * 2, projectile.knockBack, projectile.owner, 6);
            proj.timeLeft = 300;
            proj.tileCollide = false;
            proj.width = proj.height = 320;
            proj.Center = Main.MouseWorld;
            proj.penetrate = -1;
            proj.usesLocalNPCImmunity = true;
            proj.localNPCHitCooldown = 20;
        }
        base.SpecialAttack(dustColor, trigger);
    }
}
