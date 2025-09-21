using LogSpiralLibrary;
using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing.RenderDrawingContents;
using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing.RenderDrawingEffects;
using LogSpiralLibrary.CodeLibrary.Utilties.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SilkyUIFramework;
using SilkyUIFramework.Attributes;
using SilkyUIFramework.Elements;
using SilkyUIFramework.Extensions;
using StoneOfThePhilosophers.Contents;
using StoneOfThePhilosophers.Contents.Philosopher;
using System.Collections.Generic;
using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace StoneOfThePhilosophers.UI;

[RegisterUI("Vanilla: Radial Hotbars", $"{nameof(StoneOfThePhilosophers)}: {nameof(ElementCombineUI)}")]
public class ElementCombineUI : BaseBody
{
    public static ElementCombineUI Instance { get; private set; }

    public ElementCombinePanel ElementCombinePanelNeo { get; private set; }

    private static bool Active;

    public override bool Enabled { get => Active || timer > 0; set => Active = value; }

    public static bool IsExtra;

    private static readonly AirDistortEffect airDistortEffect = new(8, 1.5f, 0, .5f);
    private static readonly BloomEffect bloomEffect = new(0, 1.15f, 1, 3, true, 2, true);
    private static readonly IRenderEffect[][] renderInfos = [[airDistortEffect], [bloomEffect]];
    internal const string CanvasName = $"{nameof(StoneOfThePhilosophers)}: {nameof(ElementCombineUI)}";

    protected override void OnInitialize()
    {
        Instance = this;
        FitWidth = true;
        FitHeight = true;
        ElementCombinePanelNeo = new ElementCombinePanel(this);
        ElementCombinePanelNeo.SetSize(256, 320);
        ElementCombinePanelNeo.Join(this);
        BorderColor = Color.Transparent;

        RenderCanvasSystem.RegisterCanvasFactory(CanvasName, () => new RenderingCanvas(renderInfos));
    }

    public static void Open(int itemID)
    {
        Instance.ElementCombinePanelNeo.SetSize(256, 320);
        Instance.ElementCombinePanelNeo.Reset();
        Instance.DragOffset = default;
        Active = true;
        SoundEngine.PlaySound(SoundID.MenuOpen);
        Instance.itemType = itemID;
        var targetVector = Main.MouseScreen / Main.ScreenSize.ToVector2();
        targetVector -= new Vector2(.5f);
        targetVector *= Main.GameZoomTarget * Main.ForcedMinimumZoom;
        targetVector += new Vector2(.5f);
        Instance.SetLeft(0, targetVector.X - .5f, .5f);
        Instance.SetTop(0, targetVector.Y - .5f, .5f);
    }

    public static void Close()
    {
        SoundEngine.PlaySound(SoundID.MenuClose);
        Active = false;
    }

    private int timer;
    private int itemType;

    protected override void UpdateStatus(GameTime gameTime)
    {
        timer += Active ? 1 : -1;
        timer = MathHelper.Clamp(timer, 0, 15);
        ElementCombinePanelNeo.Factor = MathHelper.SmoothStep(0, 1, timer / 15f);
        if (Main.LocalPlayer.HeldItem.type != itemType && Active) Close();
        base.UpdateStatus(gameTime);
    }

    protected override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        return;
        base.Draw(gameTime, spriteBatch);
        //return;
    }
}

public class ElementCombinePanel : SUIDraggableView
{
    public List<ElementButton> Buttons = [];
    public float Factor { set; get; }
    private StoneElements element1;
    private StoneElements element2;
    public UITextView CombineButton;

    public ElementCombinePanel(UIElementGroup controlTarget) : base(controlTarget)
    {
        CombineButton = new UITextView
        {
            Text = "确定！"
        };
        CombineButton.SetLeft(0, 0, .5f);
        CombineButton.SetTop(0, 0, .95f);
        CombineButton.TextColor = Color.LightYellow;
        CombineButton.MouseEnter += delegate
        {
            CombineButton.TextColor = Color.Yellow;
            SoundEngine.PlaySound(SoundID.MenuTick);
        };
        CombineButton.MouseLeave += delegate
        {
            CombineButton.TextColor = Color.LightYellow;
        };
        CombineButton.LeftMouseClick += delegate
        {
            var elementPlr = Main.LocalPlayer.GetModPlayer<ElementCombinePlayer>();
            if (ElementCombineUI.IsExtra)
                elementPlr.CombinationEX.SetElement(element1, element2);
            else
                elementPlr.Combination.SetElement(element1, element2);

            ElementCombineUI.Close();
            ElementSkillUI.Close();
            SoundEngine.PlaySound(SoundID.Research);
            SoundEngine.PlaySound(SoundID.ResearchComplete);

            if (element1 != 0)
            {
                var dummyelement = element2;
                if (dummyelement == 0)
                    dummyelement = element1;
                for (int n = 0; n < 30; n++)
                {
                    MiscMethods.FastDust(Main.LocalPlayer.Center,
                        Main.rand.NextVector2Unit() * Main.rand.NextFloat(1, 16),
                        Main.rand.Next([StoneOfThePhilosopherProj.ElementColor[element1],
                                        StoneOfThePhilosopherProj.ElementColor[dummyelement]]),
                        Main.rand.NextFloat(.75f, 2f));
                }
                bool flag = Main.rand.NextBool();
                var swoosh1 = UltraSwoosh.NewUltraSwoosh(ElementCombineUI.CanvasName, 15, flag ? 90 : 120, Main.LocalPlayer.Center, (0, 4));
                LogSpiralLibraryMod.SetTempHeatMap(0, [(Color.Black, 0), (StoneOfThePhilosopherProj.ElementColor[element1], 1f)]);
                swoosh1.heatMap = LogSpiralLibraryMod.TempHeatMaps[0];
                swoosh1.ColorVector = new Vector3(0, 0, 1);
                swoosh1.rotation = Main.rand.NextFloat(0, MathHelper.TwoPi);
                swoosh1.xScaler = Main.rand.NextFloat(2, 3);
                swoosh1.baseTexIndex = 0;

                var swoosh2 = UltraSwoosh.NewUltraSwoosh(ElementCombineUI.CanvasName, 15, flag ? 120 : 90, Main.LocalPlayer.Center, (0, 4));
                LogSpiralLibraryMod.SetTempHeatMap(1, [(Color.Black, 0), (StoneOfThePhilosopherProj.ElementColor[dummyelement], 1f)]);
                swoosh2.heatMap = LogSpiralLibraryMod.TempHeatMaps[1];
                swoosh2.ColorVector = new Vector3(0, 0, 1);
                swoosh2.rotation = Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4) + swoosh1.rotation + MathHelper.PiOver2;
                swoosh2.xScaler = Main.rand.NextFloat(2, 3);
                swoosh2.baseTexIndex = 0;

                if ((int)element1 < (int)dummyelement)
                    Utils.Swap(ref element1, ref dummyelement);

                //Main.NewText(Language.GetTextValue($"Mods.StoneOfThePhilosophers.Spells.{element1}{dummyelement}"));
            }
            else
            {
                int m = ElementCombineUI.IsExtra ? 7 : 5;
                for (int n = 0; n < m; n++)
                {
                    var color = StoneOfThePhilosopherProj.ElementColor[(StoneElements)(n + 1)];
                    for (int k = 0; k < 10; k++)
                    {
                        MiscMethods.FastDust(Main.LocalPlayer.Center,
                            (MathHelper.TwoPi / m * (n + Main.rand.NextFloat())).ToRotationVector2() * Main.rand.NextFloat(1, 16), color,
                            Main.rand.NextFloat(.75f, 2f));
                    }

                    var swoosh = UltraSwoosh.NewUltraSwoosh(ElementCombineUI.CanvasName, 30, Main.rand.NextFloat(90, 180), Main.LocalPlayer.Center, (0, 4));
                    LogSpiralLibraryMod.SetTempHeatMap(n, [(Color.Black, 0), (color, 1f)]);
                    swoosh.heatMap = LogSpiralLibraryMod.TempHeatMaps[n];
                    swoosh.ColorVector = new Vector3(0, 0, 1f);
                    swoosh.rotation = MathHelper.TwoPi / m * n;
                    swoosh.center += swoosh.rotation.ToRotationVector2() * Main.rand.NextFloat(0, 90);
                    swoosh.xScaler = Main.rand.NextFloat(2, 3);
                }
            }
        };
        CombineButton.Positioning = Positioning.Absolute;
    }

    public override void HandleDraw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        var graphicDevice = Main.instance.GraphicsDevice;
        SamplerState samplerState = graphicDevice.SamplerStates[0];
        DepthStencilState depthState = graphicDevice.DepthStencilState;
        RasterizerState rasterizerState = graphicDevice.RasterizerState;
        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.AnisotropicClamp, depthState, rasterizerState, null, Main.UIScaleMatrix);

        var panelTexture = ModContent.Request<Texture2D>("StoneOfThePhilosophers/UI/ElementPanel").Value;
        Vector2 center = Bounds.Position + new Vector2(Bounds.Width * .5f, Bounds.Height * .4f);
        spriteBatch.Draw(panelTexture, center, null, Color.White, MathHelper.Pi, new Vector2(118), 1f * Factor, 0, 0);
        spriteBatch.Draw(panelTexture, center, null, Color.White * .5f, Main.GlobalTimeWrappedHourly, new Vector2(118), 1.5f * Factor, 0, 0);
        spriteBatch.Draw(panelTexture, center, null, Color.White * .75f, -Main.GlobalTimeWrappedHourly * 2, new Vector2(118), 1.25f * Factor, 0, 0);

        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, samplerState, depthState, rasterizerState, null, Main.UIScaleMatrix);
        //CombineButton.SetSize(80,40);
        DrawChildren(gameTime, spriteBatch);
    }

    private void SetUpElementList()
    {
        var elementSkillPlr = Main.LocalPlayer.GetModPlayer<ElementSkillPlayer>();
        Buttons.Clear();
        Elements.Clear();
        CombineButton.Join(this);
        CombineButton.SetTop(0, 0.45f, 0.5f);
        CombineButton.Text = "确定\n" + Language.GetTextValue($"Mods.StoneOfThePhilosophers.Spells.{(ElementCombineUI.IsExtra ? "SevenElements" : "FiveElements")}");
        int max = ElementCombineUI.IsExtra ? 7 : 5;
        for (int n = 0; n < max; n++)
        {
            int index = n;
            var button = new ElementButton(ModContent.Request<Texture2D>("StoneOfThePhilosophers/UI/Element" + n));

            var element = (StoneElements)(n + 1);
            button.SetSize(40, 40);
            button.MouseEnter += delegate
            {
                if (button.Active && element2 == 0)
                    SoundEngine.PlaySound(SoundID.MenuTick);
            };
            button.LeftMouseClick += delegate
            {
                if (button.Active && element2 == 0)
                {
                    if (element1 == 0)
                    {
                        element1 = element;
                        CombineButton.Text = "确定\n" + Language.GetTextValue($"Mods.StoneOfThePhilosophers.Spells.{element}{element}");
                    }
                    else if (element2 == 0)
                    {
                        element2 = element;

                        var dummy1 = element1;
                        var dummy2 = element2;
                        if ((int)dummy1 < (int)dummy2)
                            Utils.Swap(ref dummy1, ref dummy2);

                        CombineButton.Text = "确定\n" + Language.GetTextValue($"Mods.StoneOfThePhilosophers.Spells.{dummy1}{dummy2}");
                    }

                    button.Active = false;
                    SoundEngine.PlaySound(SoundID.Unlock);
                }
            };
            //Vector2 target = (-MathHelper.TwoPi * (1f / max * n)).ToRotationVector2() * (max - 1) * 64;

            Vector2 target = (-MathHelper.PiOver2 + MathHelper.TwoPi * (0.6f + 1f / max * n)).ToRotationVector2() * 118;

            button.SetLeft(target.X - 20, .5f);
            button.SetTop(target.Y - 20, .4f);
            button.Positioning = Positioning.Absolute;

            Buttons.Add(button);
            button.Join(this);
        }
    }

    protected override void UpdateStatus(GameTime gameTime)
    {
        foreach (var button in Buttons)
            button.Factor = Factor;
    }

    public void Reset()
    {
        SetUpElementList();
        DragOffset = default;
        MouseDragOffset = default;
        element1 = element2 = 0;
    }
}

public class ElementButton : UIView
{
    public bool Active = true;
    public Asset<Texture2D> Texture { get; private set; }

    public ElementButton(Asset<Texture2D> texture)
    {
        if (texture is not null)
        {
            Texture = texture;
            SetSize(texture.Width(), texture.Height());
        }
    }

    public float Factor { get; set; }

    //public float factorInActive;
    private int openingAnimationTimer = 0;

    private float scaleFactor;

    protected override void UpdateStatus(GameTime gameTime)
    {
        if (!Active && openingAnimationTimer < 15) openingAnimationTimer++;
        scaleFactor = MathHelper.Lerp(scaleFactor, (Active && IsMouseHovering) ? 1f : 0f, 0.1f);

        base.UpdateStatus(gameTime);
    }

    public override void HandleDraw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        Vector2 selfCen = Bounds.Center;
        Vector2 parentCen = Parent?.Bounds.Center ?? selfCen;
        Vector2 normalVec = selfCen - parentCen;
        normalVec = new Vector2(-normalVec.Y, normalVec.X);
        var t = Factor;
        var scaler = t;
        var scaler2 = t;
        if (!Active)
        {
            t = MathHelper.SmoothStep(0, 1, openingAnimationTimer / 15f);
            parentCen = Parent?.Bounds.Center ?? selfCen;
            parentCen = Vector2.Lerp(parentCen, selfCen, 0.2f);
            normalVec = selfCen - parentCen;
            normalVec = new Vector2(normalVec.Y, -normalVec.X);
            scaler -= t / 4;
            scaler2 -= t;
            t = 1 - t;
        }
        Vector2 result = Vector2.Lerp(parentCen, Vector2.Lerp(normalVec + (selfCen + parentCen) * .5f, selfCen, t), t);//t * t * selfCen + 2 * t * (1 - t) * (normalVec + (selfCen + parentCen) * .5f) + (1 - t) * (1 - t) * parentCen

        #region 底板

        var graphicDevice = Main.instance.GraphicsDevice;
        SamplerState samplerState = graphicDevice.SamplerStates[0];
        DepthStencilState depthState = graphicDevice.DepthStencilState;
        RasterizerState rasterizerState = graphicDevice.RasterizerState;
        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.AnisotropicClamp, depthState, rasterizerState, null, Main.UIScaleMatrix);

        var panelTexture = ModContent.Request<Texture2D>("StoneOfThePhilosophers/UI/ElementPanel").Value;
        spriteBatch.Draw(panelTexture, result, null, Color.White * scaler2, Main.GlobalTimeWrappedHourly * .5f, new Vector2(118), .5f * scaler2 * (1f + .5f * scaleFactor), 0, 0);

        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, samplerState, depthState, rasterizerState, null, Main.UIScaleMatrix);

        #endregion 底板

        spriteBatch.Draw(Texture.Value, result, null, Color.White with { A = 127 } * Factor, 0f, Texture.Size() / 2f, (1f + .5f * scaleFactor) * scaler, SpriteEffects.None, 0f);
        spriteBatch.Draw(Texture.Value, result + Main.rand.NextVector2Unit() * 4, null, Color.White with { A = 51 } * scaler2 * .5f, 0f, Texture.Size() / 2f, (1f + .5f * scaleFactor) * scaler, SpriteEffects.None, 0f);
    }
}