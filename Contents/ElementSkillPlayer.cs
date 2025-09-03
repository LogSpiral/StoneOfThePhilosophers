using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace StoneOfThePhilosophers.Contents;

public class ElementSkillPlayer : ModPlayer
{
    /// <summary>
    /// 和那边的枚举类型对应，当然，你得把Empty踢开
    /// 最大值100，最小0
    /// </summary>
    public float[] ElementChargeValue = new float[7];

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

    public override void ResetEffects()
    {
        for (int n = 0; n < 7; n++)
        {
            ElementChargeValue[n] = MathHelper.Clamp(ElementChargeValue[n], 0, 100);
        }
        EarthQuaking = false;
        base.ResetEffects();
    }

    /// <summary>
    /// 当前某元素所使用的技能的编号
    /// </summary>
    public int[] skillIndex = [1, 1, 1, 1, 1, 1, 1];

    /// <summary>
    /// 符卡名，快乐打表
    /// </summary>
    public static string GetSkillName(int idx, int idy)
    {
        string prefix = "Mods.StoneOfThePhilosophers.Items.";
        string elementName = idx switch
        {
            0 => "Fire",
            1 => "Water",
            2 => "Wood",
            3 => "Metal",
            4 => "Earth",
            5 => "Moon",
            6 => "Sun",
            _ => ""
        };
        return $"{Language.GetTextValue(prefix + "SpellPrefixs." + elementName)}「{Language.GetTextValue(prefix + "SpellNames." + elementName + idy)}」";
    }

    /// <summary>
    /// 元素消耗打表
    /// </summary>
    private static float[,] skillCost = new float[7, 3];

    public static int[] skillCounts = [1, 2, 3, 1, 2, 1, 1];

    public override void Load()
    {
        for (int n = 0; n < 7; n++)
        {
            for (int i = 0; i < skillCounts[n]; i++)
            {
                int SpellCost = n switch
                {
                    0 => 1,
                    1 => i switch { 0 => 1, 1 or _ => 2 },
                    2 => i switch { 0 => 1, 1 => 3, 2 or _ => 5 },
                    3 => 1,
                    4 => i switch { 0 => 1, 1 or _ => 4 },
                    5 => 5,
                    6 or _ => 5
                };
                skillCost[n, i] = SpellCost * 20;
            }
        }
    }

    /// <summary>
    /// 输入元素下标，也就是(int){StoneElements} - 1
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public float GetElementCost(int index) => skillCost[index, skillIndex[index] - 1];
}