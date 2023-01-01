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

namespace StoneOfThePhilosophers.Contents
{
    public static class StoneOfThePhilosophersHelper
    {
        public static void VertexDraw(CustomVertexInfo[] vertexs, Texture2D baseTex, Texture2D aniTex, Vector2 uTime = default)
        {
            Effect effect = StoneOfThePhilosophers.VertexDraw;
            if (effect == null) return;
            SpriteBatch spriteBatch = Main.spriteBatch;
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            //List<CustomVertexInfo> triangleList = new List<CustomVertexInfo>();
            //for (int i = 0; i < vertexs.Length - 2; i += 2)
            //{
            //    triangleList.Add(vertexs[i]);
            //    triangleList.Add(vertexs[i + 2]);
            //    triangleList.Add(vertexs[i + 1]);

            //    triangleList.Add(vertexs[i + 1]);
            //    triangleList.Add(vertexs[i + 2]);
            //    triangleList.Add(vertexs[i + 3]);
            //}
            RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
            var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0));
            effect.Parameters["uTransform"].SetValue(model * Main.GameViewMatrix.TransformationMatrix * projection);
            effect.Parameters["uTimeX"].SetValue(uTime.X);
            effect.Parameters["uTimeY"].SetValue(uTime.Y);
            Main.graphics.GraphicsDevice.Textures[0] = baseTex;
            Main.graphics.GraphicsDevice.Textures[1] = aniTex;
            Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.AnisotropicWrap;
            Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.AnisotropicWrap;
            effect.CurrentTechnique.Passes[0].Apply();
            //Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertexs, 0, vertexs.Length / 3);
            Main.graphics.GraphicsDevice.RasterizerState = originalState;
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
        public static void VertexDraw3DPlanes()
        {

        }
    }
    public struct CustomVertexInfo : IVertexType
    {
        private static VertexDeclaration _vertexDeclaration = new VertexDeclaration(new VertexElement[3]
        {
                new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
                new VertexElement(8, VertexElementFormat.Color, VertexElementUsage.Color, 0),
                new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 0)
        });
        public Vector2 Position;
        public Color Color;
        public Vector3 TexCoord;

        public CustomVertexInfo(Vector2 position, Color color, Vector3 texCoord)
        {
            Position = position;
            Color = color;
            TexCoord = texCoord;
        }
        public CustomVertexInfo(Vector2 position, float alpha, Vector3 texCoord)
        {
            Position = position;
            Color = Color.White with { A = (byte)(MathHelper.Clamp(255 * alpha, 0, 255)) };
            TexCoord = texCoord;
        }
        public CustomVertexInfo(Vector2 position, Vector3 texCoord)
        {
            Position = position;
            Color = Color.White;
            TexCoord = texCoord;
        }

        public VertexDeclaration VertexDeclaration
        {
            get
            {
                return _vertexDeclaration;
            }
        }
    }
    public abstract class MagicArea : ModProjectile
    {
        public override string Texture => "StoneOfThePhilosophers/MagicArea_1";//{StarBound.NPCs.Bosses.BigApe.BigApeTools.ApePath}StrawBerryArea

        public Projectile projectile => Projectile;
        public Player player => Main.player[projectile.owner];
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
        public float Beta => projectile.ai[0] >= 60f ? (Main.MouseWorld - player.Center).ToRotation() : 0;
        public float Size => Released ? MathHelper.Lerp(128, 160, (12 - projectile.timeLeft) / 12f) : 128;
        public const float l = 240;
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
            vertexInfos[0] = new CustomVertexInfo(default, MainColor, new Vector3(0, 0, 0));
            vertexInfos[1] = new CustomVertexInfo(default, MainColor, new Vector3(1, 0, 0));
            vertexInfos[2] = new CustomVertexInfo(default, MainColor, new Vector3(1, 1, 0));
            vertexInfos[3] = new CustomVertexInfo(default, MainColor, new Vector3(0, 1, 0));
        }
        public Vector2 GetVec(Vector3 vector, float d = dis, bool negativeTheta = false)
        {
            float x = vector.X;
            float y = vector.Y;
            float z = vector.Z;
            float sA = (float)Math.Sin(Alpha);
            float cA = (float)Math.Cos(Alpha);
            float sB = (float)Math.Sin(Beta);
            float cB = (float)Math.Cos(Beta);
            float sT = (float)Math.Sin(Theta) * (negativeTheta ? -1 : 1);
            float cT = (float)Math.Cos(Theta);
            float value1 = cA * (cT * x - sT * y) - sA * (z + d);
            float value2 = sT * x + cT * y;
            return l / (l - z) * new Vector2(cB * value1 - sB * value2, sB * value1 + cB * value2);
        }
        public CustomVertexInfo[] vertexInfos = new CustomVertexInfo[4];
        public virtual void ShootProj(bool dying = false)
        {

        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            Main.instance.DrawCacheProjsOverWiresUI.Add(index);
            base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
        }
        public virtual void UpdateVertex()
        {
            for (int n = 0; n < vertexInfos.Length; n++)
            {
                vertexInfos[n].Position = GetVec(new Vector3(vertexInfos[n].TexCoord.X - 0.5f, vertexInfos[n].TexCoord.Y - 0.5f, 0) * Size) + player.Center;
                vertexInfos[n].TexCoord.Z = Light;
                if (MainColor != vertexInfos[n].Color)
                {
                    vertexInfos[n].Color = MainColor;
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            CustomVertexInfo[] vertexInfos1 = new[] { vertexInfos[0], vertexInfos[1], vertexInfos[2], vertexInfos[2], vertexInfos[3], vertexInfos[0] };

            RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
            var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0));
            Effect effect = StoneOfThePhilosophers.VertexDraw;
            effect.Parameters["uTransform"].SetValue(model * projection);
            effect.Parameters["uTimeX"].SetValue(Main.GameUpdateCount * -0.03f);
            effect.Parameters["uTimeY"].SetValue(Main.GlobalTimeWrappedHourly * 0.2f);
            Main.graphics.GraphicsDevice.Textures[0] = TextureAssets.Projectile[projectile.type].Value;
            Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("StoneOfThePhilosophers/Style_4").Value;
            Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.AnisotropicWrap;
            Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.AnisotropicWrap;
            effect.CurrentTechnique.Passes[0].Apply();
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertexInfos1, 0, 2);
            Main.graphics.GraphicsDevice.RasterizerState = originalState;
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
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
            UpdateVertex();
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
    public abstract class MagicStone : ModItem
    {
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.ManaCrystal, 10);
            recipe.AddTile(TileID.DemonAltar);
            AddOtherIngredients(recipe);
            recipe.Register();
        }
        public virtual void AddOtherIngredients(Recipe recipe)
        {
            recipe.AddIngredient(ItemID.HellstoneBar, 15);
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
            item.useStyle = 5;
            item.channel = true;
            item.value = 150;
            item.knockBack = 4f;
            item.shootSpeed = 10;
            item.damage = 50;
            item.mana = 5;
        }
    }
}
