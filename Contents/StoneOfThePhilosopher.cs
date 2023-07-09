using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StoneOfThePhilosophers.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace StoneOfThePhilosophers.Contents
{
    public class StoneOfThePhilosopher : MagicStone
    {
        //public virtual bool Extra => false;
        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            //var elementPlr = player.GetModPlayer<ElementPlayer>();

            //if (player.itemAnimation == player.itemAnimationMax)
            //{
            //    if (player.altFunctionUse != 2 && elementPlr.element1 != 0 && elementPlr.element2 != 0)
            //    {
            //    }
            //    else
            //    {
            //        if (ElementUI.Visible)
            //            ElementSystem.Instance.elementUI.Close();
            //        else
            //        {
            //            ElementUI.IsExtra = Extra;
            //            ElementSystem.Instance.elementUI.Open();

            //        }
            //    }
            //}
        }
        public override bool AltFunctionUse(Player player) => true;
        public override void AddRecipes()
        {
            CreateRecipe().Register();
        }
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("贤者之石");
            // Tooltip.SetDefault("使用元素魔法程度的能力。\n五耀「贤者之石」");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.shoot = ModContent.ProjectileType<StoneOfThePhilosopherProj>();
            item.damage = 30;
            item.mana = 5;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var elementPlr = player.GetModPlayer<ElementPlayer>();
            return player.altFunctionUse != 2 && elementPlr.element1 != 0 && elementPlr.element2 != 0;
        }
        public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
        {
            if (player.altFunctionUse == 2) mult = 0;
        }
    }
    public class StoneOfThePhilosopherEX : StoneOfThePhilosopher
    {
        public override bool Extra => true;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("真·贤者之石");
            // Tooltip.SetDefault("掌握元素魔法程度的能力。\n七耀「贤者之石」");
        }
    }
    public partial class StoneOfThePhilosopherProj : ModProjectile
    {
        public override string Texture => "StoneOfThePhilosophers/Images/MagicArea_1";
        public StoneElements Element1 => ElementPlr.element1;
        public StoneElements Element2 => ElementPlr.element2;
        public ElementCombination ElementCombination => (Element1, Element2);
        /// <summary>
        /// 0为二融合 1为五融合 2为七融合 3为*全融合*
        /// </summary>
        public int CombineState;
        public Projectile projectile => Projectile;
        public Player player => Main.player[projectile.owner];
        public ElementPlayer ElementPlr => player.GetModPlayer<ElementPlayer>();

        public bool Released => projectile.timeLeft < 12;
        public float Light => Released ? MathHelper.Lerp(1, 0, (12 - projectile.timeLeft) / 12f) : MathHelper.Clamp(projectile.ai[0] / ChargeTime * 2, 0, 1);
        public float Theta => projectile.ai[0] / 120f * MathHelper.TwoPi;
        public float Alpha
        {
            get
            {
                float value = -MathHelper.Pi / 2f * (1 - 1 / (projectile.velocity.Length() / 64 + 1));
                return MathHelper.SmoothStep(0, value, Utils.GetLerpValue(ChargeTime / 2, ChargeTime, projectile.ai[0], true));
                if (projectile.ai[0] >= ChargeTime)
                {
                    return -MathHelper.Pi / 3f;
                }
                else if (projectile.ai[0] >= ChargeTime / 2)
                {
                    return -(projectile.ai[0] - ChargeTime / 2) * (projectile.ai[0] - ChargeTime / 2) / ChargeTime / ChargeTime * 4 * MathHelper.Pi / 3f;
                }
                return 0;
            }
        }
        public float ChargeTime => 45f;
        public float Beta => MathHelper.SmoothStep(0, 1, projectile.ai[0] / ChargeTime * 2) * projectile.velocity.ToRotation();
        public float Size => Released ? MathHelper.Lerp(96, 144, (12 - projectile.timeLeft) / 12f) : 96;
        public const float dis = 64;
        public int Cycle => 60;
        public Color MainColor => new Color(248, 191, 37);//Color.Yellow
        public bool UseMana => (int)projectile.ai[0] % Cycle == 0;
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
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            Main.instance.DrawCacheProjsOverWiresUI.Add(index);
            base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            #region 顶点准备
            int MagicFieldCount = 4;
            int vertexCount = 4 * MagicFieldCount;
            CustomVertexInfoEX[] vertexInfos = new CustomVertexInfoEX[vertexCount];//这里是最基本的顶点们
            for (int n = 0; n < vertexCount; n++)
            {
                var vec = new Vector4(n % 2, n / 2 % 2, 0, 1);
                vertexInfos[n].TexCoord = new Vector3(vec.X, vec.Y, Light);
                vertexInfos[n].Color = (n / 4) switch
                {
                    0 or 1 => Color.Gold,
                    2 => ElementColor[Element1],
                    3 or _ => ElementColor[Element2]
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
                    0 or 1 => "OriginColor",
                    2 or 3 or _ => "VertexColor"
                };
                string path = n switch
                {
                    0 => "UI/ElementPanel",
                    1 => "Images/MagicArea_2",
                    2 or 3 or _ => "Images/MagicArea_1"
                };
                Vector2 scaler = n switch
                {
                    0 => new Vector2(-0.09f, 0.6f),
                    1 => new Vector2(-0.15f, 1f),
                    2 or 3 or _ => new Vector2(0.12f, -0.8f)
                };
                Matrix translation = n switch
                {
                    0 or 1 => Matrix.CreateTranslation(-Vector3.One),
                    2 or 3 or _ => Matrix.CreateTranslation(-.5f, -.5f, -1f)
                };
                float theta = n switch
                {
                    0 => 1,
                    1 => -1.5f,
                    2 => -2f,
                    3 or _ => 3f
                } * Theta;
                float sinValue = .25f * MathF.Sin(projectile.ai[0] / 120f * MathHelper.TwoPi);
                Vector3 scale = n switch
                {
                    0 => new Vector3(1, 1, 1f),
                    1 => new Vector3(1.5f, 1.5f, 2f),
                    2 => new Vector3(.5f, .5f, 1.5f + sinValue),
                    3 or _ => new Vector3(-.5f, -.5f, 1.5f - sinValue)
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
                StoneOfThePhilosophersHelper.VertexDrawEX(vertexs[(6 * n)..(6 * n + 6)],
                    ModContent.Request<Texture2D>($"StoneOfThePhilosophers/{path}").Value,
                    ModContent.Request<Texture2D>("StoneOfThePhilosophers/Images/Style_4").Value, null,
                    new Vector2(Main.GameUpdateCount, Main.GlobalTimeWrappedHourly) * scaler, false, transform, pass, n == 0, false);
            }
            #endregion
            #region 环环
            CustomVertexInfoEX[] loopVertexInfos = new CustomVertexInfoEX[62];
            for (int n = 0; n < 31; n++)
            {
                float factor = n / 30f;
                loopVertexInfos[2 * n].Position = loopVertexInfos[2 * n + 1].Position = new Vector4((factor * MathHelper.TwoPi).ToRotationVector2(), -1.5f, 1);
                loopVertexInfos[2 * n + 1].Position *= .8f;
                loopVertexInfos[2 * n + 1].Position.Z = -1f;
                loopVertexInfos[2 * n + 1].Position.W = 1f;
                loopVertexInfos[2 * n].TexCoord = new Vector3(factor * 2, 0, Light);
                loopVertexInfos[2 * n + 1].TexCoord = new Vector3(factor * 2, 1, Light);
                loopVertexInfos[2 * n].Color = Color.Gold * MathHelper.Clamp((projectile.ai[0] - ChargeTime) - n, 0, 1);
                loopVertexInfos[2 * n + 1].Color = default;
            }
            Vector2 loopScaler = new Vector2(0, 0.6f);
            float loopTheta = Theta * .5f;
            Vector3 loopScale = new Vector3(2f, 2f, 2f);
            Matrix loopTransform =
            Matrix.CreateScale(loopScale * new Vector3(Size, Size, dis)) *
            Matrix.CreateRotationZ(loopTheta) *
            Matrix.CreateRotationY(Alpha) *
            Matrix.CreateRotationZ(Beta) *
            Matrix.CreateTranslation(new Vector3(projectile.Center, 0) - offset) *
            new Matrix(height, 0, 0, 0,
                            0, height, 0, 0,
                            0, 0, 0, -1,
                            0, 0, 0, height) *
            Matrix.CreateTranslation(offset);
            StoneOfThePhilosophersHelper.VertexDrawEX(loopVertexInfos,
                ModContent.Request<Texture2D>("StoneOfThePhilosophers/Images/line_1").Value,
                ModContent.Request<Texture2D>("StoneOfThePhilosophers/Images/Style_4").Value, null,
                new Vector2(Main.GameUpdateCount, Main.GlobalTimeWrappedHourly) * loopScaler, true, loopTransform, null, false, true);
            #endregion
            return false;
        }
        public override void Kill(int timeLeft)
        {
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
                if (projectile.ai[0] > (ChargeTime + 30))// && projectile.ai[0] > 300
                {
                    projectile.timeLeft = 12;
                }
                else
                {
                    projectile.timeLeft = 14;
                    if (projectile.ai[0] < ChargeTime / 2)
                    {
                        var fac = projectile.ai[0] * 2 / ChargeTime;
                        fac *= 4 * (1 - fac);

                    }
                    if (UseMana)
                    {
                        if (!player.CheckMana(player.inventory[player.selectedItem].mana, true))
                        {
                            projectile.timeLeft = 12;
                        }
                        if (projectile.ai[0] >= 30)
                            ShootProj();
                    }
                }
            }
            else
            {
                if (projectile.ai[0] > 15f)
                {
                    ShootProj(true);
                }
            }
        }
    }
}
