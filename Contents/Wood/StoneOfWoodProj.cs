using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace StoneOfThePhilosophers.Contents.Wood;

public class StoneOfWoodProj : MagicArea
{
    protected override StoneElements Elements => StoneElements.Wood;
    protected override int Cycle => Extra ? 18 : 24;//24

    public override void SpecialAttack(bool trigger) => SpecialAttackStatic(projectile, trigger, SpecialAttackIndex);

    public override void ShootProj(Vector2 unit, bool dying = false) => ShootProjStatic(Projectile, unit, dying, AttackCounter, Extra);

    public static void SpecialAttackStatic(Projectile projectile, bool trigger, int SpecialAttackIndex)
    {
        if (!trigger) return;
        var player = Main.player[projectile.owner];
        switch (SpecialAttackIndex)
        {
            case 1:
                {
                    // 常青藤鞭
                    player.AddBuff(ModContent.BuffType<WoodUltra>(), 450);

                    break;
                }
            case 2:
                {
                    // 巨木之晶
                    for (int n = 0; n < 3; n++)
                    {
                        var npc = NPC.NewNPCDirect(projectile.GetNPCSource_FromThis(), player.Center, ModContent.NPCType<WoodCrystal>(), 0, player.whoAmI, n);
                        npc.defense = player.statDefense;
                    }
                    break;
                }
            case 3:
                {
                    // 愈伤组织
                    player.AddBuff(ModContent.BuffType<Reincarnation>(), 300);
                    int healValue = player.statLifeMax2 - player.statLife;
                    healValue = MathHelper.Clamp(healValue, 0, 50);
                    player.statLife += healValue;
                    player.HealEffect(healValue);
                    break;
                }
        }
    }

    public static void ShootProjStatic(Projectile projectile, Vector2 unit, bool dying, int AttackCounter, bool Extra)
    {
        if (dying && projectile.timeLeft % 2 == 1) return;
        var player = Main.player[projectile.owner];
        SoundEngine.PlaySound(SoundID.Item74);
        int m = Main.rand.Next(4, 6) - (dying ? 2 : 0) + (Extra ? Main.rand.Next(3) : 0);
        float randAngle = Main.rand.NextFloat(-MathHelper.Pi / 12, MathHelper.Pi / 12);
        for (int n = 0; n < m; n++)
        {
            var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center + 64 * unit, unit.RotatedBy(randAngle + MathHelper.Pi / 3 * (n / (m - 1f) - 0.5f)) * 32,
ModContent.ProjectileType<WoodAttack>(), projectile.damage, projectile.knockBack, projectile.owner, Main.rand.Next(Main.rand.Next(5)), Main.rand.NextFloat(24, 48));
            proj.penetrate = Extra ? 3 : 2;
            proj.tileCollide = Extra && Main.rand.NextBool(2);
            proj.frameCounter = Extra ? 1 : 0;
        }
        if (Extra && player.HasBuff<WoodUltra>() && player.GetZenithTarget(Main.MouseWorld, 512, out int index))
        {
            NPC npc = Main.npc[index];
            for (int n = 0; n < 3; n++)
            {
                int x = Main.rand.Next(-64, 64);
                var num22 = Main.rand.Next(10, 80) * 0.001f;
                var num21 = -x * 0.003f;
                Projectile.NewProjectile(projectile.GetSource_FromThis(), npc.Center + new Vector2(x, 64), new Vector2(0, -8), ModContent.ProjectileType<WoodAttackUltra>(), projectile.damage, projectile.knockBack, projectile.owner, num21, num22);
            }
        }
    }
}