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

        [Label("����Ԫ���ں�")]
        [Tooltip("Ĭ���ǿ����ģ�����Ҳ����Щ��ϲ���Լ��ֶ�������ںϡ�")]
        [DefaultValue(true)]
        public bool combineFaster;
        [JsonIgnore]
        public static StoneOfThePhilosophersConfig instance => ModContent.GetInstance<StoneOfThePhilosophersConfig>();
        [JsonIgnore]
        public static bool CombineFaster => instance?.combineFaster ?? false;
    }
}