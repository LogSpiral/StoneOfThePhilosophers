using LogSpiralLibrary;
using LogSpiralLibrary.CodeLibrary.Utilties.Extensions;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace StoneOfThePhilosophers.Contents.Philosopher.Attacks;

public class LunarFireTorch : ModProjectile
{
    private Player Owner => Main.player[Projectile.owner];
    public override string Texture => $"Terraria/Images/Item_{ItemID.LivingDemonFireBlock}";

    public override void SetDefaults()
    {
        Projectile.timeLeft = 900;
        Projectile.width = Projectile.height = 1;
        Projectile.friendly = false;
        Projectile.hostile = false;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.aiStyle = -1;
        base.SetDefaults();
    }

    public override void AI()
    {
        if (Projectile.timeLeft == 880)
            SoundEngine.PlaySound(SoundID.Item62, Projectile.Center);

        Projectile.Center = Owner.Center - Vector2.UnitY * 96 + Vector2.UnitY * Owner.gfxOffY;
        if (Projectile.timeLeft is > 15 and < 885 && Projectile.timeLeft % 10 == 0)
            foreach (var npc in Main.npc)
            {
                if (!npc.active || npc.friendly || NPCID.Sets.ImmuneToAllBuffs[npc.type] || npc.buffImmune[ModContent.BuffType<LunarFireBuff>()]) continue;
                var vec = npc.Center - Projectile.Center;
                if (vec.Length() > 400) continue;

                if (!npc.HasBuff<LunarFireBuff>())
                {
                    var tCenter = npc.Center;
                    var pCenter = Projectile.Center;
                    for (int n = 0; n < 30; n++)
                    {
                        Dust.NewDustPerfect(tCenter, DustID.Shadowflame, Main.rand.NextVector2Unit() * Main.rand.NextFloat(0, 16), 0, default, Main.rand.NextFloat(.5f, 1.25f));
                        Dust.NewDustPerfect(Vector2.Lerp(pCenter, tCenter, n / 29f), DustID.Shadowflame);
                    }
                    SoundEngine.PlaySound(SoundID.Item74 with { volume = .5f, MaxInstances = -1 }, Projectile.Center);
                }
                npc.AddBuff(ModContent.BuffType<LunarFireBuff>(), 180);
            }
        base.AI();
    }

    public override bool PreDraw(ref Color lightColor)
    {
        var spriteBatch = Main.spriteBatch;
        var texture = TextureAssets.Projectile[Type].Value;
        var center = Projectile.Center - Main.screenPosition;
        var alpha = ((float)Projectile.timeLeft).SymmetricalFactor(450f, 30);
        alpha = alpha < .8f ? MathHelper.SmoothStep(0, 1.1f, alpha / .8f) : MathHelper.SmoothStep(1.1f, 1f, (alpha - .8f) / .2f);
        alpha = MathF.Pow(alpha, 3);
        var color = Color.White * alpha;
        var origin = texture.Size() * .5f;
        var scalerVector = new Vector2(.75f + MathF.Cos(Projectile.timeLeft * .1f) * .15f, 1);

        spriteBatch.DrawQuadraticLaser_PassHeatMap(Projectile.position + Vector2.UnitY * 48, -Vector2.UnitY, LogSpiralLibraryMod.HeatMap[8].Value, LogSpiralLibraryMod.AniTex[8].Value,
    120 * scalerVector.X * alpha, 96 * scalerVector.Y * alpha, 0, alpha);

        spriteBatch.Draw(texture, center, null, color with { A = 0 }, 0, origin, 4 * alpha * scalerVector, 0, 0);
        for (int n = 0; n < 3; n++)
            spriteBatch.Draw(texture, center + (MathHelper.TwoPi * (n / 3f + Projectile.timeLeft * .05f)).ToRotationVector2() * 4, null, color with { A = 0 } * .25f, 0, origin, 5 * alpha * alpha * scalerVector, 0, 0);

        for (int n = 0; n < 5; n++)
        {
            float t = (Projectile.timeLeft / 60f + n * .2f) % 1;
            float y = (2 * t - 1) * 60;
            float k = 4 * t * (1 - t);
            Vector2 offset = new Vector2(MathF.Sin(t * MathHelper.TwoPi + Projectile.timeLeft / 30f) * 64 * k, y) * scalerVector;
            spriteBatch.Draw(texture, center + offset, null, color with { A = 0 } * k, 0, origin, alpha * scalerVector, 0, 0);

            spriteBatch.DrawQuadraticLaser_PassHeatMap(Projectile.position + offset + Vector2.UnitY * 8, -Vector2.UnitY, LogSpiralLibraryMod.HeatMap[8].Value, LogSpiralLibraryMod.AniTex[8].Value,
                30 * scalerVector.X * alpha, 24 * scalerVector.Y * alpha, 0, alpha * k * .5f);
        }

        return false;
    }
}

public class LunarFireBuff : ModBuff
{
    public override string Texture => StoneOfThePhilosopher.DebuffTexturePath;

    public override void Update(NPC npc, ref int buffIndex)
    {
        npc.lifeRegen -= 200;
        if (Main.rand.NextBool(3))
            Dust.NewDust(npc.position, npc.width, npc.height, DustID.Shadowflame, Scale: Main.rand.NextFloat(.5f, 1.25f));
        base.Update(npc, ref buffIndex);
    }
}

public class LunarFireGlobalNPC : GlobalNPC
{
    public override void ModifyHitNPC(NPC npc, NPC target, ref NPC.HitModifiers modifiers)
    {
        if (npc.HasBuff<LunarFireBuff>())
            modifiers.FinalDamage *= 1.2f;
        base.ModifyHitNPC(npc, target, ref modifiers);
    }

    public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
    {
        if (npc.HasBuff<LunarFireBuff>())
            modifiers.FinalDamage *= 1.2f;
        base.ModifyHitPlayer(npc, target, ref modifiers);
    }

    public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
    {
        if (npc.HasBuff<LunarFireBuff>())
            modifiers.ScalingArmorPenetration += .5f;

        base.ModifyIncomingHit(npc, ref modifiers);
    }

    public override void UpdateLifeRegen(NPC npc, ref int damage)
    {
        if (npc.HasBuff<LunarFireBuff>() && damage < 20)
            damage = 20;
        base.UpdateLifeRegen(npc, ref damage);
    }

    public override void AI(NPC npc)
    {
        if (npc.HasBuff<LunarFireBuff>())
            npc.velocity *= 1.015f;
        base.AI(npc);
    }
}