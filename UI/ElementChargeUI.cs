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
                spriteBatch.Draw(ModAsset.Style_8.Value, drawCen, null, _color with { A = 0 } * factor1, MathHelper.PiOver2, new Vector2(256, 0), new Vector2(MathF.Min(l * 12 * scaler,offsetY + 12), 24) / new Vector2(256), 0, 0);
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

            if (IsMouseHovering)
                Main.instance.MouseText((elementBarValue * 100).ToString("0.0") + "%");

        }
        public override void Update(GameTime gameTime)
        {
            elementBarProgress = MathHelper.Lerp(elementBarProgress, active ? 1 : 0, 0.1f);
            if (elementBarProgress < 0.01f) elementBarProgress = 0;
            if (elementBarProgress > 0.99f) elementBarProgress = 1;

        }

    }
    public class ElementChargeUI : UIState
    {
        public override void OnInitialize()
        {
            elementChargeBar = new()
            {
                Top = new(80, 0),
                Left = new(-360, 1),
                Width = new(40, 0),
                Height = new(280, 0)
            };
            Append(elementChargeBar);
            Recalculate();
        }
        public bool active;
        public override void Update(GameTime gameTime)
        {
            Player player = Main.LocalPlayer;
            StoneElements elements = player.HeldItem.ModItem switch
            {
                StoneOfFireEX => StoneElements.Fire,
                StoneOfWaterEX => StoneElements.Water,
                StoneOfWoodEX => StoneElements.Wood,
                StoneOfMetalEX => StoneElements.Metal,
                StoneOfEarthEX => StoneElements.Soil,
                StoneOfMoon => StoneElements.Lunar,
                StoneOfSun => StoneElements.Solar,
                _ => StoneElements.Empty
            };
            elementChargeBar.active = elements != StoneElements.Empty;
            if (elements != StoneElements.Empty)
            {
                if (!active)
                {
                    active = true;
                    Recalculate();
                }
                elementChargeBar.CurrentElement = elements;
                var current = elementChargeBar.elementBarValue;
                var target = player.GetModPlayer<Contents.ElementSkillPlayer>().ElementChargeValue[(int)elementChargeBar.CurrentElement - 1] / 100f;
                
                var result = MathHelper.Lerp(current, target, 0.1f);
                if (result < 0.0001f) result = 0;
                if (result > 0.9999f) result = 1;
                elementChargeBar.elementBarValue = result;
            }
            else
                active = false;
            base.Update(gameTime);
        }
        public ElementChargeBar elementChargeBar;
    }
}
