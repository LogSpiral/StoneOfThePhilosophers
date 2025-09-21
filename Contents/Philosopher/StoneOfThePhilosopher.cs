using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing;
using LogSpiralLibrary.CodeLibrary.Utilties.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StoneOfThePhilosophers.Contents.Earth;
using StoneOfThePhilosophers.Contents.Fire;
using StoneOfThePhilosophers.Contents.Metal;
using StoneOfThePhilosophers.Contents.Moon;
using StoneOfThePhilosophers.Contents.Sun;
using StoneOfThePhilosophers.Contents.Water;
using StoneOfThePhilosophers.Contents.Wood;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace StoneOfThePhilosophers.Contents.Philosopher;

public class StoneOfThePhilosopher : MagicStone
{
    public const string DebuffTexturePath = "StoneOfThePhilosophers/Contents/Philosopher/Attacks/DebuffTemplate";

    public override bool AltFunctionUse(Player player)
    {
        var cplr = player.GetModPlayer<ElementCombinePlayer>();
        var combination = Extra ? cplr.CombinationEX : cplr.Combination;
        if (combination.Mode != 1) return false;

        var mplr = player.GetModPlayer<ElementSkillPlayer>();
        int index = (int)combination.MainElements - 1;
        return mplr.ElementChargeValue[index] >= mplr.GetElementCost(index);
    }

    public override void UpdateEquip(Player player)
    {
        int m = Main.debuff.Length;
        for (int n = 0; n < m; n++)
            if (Main.debuff[n])
                player.buffImmune[n] = true;
        base.UpdateEquip(player);
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<StoneOfFireEX>()
            .AddIngredient<StoneOfWaterEX>()
            .AddIngredient<StoneOfMetalEX>()
            .AddIngredient<StoneOfWoodEX>()
            .AddIngredient<StoneOfEarthEX>()
            .AddIngredient(ItemID.Ectoplasm, 10)
            .AddIngredient(ItemID.SoulofMight, 10)
            .AddIngredient(ItemID.SoulofFright, 10)
            .AddIngredient(ItemID.SoulofSight, 10)
            .AddTile(TileID.CrystalBall)
            .Register();
    }

    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.shoot = ModContent.ProjectileType<StoneOfThePhilosopherProj>();
        Item.damage = 80;
        Item.mana = 5;
    }

    public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
    {
        mult = 0;
    }
}

public class StoneOfThePhilosopherEX : StoneOfThePhilosopher
{
    public override bool Extra => true;

    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.damage = 120;
        Item.mana = 5;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<StoneOfThePhilosopher>()
            .AddIngredient<StoneOfMoon>()
            .AddIngredient<StoneOfSun>()
            .AddIngredient(ItemID.FragmentSolar, 15)
            .AddIngredient(ItemID.FragmentNebula, 15)
            .AddIngredient(ItemID.FragmentVortex, 15)
            .AddIngredient(ItemID.FragmentStardust, 15)
            .AddIngredient(ItemID.LunarBar, 30)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
    }
}

public partial class StoneOfThePhilosopherProj : ModProjectile
{
    public override string Texture => "StoneOfThePhilosophers/Images/MagicArea_1";
    public const float dis = 64;

    #region 辅助属性

    public Projectile projectile => Projectile;
    public Player player => Main.player[projectile.owner];
    public ElementCombinePlayer ElementPlr => player.GetModPlayer<ElementCombinePlayer>();

    public int Cycle
    {
        get
        {
            if (field != -1) return field;
            ElementCombination combination = Extra ? ElementPlr.CombinationEX : ElementPlr.Combination;

            int state = combination.Mode;
            if (state == 1)
            {
                field = combination.MainElements switch
                {
                    StoneElements.Fire => 30,
                    StoneElements.Water => player.HasBuff<WaterUltra>() ? 4 : 6,
                    StoneElements.Wood => 18,
                    StoneElements.Metal => 18,
                    StoneElements.Soil => 24,
                    StoneElements.Lunar => (int)((Main.dayTime ? 18 : 12) * (player.HasBuff<BlessingFromLunarGod>() ? 0.75f : 1f)),
                    StoneElements.Solar or _ => 45
                };
            }
            else
                field = 60;

            return field;
        }
    } = -1;

    public Color MainColor => new(248, 191, 37);//Color.Yellow
    public bool UseMana => (int)Timer % Cycle == 0;
    public bool Extra { get; set; }

    public int SpecialAttackIndex { get; set; }
    protected int AttackCounter { get; private set; }

    #endregion 辅助属性

    #region 插值属性

    public bool Released => projectile.timeLeft < 12;
    public float Light => Released ? MathHelper.Lerp(1, 0, (12 - projectile.timeLeft) / 12f) : MathHelper.Clamp(Timer / ChargeTime * 2, 0, 1);
    public float Theta => Timer / 120f * MathHelper.TwoPi;

    public float Alpha
    {
        get
        {
            float value = -MathHelper.Pi / 2f * (1 - 1 / (projectile.velocity.Length() / 64 + 1));
            return MathHelper.SmoothStep(0, value, Utils.GetLerpValue(ChargeTime / 2, ChargeTime, Timer, true));
        }
    }

    public float ChargeTime => 45f;
    public float Beta => MathHelper.SmoothStep(0, 1, Timer / ChargeTime * 2) * (SpecialAttackIndex == 0 ? projectile.velocity.ToRotation() : -MathHelper.PiOver2);
    public float Size => Released ? MathHelper.Lerp(96, 144, (12 - projectile.timeLeft) / 12f) : 96;

    #endregion 插值属性

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

    public override bool ShouldUpdatePosition() => false;

    public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
    {
        Main.instance.DrawCacheProjsOverWiresUI.Add(index);
        base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
    }

    public override bool PreDraw(ref Color lightColor)
    {
        #region 顶点准备

        ElementCombination combination = Extra ? ElementPlr.CombinationEX : ElementPlr.Combination;

        int state = combination.Mode;

        int MagicFieldCount = state switch
        {
            0 => Extra ? 9 : 7,
            1 => 3,
            2 or _ => 4,
        };
        int vertexCount = 4 * MagicFieldCount;
        CustomVertexInfoEX[] vertexInfos = new CustomVertexInfoEX[vertexCount];//这里是最基本的顶点们
        for (int n = 0; n < vertexCount; n++)
        {
            var vec = new Vector4(n % 2, n / 2 % 2, 0, 1);
            vertexInfos[n].TexCoord = new Vector3(vec.X, vec.Y, Light);
            vertexInfos[n].Color = Color.Gold;
            if (n > 7)
            {
                vertexInfos[n].Color = state switch
                {
                    0 => ElementColor[(StoneElements)(n / 4 - 1)],
                    1 => ElementColor[combination.MainElements],
                    2 or _ => ElementColor[n > 11 ? combination.RealCoElements : combination.MainElements]
                };
            }

            //vertexInfos[n].Color = n < 4 ? Color.Red : Color.Cyan;
            vertexInfos[n].Position = new Vector4(n % 2, n / 2 % 2, 0, 1);
        }

        #endregion 顶点准备

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

        #endregion 顶点连接

        #region 矩阵生成与绘制

        float height = 2000f;
        Vector3 offset = new(Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) * .5f, 0);
        for (int n = 0; n < MagicFieldCount; n++)
        {
            string pass = n switch
            {
                0 or 1 => "OriginColor",
                _ => "VertexColor"
            };
            string path = n switch
            {
                0 => "UI/ElementPanel",
                1 => "Images/MagicArea_2",
                _ => "Images/MagicArea_1"
            };
            Vector2 uTimeScaler = n switch
            {
                0 => new Vector2(-0.09f, 0.6f),
                1 => new Vector2(-0.15f, 1f),
                2 or 3 or _ => new Vector2(0.12f, -0.8f)
            };
            Matrix translation = Matrix.CreateTranslation(-Vector3.One);
            if (state is 0 or 2 && n > 1)
            {
                if (state == 0)
                {
                    float s = -MathF.Sin(MathF.Sqrt(Timer / Cycle) * MathHelper.TwoPi);
                    translation = Matrix.CreateTranslation(s, s, -1f);
                }
                else
                    translation = Matrix.CreateTranslation(-.5f, -.5f, -1f);
            }
            float theta = n switch
            {
                0 => 1,
                1 => -1.5f,
                2 => -2f,
                3 or _ => 3f
            } * Theta;

            if (state == 0 && n > 1)
                theta = n * MathHelper.TwoPi / (Extra ? 7 : 5f) - Theta;

            Vector3 scale = default;
            if (n > 1)
                switch (state)
                {
                    case 0:
                        float sinValue = .25f * MathF.Sin((Timer / 120f + (n - 2) / (Extra ? 7f : 5f)) * MathHelper.TwoPi);
                        scale = new Vector3(.5f, .5f, 1.5f + sinValue);
                        break;

                    case 1:
                        scale = new Vector3(1.25f, 1.25f, 1.5f);
                        break;

                    case 2:
                        sinValue = .25f * MathF.Sin(Timer / 120f * MathHelper.TwoPi);
                        scale = new Vector3(.5f, .5f, 1.5f + sinValue * (n % 2 == 0 ? 1 : -1));
                        break;
                }
            else
                scale = n switch
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
                ModAsset.Style_4.Value, null,
                new Vector2(Main.GameUpdateCount, Main.GlobalTimeWrappedHourly) * uTimeScaler, false, transform, pass, n == 0, false);
        }

        #endregion 矩阵生成与绘制

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
            loopVertexInfos[2 * n].Color = Color.Gold * MathHelper.Clamp(Timer - ChargeTime - n, 0, 1);
            loopVertexInfos[2 * n + 1].Color = default;
        }
        Vector2 loopScaler = new(0, 0.6f);
        float loopTheta = Theta * .5f;
        Vector3 loopScale = new(2f, 2f, 2f);
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
        DrawingMethods.VertexDrawEX(loopVertexInfos,
            ModAsset.line_1.Value,
            ModAsset.Style_4.Value, null,
            new Vector2(Main.GameUpdateCount, Main.GlobalTimeWrappedHourly) * loopScaler, true, loopTransform, null, false);

        #endregion 环环

        return false;
    }

    public override void OnKill(int timeLeft)
    {
    }

    public float Timer
    {
        get => projectile.ai[0];
        set => projectile.ai[0] = value;
    }

    public override void AI()
    {
        #region UpdateProjectile

        Timer++;
        projectile.friendly = false;
        if (projectile.owner == Main.myPlayer)
        {
            Vector2 diff = Main.MouseWorld - player.Center;
            projectile.velocity = diff;
            projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
            projectile.netUpdate = true;
        }
        projectile.Center = player.Center;

        #endregion UpdateProjectile

        #region UpdatePlayer

        int dir = projectile.direction;
        player.ChangeDir(dir);
        player.itemTime = 2;
        player.itemAnimation = 2;
        player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * dir, projectile.velocity.X * dir);

        #endregion UpdatePlayer

        ElementCombination combination = Extra ? ElementPlr.CombinationEX : ElementPlr.Combination;
        int state = combination.Mode;
        if (!Released)
        {
            if ((state == 1 && !player.channel && (SpecialAttackIndex == 0 || Timer > 60)) || (state != 1 && (Timer > ChargeTime + 45)))
                projectile.timeLeft = 12;
            else
            {
                projectile.timeLeft = 14;
                if (state == 1 && SpecialAttackIndex != 0)
                {
                    MagicArea.SpecialAttackDustSpawning(Projectile, elementColor[combination.MainElements], Timer == 55);
                    HandleSpecialAttack(combination.MainElements);
                }
                else if (UseMana)
                {
                    if (!player.CheckMana(player.inventory[player.selectedItem].mana, true))
                        projectile.timeLeft = 12;
                    else if (Timer >= ChargeTime)
                        ShootProj(combination);
                }
            }
        }
        else
            if (SpecialAttackIndex == 0)
            ShootProj(combination, true);
    }
}