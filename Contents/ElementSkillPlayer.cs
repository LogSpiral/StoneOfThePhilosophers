using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using StoneOfThePhilosophers.UI;
namespace StoneOfThePhilosophers.Contents;

public class ElementSkillPlayer : ModPlayer
{
    /// <summary>
    /// 和那边的枚举类型对应，当然，你得把Empty踢开
    /// 最大值100，最小0
    /// </summary>
    public float[] ElementChargeValue = new float[7];
    public override void ResetEffects()
    {
        for (int n = 0; n < 7; n++)
        {
            ElementChargeValue[n] = MathHelper.Clamp(ElementChargeValue[n], 0, 100);
        }
        EarthQuaking = false;
        base.ResetEffects();
    }
    public static float Devider => 200f;
    public static bool EarthQuaking;
    public float strengthOfShake;
    public override void ModifyScreenPosition()
    {
        strengthOfShake *= 0.8f;
        if (strengthOfShake < 0.025f) strengthOfShake = 0;
        Main.screenPosition += Main.rand.NextVector2Unit() * strengthOfShake * 48;
        base.ModifyScreenPosition();
    }


    /// <summary>
    /// 输入元素下标，输出技能下标
    /// </summary>
    public int[] skillIndex = new int[7];
    /// <summary>
    /// 符卡名，快乐打表
    /// </summary>
    public static string[,] skillName = new string[7, 3];
    /// <summary>
    /// 元素消耗打表
    /// </summary>
    public static float[,] skillCost = new float[7, 3];
    /// <summary>
    /// 方便访问，仅此而已
    /// </summary>
    public float[,] SkillCost => skillCost;
    public static int[] skillCounts = [1, 2, 3, 1, 2, 1, 1];
    public override void Load()
    {
        for (int n = 0; n < 7; n++)
        {
            string ElementName = n switch
            {
                0 => "焱火",
                1 => "淼水",
                2 => "森木",
                3 => "鑫金",
                4 => "垚土",
                5 => "寒月",
                6 or _ => "炎日"
            };
            for (int i = 0; i < skillCounts[n]; i++)
            {
                string SpellName = n switch
                {
                    0 => "灼炎炼狱",
                    1 => i switch { 0 => "穿石之流", 1 or _ => "潮汐领域" },
                    2 => i switch { 0 => "常青藤鞭", 1 => "巨木之晶", 2 or _ => "愈伤组织" },
                    3 => "钢铁洪流",
                    4 => i switch { 0 => "大地之柱", 1 or _ => "山崩地裂" },
                    5 => "月影降临",
                    6 or _ => "歌未竟"
                };
                int SpellCost = n switch
                {
                    0 => 1,
                    1 => i switch { 0 => 1, 1 or _ => 2 },
                    2 => i switch { 0 => 1, 1 => 3, 2 or _ => 5 },
                    3 => 1,
                    4 => i switch { 0 => 1, 1 or _ => 4 },
                    5 => 1,
                    6 or _ => 1
                };
                skillName[n, i] = $"{ElementName}「{SpellName}」";
                skillCost[n, i] = SpellCost * 20;
            }
        }
    }

    /// <summary>
    /// 输入元素下标，也就是(int){StoneElements} - 1
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public float GetElementCost(int index) => SkillCost[index, skillIndex[index]];

    /// <summary>
    /// 输入元素枚举 似乎用不到的样子
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public float GetElementCost(StoneElements element) => element == 0 ? 0 : GetElementCost((int)element - 1);
}