using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using StoneOfThePhilosophers.Contents;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.ID;

namespace StoneOfThePhilosophers.UI
{
    public class ElementSystem : ModSystem
    {
        public ElementUI elementUI;
        public UserInterface chooserInterface;
        public static ElementSystem Instance;
        public override void Load()
        {
            elementUI = new ElementUI();
            chooserInterface = new UserInterface();
            elementUI.Activate();
            chooserInterface.SetState(elementUI);
            Instance = this;
            base.Load();
        }
        public override void Unload()
        {
            elementUI = null;
            chooserInterface = null;
            Instance = null;
            base.Unload();
        }
        public override void UpdateUI(GameTime gameTime)
        {
            if (ElementUI.Visible)
                chooserInterface?.Update(gameTime);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int inventoryIndex = layers.FindIndex(layer => layer.Name == "Vanilla: Inventory");
            if (inventoryIndex != -1)
            {
                layers.Insert(inventoryIndex + 1, new LegacyGameInterfaceLayer("ImproveGame: Structure GUI", () =>
                {
                    if (ElementUI.Visible)
                        elementUI.Draw(Main.spriteBatch);
                    return true;
                }, InterfaceScaleType.UI));
            }
        }
    }
    public class ElementPanel : UIElement
    {
        public bool Draggable;
        public bool Dragging;
        public Vector2 Offset;
        public float border;
        public bool CalculateBorder;
        public List<ElementButton> Buttons;
        public ElementPanel(float border = 3, bool CalculateBorder = true)
        {
            SetPadding(10f);
            this.border = border;
            this.CalculateBorder = CalculateBorder;
            OnMouseDown += DragStart;
            OnMouseUp += DragEnd;
        }
        public override void Recalculate()
        {
            base.Recalculate();
            //panelInfo.destination = GetDimensions().ToRectangle();
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimenstions = GetDimensions();
            var rect = dimenstions.ToRectangle();
            if (CalculateBorder)
            {
                rect = Utils.CenteredRectangle(rect.Center.ToVector2(), rect.Size() + new Vector2(border * 2));
            }
            var graphicDevice = Main.instance.GraphicsDevice;
            SamplerState samplerState = graphicDevice.SamplerStates[0];
            DepthStencilState depthState = graphicDevice.DepthStencilState;
            RasterizerState rasterizerState = graphicDevice.RasterizerState;
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, depthState, rasterizerState, null, Main.UIScaleMatrix);
            //spriteBatch.Draw(ModContent.Request<Texture2D>("StoneOfThePhilosophers/UI/ElementPanel").Value, rect, Color.White);
            spriteBatch.Draw(ModContent.Request<Texture2D>("StoneOfThePhilosophers/UI/ElementPanel").Value, rect.Center.ToVector2(), null, Color.White, MathHelper.Pi, new Vector2(118), 1f, 0, 0);
            spriteBatch.Draw(ModContent.Request<Texture2D>("StoneOfThePhilosophers/UI/ElementPanel").Value, rect.Center.ToVector2(), null, Color.White * .5f, Main.GlobalTimeWrappedHourly, new Vector2(118), 1.5f, 0, 0);
            spriteBatch.Draw(ModContent.Request<Texture2D>("StoneOfThePhilosophers/UI/ElementPanel").Value, rect.Center.ToVector2(), null, Color.White * .75f, -Main.GlobalTimeWrappedHourly * 2, new Vector2(118), 1.25f, 0, 0);



            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, samplerState, depthState, rasterizerState, null, Main.UIScaleMatrix);
        }

        // 可拖动界面
        private void DragStart(UIMouseEvent evt, UIElement listeningElement)
        {
            // 当点击的是子元素不进行移动
            //Main.NewText((Draggable, evt.Target == this));
            if (Draggable && evt.Target == this)
            {
                Offset = new Vector2(evt.MousePosition.X - Left.Pixels, evt.MousePosition.Y - Top.Pixels);
                Dragging = true;
            }
        }

        // 可拖动/调整大小界面
        private void DragEnd(UIMouseEvent evt, UIElement listeningElement)
        {
            Dragging = false;
        }
        public override void OnInitialize()
        {

        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsMouseHovering)
            {
                Main.LocalPlayer.mouseInterface = true;
                //panelInfo.mainColor = KeepOrigin ? CoolerUIPanel.BackgroundDefaultUnselectedColor : Color.White;

            }

            if (Dragging)
            {
                Left.Set(Main.mouseX - Offset.X, 0f);
                Top.Set(Main.mouseY - Offset.Y, 0f);
                Recalculate();
                OnDrag?.Invoke(this);
            }
        }
        public void SetUpElementList()
        {
            var elementPlr = Main.LocalPlayer.GetModPlayer<ElementPlayer>();
            //Buttons.Clear();
            Elements.Clear();
            int max = ElementUI.IsSeven ? 7 : 5;
            for (int n = 0; n < max; n++)
            {
                var button = new ElementButton(ModContent.Request<Texture2D>("StoneOfThePhilosophers/UI/Element" + n));
                button.SetSize(40, 40);
                button.OnMouseOver += (_, _) =>
                {
                    SoundEngine.PlaySound(SoundID.MenuTick);
                };
                button.OnClick += (_, _) =>
                {
                    elementPlr.element1 = (StoneElements)n;
                    SoundEngine.PlaySound(SoundID.Unlock);
                };
                button.OnRightClick += (_, _) =>
                {
                    elementPlr.element2 = (StoneElements)n;
                    SoundEngine.PlaySound(SoundID.Unlock);
                };
                Vector2 target = (-MathHelper.PiOver2 + MathHelper.TwoPi * (0.6f + 1f / max * n * 2)).ToRotationVector2() * 118;
                button.Left.Set(target.X - 20, .5f);
                button.Top.Set(target.Y - 20, .5f);

                //Buttons.Add(button);
                Append(button);
            }
            Recalculate();
        }
        public event ElementEvent OnDrag;
    }
    public class ElementButton : UIElement
    {
        public bool Active;
        public Asset<Texture2D> Texture { get; private set; }
        public ElementButton(Asset<Texture2D> texture, Color activeColor = default, Color inactiveColor = default)
        {
            if (texture is not null)
            {
                Texture = texture;
                Width.Set(Texture.Width(), 0f);
                Height.Set(Texture.Height(), 0f);
            }
        }

        #region 各种设置方法

        public void SetImage(Asset<Texture2D> texture, bool changeSize = false)
        {
            Texture = texture;
            if (changeSize)
            {
                Width.Set(Texture.Width(), 0f);
                Height.Set(Texture.Height(), 0f);
            }
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();

            //var mainColor = IsMouseHovering ? ColorActive : ColorInactive;
            //if (DrawColor is not null)
            //{
            //    mainColor = DrawColor.Invoke();
            //}
            //currentColor = Color.Lerp(currentColor, mainColor, 0.05f);
            ////drawcoolertex
            //if (currentStyleTex != null)
            //{
            //    //var rect = dimensions.ToRectangle();
            //    //rect = rect.OffsetSize((int)dimensions.Width / 2, (int)(-dimensions.Height / 4));
            //    //rect.Offset(0, (int)(dimensions.Height / 8));
            //    //DrawCoolerTextBox_Combine(spriteBatch, currentStyleTex, rect, currentColor);
            //    var rect = dimensions.ToRectangle().OffsetSize(8, 8);
            //    rect.Offset(-4, -4);
            //    var info = new CoolerPanelInfo();
            //    info.configTexStyle = currentStyle;
            //    info.destination = rect;
            //    info.scaler = 1f;
            //    info.origin = default;
            //    info.glowShakingStrength = IsMouseHovering ? .25f : 0f;
            //    info.glowHueOffsetRange = .1f;
            //    info.glowEffectColor = currentColor;
            //    info.backgroundColor = Color.White;
            //    info.DrawCoolerPanel(spriteBatch);
            //}
            //else
            //{
            //    spriteBatch.Draw(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanel").Value, dimensions.Center(), null, currentColor, 0f, new Vector2(22), 1f, SpriteEffects.None, 0f);
            //    if (IsMouseHovering)
            //        spriteBatch.Draw(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelBorder").Value, dimensions.Center(), null, currentColor, 0f, new Vector2(22), 1f, SpriteEffects.None, 0f);
            //}
            spriteBatch.Draw(Texture.Value, dimensions.Center(), null, Color.White, 0f, Texture.Size() / 2f, 1f, SpriteEffects.None, 0f);
        }
    }
    public enum StoneElements
    {
        Fire,
        Water,
        Wood,
        Metal,
        Soil,
        Lunar,
        Sun,
    }
    //public enum CombinedElements
    //{
    //    FireFire = 0,
    //    FireWater = 1,
    //    FireWood = 2,
    //    FireMetal = 3,
    //    FireSoil = 4,
    //    FireLunar = 5,
    //    FireSun = 6,
    //    WaterWater = 11,
    //    WaterWood = 12,
    //    WaterMetal = 13,
    //    WaterSoil = 14,
    //    WaterLunar = 15,
    //    WaterSun = 16,
    //    WoodWood = 22,
    //    WoodMetal = 23,
    //    WoodSoil = 24,
    //    WoodLunar = 25,
    //    WoodSun = 26,
    //    MetalMetal = 33,
    //    MetalSoil = 34,
    //    MetalLunar = 35,
    //    MetalSun = 36,
    //    SoilSoil = 44,
    //    SoilLunar = 45,
    //    SoilSun = 46,
    //    LunarLunar = 55,
    //    LunarSun = 56,
    //    SunSun = 66
    //}
    public class ElementPlayer : ModPlayer
    {
        public StoneElements element1;
        public StoneElements element2;
        //public CombinedElements CombinedElement
        //{
        //    get
        //    {
        //        int a = (int)element1;
        //        int b = (int)element2;
        //        if (a > b) Utils.Swap(ref a, ref b);
        //        return (CombinedElements)(a * 10 + b);
        //    }
        //}
    }
    public class ElementUI : UIState
    {
        public static bool Visible { get; private set; }
        public void Open()
        {
            Visible = true;
            SoundEngine.PlaySound(SoundID.MenuOpen);
            BasePanel.SetUpElementList();
        }
        public void Close()
        {
            Visible = false;
            Main.blockInput = false;
            SoundEngine.PlaySound(SoundID.MenuClose);
        }
        public bool CacheSetupElements; // 缓存，在下一帧Setup
        public static bool IsSeven;
        public ElementPanel BasePanel;

        public override void Update(GameTime gameTime)
        {
            if (CacheSetupElements)
            {
                BasePanel.SetUpElementList();
                CacheSetupElements = false;
            }
            base.Update(gameTime);
        }
        //一个底板 一堆按钮 右键切换开关状态 默认水火符 
        public override void OnInitialize()
        {
            #region 贴图加载

            #endregion

            #region 面板初始化
            BasePanel = new ElementPanel()
            {
                Top = StyleDimension.FromPixels(256f),
                HAlign = 0.2f
            };
            //BasePanel.SetPos(new Vector2(256, 256));
            BasePanel.SetSize(256, 256).SetPadding(12f);
            BasePanel.Draggable = true;
            Append(BasePanel);
            #endregion
        }
    }
}
