using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.GameContent;
using Terraria.DataStructures;
using Terraria.UI;
using UtfUnknown.Core.Probers.MultiByte.Chinese;
using StoneOfThePhilosophers.UI;
using Terraria.GameInput;
using LogSpiralLibrary.CodeLibrary.DataStructures;

namespace StoneOfThePhilosophers.Contents
{
    public static class StoneOfThePhilosophersHelper
    {
        public static (ElementChargePlayer, ElementSkillPlayer) ElementPlayer(this Player player) => (player.GetModPlayer<ElementChargePlayer>(), player.GetModPlayer<ElementSkillPlayer>());
    }
    public abstract class MagicArea : ModProjectile
    {
        /// <summary>
        /// 0让给正常攻击了，需要减一以对应那边的下标
        /// </summary>
        public int specialAttackIndex;
        public int attackCounter;
        public override string Texture => "StoneOfThePhilosophers/Images/MagicArea_4";//{StarBound.NPCs.Bosses.BigApe.BigApeTools.ApePath}StrawBerryArea
        public bool Extra { get; set; } = false;
        public virtual StoneElements Elements => StoneElements.Empty;
        public Projectile projectile => Projectile;
        public Player player => Main.player[projectile.owner];
        public bool Released => projectile.timeLeft < 12;
        public float Light => Released ? MathHelper.Lerp(1, 0, (12 - projectile.timeLeft) / 12f) : MathHelper.Clamp(projectile.ai[0] / ChargeTime * 2, 0, 1);
        public float Theta => projectile.ai[0] / 60f * MathHelper.TwoPi;
        //public float Alpha
        //{
        //    get
        //    {

        //        if (projectile.ai[0] >= 120)
        //        {
        //            return -MathHelper.Pi / 3f;
        //        }
        //        else if (projectile.ai[0] >= 60)
        //        {
        //            return -(projectile.ai[0] - 60) * (projectile.ai[0] - 60) / 3600f * MathHelper.Pi / 3f;
        //        }
        //        return 0;
        //    }
        //}
        //public float Beta => MathHelper.SmoothStep(0, 1, projectile.ai[0] / 60f) * projectile.velocity.ToRotation();
        public float Alpha
        {
            get
            {
                float value = -MathHelper.Pi / 2f * (1 - 1 / (projectile.velocity.Length() / 64 + 1));
                return MathHelper.SmoothStep(0, value, Utils.GetLerpValue(ChargeTime / 2, ChargeTime, projectile.ai[0], true));
            }
        }
        public float ChargeTime => Extra ? 45 : 75;
        public float Beta => MathHelper.SmoothStep(0, 1, projectile.ai[0] / ChargeTime * 2) * (specialAttackIndex == 0 ? projectile.velocity.ToRotation() : -MathHelper.PiOver2);
        public float Size => Released ? MathHelper.Lerp(96, 144, (12 - projectile.timeLeft) / 12f) : 96;
        public const float dis = 64;
        public virtual int Cycle => 60;
        public virtual Color MainColor => Color.White;
        public virtual bool UseMana => (int)projectile.ai[0] % Cycle == 0;
        public override void SetDefaults()
        {
            projectile.width = 1;
            projectile.height = 1;
            projectile.aiStyle = -1;
            projectile.friendly = false;
            projectile.DamageType = DamageClass.Magic;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.hide = true;
            //vertexInfos[0] = new CustomVertexInfo(default, MainColor, new Vector3(0, 0, 0));
            //vertexInfos[1] = new CustomVertexInfo(default, MainColor, new Vector3(1, 0, 0));
            //vertexInfos[2] = new CustomVertexInfo(default, MainColor, new Vector3(1, 1, 0));
            //vertexInfos[3] = new CustomVertexInfo(default, MainColor, new Vector3(0, 1, 0));
        }
        //public Vector2 GetVec(Vector3 vector, float d = dis, bool negativeTheta = false)
        //{
        //    float x = vector.X;
        //    float y = vector.Y;
        //    float z = vector.Z;
        //    float sA = (float)Math.Sin(Alpha);
        //    float cA = (float)Math.Cos(Alpha);
        //    float sB = (float)Math.Sin(Beta);
        //    float cB = (float)Math.Cos(Beta);
        //    float sT = (float)Math.Sin(Theta) * (negativeTheta ? -1 : 1);
        //    float cT = (float)Math.Cos(Theta);
        //    float value1 = cA * (cT * x - sT * y) - sA * (z + d);
        //    float value2 = sT * x + cT * y;
        //    return l / (l - z) * new Vector2(cB * value1 - sB * value2, sB * value1 + cB * value2);
        //}
        //public CustomVertexInfo[] vertexInfos = new CustomVertexInfo[4];
        public virtual void ShootProj(Vector2 unit, bool dying = false)
        {

        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            Main.instance.DrawCacheProjsOverWiresUI.Add(index);
            base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
        }
        //public virtual void UpdateVertex()
        //{
        //    for (int n = 0; n < vertexInfos.Length; n++)
        //    {
        //        vertexInfos[n].Position = GetVec(new Vector3(vertexInfos[n].TexCoord.X - 0.5f, vertexInfos[n].TexCoord.Y - 0.5f, 0) * Size) + player.Center;
        //        vertexInfos[n].TexCoord.Z = Light;
        //        if (MainColor != vertexInfos[n].Color)
        //        {
        //            vertexInfos[n].Color = MainColor;
        //        }
        //    }
        //}
        public override bool PreDraw(ref Color lightColor)
        {
            //Matrix transform =
            //Matrix.CreateScale(2) *
            //Matrix.CreateTranslation(-1, -1, -1) *
            //new Matrix(Size, 0, 0, 0,
            //              0, Size, 0, 0,
            //              0, 0, dis, 0,
            //              0, 0, 0, 1) *
            //Matrix.CreateRotationZ(Theta) *
            //Matrix.CreateRotationY(Alpha) *
            //Matrix.CreateRotationZ(Beta);
            //CustomVertexInfo[] vertexInfos = new CustomVertexInfo[8];
            //for (int n = 0; n < 8; n++)
            //{
            //    var vec = new Vector3(n % 2, n / 2 % 2, 0);
            //    vertexInfos[n].TexCoord = new Vector3(vec.X, vec.Y, Light);
            //    vertexInfos[n].Color = MainColor;
            //    vec = Vector3.Transform(vec, transform);
            //    if (n == 3)
            //    {
            //        transform =
            //        Matrix.CreateScale(2) *
            //        Matrix.CreateTranslation(-1, -1, -1) *
            //        new Matrix(Size * 1.5f, 0, 0, 0,
            //                             0, Size * 1.5f, 0, 0,
            //                             0, 0, dis * 1.5f, 0,
            //                             0, 0, 0, 1) *
            //        Matrix.CreateRotationZ(-Theta * 1.5f) *
            //        Matrix.CreateRotationY(Alpha) *
            //        Matrix.CreateRotationZ(Beta);
            //    }
            //    vec += new Vector3(projectile.Center - Main.screenPosition - new Vector2(Main.screenWidth, Main.screenHeight) * .5f, 0);
            //    vec.Z = (2000 - vec.Z) / 2000f;
            //    vec /= vec.Z;
            //    vertexInfos[n].Position = new Vector2(vec.X, vec.Y) + Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) * .5f;
            //}
            //CustomVertexInfo[] vertexs = new CustomVertexInfo[12];
            //for (int n = 0; n < 12; n++)
            //{
            //    int index = (n % 6) switch
            //    {
            //        0 => 0,
            //        1 => 2,
            //        2 => 1,
            //        3 => 1,
            //        4 => 2,
            //        5 or _ => 3,
            //    };
            //    if (n > 5) index += 4;
            //    vertexs[n] = vertexInfos[index];
            //}
            //StoneOfThePhilosophersHelper.VertexDraw(vertexs[0..6],
            //    ModContent.Request<Texture2D>("StoneOfThePhilosophers/Images/MagicArea_1").Value,
            //    ModContent.Request<Texture2D>("StoneOfThePhilosophers/Images/Style_4").Value,
            //    new Vector2(Main.GameUpdateCount * -0.09f, Main.GlobalTimeWrappedHourly * 0.6f));
            //StoneOfThePhilosophersHelper.VertexDraw(vertexs[6..12],
            //    ModContent.Request<Texture2D>("StoneOfThePhilosophers/Images/MagicArea_2").Value,
            //    ModContent.Request<Texture2D>("StoneOfThePhilosophers/Images/Style_4").Value,
            //    new Vector2(Main.GameUpdateCount * -0.15f, Main.GlobalTimeWrappedHourly));
            #region 顶点准备
            int MagicFieldCount = 2;
            int vertexCount = 4 * MagicFieldCount;
            CustomVertexInfoEX[] vertexInfos = new CustomVertexInfoEX[vertexCount];//这里是最基本的顶点们
            for (int n = 0; n < vertexCount; n++)
            {
                var vec = new Vector4(n % 2, n / 2 % 2, 0, 1);
                vertexInfos[n].TexCoord = new Vector3(vec.X, vec.Y, Light);
                vertexInfos[n].Color = (n / 4) switch
                {
                    0 or 1 or _ => MainColor
                };
                //vertexInfos[n].Color = n < 4 ? Color.Red : Color.Cyan;
                vertexInfos[n].Position = new Vector4(n % 2, n / 2 % 2, 0, 1);
            }
            #endregion
            #region 顶点连接
            vertexCount = 6 * MagicFieldCount;
            CustomVertexInfoEX[] vertexs = new CustomVertexInfoEX[vertexCount];//三角形会共用顶点对吧，所以我就不得不准备个大一点的数组然后给所有的三角形安排上自己的顶点
            for (int n = 0; n < vertexCount; n++)
            {
                int index = (n % 6) switch
                {
                    0 => 0,
                    1 => 2,
                    2 => 1,
                    3 => 1,
                    4 => 2,
                    5 or _ => 3,
                };
                index += n / 6 * 4;
                vertexs[n] = vertexInfos[index];
            }
            #endregion
            #region 矩阵生成与绘制
            float height = 2000f;
            Vector3 offset = new Vector3(Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) * .5f, 0);
            for (int n = 0; n < MagicFieldCount; n++)
            {
                string pass = n switch
                {
                    0 or 1 or _ => "VertexColor"
                };
                string path = n switch
                {
                    0 => "Images/MagicArea_1",
                    1 or _ => "Images/MagicArea_2"
                };
                Vector2 scaler = n switch
                {
                    0 => new Vector2(-0.09f, 0.6f),
                    1 or _ => new Vector2(-0.15f, 1f)
                };
                Matrix translation = n switch
                {
                    0 or 1 or _ => Matrix.CreateTranslation(-Vector3.One)
                };
                float theta = n switch
                {
                    0 => 1,
                    1 or _ => -1.5f
                } * Theta;
                Vector3 scale = n switch
                {
                    0 => new Vector3(1, 1, 1f),
                    1 or _ => new Vector3(1.5f, 1.5f, 2f)
                };
                //声明矩阵transform
                //缩放为原来的两倍
                //平移至以原点为中心
                //最重要的缩放矩阵
                //旋转量一号，这个是法阵旋转动画，去掉了就是静止的3d法阵
                //旋转量二号，这个是最重要的之一，把朝向你的法阵逐渐转到朝向角色前方
                //旋转量三号，这个让法阵能跟着鼠标走
                //平移，确保投影中心正确
                //投影
                //平移回去
                //非常ez啊
                Matrix transform =
                Matrix.CreateScale(2) *
                translation *
                Matrix.CreateScale(scale * new Vector3(Size, Size, dis)) *
                Matrix.CreateRotationZ(theta) *
                Matrix.CreateRotationY(Alpha) *
                Matrix.CreateRotationZ(Beta) *
                Matrix.CreateTranslation(new Vector3(projectile.Center, 0) - offset) *
                new Matrix(height, 0, 0, 0,
                                0, height, 0, 0,
                                0, 0, 0, -1,
                                0, 0, 0, height) *
                Matrix.CreateTranslation(offset);
                DrawingMethods.VertexDrawEX(vertexs[(6 * n)..(6 * n + 6)],
                    ModContent.Request<Texture2D>($"StoneOfThePhilosophers/{path}").Value,
                    ModContent.Request<Texture2D>("StoneOfThePhilosophers/Images/Style_4").Value, null,
                    new Vector2(Main.GameUpdateCount, Main.GlobalTimeWrappedHourly) * scaler, false, transform, pass, n == 0, n == 1);
            }
            #endregion
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void Kill(int timeLeft)
        {
            //if (projectile.ai[0] >= 60f)
            //{
            //    for (int n = 0; n < 10; n++)
            //    {
            //    }
            //}
        }
        public override void AI()
        {
            projectile.friendly = false;
            //if (projectile.ai[0] == 0) 
            //{

            //}
            projectile.ai[0]++;
            //UpdateVertex();
            if (projectile.owner == Main.myPlayer)
            {
                Vector2 diff = Main.MouseWorld - player.Center;
                //diff.Normalize();
                projectile.velocity = diff;
                projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
                projectile.netUpdate = true;
            }
            projectile.Center = player.Center;
            int dir = projectile.direction;
            player.ChangeDir(dir);
            //player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * dir, projectile.velocity.X * dir);
            if (projectile.timeLeft > 12)
            {
                if (!(player.channel || (specialAttackIndex > 0 && projectile.ai[0] < 60)))
                {
                    projectile.timeLeft = 12;
                }
                else
                {
                    projectile.timeLeft = 14;
                    if (specialAttackIndex == 0)
                    {
                        if (UseMana)
                        {
                            if (!player.CheckMana(player.inventory[player.selectedItem].mana, true))
                            {
                                projectile.timeLeft = 12;
                            }
                            if (projectile.ai[0] >= ChargeTime)
                                ShootProj(projectile.velocity.SafeNormalize(default));
                        }
                    }
                    else
                    {
                        SpecialAttack(StoneOfThePhilosopherProj.ElementColor[Elements], (int)projectile.ai[0] == 55);
                    }
                }
            }
            else
            {
                if (projectile.ai[0] > ChargeTime / 2 && specialAttackIndex == 0)
                {
                    ShootProj(projectile.velocity.SafeNormalize(default), true);
                }
            }
        }
        public virtual void SpecialAttack(Color dustColor, bool trigger)
        {
            dustColor = Color.Lerp(Color.White, dustColor, 0.5f);
            if (projectile.ai[0] > 30 && projectile.ai[0] < 55 && (int)projectile.ai[0] % 5 == 0)
            {
                float factor = Utils.GetLerpValue(30, 55, projectile.ai[0]);
                for (int n = 0; n < 30 * factor; n++)
                {
                    var unit = Main.rand.NextVector2Unit();
                    Dust.NewDustPerfect(player.Center + ((1 - factor / 2) * 64 + Main.rand.Next(-4, 4)) * unit, 278, -unit * 8 * (1 - factor), 0, dustColor, Main.rand.NextFloat(1, 2f) * (1 - factor)).noGravity = true;
                }
            }
            if (trigger)
            {
                var r = Main.rand.NextFloat();
                for (int n = 0; n < 60; n++)
                {
                    var unit = Main.rand.NextVector2Unit() * new Vector2(8, 4);
                    unit = unit.RotatedBy(r);
                    Dust.NewDustPerfect(player.Center, 278, unit, 0, dustColor, Main.rand.NextFloat(1, 2f)).noGravity = true;
                }
            }
        }
    }
    public abstract class MagicStone : ModItem
    {
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var tooltip = new TooltipLine(Mod, "OpenUI", "使用鼠标中键以开启配置ui");
            tooltips.Add(tooltip);
            base.ModifyTooltips(tooltips);
        }
        public override void HoldStyle(Player player, Rectangle heldItemFrame)
        {
            uiTimer--;
            if (PlayerInput.MouseInfo.MiddleButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && uiTimer <= 0)
            {
                if (Extra && Type != ModContent.ItemType<StoneOfThePhilosopher>() && Type != ModContent.ItemType<StoneOfThePhilosopherEX>())
                {
                    if (ElementSkillUI.Visible)
                        ElementSkillSystem.Instance.skillUI.Close();
                    else
                    {
                        ElementSkillSystem.Instance.skillUI.Open();
                        ElementSkillSystem.Instance.skillUI.itemType = Type;
                    }
                }
                else
                {
                    if (ElementUI.Visible)
                        ElementSystem.Instance.elementUI.Close();
                    else
                    {
                        ElementUI.IsExtra = Extra;
                        ElementSystem.Instance.elementUI.Open();
                    }
                }
                uiTimer = 15;
            }
            base.HoldStyle(player, heldItemFrame);
        }
        public int uiTimer;
        public virtual bool Extra => false;
        public virtual StoneElements Elements => StoneElements.Empty;
        public override bool AltFunctionUse(Player player)
        {
            if (Elements == 0 || !Extra) return false;
            var (c, s) = player.ElementPlayer();
            int index = (int)Elements - 1;
            return c.ElementChargeValue[index] >= s.GetElementCost(index);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var (c, s) = player.ElementPlayer();
            var area = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI, 0, 0).ModProjectile as MagicArea;
            area.Extra = Extra;
            if (player.altFunctionUse == 2 && Elements != 0 && Extra)
            {
                var index = (int)Elements - 1;
                area.specialAttackIndex = s.skillIndex[index] + 1;
                c.ElementChargeValue[index] -= s.GetElementCost(index);
            }
            return false;
        }
        public static void AddEXRequire<T>(Recipe recipe, bool metalStone = false) where T : MagicStone
        {
            recipe.AddIngredient<T>();
            recipe.AddRecipeGroup(StoneOfThePhilosophersSystem.CobaltPalladiumBars, metalStone ? 20 : 10);
            recipe.AddRecipeGroup(StoneOfThePhilosophersSystem.MythrilOrichalcumBars, metalStone ? 16 : 8);
            recipe.AddRecipeGroup(StoneOfThePhilosophersSystem.AdamantiteTitaniumBars, metalStone ? 10 : 5);
            recipe.AddIngredient(ItemID.SoulofLight, 15);
            recipe.AddIngredient(ItemID.SoulofNight, 15);
            recipe.AddTile(TileID.CrystalBall);
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.ManaCrystal, 3);
            recipe.AddTile(TileID.DemonAltar);
            AddOtherIngredients(recipe);
            recipe.Register();
        }
        public virtual void AddOtherIngredients(Recipe recipe)
        {
            //recipe.AddIngredient(ItemID.HellstoneBar, 15);
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[item.shoot] < 1;
        }
        public Item item => Item;
        public override void SetDefaults()
        {
            item.DamageType = DamageClass.Magic;
            item.width = 34;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.height = 40;
            item.rare = ItemRarityID.Cyan;
            item.autoReuse = true;
            item.useAnimation = 12;
            item.useTime = 12;
            item.useStyle = ItemUseStyleID.Shoot;
            item.channel = true;
            item.value = 150;
            item.knockBack = 4f;
            item.shootSpeed = 10;
            item.damage = 30;
            item.mana = 15;
        }
    }
}
