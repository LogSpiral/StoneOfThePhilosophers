using StoneOfThePhilosophers.UI;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
namespace StoneOfThePhilosophers.Contents.Moon;
public class StoneOfMoonProj : MagicArea
{
    public override StoneElements Elements => StoneElements.Lunar;
    public override int Cycle => (int)((Main.dayTime ? 18 : 12) * (player.HasBuff<BlessingFromLunarGod>() ? 0.75f : 1f));//12
    public override Color MainColor => Color.Purple;
    public override void SpecialAttack(Color dustColor, bool trigger)
    {
        if (trigger)
        {
            player.AddBuff(ModContent.BuffType<BlessingFromLunarGod>(), 450);

        }
        base.SpecialAttack(dustColor, trigger);
    }
    public override void ShootProj(Vector2 unit, bool dying = false)
    {
        attackCounter++;
        float r = Main.rand.Next(-32, 32) * (dying ? 8f : 1f);
        int randX = Main.rand.Next(-256, 256);//Main.rand.Next(-64, 64);
        var v = new Vector2(randX, -Main.rand.Next(280, 560));
        var flag = player.HasBuff<BlessingFromLunarGod>();
        var center = Main.MouseWorld + new Vector2(r, 0);
        if (flag && (!dying || Main.rand.NextBool(2)) && player.GetZenithTarget(Main.MouseWorld, 1024f, out var index))
        {
            center = Main.npc[index].Center + Main.npc[index].velocity * 15;
        }
        var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), center - v, default, ModContent.ProjectileType<MoonAttack>(), flag ? (int)(projectile.damage * 5 / 4) : projectile.damage, projectile.knockBack, projectile.owner, v.ToRotation(), v.Length() * 2);
        (proj.ModProjectile as MoonAttack).boost = flag;
    }
}
