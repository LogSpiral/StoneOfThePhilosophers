using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SilkyUIFramework;
using SilkyUIFramework.Attributes;
using SilkyUIFramework.BasicElements;
using SilkyUIFramework.Extensions;
using StoneOfThePhilosophers.Contents;
using StoneOfThePhilosophers.Contents.Philosopher;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace StoneOfThePhilosophers.UI;

[RegisterUI("Vanilla: Radial Hotbars", $"{nameof(StoneOfThePhilosophers)}: {nameof(ElementSkillUI)}")]
public class ElementSkillUI : BasicBody
{
    public static ElementSkillUI Instance { get; private set; }

    public ElementSkillPanel ElementSkillPanelNeo { get; private set; }

    private static bool Active;

    public override bool Enabled { get => Active || timer > 0; set => Active = value; }

    protected override void OnInitialize()
    {
        Instance = this;
        FitWidth = true;
        FitHeight = true;
        ElementSkillPanelNeo = new ElementSkillPanel(this);
        ElementSkillPanelNeo.SetSize(256, 256);
        ElementSkillPanelNeo.Join(this);
        BorderColor = Color.Transparent;
    }

    public static void Open(int itemID, Vector2? position = null)
    {
        Instance.ElementSkillPanelNeo.DragOffset = new Vector2(0f, 0f);
        Instance.ElementSkillPanelNeo.SetUpElementList();
        Instance.ElementSkillPanelNeo.MouseDragOffset = default;
        Instance.DragOffset = default;
        Active = true;
        SoundEngine.PlaySound(SoundID.MenuOpen);
        Instance.itemType = itemID;
        var targetVector = (position ?? Main.MouseScreen) / Main.ScreenSize.ToVector2();
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
        ElementSkillPanelNeo.Factor = MathHelper.SmoothStep(0, 1, timer / 15f);
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

public class ElementSkillPanel : SUIDraggableView
{
    public List<ElementSkillButton> Buttons = [];
    public float Factor { set; get; }

    public ElementSkillPanel(UIElementGroup controlTarget) : base(controlTarget)
    {
    }

    public override void HandleDraw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        //spriteBatch.Draw(TextureAssets.MagicPixel.Value, rect, Color.Black);
        var graphicDevice = Main.instance.GraphicsDevice;
        SamplerState samplerState = graphicDevice.SamplerStates[0];
        DepthStencilState depthState = graphicDevice.DepthStencilState;
        RasterizerState rasterizerState = graphicDevice.RasterizerState;
        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.AnisotropicClamp, depthState, rasterizerState, null, Main.UIScaleMatrix);

        var panelText = ModContent.Request<Texture2D>("StoneOfThePhilosophers/UI/Skill/SkillDecider").Value;
        Vector2 center = Bounds.Center;
        spriteBatch.Draw(panelText, center, null, Color.White, MathHelper.Pi, new Vector2(256), .5f * Factor, 0, 0);
        spriteBatch.Draw(panelText, center, null, Color.White * .5f, Main.GlobalTimeWrappedHourly, new Vector2(256), .75f * Factor, 0, 0);
        //spriteBatch.Draw(panelText, rect.Center.ToVector2(), null, Color.White * .75f, -Main.GlobalTimeWrappedHourly * 2, new Vector2(256), .625f * factor, 0, 0);

        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, samplerState, depthState, rasterizerState, null, Main.UIScaleMatrix);

        DrawChildren(gameTime, spriteBatch);
    }

    public void SetUpElementList()
    {
        var elementSkillPlr = Main.LocalPlayer.GetModPlayer<ElementSkillPlayer>();
        Buttons.Clear();
        Elements.Clear();
        if (Main.LocalPlayer.HeldItem.ModItem is not MagicStone magicStone) return;
        var type = (int)magicStone.Elements - 1;
        if (Main.LocalPlayer.HeldItem.ModItem is StoneOfThePhilosopher stoneOfThePhilosopher)
        {
            var mplr = Main.LocalPlayer.GetModPlayer<ElementCombinePlayer>();
            var combination = stoneOfThePhilosopher.Extra ? mplr.CombinationEX : mplr.Combination;
            if (combination.Mode != 1) return;
            type = (int)combination.MainElements - 1;
        }
        int max = ElementSkillPlayer.skillCounts[type];
        for (int n = 0; n < max; n++)
        {
            int index = n;
            var button = new ElementSkillButton(ModContent.Request<Texture2D>("StoneOfThePhilosophers/UI/Element" + type), ElementSkillPlayer.GetSkillName(type, index));
            button.SetSize(40, 40);
            button.MouseEnter += delegate
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
            };
            button.LeftMouseClick += delegate
            {
                SoundEngine.PlaySound(SoundID.Unlock);
                //elementSkillPlr.skillIndex[type] = index;
                elementSkillPlr.skillIndex[type] = index + 1;

                ElementSkillUI.Close();
                ElementCombineUI.Close();
            };
            Vector2 target = (-MathHelper.TwoPi * (1f / max * n)).ToRotationVector2() * (max - 1) * 64;
            button.SetLeft(target.X - 20, .5f);
            button.SetTop(target.Y - 20, .5f);
            button.Positioning = Positioning.Absolute;

            Buttons.Add(button);
            button.Join(this);
        }
    }

    protected override void UpdateStatus(GameTime gameTime)
    {
        foreach (var button in Buttons)
            button.Factor = Factor;
        base.UpdateStatus(gameTime);
    }
}

public class ElementSkillButton : UIView
{
    public bool Active = true;
    public Asset<Texture2D> Texture { get; private set; }

    public ElementSkillButton(Asset<Texture2D> texture, string spellName)
    {
        if (texture is not null)
        {
            Texture = texture;
            SetSize(texture.Width(), texture.Height());
        }
        SpellName = spellName;
    }

    public string SpellName;
    public float Factor { get; set; }

    //public float factorInActive;
    public int timer = 0;

    private float scaleFactor;

    protected override void UpdateStatus(GameTime gameTime)
    {
        if (!Active && timer < 15) timer++;
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
            t = MathHelper.SmoothStep(0, 1, timer / 15f);
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

        var panelText = ModContent.Request<Texture2D>("StoneOfThePhilosophers/UI/Skill/SkillDecider").Value;
        spriteBatch.Draw(panelText, result, null, Color.White * scaler2, Main.GlobalTimeWrappedHourly * .5f, new Vector2(256), .25f * scaler2 * (1f + .5f * scaleFactor), 0, 0);
        //spriteBatch.Draw(panelText, result, null, Color.White * scaler2 * .5f, -Main.GlobalTimeWrappedHourly, new Vector2(118), .35f * scaler2 * (1f + .5f * timer2), 0, 0);

        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, samplerState, depthState, rasterizerState, null, Main.UIScaleMatrix);

        #endregion 底板

        spriteBatch.Draw(Texture.Value, result, null, Color.White with { A = 127 } * Factor, 0f, Texture.Size() / 2f, (1f + .5f * scaleFactor) * scaler, SpriteEffects.None, 0f);
        spriteBatch.Draw(Texture.Value, result + Main.rand.NextVector2Unit() * 4, null, Color.White with { A = 51 } * scaler2 * .5f, 0f, Texture.Size() / 2f, (1f + .5f * scaleFactor) * scaler, SpriteEffects.None, 0f);
        //TODO 按钮按下之后的效果
        //TODO 按钮旁边的光效之类的东西
        //TODO 融合按钮
        //TODO 融合后的气场

        //spriteBatch.Draw(Texture.Value, parentCen, null, Color.White * factor * .5f, 0f, Texture.Size() / 2f, 1f, SpriteEffects.None, 0f);

        if (IsMouseHovering)
            Main.instance.MouseText(SpellName);
    }
}