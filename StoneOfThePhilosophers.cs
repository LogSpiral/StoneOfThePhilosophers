using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
namespace StoneOfThePhilosophers;

public class StoneOfThePhilosophers : Mod
{
    public static Effect HeatMap => ModAsset.HeatMapEffect.Value;
}
public class StoneOfThePhilosophersConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ClientSide;

    [DefaultValue(true)]
    public bool combineFaster;

    public static StoneOfThePhilosophersConfig Instance { get; private set; }

    public override void OnLoaded()
    {
        Instance = this;
        base.OnLoaded();
    }

    public static bool CombineFaster => Instance?.combineFaster ?? false;
}