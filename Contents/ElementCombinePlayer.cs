using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace StoneOfThePhilosophers.Contents;

public class ElementCombination
{
    public StoneElements MainElements { get; private set; }

    public StoneElements CoElements { get; private set; }

    public StoneElements RealCoElements => CoElements == StoneElements.Empty ? MainElements : CoElements;

    public int Mode
    {
        get
        {
            if (MainElements == 0) // 如果主元素为0说明副元素一定也为0
                return 0; // 采用多元素融合模式
            if (CoElements == 0) // 如果副元素为0则只有主元素，采用单元素模式
                return 1;
            return 2; // 否则是双元素模式
        }
    }

    public void SetElement(StoneElements element1, StoneElements element2)
    {
        if ((int)element1 > (int)element2)
        {
            MainElements = element1;
            CoElements = element2;
        }
        else
        {
            MainElements = element2;
            CoElements = element1;
        }
    }
}

public class ElementCombinePlayer : ModPlayer
{
    public ElementCombination Combination { get; } = new();

    public ElementCombination CombinationEX { get; } = new();

    public override void SaveData(TagCompound tag)
    {
        tag.Add("element1", (byte)Combination.MainElements);
        tag.Add("element2", (byte)Combination.CoElements);

        tag.Add("element3", (byte)Combination.MainElements);
        tag.Add("element4", (byte)Combination.CoElements);
    }

    public override void LoadData(TagCompound tag)
    {
        Combination.SetElement(
            (StoneElements)tag.GetByte("element1"),
            (StoneElements)tag.GetByte("element2")
            );
        CombinationEX.SetElement(
            (StoneElements)tag.GetByte("element3"),
            (StoneElements)tag.GetByte("element4")
            );
    }
}

public enum StoneElements
{
    Empty,
    Fire,
    Water,
    Wood,
    Metal,
    Soil,
    Lunar,
    Solar
}