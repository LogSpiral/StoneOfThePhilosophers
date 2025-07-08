using LogSpiralLibrary;
using LogSpiralLibrary.CodeLibrary.Utilties.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace StoneOfThePhilosophers.Contents.Philosopher.Attacks;

public class MetalFireHandler : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.tileCollide = false;
        Projectile.penetrate = 5;
        Projectile.timeLeft = 300;
        Projectile.DamageType = DamageClass.Magic;
        Projectile.friendly = true;
        Projectile.height = 160;
        Projectile.width = 32;
        base.SetDefaults();
    }
    public override void AI()
    {
        try
        {
            Point point = Projectile.Center.ToTileCoordinates();
            if (point.X > 0 && point.X < Main.maxTilesX)
            {
                int t = 0;
                while (point.Y + t < Main.maxTilesY && t < 100)
                {
                    t++;
                    var tile = Main.tile[point.X, point.Y + t];
                    if (tile.HasTile && Main.tileSolid[tile.TileType])
                        break;
                }

                while (point.Y + t > 0 && t > -100)
                {
                    t--;
                    var tile = Main.tile[point.X, point.Y + t];
                    if (!tile.HasTile)
                        break;
                    if (!Main.tileSolid[tile.type])
                        break;
                }

                Projectile.Center += 16 * t * Vector2.UnitY;
            }
        }
        catch
        {

        }
        Projectile.ai[2]--;
        Projectile.friendly = Projectile.ai[2] <= 0;
        if (Projectile.timeLeft % 30 == 29)
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - Vector2.UnitY * 100,
                default, ModContent.ProjectileType<MetalFirePillar>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);


        if (Projectile.timeLeft % 10 == 0)
            Collision.HitTiles(Projectile.Center, default, 32, 32);
    }
    public override void OnKill(int timeLeft)
    {
        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - Vector2.UnitY * 100, default, ModContent.ProjectileType<MetalFirePillar>(), Projectile.damage * 2 / 3, Projectile.knockBack, Projectile.owner);
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        Projectile.ai[2] = 30;
        Projectile.timeLeft -= 60;
        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - Vector2.UnitY * 100, default, ModContent.ProjectileType<MetalFirePillar>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
    }
    public override string Texture => $"Terraria/Images/Item_{ItemID.LivingIchorBlock}";

    public override bool PreDraw(ref Color lightColor)
    {
        return false;
    }
}

public class MetalFirePillar : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.tileCollide = false;
        Projectile.penetrate = -1;
        Projectile.timeLeft = 180;
        //Projectile.hide = true;
        Projectile.DamageType = DamageClass.Magic;
        Projectile.friendly = true;
        Projectile.height = 240;
        Projectile.width = 32;
        base.SetDefaults();
    }
    public override void AI()
    {
        if (Projectile.timeLeft == 180)
        {
            SoundEngine.PlaySound(SoundID.Item74, Projectile.Center);
            for (int n = 0; n < 30; n++)
                Dust.NewDustPerfect(Projectile.Bottom, DustID.GoldFlame, Main.rand.NextVector2Unit() * Main.rand.NextFloat(0, 8), 0, default, Main.rand.NextFloat(1, 3));
        }
        if (Projectile.timeLeft % 10 == 0)
        {
            for (int n = 0; n < 10; n++)
                Dust.NewDustPerfect(Projectile.position + new Vector2(Main.rand.NextFloat(Projectile.width), Main.rand.NextFloat(Main.rand.NextFloat(Projectile.height), Projectile.height)),
                   DustID.GoldFlame, -4 * Vector2.UnitY.RotatedBy((float)Main.rand.GaussianRandom(0, MathHelper.Pi / 24)), 0, Color.White, Main.rand.NextFloat(.5f, 1.5f));
        }
        base.AI();
    }
    public override bool PreDraw(ref Color lightColor)
    {
        SpriteBatch spriteBatch = Main.spriteBatch;
        float k = MathHelper.SmoothStep(0,1, Projectile.timeLeft / 170f);
        float h = 1;
        if (Projectile.timeLeft > 170)
        {

            float t = 180 - Projectile.timeLeft;
            t *= .1f;
            k = MathHelper.SmoothStep(0, 1, t);
            h = k;
            float t2 = t.HillFactor2();
            //float t3 = MathF.Pow(t, 3.0f);
            spriteBatch.Draw(ModAsset.FlameOfNuclear.Value, Projectile.Bottom - Main.screenPosition, null, Color.Orange with { A = 0 } * t2, 0, new Vector2(128), t * 1.5f, 0, 0);
            spriteBatch.Draw(ModAsset.FlameOfNuclear.Value, Projectile.Bottom - Main.screenPosition, null, Color.White with { A = 0 } * t2 * .5f, 0, new Vector2(128), t, 0, 0);

            /*spriteBatch.DrawQuadraticLaser_PassHeatMap(Projectile.Bottom, -Vector2.UnitY, LogSpiralLibraryMod.HeatMap[4].Value, LogSpiralLibraryMod.AniTex[8].Value,
    120f * t3 + 240f, 128f * t3 + 32f, 0, .5f * t2);*/
        }
        spriteBatch.DrawQuadraticLaser_PassHeatMap(Projectile.Bottom, -Vector2.UnitY, LogSpiralLibraryMod.HeatMap[4].Value, LogSpiralLibraryMod.AniTex[8].Value,
            240 * h, k * 64, 0, 4f * k);

        spriteBatch.DrawQuadraticLaser_PassHeatMap(Projectile.Bottom, -Vector2.UnitY, LogSpiralLibraryMod.HeatMap[4].Value, LogSpiralLibraryMod.AniTex[8].Value,
    120 * h, k * 128, 0, 1f * k);
        return false;
    }
    public override string Texture => $"Terraria/Images/Item_{ItemID.LivingIchorBlock}";

}
