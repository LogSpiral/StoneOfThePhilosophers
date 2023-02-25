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
        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            var elementPlr = player.GetModPlayer<ElementPlayer>();

            if (player.itemAnimation == player.itemAnimationMax)
            {
                if (player.altFunctionUse != 2 && elementPlr.element1 != 0 && elementPlr.element2 != 0)
                {
                }
                else
                {
                    if (ElementUI.Visible)
                        ElementSystem.Instance.elementUI.Close();
                    else 
                    {
                        ElementUI.IsSeven = !ElementUI.IsSeven;
                        ElementSystem.Instance.elementUI.Open();

                    }
                }
            }
        }
        public override bool AltFunctionUse(Player player) => true;
        public override void AddRecipes()
        {
            CreateRecipe().Register();
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("贤者之石");
            Tooltip.SetDefault("使用元素魔法程度的能力。\n五耀「贤者之石」");
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
    public partial class StoneOfThePhilosopherProj : ModProjectile
    {
        public override string Texture => "StoneOfThePhilosophers/MagicArea_1";
        public StoneElements Element1 => ElementPlr.element1;
        public StoneElements Element2 => ElementPlr.element2;
        public ElementCombination ElementCombination => (Element1, Element2);

        public Projectile projectile => Projectile;
        public Player player => Main.player[projectile.owner];
        public ElementPlayer ElementPlr => player.GetModPlayer<ElementPlayer>();

        public bool Released => projectile.timeLeft < 12;
        public float Light => Released ? MathHelper.Lerp(1, 0, (12 - projectile.timeLeft) / 12f) : MathHelper.Clamp(projectile.ai[0] / 60f, 0, 1);
        public float Theta => projectile.ai[0] / 60f * MathHelper.TwoPi;
        public float Alpha
        {
            get
            {

                if (projectile.ai[0] >= 120)
                {
                    return -MathHelper.Pi / 3f;
                }
                else if (projectile.ai[0] >= 60)
                {
                    return -(projectile.ai[0] - 60) * (projectile.ai[0] - 60) / 3600f * MathHelper.Pi / 3f;
                }
                return 0;
            }
        }
        public float Beta => MathHelper.SmoothStep(0, 1, projectile.ai[0] / 60f) * projectile.velocity.ToRotation();
        public float Size => Released ? MathHelper.Lerp(96, 144, (12 - projectile.timeLeft) / 12f) : 96;
        public const float dis = 64;
        public int Cycle => 60;
        public Color MainColor => Color.White;
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
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            Main.instance.DrawCacheProjsOverWiresUI.Add(index);
            base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Matrix transform =
            Matrix.CreateScale(2) *
            Matrix.CreateTranslation(-1, -1, -1) *
            new Matrix(Size, 0, 0, 0,
                          0, Size, 0, 0,
                          0, 0, dis, 0,
                          0, 0, 0, 1) *
            Matrix.CreateRotationZ(Theta) *
            Matrix.CreateRotationY(Alpha) *
            Matrix.CreateRotationZ(Beta);
            CustomVertexInfo[] vertexInfos = new CustomVertexInfo[8];
            for (int n = 0; n < 8; n++)
            {
                var vec = new Vector3(n % 2, n / 2 % 2, 0);
                vertexInfos[n].TexCoord = new Vector3(vec.X, vec.Y, Light);
                vertexInfos[n].Color = MainColor;
                vec = Vector3.Transform(vec, transform);
                if (n == 3)
                {
                    transform =
                    Matrix.CreateScale(2) *
                    Matrix.CreateTranslation(-1, -1, -1) *
                    new Matrix(Size * 1.5f, 0, 0, 0,
                                         0, Size * 1.5f, 0, 0,
                                         0, 0, dis * 1.5f, 0,
                                         0, 0, 0, 1) *
                    Matrix.CreateRotationZ(-Theta * 1.5f) *
                    Matrix.CreateRotationY(Alpha) *
                    Matrix.CreateRotationZ(Beta);
                }
                vec += new Vector3(projectile.Center - Main.screenPosition - new Vector2(Main.screenWidth, Main.screenHeight) * .5f, 0);
                vec.Z = (2000 - vec.Z) / 2000f;
                vec /= vec.Z;
                vertexInfos[n].Position = new Vector2(vec.X, vec.Y) + Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) * .5f;
            }
            CustomVertexInfo[] vertexs = new CustomVertexInfo[12];
            for (int n = 0; n < 12; n++)
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
                if (n > 5) index += 4;
                vertexs[n] = vertexInfos[index];
            }
            StoneOfThePhilosophersHelper.VertexDraw(vertexs[0..6],
                TextureAssets.Projectile[Type].Value,
                ModContent.Request<Texture2D>("StoneOfThePhilosophers/Style_4").Value,
                new Vector2(Main.GameUpdateCount * -0.09f, Main.GlobalTimeWrappedHourly * 0.6f));
            StoneOfThePhilosophersHelper.VertexDraw(vertexs[6..12],
                ModContent.Request<Texture2D>("StoneOfThePhilosophers/MagicArea_2").Value,
                ModContent.Request<Texture2D>("StoneOfThePhilosophers/Style_4").Value,
                new Vector2(Main.GameUpdateCount * -0.15f, Main.GlobalTimeWrappedHourly));
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
                diff.Normalize();
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
                if (!player.channel)
                {
                    projectile.timeLeft = 12;
                }
                else
                {
                    projectile.timeLeft = 14;
                    if (UseMana)
                    {
                        if (!player.CheckMana(player.inventory[player.selectedItem].mana, true))
                        {
                            projectile.timeLeft = 12;
                        }
                        if (projectile.ai[0] >= 120)
                            ShootProj();
                    }
                }
            }
            else
            {
                if (projectile.ai[0] > 60f)
                {
                    ShootProj(true);
                }
            }
        }
    }
}
