using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.ComponentModel;
using System.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace StoneOfThePhilosophers
{
    public class StoneOfThePhilosophers : Mod
    {
        public static Effect VertexDraw => vertexDraw ??= ModContent.Request<Effect>("StoneOfThePhilosophers/Effects/VertexDraw", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
        static Effect vertexDraw;

    }
    public class StoneOfThePhilosophersConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Label("快速元素融合")]
        [Tooltip("默认是开启的，但是也许有些人喜欢自己手动点击「融合」")]
        [DefaultValue(true)]
        public bool combineFaster;
        [JsonIgnore]
        public static StoneOfThePhilosophersConfig instance => ModContent.GetInstance<StoneOfThePhilosophersConfig>();
        [JsonIgnore]
        public static bool CombineFaster => instance?.combineFaster ?? false;
    }
}