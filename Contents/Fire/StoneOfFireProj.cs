using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace StoneOfThePhilosophers.Contents.Fire;

public class StoneOfFireProj : MagicArea
{
    protected override StoneElements Elements => StoneElements.Fire;
    protected override int Cycle => 30;

    public override void ShootProj(Vector2 unit, bool dying = false) => ShootProjStatic(Projectile, unit, dying, AttackCounter, Extra);

    public override void SpecialAttack(bool trigger) => SpecialAttackStatic(Projectile, trigger, SpecialAttackIndex);

    public static void SpecialAttackStatic(Projectile projectile, bool trigger, int SpecialAttackIndex)
    {
        if (!trigger) return;
        var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), Main.MouseWorld, default, ModContent.ProjectileType<FireAttack>(), projectile.damage * 2, projectile.knockBack, projectile.owner, 6);
        proj.timeLeft = 300;
        proj.tileCollide = false;
        proj.width = proj.height = 320;
        proj.Center = Main.MouseWorld;
        proj.penetrate = -1;
        proj.usesLocalNPCImmunity = true;
        proj.localNPCHitCooldown = 20;
    }

    public static void ShootProjStatic(Projectile projectile, Vector2 unit, bool dying, int AttackCounter, bool Extra)
    {
        if (dying && projectile.timeLeft % 2 == 1) return;
        SoundEngine.PlaySound(SoundID.Item74, projectile.Center);
        if (AttackCounter % 5 == 0 && Extra)
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
}