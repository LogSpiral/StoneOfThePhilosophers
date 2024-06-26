﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using StoneOfThePhilosophers.Contents;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace StoneOfThePhilosophers.UI
{
    public class ElementChargeSystem : ModSystem
    {
        public ElementChargeUI elementChargeUI;
        public UserInterface elementChargeInterface;
        public static ElementChargeSystem Instance;
        public override void Load()
        {
            elementChargeUI = new ElementChargeUI();
            elementChargeInterface = new UserInterface();
            elementChargeUI.Activate();
            elementChargeInterface.SetState(elementChargeUI);
            Instance = this;
            base.Load();
        }
        public override void Unload()
        {
            elementChargeUI = null;
            elementChargeInterface = null;
            Instance = null;
            base.Unload();
        }
        public override void UpdateUI(GameTime gameTime)
        {
            elementChargeInterface?.Update(gameTime);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int inventoryIndex = layers.FindIndex(layer => layer.Name == "Vanilla: Inventory");
            if (inventoryIndex != -1)
            {
                layers.Insert(inventoryIndex + 1, new LegacyGameInterfaceLayer("StoneOfThePhilosophers: ElementChargeUI", () =>
                {
                    if (elementChargeUI.elementChargeBar.elementBarProgress > 0)
                        elementChargeUI.Draw(Main.spriteBatch);
                    return true;
                }, InterfaceScaleType.UI));
            }
        }
    }
    public class ElementChargeBar : UIElement
    {
        public bool active;
        public float elementBarProgress;
        public float elementBarValue;
        public StoneElements CurrentElement;
        public static Asset<Texture2D> ElementPanelBorder;
        public static Asset<Texture2D> ElementPanelContent;
        public static Asset<Texture2D> ElementPanelMiddleBorder;
        public static Asset<Texture2D> ElementPanelMiddleGround;
        public static Asset<Texture2D> ElementPanelFill;
        public static Asset<Texture2D> ElementPanelEnd;
        public override void OnInitialize()
        {
            var str = "StoneOfThePhilosophers/UI/Charge/";
            ElementPanelBorder = ModContent.Request<Texture2D>($"{str}Element_Panel_Right_Border");
            ElementPanelContent = ModContent.Request<Texture2D>($"{str}Element_Panel_Right_Content");
            ElementPanelMiddleBorder = ModContent.Request<Texture2D>($"{str}Element_Panel_Middle_Border");
            ElementPanelMiddleGround = ModContent.Request<Texture2D>($"{str}Element_Panel_Middle_Ground");
            ElementPanelFill = ModContent.Request<Texture2D>($"{str}Element_Fill");
            ElementPanelEnd = ModContent.Request<Texture2D>($"{str}Panel_Left");
        }
        public override void DrawSelf(SpriteBatch spriteBatch)
        {
            Vector2 topLeft = GetDimensions().Position();
            topLeft.X += 44;
            var flag = StoneOfThePhilosopherProj.ElementColor.TryGetValue(CurrentElement, out Color color);
            float factor1 = MathHelper.SmoothStep(0, 1, 2 * elementBarProgress);
            float factor2 = MathHelper.SmoothStep(0, 1, elementBarProgress * 2 - 1);
            #region Bar
            var offsetUnit = new Vector2(0, 12 * factor2);
            var offset = new Vector2(-30, 40);
            for (int n = 0; n < 19; n++)
            {
                if (flag)
                {
                    spriteBatch.Draw(ElementPanelMiddleGround.Value, topLeft + offset + offsetUnit * n, new Rectangle(0, 0, (int)(12f * factor2 + 1), 24), color * factor1, -MathHelper.PiOver2, default, 1f, 0, 0);
                }
            }
            int count = (int)MathHelper.Clamp(elementBarValue * 19 + 1, 0, 20);
            for (int n = 0; n < count; n++)
            {
                if (flag)
                {
                    var _color = Color.Lerp(color, Color.White, 0.25f - 0.25f * MathF.Cos(elementBarValue * Main.GlobalTimeWrappedHourly * 8));
                    spriteBatch.Draw(ElementPanelFill.Value, topLeft + offset + new Vector2(18, -12) + offsetUnit * n, new Rectangle(0, 0, (int)((n == count - 1 ? (elementBarValue * 19 % 1) : 1) * 12f * factor2 + 1), 12), _color * factor1, MathHelper.PiOver2, default, 1f, 0, 0);
                }
            }
            var (c, s) = Main.LocalPlayer.ElementPlayer();
            float l = s.GetElementCost((int)CurrentElement - 1) / 5;
            if (count >= l)
            {
                //CustomVertexInfo[] customVertexInfos = new CustomVertexInfo[count];
                //Vector2 unit = new Vector2(12, 48 * (s.GetElementCost((int)CurrentElement - 1) / 20f));
                //Vector2 center = topLeft + offset + Main.screenPosition;
                //for (int n = 0; n < 4; n++)
                //{
                //    int i = n / 2;
                //    int j = n % 2;
                //    customVertexInfos[i] = new CustomVertexInfo(center + unit * new Vector2(i, j), Color.White, new Vector3(i, j, 0.5f));
                //}
                //var baseTex = ModContent.Request<Texture2D>("StoneOfThePhilosophers/Images/Style_6").Value;
                //var aniTex = ModContent.Request<Texture2D>("StoneOfThePhilosophers/Images/Style_8").Value;
                //var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0));
                //spriteBatch.End();
                //spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
                //StoneOfThePhilosophersHelper.VertexDraw(customVertexInfos, baseTex, aniTex, null, new Vector2(Main.GlobalTimeWrappedHourly * 0.05f, 0), true, model * Main.UIScaleMatrix * Matrix.Invert(model * Main.GameViewMatrix.TransformationMatrix), null, false, false);
                //spriteBatch.End();
                //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
                var drawCen = topLeft + offsetUnit * MathHelper.Clamp(elementBarValue * 19 + 1, 0, 20) + new Vector2(-6, 16);// - (l - 1f) * 24
                var scaler = MathHelper.SmoothStep(0, 1, elementBarProgress * elementBarProgress);
                var _color = Color.Lerp(color, Color.White, 0.25f - 0.25f * MathF.Cos(elementBarValue * Main.GlobalTimeWrappedHourly * 16)) * scaler;

                spriteBatch.Draw(ModContent.Request<Texture2D>("StoneOfThePhilosophers/Images/Style_8").Value, drawCen, null, _color with { A = 0 } * factor1, MathHelper.PiOver2, new Vector2(256, 0), new Vector2(l * 12 * scaler, 24) / new Vector2(256), 0, 0);
                //spriteBatch.Draw(TextureAssets.MagicPixel.Value, drawCen, new Rectangle(0, 0, 1, 1), Color.Red, 0, new Vector2(.5f), 4f, 0, 0);

            }
            for (int n = 0; n < 19; n++)
            {
                spriteBatch.Draw(ElementPanelMiddleBorder.Value, topLeft + offset + offsetUnit * n + new Vector2(24, -12), new Rectangle(0, 0, (int)(12f * factor2 + 1), 24), Color.White * factor1 * factor1, MathHelper.PiOver2, default, 1f, 0, 0);
            }
            #endregion

            #region icon
            spriteBatch.Draw(ElementPanelEnd.Value, topLeft + offset + offsetUnit * 19 - new Vector2(0, 6), null, Color.White * factor1, -MathHelper.PiOver2, default, 1f, 0, 0);

            if (flag)
            {
                spriteBatch.Draw(ElementPanelContent.Value, topLeft, null, color * factor1, MathHelper.PiOver2, default, 1f, SpriteEffects.FlipHorizontally, 0);
                for (int n = 0; n < 3; n++)
                    spriteBatch.Draw(ElementPanelContent.Value, topLeft + Main.rand.NextVector2Unit() * elementBarValue * 4, null, Color.White with { A = 0 } * .25f * factor1 * elementBarValue, MathHelper.PiOver2, default, 1f, SpriteEffects.FlipHorizontally, 0);

            }
            spriteBatch.Draw(ElementPanelBorder.Value, topLeft, null, Color.White * factor1, MathHelper.PiOver2, default, 1f, SpriteEffects.FlipHorizontally, 0);
            #endregion


        }
        public override void Update(GameTime gameTime)
        {
            elementBarProgress = MathHelper.Lerp(elementBarProgress, active ? 1 : 0, 0.1f);
            if (elementBarProgress < 0.001f) elementBarProgress = 0;
            if (elementBarProgress > 0.999f) elementBarProgress = 1;
            if (IsMouseHovering)
            {
                Main.instance.MouseText((elementBarValue * 100).ToString("0.0") + "%");

            }
        }

    }
    public class ElementChargeUI : UIState
    {
        public override void OnInitialize()
        {
            elementChargeBar = new ElementChargeBar();
            elementChargeBar.Top = new StyleDimension(80, 0);
            elementChargeBar.Left = new StyleDimension(-44, 0.95f);
            elementChargeBar.Width = new StyleDimension(40, 0);
            elementChargeBar.Height = new StyleDimension(256, 0);
            Append(elementChargeBar);
        }
        public override void Update(GameTime gameTime)
        {
            Player player = Main.LocalPlayer;
            int type = player.HeldItem.type;
            StoneElements elements = StoneElements.Empty;
            if (type == ModContent.ItemType<StoneOfFireEX>()) elements = StoneElements.Fire;
            if (type == ModContent.ItemType<StoneOfWaterEX>()) elements = StoneElements.Water;
            if (type == ModContent.ItemType<StoneOfWoodEX>()) elements = StoneElements.Wood;
            if (type == ModContent.ItemType<StoneOfMetalEX>()) elements = StoneElements.Metal;
            if (type == ModContent.ItemType<StoneOfEarthEX>()) elements = StoneElements.Soil;
            if (type == ModContent.ItemType<StoneOfMoon>()) elements = StoneElements.Lunar;
            if (type == ModContent.ItemType<StoneOfSun>()) elements = StoneElements.Solar;
            elementChargeBar.active = elements != StoneElements.Empty;
            if (elementChargeBar.active)
                elementChargeBar.CurrentElement = elements;
            if (elementChargeBar.CurrentElement != StoneElements.Empty)
            {
                var current = elementChargeBar.elementBarValue;
                var target = player.GetModPlayer<ElementChargePlayer>().ElementChargeValue[(int)elementChargeBar.CurrentElement - 1] / 100f;
                var result = MathHelper.Lerp(current, target, 0.1f);
                if (result < 0.01f) result = 0;
                if (result > 0.99f) result = 1;
                elementChargeBar.elementBarValue = result;
                //Main.NewText((current, target, result));
                //elementChargeBar.elementBarValue = 1f;
            }
            //elementChargeBar.elementBarValue = MathF.Cos(Main.GlobalTimeWrappedHourly) * .5f + .5f;
            //var rect = elementChargeBar.GetDimensions().ToRectangle();
            //var point = ElementChargeSystem.Instance.elementChargeInterface.MousePosition.ToPoint();
            //var flag = false;
            //if (rect.Contains(point))
            //{
            //    Main.NewText("HENHENAAAA");
            //    Main.instance.MouseText(elementChargeBar.elementBarValue * 100 + "%");
            //    flag = true;
            //}
            //Main.NewText((rect, point, flag));
            base.Update(gameTime);
        }
        public ElementChargeBar elementChargeBar;
    }
}
