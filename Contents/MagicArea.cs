using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing;
using LogSpiralLibrary.CodeLibrary.Utilties.Extensions;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using StoneOfThePhilosophers.UI;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;
using StoneOfThePhilosophers.Contents.Philosopher;
using LogSpiralLibrary;
using System.IO;

namespace StoneOfThePhilosophers.Contents;
public abstract class MagicArea : ModProjectile
{

    public const float dis = 64;

    #region 辅助属性
    public int SpecialAttackIndex { get; set; }
    public bool Extra { get; set; } = false;
    protected int AttackCounter { get; private set; }

    public Projectile projectile => Projectile;
    public Player player => Main.player[projectile.owner];
    public bool Released => projectile.timeLeft < 12;
    protected Color MainColor 
    {
        get 
        {
            if (field == default)
                field = StoneOfThePhilosopherProj.ElementColor[Elements];
            return field;
        }
    }
    #endregion

    #region 插值属性
    public float Light => Released ? MathHelper.Lerp(1, 0, (12 - projectile.timeLeft) / 12f) : MathHelper.Clamp(projectile.ai[0] / ChargeTime * 2, 0, 1);
    public float Theta => projectile.ai[0] / 60f * MathHelper.TwoPi;
    public float Alpha
    {
        get
        {
            float value = -MathHelper.Pi / 2f * (1 - 1 / (projectile.velocity.Length() / 64 + 1));
            return MathHelper.SmoothStep(0, value, Utils.GetLerpValue(ChargeTime / 2, ChargeTime, projectile.ai[0], true));
        }
    }
    public float ChargeTime => Extra ? 45 : 75;
    public float Beta => MathHelper.SmoothStep(0, 1, projectile.ai[0] / ChargeTime * 2) * (SpecialAttackIndex == 0 ? projectile.velocity.ToRotation() : -MathHelper.PiOver2);
    public float Size => Released ? MathHelper.Lerp(96, 144, (12 - projectile.timeLeft) / 12f) : 96;
    #endregion

    #region 虚属性
    protected virtual int Cycle => 60;
    protected virtual bool UseMana => (int)projectile.ai[0] % Cycle == 0;
    protected virtual StoneElements Elements => StoneElements.Empty;
    #endregion

    #region 重写函数
    public override string Texture => "StoneOfThePhilosophers/Images/MagicArea_4";
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
    public override void AI()
    {
        #region UpdateProjectile
        projectile.friendly = false;
        projectile.ai[0]++;
        if (projectile.owner == Main.myPlayer)
        {
            Vector2 diff = Main.MouseWorld - player.Center;
            projectile.velocity = diff;
            projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
            projectile.netUpdate = true;
        }
        projectile.Center = player.Center;
        #endregion

        #region UpdatePlayer
        int dir = projectile.direction;
        player.ChangeDir(dir);
        player.itemTime = 2;
        player.itemAnimation = 2;
        player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * dir, projectile.velocity.X * dir);
        #endregion

        if (projectile.timeLeft > 12)
        {
            if (!(player.channel || (SpecialAttackIndex > 0 && projectile.ai[0] < 60)))
                projectile.timeLeft = 12;

            else
            {
                projectile.timeLeft = 14;
                if (SpecialAttackIndex == 0)
                {
                    if (UseMana)
                    {
                        if (!player.CheckMana(player.inventory[player.selectedItem].mana, true))
                            projectile.timeLeft = 12;

                        if (projectile.ai[0] >= ChargeTime)
                        {
                            AttackCounter++;
                            ShootProj(projectile.velocity.SafeNormalize(default));
                        }
                    }
                }
                else 
                {
                    SpecialAttackDustSpawning(projectile, MainColor, (int)projectile.ai[0] == 55);
                    SpecialAttack((int)projectile.ai[0] == 55);
                }

            }
        }
        else if (projectile.ai[0] > ChargeTime / 2 && SpecialAttackIndex == 0) 
        {
            AttackCounter++;
            ShootProj(projectile.velocity.SafeNormalize(default), true);
        }

    }
    public override void SendExtraAI(BinaryWriter writer)
    {
        writer.Write((byte)SpecialAttackIndex);
        writer.Write((ushort)AttackCounter);
        base.SendExtraAI(writer);
    }
    public override void ReceiveExtraAI(BinaryReader reader)
    {
        SpecialAttackIndex = reader.ReadByte();
        AttackCounter = reader.ReadUInt16();
    }
    public override bool ShouldUpdatePosition() => false;
    public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
    {
        Main.instance.DrawCacheProjsOverWiresUI.Add(index);
        base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
    }
    public override bool PreDraw(ref Color lightColor)
    {
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
        Vector3 offset = new(Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) * .5f, 0);
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
            Matrix.CreateScale(scale * new Vector3(Size, Size, dis * (1 + MathF.Cos((float)GlobalTimeSystem.GlobalTime * .03f + n * MathHelper.PiOver2) * .125f))) *
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
                ModAsset.Style_4.Value, null,
                new Vector2(Main.GameUpdateCount, Main.GlobalTimeWrappedHourly) * scaler, false, transform, pass, n == 0, n == 1);
        }
        #endregion
        return false;
    }
    #endregion

    #region 虚函数
    public virtual void ShootProj(Vector2 unit, bool dying = false)
    {

    }

    public virtual void SpecialAttack(bool trigger)
    {

    }
    public static void SpecialAttackDustSpawning(Projectile projectile, Color dustColor, bool trigger) 
    {
        var player = Main.player[projectile.owner];
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
    #endregion

}
