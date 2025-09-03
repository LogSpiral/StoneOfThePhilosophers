using LogSpiralLibrary;
using LogSpiralLibrary.CodeLibrary.Utilties;
using LogSpiralLibrary.CodeLibrary.Utilties.Extensions;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace StoneOfThePhilosophers.Contents.Philosopher.Attacks;

public class WaterFireRain : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.timeLeft = 1800;
        Projectile.width = Projectile.height = 1;
        Projectile.friendly = false;
        Projectile.hostile = false;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.hide = true;
        Projectile.aiStyle = -1;
        base.SetDefaults();
    }

    public override string Texture => $"Terraria/Images/Item_{ItemID.LivingFrostFireBlock}";
    private Player Owner => Main.player[Projectile.owner];

    public override void AI()
    {
        Projectile.Center = Owner.Center;
        foreach (var npc in Main.npc)
        {
            if (!npc.active || npc.friendly) continue;
            var vec = npc.Center - Projectile.Center;
            if (Math.Abs(vec.X) > 960 || Math.Abs(vec.Y) > 540) continue;
            npc.AddBuff(ModContent.BuffType<WaterFireBuff>(), 2);
        }

        if (Projectile.timeLeft % 5 == 0)
        {
            for (int n = 0; n < 3; n++)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + new Vector2(Main.rand.NextFloat(-960, 960),
                    -Main.rand.NextFloat(480, 560)), new Vector2(0, 9).RotatedBy(-Main.windSpeedCurrent / 1.2f * MathHelper.PiOver4) * 1.5f,
                    ModContent.ProjectileType<WaterFireAttack>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
        }

        base.AI();
    }
}

public class WaterFireAttack : ModProjectile
{
    public override string Texture => $"Terraria/Images/Item_{ItemID.LivingFrostFireBlock}";

    public override void SetDefaults()
    {
        Projectile.timeLeft = 120;
        Projectile.width = Projectile.height = 16;
        Projectile.friendly = true;
        Projectile.penetrate = 1;
        Projectile.tileCollide = true;
        Projectile.ignoreWater = true;
        Projectile.DamageType = DamageClass.Magic;
        Projectile.aiStyle = -1;
        base.SetDefaults();
    }

    public override void AI()
    {
        if (Projectile.velocity != default)
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        if (Main.rand.NextBool(3))
            Dust.NewDustPerfect(Projectile.Center, DustID.FrostStaff).noGravity = true;
        base.AI();
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        for (int n = 0; n < 10; n++)
            Dust.NewDust(Projectile.position, 16, 16, DustID.FrostStaff);
        target.AddBuff(BuffID.OnFire, 180);
        target.AddBuff(BuffID.Frostburn, 180);
        base.OnHitNPC(target, hit, damageDone);
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        for (int n = 0; n < 10; n++)
            Dust.NewDust(Projectile.position, 16, 16, DustID.FrostStaff);
        SoundEngine.PlaySound(MySoundID.ProjectileHit with { volume = 0.5f, MaxInstances = -1 });
        return true;
    }

    public override bool PreDraw(ref Color lightColor)
    {
        var spriteBatch = Main.spriteBatch;
        var texture = TextureAssets.Projectile[Type].Value;
        var center = Projectile.Center - Main.screenPosition;
        var alpha = (Projectile.timeLeft / 120f).SymmetricalFactor(0.5f, 0.2f);
        var color = Color.White * alpha;
        var origin = texture.Size() * .5f;
        spriteBatch.Draw(texture, center, null, color, Projectile.rotation, origin, 1, 0, 0);
        float scaler = 1f;
        spriteBatch.Draw(texture, center, null, color with { A = 0 } * .5f, Projectile.rotation, origin, 1.5f, 0, 0);
        for (int n = 0; n < 10; n++)
        {
            spriteBatch.Draw(texture, center + Main.rand.NextVector2Unit() * Main.rand.NextFloat(0, 8) * scaler, null, color with { A = 0 } * Main.rand.NextFloat(0, 0.5f),
                Projectile.rotation, origin, Main.rand.NextFloat(1, 1.25f) * scaler, 0, 0);
            center -= Projectile.velocity * .25f;
            scaler *= .9f;
        }
        spriteBatch.DrawQuadraticLaser_PassHeatMap(Projectile.Center, -Projectile.velocity.SafeNormalize(default), LogSpiralLibraryMod.HeatMap[1].Value, LogSpiralLibraryMod.AniTex[8].Value,
            80f, 16f, 0, alpha);
        spriteBatch.DrawQuadraticLaser_PassHeatMap(Projectile.Center + Projectile.velocity, -Projectile.velocity.SafeNormalize(default), LogSpiralLibraryMod.HeatMap[1].Value, LogSpiralLibraryMod.AniTex[8].Value,
    64f, 64f, 0, alpha * .75f);
        return false;
    }
}

public class WaterFireBuff : ModBuff
{
    public override string Texture => $"Terraria/Images/Buff_44";
}

public class WaterFireGlobalNPC : GlobalNPC
{
    private static bool CountAsBoss(NPC npc) => npc.boss || npc.type is NPCID.EaterofWorldsHead or NPCID.EaterofWorldsBody or NPCID.EaterofWorldsTail;

    public override void ModifyHitNPC(NPC npc, NPC target, ref NPC.HitModifiers modifiers)
    {
        if (npc.HasBuff<WaterFireBuff>())
            modifiers.FinalDamage *= CountAsBoss(npc) ? .95f : .9f;
    }

    public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
    {
        if (npc.HasBuff<WaterFireBuff>())
            modifiers.FinalDamage *= CountAsBoss(npc) ? .95f : .9f;
    }

    public override void AI(NPC npc)
    {
        if (npc.HasBuff<WaterFireBuff>())
            npc.velocity *= .975f;
    }
}