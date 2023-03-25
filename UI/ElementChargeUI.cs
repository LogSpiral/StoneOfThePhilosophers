using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using StoneOfThePhilosophers.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using static System.Net.Mime.MediaTypeNames;

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
                    spriteBatch.Draw(ElementPanelFill.Value, topLeft + offset + new Vector2(18, -12) + offsetUnit * n, new Rectangle(0, 0, (int)((n == count - 1 ? (elementBarValue * 19 % 1) : 1) * 12f * factor2 + 1), 12), color * factor1, MathHelper.PiOver2, default, 1f, 0, 0);
                }
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
