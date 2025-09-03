using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace StoneOfThePhilosophers.Contents.Earth;

public class StoneOfEarthProj : MagicArea
{
    protected override StoneElements Elements => StoneElements.Soil;
    protected override int Cycle => Extra ? 24 : 36;

    public override void SpecialAttack(bool trigger) => SpecialAttackStatic(Projectile, trigger, SpecialAttackIndex);

    public override void ShootProj(Vector2 unit, bool dying = false) => ShootProjStatic(Projectile, unit, dying, AttackCounter, Extra);

    public static void SpecialAttackStatic(Projectile projectile, bool trigger, int SpecialAttackIndex)
    {
        if (!trigger) return;
        var player = Main.player[projectile.owner];
        var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), player.Center, default, ModContent.ProjectileType<EarthSpecialAttack>(), 0, 0, projectile.owner, 0, SpecialAttackIndex - 1);
        if (SpecialAttackIndex == 2)
        {
            proj.timeLeft = 1200;
            proj.friendly = false;
            proj.damage = projectile.damage;
        }
        else
        {
            int k = 0;
            Point mouseCen = Main.MouseWorld.ToTileCoordinates();
            while (k < 64)
            {
                var tile = Framing.GetTileSafely(mouseCen + new Point(0, k));
                if (tile.HasTile && Main.tileSolid[tile.TileType])
                    break;
                k++;
            }
            proj.Center = (mouseCen + new Point(0, k)).ToVector2() * 16 + new Vector2(0, 4);
        }
    }

    public static void ShootProjStatic(Projectile projectile, Vector2 unit, bool dying, int AttackCounter, bool Extra)
    {
        if (dying && projectile.timeLeft % 2 == 1) return;
        SoundEngine.PlaySound(SoundID.Item69);
        if (AttackCounter % 5 == 0 && Extra)
        {
            for (int n = 0; n < 3; n++)
            {
                var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center + 64 * unit, unit.RotatedBy((n - 1) * MathHelper.Pi / 12) * 32,
ModContent.ProjectileType<EarthAttack>(), (int)(projectile.damage * 1.25f), projectile.knockBack, projectile.owner, 0, 2);
                (proj.ModProjectile as EarthAttack).SetDefaultStorm();
                proj.Center = projectile.Center + 64 * unit;
            }
        }
        else
        {
            for (int n = 0; n < 4; n++)
            {
                var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center + 64 * unit, unit.RotatedByRandom(MathHelper.TwoPi / 24f * (dying ? 2 : 1)) * 32,
ModContent.ProjectileType<EarthAttack>(), projectile.damage, projectile.knockBack, projectile.owner);
                proj.frame = Main.rand.Next(1, 5);
                proj.tileCollide = !Extra;
            }
        }
    }
}