using LogSpiralLibrary.CodeLibrary.Utilties;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace StoneOfThePhilosophers.Contents.Wood;
public class WoodCrystal : ModNPC
{
    public override void SetDefaults()
    {
        NPC.width = NPC.height = 60;
        NPC.HitSound = SoundID.Item27;
        NPC.lifeMax = 500;
        NPC.noTileCollide = true;
        NPC.friendly = true;
        base.SetDefaults();
    }
    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("巨木之晶");
        base.SetStaticDefaults();
    }
    Player Player => Main.player[(int)NPC.ai[0]];
    public override void AI()
    {
        NPC.ai[2]++;
        NPC.Center = Player.Center + (NPC.ai[2] / 60f * MathHelper.TwoPi + MathHelper.TwoPi / 3 * NPC.ai[1]).ToRotationVector2() * 80;

        Rectangle hitbox = NPC.Hitbox;
        for (int i = 0; i < 200; i++)
        {
            NPC nPC = Main.npc[i];
            if (nPC.active && !nPC.friendly && nPC.damage > 0)
            {
                Rectangle npcRect = nPC.Hitbox;
                int specialHitSetter = 1;
                float damageMultiplier = 1;

                NPC.GetMeleeCollisionData(hitbox, i, ref specialHitSetter, ref damageMultiplier, ref npcRect);
                bool? modCanHit = NPCLoader.CanHitNPC(Main.npc[i], NPC);
                if (modCanHit.HasValue && !modCanHit.Value)
                    continue;
                if (hitbox.Intersects(npcRect) && (modCanHit == true || !NPCID.Sets.Skeletons[nPC.type]) && nPC.type != NPCID.Gnome)
                {
                    OnHitByNPC(nPC);
                }
            }
        }

        base.AI();
    }
    public void OnHitByNPC(NPC target)
    {
        if (!target.CanBeChasedBy()) return;
        target.velocity += (target.Center - Player.Center).SafeNormalize(default) * 4 * (target.knockBackResist * 0.67f + 0.33f);
        for (int n = 0; n < 30; n++)
            Dust.NewDustPerfect(target.Center, MyDustId.GreenFXPowder, Main.rand.NextFloat(0, MathHelper.TwoPi).ToRotationVector2() * Main.rand.NextFloat(4, 8)).noGravity = true;//MathHelper.TwoPi / 30 * n
        Player.ApplyDamageToNPC(target, (int)(target.damage * Main.rand.NextFloat(0.85f, 1.15f) * .5f), 0, 1, Main.rand.NextBool(10));
    }
    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        var r = NPC.ai[2] / 60f * MathHelper.TwoPi + MathHelper.TwoPi / 3 * NPC.ai[1] + MathHelper.Pi;
        var scaler = .25f + 0.25f * MathF.Cos(Main.GlobalTimeWrappedHourly * 4);
        spriteBatch.Draw(TextureAssets.Npc[Type].Value, NPC.Center - screenPos, null, Color.White with { A = 204 }, r, new Vector2(15), 1, 0, 0);
        spriteBatch.Draw(TextureAssets.Npc[Type].Value, NPC.Center + Main.rand.NextVector2Unit() * scaler * 4 - screenPos, null, Color.White with { A = 0 } * .5f, r, new Vector2(15), 1 + scaler, 0, 0);
        for (int n = 1; n < 4; n++)
        {
            spriteBatch.Draw(TextureAssets.Npc[Type].Value, NPC.Center - screenPos + r.ToRotationVector2() * 80 - (r - n / 30f * MathHelper.TwoPi).ToRotationVector2() * 80, null, Color.White with { A = 0 } * (1 - 0.25f * n), r - n / 30f * MathHelper.TwoPi, new Vector2(15), 1f * (1 - 0.25f * n), 0, 0);

        }
        return false;
    }
    public override void HitEffect(NPC.HitInfo hit)
    {
        base.HitEffect(hit);
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit)
    {
        base.OnHitNPC(target, hit);
    }
    public override bool? CanBeHitByProjectile(Projectile projectile)
    {
        return base.CanBeHitByProjectile(projectile);
    }
    public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
    {
        projectile.Kill();
        base.OnHitByProjectile(projectile, hit, damageDone);
    }
}