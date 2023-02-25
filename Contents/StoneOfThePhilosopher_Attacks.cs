using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using StoneOfThePhilosophers.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace StoneOfThePhilosophers.Contents
{
    public partial class StoneOfThePhilosopherProj : ModProjectile
    {
        public void ShootProj(bool dying = false)
        {
            var combination = ElementCombination;
            bool skiped = false;
        label:
            if (Attacks.TryGetValue(combination, out var attack))
            {
                attack?.Invoke(this, projectile, dying);
            }
            else
            {
                if (!skiped)
                {
                    skiped = true;
                    combination = (combination.element2, combination.element1);
                    goto label;
                }
                else
                {
                    throw new Exception("未知元素组合，请向阿汪反馈");
                }
            }
        }
        public override void Load()
        {
            //var type = GetType();
            //var methods = type.GetMethods();
            //foreach (var attack in methods) 
            //{
            //    if (attack.IsStatic) { }
            //}
            #region 火
            //Attacks.Add((StoneElements.Fire, StoneElements.Fire), FireFire);
            ElementColor.Add(StoneElements.Fire, Color.Red);

            Attacks.Add((StoneElements.Fire, StoneElements.Fire), FireFire);
            Names.Add((StoneElements.Fire, StoneElements.Fire), "火符「火神的辉光」");
            #endregion

            #region 水
            ElementColor.Add(StoneElements.Water, Color.Blue);

            Attacks.Add((StoneElements.Fire, StoneElements.Water), FireWater);
            Names.Add((StoneElements.Fire, StoneElements.Water), "水火「燃素之雨」");

            Attacks.Add((StoneElements.Water, StoneElements.Water), WaterWater);
            Names.Add((StoneElements.Water, StoneElements.Water), "水符「湖葬」");
            #endregion

            #region 木
            ElementColor.Add(StoneElements.Wood, Color.Green);

            Attacks.Add((StoneElements.Fire, StoneElements.Wood), FireWood);
            Names.Add((StoneElements.Fire, StoneElements.Wood), "木火「森林大火」");

            Attacks.Add((StoneElements.Water, StoneElements.Wood), WaterWood);
            Names.Add((StoneElements.Water, StoneElements.Wood), "水木「水精灵」");

            Attacks.Add((StoneElements.Wood, StoneElements.Wood), WoodWood);
            Names.Add((StoneElements.Wood, StoneElements.Wood), "木符「翠绿风暴」");
            #endregion

            #region 金
            ElementColor.Add(StoneElements.Metal, Color.Yellow);

            Attacks.Add((StoneElements.Fire, StoneElements.Metal), FireMetal);
            Names.Add((StoneElements.Fire, StoneElements.Metal), "火金「圣爱尔摩火柱」");

            Attacks.Add((StoneElements.Water, StoneElements.Metal), WaterMetal);
            Names.Add((StoneElements.Water, StoneElements.Metal), "金水「水银之毒」");

            Attacks.Add((StoneElements.Wood, StoneElements.Metal), WoodMetal);
            Names.Add((StoneElements.Wood, StoneElements.Metal), "金木「元素收割者」");

            Attacks.Add((StoneElements.Metal, StoneElements.Metal), MetalMetal);
            Names.Add((StoneElements.Metal, StoneElements.Metal), "金符「银龙」");
            #endregion

            #region 土
            ElementColor.Add(StoneElements.Soil, Color.Orange);

            Attacks.Add((StoneElements.Fire, StoneElements.Soil), FireSoil);
            Names.Add((StoneElements.Fire, StoneElements.Soil), "火土「环状熔岩带」");

            Attacks.Add((StoneElements.Water, StoneElements.Soil), WaterSoil);
            Names.Add((StoneElements.Water, StoneElements.Soil), "土水「诺亚的大洪水」");

            Attacks.Add((StoneElements.Wood, StoneElements.Soil), WoodSoil);
            Names.Add((StoneElements.Wood, StoneElements.Soil), "木土「自然之握」");

            Attacks.Add((StoneElements.Metal, StoneElements.Soil), MetalSoil);
            Names.Add((StoneElements.Metal, StoneElements.Soil), "土金「翡翠巨城」");

            Attacks.Add((StoneElements.Soil, StoneElements.Soil), SoilSoil);
            Names.Add((StoneElements.Soil, StoneElements.Soil), "土符「三石塔之震」");
            #endregion

            #region 月
            ElementColor.Add(StoneElements.Lunar, Color.Purple);

            Attacks.Add((StoneElements.Fire, StoneElements.Lunar), FireLunar);
            Names.Add((StoneElements.Fire, StoneElements.Lunar), "月火「月之秽火」");

            Attacks.Add((StoneElements.Water, StoneElements.Lunar), WaterLunar);
            Names.Add((StoneElements.Water, StoneElements.Lunar), "月水「静月之海」");

            Attacks.Add((StoneElements.Wood, StoneElements.Lunar), WoodLunar);
            Names.Add((StoneElements.Wood, StoneElements.Lunar), "月木「卫星向日葵」");

            Attacks.Add((StoneElements.Metal, StoneElements.Lunar), MetalLunar);
            Names.Add((StoneElements.Metal, StoneElements.Lunar), "月金「卫星监察者」");

            Attacks.Add((StoneElements.Soil, StoneElements.Lunar), SoilLunar);
            Names.Add((StoneElements.Soil, StoneElements.Lunar), "月土「月狂冲击」");

            Attacks.Add((StoneElements.Lunar, StoneElements.Lunar), LunarLunar);
            Names.Add((StoneElements.Lunar, StoneElements.Lunar), "月符「沉静的月神」");
            #endregion

            #region 日
            ElementColor.Add(StoneElements.Sun, Color.White);

            Attacks.Add((StoneElements.Fire, StoneElements.Sun), FireSun);
            Names.Add((StoneElements.Fire, StoneElements.Sun), "日火「希腊火」");

            Attacks.Add((StoneElements.Water, StoneElements.Sun), WaterSun);
            Names.Add((StoneElements.Water, StoneElements.Sun), "日水「氢化日珥」");

            Attacks.Add((StoneElements.Wood, StoneElements.Sun), WoodSun);
            Names.Add((StoneElements.Wood, StoneElements.Sun), "日木「光合作用」");

            Attacks.Add((StoneElements.Metal, StoneElements.Sun), MetalSun);
            Names.Add((StoneElements.Metal, StoneElements.Sun), "日金「天罚剑」");

            Attacks.Add((StoneElements.Soil, StoneElements.Sun), SoilSun);
            Names.Add((StoneElements.Soil, StoneElements.Sun), "日土「陨落天星」");

            Attacks.Add((StoneElements.Lunar, StoneElements.Sun), LunarSun);
            Names.Add((StoneElements.Lunar, StoneElements.Sun), "日月「皇家钻戒」");

            Attacks.Add((StoneElements.Sun, StoneElements.Sun), SunSun);
            Names.Add((StoneElements.Sun, StoneElements.Sun), "日符「皇家圣焰」");
            #endregion

            base.Load();
        }
        public static Dictionary<ElementCombination, ElementAttack> Attacks = new Dictionary<ElementCombination, ElementAttack>();
        public static Dictionary<ElementCombination, string> Names = new Dictionary<ElementCombination, string>();
        public static Dictionary<StoneElements, Color> ElementColor = new Dictionary<StoneElements, Color>();

        #region 火相关组合
        public static void FireFire(StoneOfThePhilosopherProj modproj, Projectile projectile, bool dying)
        {

        }

        #endregion

        #region 水相关组合
        public static void FireWater(StoneOfThePhilosopherProj modproj, Projectile projectile, bool dying)
        {

        }

        public static void WaterWater(StoneOfThePhilosopherProj modproj, Projectile projectile, bool dying)
        {

        }
        #endregion

        #region 木相关组合
        public static void FireWood(StoneOfThePhilosopherProj modproj, Projectile projectile, bool dying)
        {

        }

        public static void WaterWood(StoneOfThePhilosopherProj modproj, Projectile projectile, bool dying)
        {

        }

        public static void WoodWood(StoneOfThePhilosopherProj modproj, Projectile projectile, bool dying)
        {

        }
        #endregion

        #region 金相关组合
        public static void FireMetal(StoneOfThePhilosopherProj modproj, Projectile projectile, bool dying)
        {

        }

        public static void WaterMetal(StoneOfThePhilosopherProj modproj, Projectile projectile, bool dying)
        {

        }
        public static void WoodMetal(StoneOfThePhilosopherProj modproj, Projectile projectile, bool dying)
        {

        }

        public static void MetalMetal(StoneOfThePhilosopherProj modproj, Projectile projectile, bool dying)
        {

        }
        #endregion

        #region 土相关组合
        public static void FireSoil(StoneOfThePhilosopherProj modproj, Projectile projectile, bool dying)
        {

        }

        public static void WaterSoil(StoneOfThePhilosopherProj modproj, Projectile projectile, bool dying)
        {

        }
        public static void WoodSoil(StoneOfThePhilosopherProj modproj, Projectile projectile, bool dying)
        {

        }
        public static void MetalSoil(StoneOfThePhilosopherProj modproj, Projectile projectile, bool dying)
        {

        }

        public static void SoilSoil(StoneOfThePhilosopherProj modproj, Projectile projectile, bool dying)
        {

        }
        #endregion

        #region 月相关组合
        public static void FireLunar(StoneOfThePhilosopherProj modproj, Projectile projectile, bool dying)
        {

        }

        public static void WaterLunar(StoneOfThePhilosopherProj modproj, Projectile projectile, bool dying)
        {

        }
        public static void WoodLunar(StoneOfThePhilosopherProj modproj, Projectile projectile, bool dying)
        {

        }
        public static void MetalLunar(StoneOfThePhilosopherProj modproj, Projectile projectile, bool dying)
        {

        }
        public static void SoilLunar(StoneOfThePhilosopherProj modproj, Projectile projectile, bool dying)
        {

        }

        public static void LunarLunar(StoneOfThePhilosopherProj modproj, Projectile projectile, bool dying)
        {

        }
        #endregion

        #region 日相关组合
        public static void FireSun(StoneOfThePhilosopherProj modproj, Projectile projectile, bool dying)
        {

        }

        public static void WaterSun(StoneOfThePhilosopherProj modproj, Projectile projectile, bool dying)
        {

        }
        public static void WoodSun(StoneOfThePhilosopherProj modproj, Projectile projectile, bool dying)
        {

        }
        public static void MetalSun(StoneOfThePhilosopherProj modproj, Projectile projectile, bool dying)
        {

        }
        public static void SoilSun(StoneOfThePhilosopherProj modproj, Projectile projectile, bool dying)
        {

        }
        public static void LunarSun(StoneOfThePhilosopherProj modproj, Projectile projectile, bool dying)
        {

        }
        public static void SunSun(StoneOfThePhilosopherProj modproj, Projectile projectile, bool dying)
        {

        }
        #endregion
    }
    public struct ElementCombination
    {
        public StoneElements element1;
        public StoneElements element2;
        public static implicit operator ElementCombination((StoneElements, StoneElements) value)
        {
            return new ElementCombination(value);
        }
        public static implicit operator (StoneElements, StoneElements)(ElementCombination value)
        {
            return (value.element1, value.element2);
        }

        public ElementCombination(StoneElements _element1, StoneElements _element2)
        {
            element1 = _element1;
            element2 = _element2;
        }
        public ElementCombination((StoneElements _element1, StoneElements _element2) value)
        {
            element1 = value._element1;
            element2 = value._element2;
        }
        public static bool operator ==(ElementCombination value1, ElementCombination value2)
        {
            if (value1.element1 == value2.element1)
            {
                return value1.element2 == value2.element2;
            }
            else if (value1.element1 == value2.element2) 
            {
                return value1.element2 == value2.element1;
            }
            return false;
        }

        public static bool operator !=(ElementCombination value1, ElementCombination value2)
        {
            return !(value1 == value2);
        }

        public override bool Equals(object obj)
        {
            return obj is ElementCombination combination &&
                   element1 == combination.element1 &&
                   element2 == combination.element2;
        }

        public override int GetHashCode()
        {
            return element1.GetHashCode() + element2.GetHashCode();
        }
    }
    public delegate void ElementAttack(StoneOfThePhilosopherProj modproj, Projectile projectile, bool dying);
}
