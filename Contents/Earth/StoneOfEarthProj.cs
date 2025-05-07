using Microsoft.Xna.Framework;
using StoneOfThePhilosophers.UI;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace StoneOfThePhilosophers.Contents.Earth;
public class StoneOfEarthProj : MagicArea
{
    public override StoneElements Elements => StoneElements.Soil;
    public override Color MainColor => Color.Orange;
    public override int Cycle => Extra ? 24 : 36;
    public override void SpecialAttack(Color dustColor, bool trigger)
    {
        if (trigger)
        {

            var proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), player.Center, default, ModContent.ProjectileType<EarthSpecialAttack>(), 0, 0, Projectile.owner, 0, specialAttackIndex - 1);
            if (specialAttackIndex == 2)
            {
                proj.timeLeft = 1200;
                proj.friendly = false;
                proj.damage = Projectile.damage;
            }
            else
            {
                int k = 0;
                Point mouseCen = Main.MouseWorld.ToTileCoordinates();
                while (k < 64)
                {
                    var tile = Main.tile[mouseCen + new Point(0, k)];
                    if (tile.HasTile && Main.tileSolid[tile.TileType])
                        break;
                    k++;
                }
                proj.Center = (mouseCen + new Point(0, k)).ToVector2() * 16 + new Vector2(0, 4);
            }

        }
        base.SpecialAttack(dustColor, trigger);
    }
    public override void ShootProj(Vector2 unit, bool dying = false)
    {
        if (dying && projectile.timeLeft % 2 == 1) return;
        SoundEngine.PlaySound(SoundID.Item69);
        attackCounter++;
        if (attackCounter % 5 == 0 && Extra)
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
