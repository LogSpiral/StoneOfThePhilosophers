using Microsoft.Xna.Framework;
using StoneOfThePhilosophers.UI;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace StoneOfThePhilosophers.Contents.Metal;
public class StoneOfMetalProj : MagicArea
{
    public override StoneElements Elements => StoneElements.Metal;
    public override Color MainColor => Color.Yellow;
    public override int Cycle => Extra ? 18 : 27;//48
    public override void SpecialAttack(Color dustColor, bool trigger)
    {
        if ((int)projectile.ai[0] == 1)
        {
            var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), player.Center, default, ModContent.ProjectileType<MetalAttack>(), (int)(projectile.damage * Main.rand.NextFloat(1.5f, 2.5f)), projectile.knockBack * 3, projectile.owner, 0, 2);
            proj.timeLeft = 120;
            proj.width = proj.height = 120;
            proj.Center = player.Center;
            proj.penetrate = -1;
            proj.rotation = MathHelper.TwoPi;
            proj.usesLocalNPCImmunity = false;
        }
    }
    public override void ShootProj(Vector2 unit, bool dying = false)
    {
        attackCounter++;
        bool flag = dying && projectile.timeLeft % 3 != 0;
        SoundEngine.PlaySound(SoundID.Item69);
        for (int n = 0; n < (flag ? 3 : 1); n++)
            Projectile.NewProjectile(projectile.GetSource_FromThis(), player.Center + (dying ? unit.RotatedByRandom(MathHelper.Pi / 3) : unit) * 64, unit * 32 * (flag ? 1 : 1), ModContent.ProjectileType<MetalAttack>(), (int)(projectile.damage * Main.rand.NextFloat(1.25f, 0.95f)), projectile.knockBack * 3, projectile.owner, flag ? Main.rand.Next(4) + 1 : 0, Extra ? 1 : 0);
    }
}