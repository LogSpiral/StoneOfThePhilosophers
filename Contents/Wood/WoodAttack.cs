using LogSpiralLibrary.CodeLibrary.Utilties;
using LogSpiralLibrary.CodeLibrary.Utilties.Extensions;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace StoneOfThePhilosophers.Contents.Wood;

public class WoodAttack : ModProjectile
{
    private bool Extra => Projectile.frameCounter != 0;

    public override void SetDefaults()
    {
        Projectile.width = Projectile.height = 8;
        Projectile.timeLeft = 180;
        Projectile.DamageType = DamageClass.Magic;
        Projectile.friendly = true;
        Projectile.tileCollide = true;
        Projectile.ignoreWater = true;
        Projectile.penetrate = 3;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 5;
        Projectile.aiStyle = -1;
    }

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();
    }

    public override bool PreDraw(ref Color lightColor)
    {
        float alpha = (Projectile.timeLeft / 180f).SmoothSymmetricFactor(1 / 12f);

        for (int n = 9; n > -1; n--)
            for (int m = 0; m < 5; m++)
                Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.oldPos[n] - Main.screenPosition + (m == 0 ? default : Main.rand.NextVector2Unit() * 4), new Rectangle((int)(16 * Projectile.ai[0]), Extra ? 16 : 0, 16, 16), Color.Lerp(lightColor, Color.White, .5f) with { A = 127 } * alpha * ((10 - n) * .1f) * (m == 0 ? 1 : Main.rand.NextFloat(0.25f, 0.5f)), Projectile.oldRot[n], new Vector2(8), 2f * ((10 - n) * .1f) * new Vector2(1.5f, 1f), 0, 0);
        return false;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        for (int n = 0; n < 4 - Projectile.penetrate; n++)
        {
            for (int k = 0; k < 15; k++)
            {
                Dust.NewDustPerfect(target.Center, DustID.TerraBlade, Main.rand.NextVector2Unit() * Main.rand.NextFloat(4) + Projectile.velocity, 0, default, Main.rand.NextFloat(0.5f, 1.5f));
            }
        }
        if (Extra && Main.rand.NextBool(3)) target.AddBuff(BuffID.Venom, 120);
        else target.AddBuff(BuffID.Poisoned, 360);
        if (!target.friendly && target.active && target.CanBeChasedBy())
        {
            Main.player[Projectile.owner].GetModPlayer<ElementSkillPlayer>().ElementChargeValue[2] += damageDone / ElementSkillPlayer.Devider;
        }
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        for (int k = 0; k < 15; k++)
        {
            Dust.NewDustPerfect(Projectile.Center + oldVelocity, DustID.TerraBlade, Main.rand.NextVector2Unit() * Main.rand.NextFloat(4) + oldVelocity, 0, default, Main.rand.NextFloat(0.5f, 1.5f));
        }
        return base.OnTileCollide(oldVelocity);
    }

    public override void AI()
    {
        NPC target = null;
        float maxDistance = Extra ? 256 : 128;
        float maxDistanceCopy = maxDistance;
        foreach (var npc in Main.npc)
        {
            if (!npc.CanBeChasedBy() || npc.friendly) continue;
            var currentDistance = Vector2.Distance(npc.Center, Projectile.Center);
            if (currentDistance < maxDistance)
            {
                maxDistance = currentDistance;
                target = npc;
            }
        }
        if (target != null)
        {
            //var fac = MathF.Cos(Main.GameUpdateCount * MathHelper.Pi / 7.5f) * .5f + .5f;
            float factor = Projectile.frameCounter == 2 ? Utils.GetLerpValue(180, 150, Projectile.timeLeft, true) : 1;
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, (target.Center - Projectile.Center).SafeNormalize(default) * Projectile.ai[1] * factor, Utils.GetLerpValue(maxDistanceCopy / 8, maxDistanceCopy, maxDistance, true) * (Extra ? 0.25f : 0.125f) * factor);
            //Projectile.velocity = Projectile.velocity.SafeNormalize(default) * Projectile.ai[1];
            if (Main.GameUpdateCount % 3 == 0)
            {
                if (Projectile.timeLeft > 165) Projectile.timeLeft--;
                if (Projectile.timeLeft < 15) Projectile.timeLeft++;
            }
        }
        //else
        //{
        //    Projectile.alpha = Projectile.timeLeft;
        //}
        Projectile.rotation = Projectile.velocity.ToRotation();
        for (int n = 9; n > 0; n--)
        {
            Projectile.oldPos[n] = Projectile.oldPos[n - 1];
            Projectile.oldRot[n] = Projectile.oldRot[n - 1];
        }
        Projectile.oldPos[0] = Projectile.Center;
        Projectile.oldRot[0] = Projectile.rotation;
    }
}

public class WoodAttackUltra : ModProjectile
{
    private Projectile projectile => Projectile;

    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("粒子触手");
    }

    public override string Texture => base.Texture.Replace("Ultra", "");

    public override void SetDefaults()
    {
        projectile.scale = 1f;
        projectile.friendly = true;
        projectile.DamageType = DamageClass.Melee;
        projectile.ignoreWater = true;
        projectile.timeLeft = 300;
        projectile.tileCollide = false;
        projectile.penetrate = -1;
        projectile.light = 0.5f;
        projectile.aiStyle = -1;
        projectile.alpha = 255;
        projectile.width = 40;
        projectile.height = 40;
        projectile.MaxUpdates = 3;
    }

    public override void AI()
    {
        Vector2 center10 = projectile.Center;
        projectile.scale = 1f - projectile.localAI[0];
        projectile.width = (int)(20f * projectile.scale);
        projectile.height = projectile.width;
        projectile.position.X = center10.X - projectile.width / 2;
        projectile.position.Y = center10.Y - projectile.height / 2;
        if (projectile.localAI[0] < 0.1)
        {
            projectile.localAI[0] += 0.01f;
        }
        else
        {
            projectile.localAI[0] += 0.025f;
        }
        if (projectile.localAI[0] >= 0.95f)
        {
            projectile.Kill();
        }
        projectile.velocity.X = projectile.velocity.X + projectile.ai[0] * 1.5f;
        projectile.velocity.Y = projectile.velocity.Y + projectile.ai[1] * 1.5f;
        if (projectile.velocity.Length() > 16f)
        {
            projectile.velocity.Normalize();
            projectile.velocity *= 16f;
        }
        if (projectile.timeLeft % 10 == 0 && Main.rand.NextBool(2))
        {
            var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity * 0.0001f,
ModContent.ProjectileType<WoodAttack>(), projectile.damage, projectile.knockBack, projectile.owner, Main.rand.Next(Main.rand.Next(5)), Main.rand.NextFloat(24, 48));
            proj.penetrate = 1;
            proj.tileCollide = false;
            proj.frameCounter = 2;
        }
        projectile.ai[0] *= 1.05f;
        projectile.ai[1] *= 1.05f;
        if (projectile.scale < 1f)
        {
            int num892 = 0;
            while (num892 < projectile.scale * 10f)
            {
                int num893 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, MyDustId.GreenFXPowder, projectile.velocity.X, projectile.velocity.Y, 0, Color.White, 1.1f);
                Main.dust[num893].position = (Main.dust[num893].position + projectile.Center) / 2f;
                Main.dust[num893].noGravity = true;
                Dust dust3 = Main.dust[num893];
                dust3.velocity *= 0.1f;
                dust3 = Main.dust[num893];
                dust3.velocity -= projectile.velocity * (1.3f - projectile.scale);
                Main.dust[num893].fadeIn = 100 + projectile.owner;
                dust3 = Main.dust[num893];
                //dust3.scale += projectile.scale * 0.75f;
                int num3 = num892;
                num892 = num3 + 1;
            }
            return;
        }
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        //if (target.CanBeChasedBy())
        //{
        //    var unit = (projectile.Center - target.Center).SafeNormalize(default);
        //    target.velocity += (Vector2.Dot(unit, target.velocity) / target.velocity.LengthSquared() - 1) * unit * -1;
        //}
        if (!target.friendly && target.active && target.CanBeChasedBy())
        {
            Main.player[Projectile.owner].GetModPlayer<ElementSkillPlayer>().ElementChargeValue[3] += damageDone / ElementSkillPlayer.Devider;
        }
        target.immune[projectile.owner] = 5;
    }
}

public class WoodUltra : ModBuff
{
}