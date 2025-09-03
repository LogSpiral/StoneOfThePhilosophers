using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace StoneOfThePhilosophers.Contents.Moon;

public class StoneOfMoonProj : MagicArea
{
    protected override StoneElements Elements => StoneElements.Lunar;
    protected override int Cycle => (int)((Main.dayTime ? 18 : 12) * (player.HasBuff<BlessingFromLunarGod>() ? 0.75f : 1f));

    public override void SpecialAttack(bool trigger) => SpecialAttackStatic(Projectile, trigger, SpecialAttackIndex);

    public override void ShootProj(Vector2 unit, bool dying = false) => ShootProjStatic(Projectile, unit, dying, AttackCounter, Extra);

    public static void SpecialAttackStatic(Projectile projectile, bool trigger, int SpecialAttackIndex)
    {
        if (trigger)
            Main.player[projectile.owner].AddBuff(ModContent.BuffType<BlessingFromLunarGod>(), 1200);
    }

    public static void ShootProjStatic(Projectile projectile, Vector2 unit, bool dying, int AttackCounter, bool Extra)
    {
        var player = Main.player[projectile.owner];
        float r = Main.rand.Next(-32, 32) * (dying ? 8f : 1f);
        int randX = Main.rand.Next(-256, 256);//Main.rand.Next(-64, 64);
        var v = new Vector2(randX, -Main.rand.Next(280, 560));
        var flag = player.HasBuff<BlessingFromLunarGod>();
        var center = Main.MouseWorld + new Vector2(r, 0);
        if (flag && (!dying || Main.rand.NextBool(2)) && player.GetZenithTarget(Main.MouseWorld, 1024f, out var index))
        {
            center = Main.npc[index].Center + Main.npc[index].velocity * 15;
        }
        Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), center - v, default,
            ModContent.ProjectileType<MoonAttack>(), flag ? (int)(projectile.damage * 5 / 4) : projectile.damage,
            projectile.knockBack, projectile.owner, v.ToRotation(), v.Length() * 2, flag ? 1 : 0);
    }
}