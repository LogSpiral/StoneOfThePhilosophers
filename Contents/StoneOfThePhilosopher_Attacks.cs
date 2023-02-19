using StoneOfThePhilosophers.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

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
                    combination = (combination.Item2, combination.Item1);
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
            Attacks.Add((StoneElements.Fire, StoneElements.Fire), FireFire);
            Names.Add((StoneElements.Fire, StoneElements.Fire), "火符「火神的辉光」");
            #endregion

            #region 水
            Attacks.Add((StoneElements.Fire, StoneElements.Water), FireWater);
            Names.Add((StoneElements.Fire, StoneElements.Water), "水火「燃素之雨」");

            Attacks.Add((StoneElements.Water, StoneElements.Water), WaterWater);
            Names.Add((StoneElements.Water, StoneElements.Water), "水符「湖葬」");
            #endregion

            #region 木
            Attacks.Add((StoneElements.Fire, StoneElements.Wood), FireWood);
            Names.Add((StoneElements.Fire, StoneElements.Wood), "木火「森林大火」");

            Attacks.Add((StoneElements.Water, StoneElements.Wood), WaterWood);
            Names.Add((StoneElements.Water, StoneElements.Wood), "水木「水精灵」");

            Attacks.Add((StoneElements.Wood, StoneElements.Wood), WoodWood);
            Names.Add((StoneElements.Wood, StoneElements.Wood), "木符「翠绿风暴」");
            #endregion

            #region 金
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
        public Dictionary<(StoneElements, StoneElements), ElementAttack> Attacks;
        public Dictionary<(StoneElements, StoneElements), string> Names;

        #region 火相关组合
        public static void FireFire(StoneOfThePhilosopherProj modproj, Projectile projectile, bool dying)
        {

        }
        public static void FireWater(StoneOfThePhilosopherProj modproj, Projectile projectile, bool dying)
        {

        }
        #endregion

        #region 水相关组合

        #endregion

        #region 木相关组合

        #endregion

        #region 金相关组合

        #endregion

        #region 土相关组合

        #endregion

        #region 月相关组合

        #endregion

        #region 日相关组合

        #endregion
    }
    public delegate void ElementAttack(StoneOfThePhilosopherProj modproj, Projectile projectile, bool dying);
}
