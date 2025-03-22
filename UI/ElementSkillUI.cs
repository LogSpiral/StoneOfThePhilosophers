using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using StoneOfThePhilosophers.Contents;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.Audio;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;

namespace StoneOfThePhilosophers.UI
{
    public class ElementSkillSystem : ModSystem
    {
        public ElementSkillUI skillUI;
        public UserInterface SkillInterface;
        public static ElementSkillSystem Instance;
        public override void Load()
        {
            skillUI = new ElementSkillUI();
            SkillInterface = new UserInterface();
            skillUI.Activate();
            SkillInterface.SetState(skillUI);
            Instance = this;
            base.Load();
        }
        public override void Unload()
        {
            skillUI = null;
            SkillInterface = null;
            Instance = null;
            base.Unload();
        }
        public override void UpdateUI(GameTime gameTime)
        {
            if (ElementSkillUI.Visible || ElementSkillUI.timer > 0)
                SkillInterface?.Update(gameTime);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int inventoryIndex = layers.FindIndex(layer => layer.Name == "Vanilla: Inventory");
            if (inventoryIndex != -1)
            {
                layers.Insert(inventoryIndex + 1, new LegacyGameInterfaceLayer("StoneOfThePhilosophers: ElementSkillUI", () =>
                {
                    if (ElementSkillUI.timer > 0)
                        skillUI.Draw(Main.spriteBatch);
                    return true;
                }, InterfaceScaleType.UI));
            }
        }
    }
    public class ElementSkillPanel : UIElement
    {
        /// <summary>
        /// 是否可拖动
        /// </summary>
        public bool Draggable;
        public bool Dragging;
        public Vector2 Offset;
        public float border;
        public bool CalculateBorder;
        public ElementSkillButton lastButton;
        /// <summary>
        /// <br>元素按钮</br>
        /// <br>为什么是list呢？难道你还打算以后整点别的？</br>
        /// </summary>
        public List<ElementSkillButton> Buttons;
        /// <summary>
        /// <br>动画插值</br>
        /// <br>决定ui的大小和透明度之类</br>
        /// </summary>
        public float factor;
        public ElementSkillPanel(float border = 3, bool CalculateBorder = true)
        {
            SetPadding(10f);
            this.border = border;
            this.CalculateBorder = CalculateBorder;
            OnLeftMouseDown += DragStart;
            OnLeftMouseUp += DragEnd;
            Buttons = [];
        }
        public override void OnActivate()
        {
            base.OnActivate();
        }
        public override void Recalculate()
        {
            base.Recalculate();
            //panelInfo.destination = GetDimensions().ToRectangle();
        }
        public override void DrawSelf(SpriteBatch spriteBatch)
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
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.AnisotropicClamp, depthState, rasterizerState, null, Main.UIScaleMatrix);

            var panelText = ModContent.Request<Texture2D>("StoneOfThePhilosophers/UI/Skill/SkillDecider").Value;
            spriteBatch.Draw(panelText, rect.Center.ToVector2(), null, Color.White, MathHelper.Pi, new Vector2(256), .5f * factor, 0, 0);
            spriteBatch.Draw(panelText, rect.Center.ToVector2(), null, Color.White * .5f, Main.GlobalTimeWrappedHourly, new Vector2(256), .75f * factor, 0, 0);
            //spriteBatch.Draw(panelText, rect.Center.ToVector2(), null, Color.White * .75f, -Main.GlobalTimeWrappedHourly * 2, new Vector2(256), .625f * factor, 0, 0);

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
            foreach (var button in Buttons)
            {
                button.factor = factor;
            }
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
            var elementSkillPlr = Main.LocalPlayer.GetModPlayer<ElementSkillPlayer>();
            Buttons.Clear();
            Elements.Clear();
            lastButton = null;
            var type = Main.LocalPlayer.HeldItem.type;
            if (type == ModContent.ItemType<StoneOfFireEX>()) type = 0;
            if (type == ModContent.ItemType<StoneOfWaterEX>()) type = 1;
            if (type == ModContent.ItemType<StoneOfWoodEX>()) type = 2;
            if (type == ModContent.ItemType<StoneOfMetalEX>()) type = 3;
            if (type == ModContent.ItemType<StoneOfEarthEX>()) type = 4;
            if (type == ModContent.ItemType<StoneOfMoon>()) type = 5;
            if (type == ModContent.ItemType<StoneOfSun>()) type = 6;
            int max = ElementSkillPlayer.skillCounts[type];
            for (int n = 0; n < max; n++)
            {
                int index = n;
                var button = new ElementSkillButton(ModContent.Request<Texture2D>("StoneOfThePhilosophers/UI/Element" + type), ElementSkillPlayer.skillName[type, index]);
                button.SetSize(40, 40);
                button.OnMouseOver += (_, _) =>
                {
                    SoundEngine.PlaySound(SoundID.MenuTick);
                };
                var element = (StoneElements)(n + 1);
                button.OnLeftClick += (_, _) =>
                {
                    SoundEngine.PlaySound(SoundID.Unlock);
                    //elementSkillPlr.skillIndex[type] = index;
                    elementSkillPlr.skillIndex[type] = index;

                    ElementSkillSystem.Instance.skillUI.Close();
                };
                Vector2 target = (-MathHelper.TwoPi * (1f / max * n)).ToRotationVector2() * (max - 1) * 64;
                button.Left.Set(target.X - 20, .5f);
                button.Top.Set(target.Y - 20, .5f);

                Buttons.Add(button);
                Append(button);
            }
            Recalculate();
        }
        public event ElementEvent OnDrag;
    }
    public class ElementSkillButton : UIElement
    {
        public bool Active = true;
        public Asset<Texture2D> Texture { get; private set; }
        public ElementSkillButton(Asset<Texture2D> texture, string spellName)
        {
            if (texture is not null)
            {
                Texture = texture;
                Width.Set(Texture.Width(), 0f);
                Height.Set(Texture.Height(), 0f);
            }
            SpellName = spellName;
        }
        public string SpellName;
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
        public float factor;
        //public float factorInActive;
        public int timer = 0;
        public float timer2;
        public override void Update(GameTime gameTime)
        {
            if (!Active && timer < 15) timer++;
            timer2 = MathHelper.Lerp(timer2, (Active && IsMouseHovering) ? 1f : 0f, 0.1f);

            base.Update(gameTime);
        }

        public override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();
            Vector2 selfCen = dimensions.Center();
            Vector2 parentCen = Parent?.GetDimensions().Center() ?? selfCen;
            Vector2 normalVec = selfCen - parentCen;
            normalVec = new Vector2(-normalVec.Y, normalVec.X);
            var t = factor;
            var scaler = t;
            var scaler2 = t;
            if (!Active)
            {
                t = MathHelper.SmoothStep(0, 1, timer / 15f);
                parentCen = Parent?.GetDimensions().Center() ?? selfCen;
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
            spriteBatch.Draw(panelText, result, null, Color.White * scaler2, Main.GlobalTimeWrappedHourly * .5f, new Vector2(256), .25f * scaler2 * (1f + .5f * timer2), 0, 0);
            //spriteBatch.Draw(panelText, result, null, Color.White * scaler2 * .5f, -Main.GlobalTimeWrappedHourly, new Vector2(118), .35f * scaler2 * (1f + .5f * timer2), 0, 0);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, samplerState, depthState, rasterizerState, null, Main.UIScaleMatrix);
            #endregion


            spriteBatch.Draw(Texture.Value, result, null, Color.White with { A = 127 } * factor, 0f, Texture.Size() / 2f, (1f + .5f * timer2) * scaler, SpriteEffects.None, 0f);
            spriteBatch.Draw(Texture.Value, result + Main.rand.NextVector2Unit() * 4, null, Color.White with { A = 51 } * scaler2 * .5f, 0f, Texture.Size() / 2f, (1f + .5f * timer2) * scaler, SpriteEffects.None, 0f);
            //TODO 按钮按下之后的效果
            //TODO 按钮旁边的光效之类的东西
            //TODO 融合按钮
            //TODO 融合后的气场


            //spriteBatch.Draw(Texture.Value, parentCen, null, Color.White * factor * .5f, 0f, Texture.Size() / 2f, 1f, SpriteEffects.None, 0f);

            if (IsMouseHovering)
                Main.instance.MouseText(SpellName);

        }
    }
    public class ElementSkillPlayer : ModPlayer
    {
        /// <summary>
        /// 输入元素下标，输出技能下标
        /// </summary>
        public int[] skillIndex = new int[7];
        /// <summary>
        /// 符卡名，快乐打表
        /// </summary>
        public static string[,] skillName = new string[7, 3];
        /// <summary>
        /// 元素消耗打表
        /// </summary>
        public static float[,] skillCost = new float[7, 3];
        /// <summary>
        /// 方便访问，仅此而已
        /// </summary>
        public float[,] SkillCost => skillCost;
        public static int[] skillCounts = new int[] { 1, 2, 3, 1, 2, 1, 1 };
        public override void Load()
        {
            for (int n = 0; n < 7; n++)
            {
                string ElementName = n switch
                {
                    0 => "焱火",
                    1 => "淼水",
                    2 => "森木",
                    3 => "鑫金",
                    4 => "垚土",
                    5 => "寒月",
                    6 or _ => "炎日"
                };
                for (int i = 0; i < skillCounts[n]; i++)
                {
                    string SpellName = n switch
                    {
                        0 => "灼炎炼狱",
                        1 => i switch { 0 => "穿石之流", 1 or _ => "潮汐领域" },
                        2 => i switch { 0 => "常青藤鞭", 1 => "巨木之晶", 2 or _ => "愈伤组织" },
                        3 => "钢铁洪流",
                        4 => i switch { 0 => "大地之柱", 1 or _ => "山崩地裂" },
                        5 => "月影降临",
                        6 or _ => "歌未竟"
                    };
                    int SpellCost = n switch
                    {
                        0 => 1,
                        1 => i switch { 0 => 1, 1 or _ => 2 },
                        2 => i switch { 0 => 1, 1 => 3, 2 or _ => 5 },
                        3 => 1,
                        4 => i switch { 0 => 1, 1 or _ => 4 },
                        5 => 1,
                        6 or _ => 1
                    };
                    skillName[n, i] = $"{ElementName}「{SpellName}」";
                    skillCost[n, i] = SpellCost * 20;
                }
            }
        }
        /// <summary>
        /// 输入元素下标，也就是(int){StoneElements} - 1
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public float GetElementCost(int index) => SkillCost[index, skillIndex[index]];
        /// <summary>
        /// 输入元素枚举 似乎用不到的样子
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public float GetElementCost(StoneElements element) => element == 0 ? 0 : GetElementCost((int)element - 1);

        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
            //Main.spriteBatch.Draw(ModContent.Request<Texture2D>("StoneOfThePhilosophers/Images/StarSky_1").Value, default, null, Color.White, 0, default, 1f, 0, 0);
            //Main.spriteBatch.Draw(ModContent.Request<Texture2D>("StoneOfThePhilosophers/Images/StarSky_1").Value, new Rectangle(0, 0, 1920, 1017), Color.White);
            //Main.NewText(Main.screenTarget.Size());
            //drawInfo = default;
            base.ModifyDrawInfo(ref drawInfo);
        }
    }
    public class ElementSkillUI : UIState
    {
        public static bool Visible { get; private set; }
        public static int timer;
        public void Open()
        {
            Visible = true;
            SoundEngine.PlaySound(SoundID.MenuOpen);
            BasePanel.SetUpElementList();
            var Offset = BasePanel.Offset = UIMethods.MouseScreenUI;
            BasePanel.Left.Set(Offset.X - 420, 0f);
            BasePanel.Top.Set(Offset.Y - 128, 0f);
            BasePanel.Recalculate();
        }
        public void Close()
        {
            Visible = false;
            Main.blockInput = false;
            SoundEngine.PlaySound(SoundID.MenuClose);

        }
        public bool CacheSetupElements;
        public ElementSkillPanel BasePanel;
        /// <summary>
        /// 记录手持物品类型，变了就自动关闭
        /// </summary>
        public int itemType;
        public override void Update(GameTime gameTime)
        {
            if (CacheSetupElements)
            {
                BasePanel.SetUpElementList();
                CacheSetupElements = false;
            }
            timer += Visible ? 1 : -1;
            timer = (int)MathHelper.Clamp(timer, 0, 15);
            BasePanel.factor = MathHelper.SmoothStep(0, 1, timer / 15f);
            if (Main.LocalPlayer.HeldItem.type != itemType && Visible) Close();
            base.Update(gameTime);
        }
        //一个底板 一堆按钮 右键切换开关状态 默认水火符 
        public override void OnInitialize()
        {
            #region 贴图加载

            #endregion

            #region 面板初始化
            BasePanel = new ElementSkillPanel()
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
