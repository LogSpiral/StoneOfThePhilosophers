using Microsoft.Xna.Framework;
using StoneOfThePhilosophers.Contents.Fire;
using StoneOfThePhilosophers.Contents.Water;
using StoneOfThePhilosophers.Contents.Metal;
using StoneOfThePhilosophers.Contents.Wood;
using StoneOfThePhilosophers.Contents.Earth;
using StoneOfThePhilosophers.Contents.Sun;
using StoneOfThePhilosophers.Contents.Moon;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Microsoft.CodeAnalysis;
using StoneOfThePhilosophers.Contents.Philosopher.Attacks;

namespace StoneOfThePhilosophers.Contents.Philosopher;

public partial class StoneOfThePhilosopherProj : ModProjectile
{
    public void HandleSpecialAttack(StoneElements element)
    {
        bool trigger = Timer == 55;
        switch (element)
        {
            case StoneElements.Fire:
                StoneOfFireProj.SpecialAttackStatic(Projectile, trigger, SpecialAttackIndex);
                break;
            case StoneElements.Water:
                StoneOfWaterProj.SpecialAttackStatic(Projectile, trigger, SpecialAttackIndex);
                break;
            case StoneElements.Wood:
                StoneOfWoodProj.SpecialAttackStatic(Projectile, trigger, SpecialAttackIndex);
                break;
            case StoneElements.Metal:
                StoneOfMetalProj.SpecialAttackStatic(Projectile, trigger, SpecialAttackIndex);
                break;
            case StoneElements.Soil:
                StoneOfEarthProj.SpecialAttackStatic(Projectile, trigger, SpecialAttackIndex);
                break;
            case StoneElements.Lunar:
                StoneOfMoonProj.SpecialAttackStatic(Projectile, trigger, SpecialAttackIndex);
                break;
            case StoneElements.Solar:
                StoneOfSunProj.SpecialAttackStatic(Projectile, trigger, SpecialAttackIndex);
                break;
        }
    }
    public void ShootProj(ElementCombination combination, bool dying = false)
    {
        int state = combination.Mode;
        Vector2 unit = projectile.velocity.SafeNormalize(default);
        switch (state)
        {
            case 1:
                {
                    AttackCounter++;
                    switch (combination.MainElements)
                    {
                        case StoneElements.Fire:
                            StoneOfFireProj.ShootProjStatic(Projectile, unit, dying, AttackCounter, true);
                            break;
                        case StoneElements.Water:
                            StoneOfWaterProj.ShootProjStatic(Projectile, unit, dying, AttackCounter, true);
                            break;
                        case StoneElements.Wood:
                            StoneOfWoodProj.ShootProjStatic(Projectile, unit, dying, AttackCounter, true);
                            break;
                        case StoneElements.Metal:
                            StoneOfMetalProj.ShootProjStatic(Projectile, unit, dying, AttackCounter, true);
                            break;
                        case StoneElements.Soil:
                            StoneOfEarthProj.ShootProjStatic(Projectile, unit, dying, AttackCounter, true);
                            break;
                        case StoneElements.Lunar:
                            StoneOfMoonProj.ShootProjStatic(Projectile, unit, dying, AttackCounter, true);
                            break;
                        case StoneElements.Solar:
                            StoneOfSunProj.ShootProjStatic(Projectile, unit, dying, AttackCounter, true);
                            break;
                    }
                    break;
                }
            case 2:
                {
                    if (dying) return;  
                    switch (combination.MainElements) 
                    {
                        case StoneElements.Fire:
                            // 不可能的情况
                            break;
                        case StoneElements.Water:
                            /*
                            switch (combination.CoElements)
                            {
                                case StoneElements.Fire:
                                    WaterFire();
                                    break;
                            }
                            */
                            // 只可能是这个了
                            WaterFire();
                            break;

                        case StoneElements.Wood:
                            switch (combination.CoElements)
                            {
                                case StoneElements.Fire:
                                    WoodFire();
                                    break;
                                case StoneElements.Water:
                                    WoodWater();
                                    break;
                            }
                            break;
                        case StoneElements.Metal:
                            switch (combination.CoElements)
                            {
                                case StoneElements.Fire:
                                    MetalFire();
                                    break;
                                case StoneElements.Water:
                                    MetalWater();
                                    break;
                                case StoneElements.Wood:
                                    MetalWood();
                                    break;
                            }
                            break;
                        case StoneElements.Soil:
                            switch (combination.CoElements)
                            {
                                case StoneElements.Fire:
                                    SoilFire();
                                    break;
                                case StoneElements.Water:
                                    SoilWater();
                                    break;
                                case StoneElements.Wood:
                                    SoilWood();
                                    break;
                                case StoneElements.Metal:
                                    SoilMetal();
                                    break;
                            }
                            break;
                        case StoneElements.Lunar:
                            switch (combination.CoElements)
                            {
                                case StoneElements.Fire:
                                    LunarFire();
                                    break;
                                case StoneElements.Water:
                                    LunarWater();
                                    break;
                                case StoneElements.Wood:
                                    LunarWood();
                                    break;
                                case StoneElements.Metal:
                                    LunarMetal();
                                    break;
                                case StoneElements.Soil:
                                    LunarSoil();
                                    break;
                            }
                            break;
                        case StoneElements.Solar:
                            switch (combination.CoElements)
                            {
                                case StoneElements.Fire:
                                    SolarFire();
                                    break;
                                case StoneElements.Water:
                                    SolarWater();
                                    break;
                                case StoneElements.Wood:
                                    SolarWood();
                                    break;
                                case StoneElements.Metal:
                                    SolarMetal();
                                    break;
                                case StoneElements.Soil:
                                    SolarSoil();
                                    break;
                                case StoneElements.Lunar:
                                    SolarLunar();
                                    break;
                            }
                            break;
                    }
                    break;
                }
            default:
            case 0:
                {
                    Main.NewText("哇是元素大融合，可惜笨蛋螺线没做");
                    break;
                }
        }
    }
    public override void Load()
    {
        elementColor.Add(StoneElements.Fire, Color.Red);
        elementColor.Add(StoneElements.Water, Color.Blue);
        elementColor.Add(StoneElements.Wood, Color.Green);
        elementColor.Add(StoneElements.Metal, Color.Yellow);
        elementColor.Add(StoneElements.Soil, Color.Orange);
        elementColor.Add(StoneElements.Lunar, Color.Purple);
        elementColor.Add(StoneElements.Solar, Color.White);
        base.Load();
    }
    static readonly Dictionary<StoneElements, Color> elementColor = [];
    public static IReadOnlyDictionary<StoneElements, Color> ElementColor => elementColor;

    #region 火相关组合
    // 不存在的

    #endregion

    #region 水相关组合
    public void WaterFire()
    {
        Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center, default, ModContent.ProjectileType<WaterFireRain>(),
            projectile.damage, projectile.knockBack, projectile.owner);
    }
    #endregion

    #region 木相关组合
    public void WoodFire()
    {
        Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center, default, ModContent.ProjectileType<WoodFireHandler>(),
projectile.damage, projectile.knockBack, projectile.owner);

    }

    public void WoodWater()
    {

    }
    #endregion

    #region 金相关组合
    public void MetalFire()
    {
        Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center,Vector2.UnitX * 4, ModContent.ProjectileType<MetalFireHandler>(),
    projectile.damage, projectile.knockBack, projectile.owner);
        Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center, Vector2.UnitX * -4, ModContent.ProjectileType<MetalFireHandler>(),
projectile.damage, projectile.knockBack, projectile.owner);
    }

    public void MetalWater()
    {

    }
    public void MetalWood()
    {

    }
    #endregion

    #region 土相关组合
    public void SoilFire()
    {
        Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center, (Main.MouseWorld - player.MountedCenter).SafeNormalize(default), ModContent.ProjectileType<SoilFireZone>(),
projectile.damage, projectile.knockBack, projectile.owner);
    }

    public void SoilWater()
    {

    }
    public void SoilWood()
    {

    }
    public void SoilMetal()
    {

    }
    #endregion

    #region 月相关组合
    public void LunarFire()
    {
        Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center, default, ModContent.ProjectileType<LunarFireTorch>(),
    projectile.damage, projectile.knockBack, projectile.owner);
    }

    public void LunarWater()
    {

    }
    public void LunarWood()
    {

    }
    public void LunarMetal()
    {

    }
    public void LunarSoil()
    {

    }


    #endregion

    #region 日相关组合
    public void SolarFire()
    {
        Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center, default, ModContent.ProjectileType<SolarFireZone>(),
projectile.damage, projectile.knockBack, projectile.owner);
    }

    public void SolarWater()
    {

    }
    public void SolarWood()
    {

    }
    public void SolarMetal()
    {

    }
    public void SolarSoil()
    {

    }
    public void SolarLunar()
    {

    }
    #endregion
}
