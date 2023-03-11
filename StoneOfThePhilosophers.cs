using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.ComponentModel;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace StoneOfThePhilosophers
{
    public class StoneOfThePhilosophers : Mod
    {
        public static Effect VertexDraw => vertexDraw ??= ModContent.Request<Effect>("StoneOfThePhilosophers/Effects/VertexDraw", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
        static Effect vertexDraw;
        public static Effect VertexDrawEX => vertexDrawEX ??= ModContent.Request<Effect>("StoneOfThePhilosophers/Effects/VertexDrawEX", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
        static Effect vertexDrawEX;
    }
    public class StoneOfThePhilosophersSystem : ModSystem
    {
        public const string IronLeadOres = "StoneOfThePhilosophers:IronLeadOres";
        public const string GoldPlatinumOres = "StoneOfThePhilosophers:GoldPlatinumOres";
        public const string CobaltPalladiumBars = "StoneOfThePhilosophers:CobaltPalladiumBars";

        public const string MythrilOrichalcumBars = "StoneOfThePhilosophers:MythrilOrichalcumBars";
        public const string AdamantiteTitaniumBars = "StoneOfThePhilosophers:AdamantiteTitaniumBars";
        public const string CursedIchorFlame = "StoneOfThePhilosophers:CursedIchorFlame";


        public override void AddRecipeGroups()
        {
            RecipeGroup group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " 铁铅矿", new int[]
            {
                ItemID.IronOre,
                ItemID.LeadOre
            });
            RecipeGroup.RegisterGroup(IronLeadOres, group);
            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " 金铂矿", new int[]
            {
                ItemID.GoldOre,
                ItemID.PlatinumOre
            });
            RecipeGroup.RegisterGroup(GoldPlatinumOres, group);
            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " 钴钯金锭", new int[]
            {
                ItemID.CobaltBar,
                ItemID.PalladiumBar
            });
            RecipeGroup.RegisterGroup(CobaltPalladiumBars, group);
            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " 秘山铜锭", new int[]
            {
                ItemID.MythrilBar,
                ItemID.OrichalcumBar
            });
            RecipeGroup.RegisterGroup(MythrilOrichalcumBars, group);
            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " 精钛金锭", new int[]
            {
                ItemID.AdamantiteBar,
                ItemID.TitaniumBar
            });
            RecipeGroup.RegisterGroup(AdamantiteTitaniumBars, group);
            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " 咒火灵液", new int[]
            {
                ItemID.CursedFlame,
                ItemID.Ichor
            });
            RecipeGroup.RegisterGroup(CursedIchorFlame, group);
        }
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