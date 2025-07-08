using LogSpiralLibrary.CodeLibrary.Utilties.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using StoneOfThePhilosophers.Contents;
using StoneOfThePhilosophers.Contents.Earth;
using StoneOfThePhilosophers.Contents.Fire;
using StoneOfThePhilosophers.Contents.Metal;
using StoneOfThePhilosophers.Contents.Moon;
using StoneOfThePhilosophers.Contents.Philosopher;
using StoneOfThePhilosophers.Contents.Sun;
using StoneOfThePhilosophers.Contents.Water;
using StoneOfThePhilosophers.Contents.Wood;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;

namespace StoneOfThePhilosophers.UI;
public class ElementChargeSystem : ModSystem
{
    #region Properties
    public static float Progress { get; set; }

    public static StoneElements CurrentElement { get; set; }

    public static float BarValue { get; set; }
    #endregion

    #region Assets
    public static Asset<Texture2D> ElementPanelBorder => ModAsset.Element_Panel_Right_Border;
    public static Asset<Texture2D> ElementPanelContent => ModAsset.Element_Panel_Right_Content;
    public static Asset<Texture2D> ElementPanelMiddleBorder => ModAsset.Element_Panel_Middle_Border;
    public static Asset<Texture2D> ElementPanelMiddleGround => ModAsset.Element_Panel_Middle_Ground;
    public static Asset<Texture2D> ElementPanelFill => ModAsset.Element_Fill;
    public static Asset<Texture2D> ElementPanelEnd => ModAsset.Panel_Left;
    #endregion

    #region Drawing
    public static void DrawChargeBar_Internal(CalculatedStyle destination, float elementBarProgress, float elementBarValue)
    {
        SpriteBatch spriteBatch = Main.spriteBatch;

        Vector2 topLeft = destination.Position();
        topLeft.X += 44;
        var flag = StoneOfThePhilosopherProj.ElementColor.TryGetValue(CurrentElement, out Color color);
        float factor1 = MathHelper.SmoothStep(0, 1, 2 * elementBarProgress);
        float factor2 = MathHelper.SmoothStep(0, 1, elementBarProgress * 2 - 1);
        #region Bar
        var offsetUnit = new Vector2(0, 12 * factor2);
        var offset = new Vector2(-30, 40);
        if (flag)
            for (int n = 0; n < 20; n++)
                spriteBatch.Draw(ElementPanelMiddleGround.Value, topLeft + offset + offsetUnit * n + new Vector2(24, -12), new Rectangle(0, 0, (int)(12f * factor2 + 1), 24), color * factor1 * factor1, MathHelper.PiOver2, default, 1f, 0, 0);
        int count = (int)Math.Ceiling(elementBarValue * 20);//向上取整
        float extraValue = elementBarValue * 20 + 1 - count;
        if (flag)
            for (int n = 0; n < count; n++)
            {
                var _color = Color.Lerp(color, Color.White, 0.25f - 0.25f * MathF.Cos(elementBarValue * Main.GlobalTimeWrappedHourly * 8));
                spriteBatch.Draw(ElementPanelFill.Value, topLeft + offset + new Vector2(18, -12) + offsetUnit * n, new Rectangle(0, 0, (int)((n == count - 1 ? extraValue : 1) * 12f * factor2 + 1), 12), _color * factor1, MathHelper.PiOver2, default, 1f, 0, 0);
            }
        var mplr = Main.LocalPlayer.GetModPlayer<ElementSkillPlayer>();
        float l = mplr.GetElementCost((int)CurrentElement - 1) / 5;
        if (mplr.ElementChargeValue[(int)CurrentElement - 1] / 5 >= l)
        {
            float offsetY = 12 * factor2 * (count - 1) - 12 + 12 * extraValue;
            var drawCen = topLeft + offset + new Vector2(24, 0) + Vector2.UnitY * offsetY;
            var scaler = factor2;
            var _color = Color.Lerp(color, Color.White, 0.25f - 0.25f * MathF.Cos(elementBarValue * Main.GlobalTimeWrappedHourly * 16)) * scaler;
            spriteBatch.Draw(ModAsset.Style_8.Value, drawCen, null, _color with { A = 0 } * factor1, MathHelper.PiOver2, new Vector2(256, 0), new Vector2(MathF.Min(l * 12 * scaler, offsetY + 12), 24) / new Vector2(256), 0, 0);
        }
        for (int n = 0; n < 20; n++)
            spriteBatch.Draw(ElementPanelMiddleBorder.Value, topLeft + offset + offsetUnit * n + new Vector2(24, -12), new Rectangle(0, 0, (int)(12f * factor2 + 1), 24), Color.White * factor1 * factor1, MathHelper.PiOver2, default, 1f, 0, 0);

        #endregion

        #region icon
        spriteBatch.Draw(ElementPanelEnd.Value, topLeft + offset + offsetUnit * 20 - new Vector2(0, 6), null, Color.White * factor1, -MathHelper.PiOver2, default, 1f, 0, 0);

        if (flag)
        {
            spriteBatch.Draw(ElementPanelContent.Value, topLeft, null, color * factor1, MathHelper.PiOver2, default, 1f, SpriteEffects.FlipHorizontally, 0);
            for (int n = 0; n < 3; n++)
                spriteBatch.Draw(ElementPanelContent.Value, topLeft + Main.rand.NextVector2Unit() * elementBarValue * 4, null, Color.White with { A = 0 } * .25f * factor1 * elementBarValue, MathHelper.PiOver2, default, 1f, SpriteEffects.FlipHorizontally, 0);

        }
        spriteBatch.Draw(ElementPanelBorder.Value, topLeft, null, Color.White * factor1, MathHelper.PiOver2, default, 1f, SpriteEffects.FlipHorizontally, 0);
        #endregion

        if (destination.ToRectangle().Contains(Main.MouseScreen.ToPoint()))
            Main.instance.MouseText((elementBarValue * 100).ToString("0.0") + "%");
    }
    public static bool DrawChargeBar()
    {
        if (Progress <= 0) return true;

        DrawChargeBar_Internal(new(Main.screenWidth - 360,80, 40, 280), Progress, BarValue);
        return true;
    }
    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int inventoryIndex = layers.FindIndex(layer => layer.Name == "Vanilla: Inventory");
        if (inventoryIndex != -1)
            layers.Insert(inventoryIndex + 1, new LegacyGameInterfaceLayer("StoneOfThePhilosophers: ElementChargeUIRework", DrawChargeBar, InterfaceScaleType.UI));

        base.ModifyInterfaceLayers(layers);
    }
    #endregion

    #region Update
    public override void PostUpdatePlayers()
    {
        Player player = Main.LocalPlayer;
        StoneElements elements = player?.HeldItem?.ModItem is MagicStone magicStone ? magicStone.Elements : StoneElements.Empty;
        if (player?.HeldItem?.ModItem is StoneOfThePhilosopher philosopherStone && player.GetModPlayer<ElementCombinePlayer>() is var elementCombinePlr) 
        {
            var combination = philosopherStone.Extra ? elementCombinePlr.CombinationEX: elementCombinePlr.Combination;
            if (combination.Mode == 1)
                elements = combination.MainElements;
        }

        float progressTarget = 0;
        if (elements != StoneElements.Empty)
        {
            var target = player.GetModPlayer<ElementSkillPlayer>().ElementChargeValue[(int)elements - 1] / 100f;
            var result = MathHelper.Lerp(BarValue, target, 0.1f);
            if (result < 0.0001f) result = 0;
            if (result > 0.9999f) result = 1;
            BarValue = result;
            CurrentElement = elements;

            progressTarget = 1;
        }

        Progress = MathHelper.Lerp(Progress, progressTarget, 0.1f);
        if (Progress < 0.01f) Progress = 0;
        if (Progress > 0.99f) Progress = 1;

        base.PostUpdatePlayers();
    }
    #endregion
}
