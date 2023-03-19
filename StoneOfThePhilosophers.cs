using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using ReLogic.Content;
using StoneOfThePhilosophers.TestStateBar;
using StoneOfThePhilosophers.UI;
using System.ComponentModel;
using System.IO;
using Terraria;
using Terraria.GameContent.UI.ResourceSets;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace StoneOfThePhilosophers
{
    public class StoneOfThePhilosophers : Mod
    {
        public static Effect VertexDraw => vertexDraw ??= ModContent.Request<Effect>("StoneOfThePhilosophers/Effects/VertexDraw", AssetRequestMode.ImmediateLoad).Value;
        static Effect vertexDraw;
        public static Effect VertexDrawEX => vertexDrawEX ??= ModContent.Request<Effect>("StoneOfThePhilosophers/Effects/VertexDrawEX", AssetRequestMode.ImmediateLoad).Value;
        static Effect vertexDrawEX;

        public static Effect HeatMap => heatMap ??= ModContent.Request<Effect>("StoneOfThePhilosophers/Effects/HeatMapEffect", AssetRequestMode.ImmediateLoad).Value;
        static Effect heatMap;
    }
    public class ColorScreenData : ScreenShaderData
    {
        public ColorScreenData(string passName) : base(passName)
        {
        }
        public ColorScreenData(Ref<Effect> shader, string passName) : base(shader, passName)
        {

        }
        public override void Apply()
        {
            Shader.Parameters["mainColor"].SetValue(new Vector4(0, 1, 0, 1));

        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
    public class ScreenTransformData : ScreenShaderData 
    {
        public ScreenTransformData(string passName) : base(passName)
        {
        }
        public ScreenTransformData(Ref<Effect> shader, string passName) : base(shader, passName)
        {

        }
        public override void Apply()
        {
            float[] m = StoneOfThePhilosophersConfig.instance.Matrix;
            Matrix matrix = new Matrix
                (
                m[0], m[1], m[2], 0,
                m[3], m[4], m[5], 0,
                m[6], m[7], m[8], 0,
                0, 0, 0, 0
                );
            Shader.Parameters["TransformMatrix"].SetValue(matrix);
            //Main.instance.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("StoneOfThePhilosophers/Images/HeatMap_0").Value;
            Shader.Parameters["useHeatMap"].SetValue(true);

        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
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
        public override void PostSetupContent()
        {
            //Main.ResourceSetsManager._sets["EmlementBars"] = new ElementPlayerResourcesDisplaySet("Emlement", "Emlement", "FancyClassic", AssetRequestMode.ImmediateLoad);
            //Main.MinimapFrameManagerInstance.Options["EmlementBars"] = new ElementPlayerResourcesDisplaySet("Emlement", "Emlement", "FancyClassic", AssetRequestMode.ImmediateLoad);
        }
        public override void Load()
        {
            Filters.Scene["StoneOfThePhilosophers:WTFScreen"] = 
                new Filter(new ScreenTransformData(new Ref<Effect>(ModContent.Request<Effect>("StoneOfThePhilosophers/Effects/ScreenTransform", AssetRequestMode.ImmediateLoad).Value),
                "ScreenTransform").UseImage(ModContent.Request<Texture2D>("StoneOfThePhilosophers/Images/HeatMap_0").Value, 1), EffectPriority.Medium);
            Filters.Scene["StoneOfThePhilosophers:WTFScreen"].Load();
            Filters.Scene["StoneOfThePhilosophers:WTFScreen2"] = 
                new Filter(new ColorScreenData(new Ref<Effect>(ModContent.Request<Effect>("StoneOfThePhilosophers/Effects/ColorScreen", AssetRequestMode.ImmediateLoad).Value), "ColorScreen"), EffectPriority.Medium);
            Filters.Scene["StoneOfThePhilosophers:WTFScreen2"].Load();
            Filters.Scene["StoneOfThePhilosophers:WTFScreen3"] =
    new Filter(new ColorScreenData(new Ref<Effect>(ModContent.Request<Effect>("StoneOfThePhilosophers/Effects/ColorScreen", AssetRequestMode.ImmediateLoad).Value), "Magnifier"), EffectPriority.Medium);
            Filters.Scene["StoneOfThePhilosophers:WTFScreen3"].Load();
        }
        public override void PreUpdateEntities()
        {
            ControlScreenShader("StoneOfThePhilosophers:WTFScreen", StoneOfThePhilosophersConfig.instance.UseScreenShader);
            ControlScreenShader("StoneOfThePhilosophers:WTFScreen2", StoneOfThePhilosophersConfig.instance.UseScreenShader2);
            ControlScreenShader("StoneOfThePhilosophers:WTFScreen3", StoneOfThePhilosophersConfig.instance.UseScreenShader3);

        }
        private void ControlScreenShader(string name, bool state)
        {
            if (!Filters.Scene[name].IsActive() && state)
            {
                Filters.Scene.Activate(name);
            }
            if (Filters.Scene[name].IsActive() && !state)
            {
                Filters.Scene.Deactivate(name);
            }
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

        [Label("使用奇妙滤镜")]
        public bool UseScreenShader;

        [Label("奇妙参数")]
        //[Range(-10, 10)]
        public float[] Matrix = new float[] { 1, 0, 0, 0, 1, 0, 0, 0, 1 };

        [Label("使用奇妙滤镜2")]
        public bool UseScreenShader2;

        [Label("使用奇妙滤镜3")]
        public bool UseScreenShader3;
    }
}