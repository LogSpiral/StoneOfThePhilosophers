using LogSpiralLibrary.CodeLibrary.Utilties.Extensions;
using Terraria.GameContent;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using LogSpiralLibrary.CodeLibrary.Utilties;
using LogSpiralLibrary;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Terraria.DataStructures;
using Terraria.Utilities;
using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing;
using Terraria.Audio;

namespace StoneOfThePhilosophers.Contents.Philosopher.Attacks;

public class WoodFireHandler : ModProjectile
{
    Player Owner => Main.player[Projectile.owner];
    public override string Texture => $"Terraria/Images/Item_{ItemID.Wood}";
    public override void SetDefaults()
    {
        Projectile.timeLeft = 600;
        Projectile.width = Projectile.height = 1;
        Projectile.friendly = false;
        Projectile.hostile = false;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.aiStyle = -1;
        Projectile.hide = true;
        base.SetDefaults();
    }

    public override void AI()
    {
        Projectile.Center = Owner.Center;
        if (Projectile.timeLeft % 60 == 0)
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + Main.rand.NextFloat(-64,64) * Vector2.UnitX + Vector2.UnitY * 24, default, ModContent.ProjectileType<WoodFirePlant>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

        base.AI();
    }
}
public class WoodFirePlant : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.timeLeft = 60;
        Projectile.DamageType = DamageClass.Magic;
        Projectile.width = Projectile.height = 1;
        Projectile.friendly = true;
        Projectile.aiStyle = -1;
        Projectile.ignoreWater = true;
        Projectile.penetrate = -1;
        Projectile.tileCollide = false;
    }
    public override string Texture => $"Terraria/Images/Item_{ItemID.Wood}";
    AshTree thornTree;
    List<CustomVertexInfo> vertexs;
    List<Vector4> endNodes;
    float Factor => (60 - Projectile.timeLeft) / 60f;
    public override void AI()
    {
        //Projectile.ai[1]++;
        float k = (1 - MathF.Cos(MathHelper.TwoPi * MathF.Sqrt(Factor))) * .5f;
        vertexs = thornTree.GetTreeVertex(-MathHelper.PiOver2, Projectile.Center - Vector2.UnitX * 8, out endNodes, k);


        if (Projectile.timeLeft == 45)
        {
            foreach (var vec in endNodes)
                Gore.NewGore(vec.XY(), -vec.Z.ToRotationVector2() * 4, GoreID.TreeLeaf_TreeAsh, 1);

            if (endNodes.Count > 0)
                for (int n = 0; n < 10; n++)
                {
                    var vec = Main.rand.Next(endNodes);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), vec.XY(), -vec.Z.ToRotationVector2() * 16, ModContent.ProjectileType<WoodFireLeaf>(),
                        Projectile.damage, Projectile.knockBack, Projectile.owner,Main.rand.Next(Main.rand.Next(5)), Main.rand.NextFloat(24, 48));
                }
            SoundEngine.PlaySound(SoundID.Item74 with { volume = .5f});
        }
    }
    public override void OnSpawn(IEntitySource source)
    {
        float stdSize = 96;
        thornTree = new AshTree(stdSize, stdSize * .25f, 0, new()
        {
            lengthScaler = .7f,
            widthScaler = .4f,
            stdRotation = MathHelper.Pi / 3,
            maxChildrens = 4,
            chanceToDecreaseChildren = .45f,
            mainBranchOffsetLength = .75f,
            mainBranchOffsetWidth = .9f,
            mainBranchOffsetRotationScaler = .25f,
            mainBranchExtraTier = 4,
            mainBranchOffsetChildren = .5f,
            fixedRotation = 0
        });//
        thornTree.BuildTree(Main.rand, 8);
    }
    public override bool PreDraw(ref Color lightColor)
    {
        if (vertexs == null)
            return false;

        Main.spriteBatch.ReBegin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

        Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
        Main.graphics.GraphicsDevice.BlendState = BlendState.NonPremultiplied;
        var swooshUL = LogSpiralLibraryMod.ShaderSwooshUL;
        swooshUL.Parameters["uTransform"].SetValue(RenderCanvasSystem.uTransform);
        var sampler = SamplerState.AnisotropicWrap;
        Main.graphics.GraphicsDevice.SamplerStates[0] = sampler;
        Main.graphics.GraphicsDevice.SamplerStates[1] = sampler;
        Main.graphics.GraphicsDevice.SamplerStates[2] = sampler;
        Main.graphics.GraphicsDevice.SamplerStates[3] = SamplerState.AnisotropicWrap;
        Main.graphics.GraphicsDevice.Textures[0] = ModAsset.AshTreeTile.Value;
        Main.graphics.GraphicsDevice.Textures[1] = LogSpiralLibraryMod.BaseTex_Swoosh[8].Value;
        Main.graphics.GraphicsDevice.Textures[2] = LogSpiralLibraryMod.BaseTex_Swoosh[8].Value;
        Main.graphics.GraphicsDevice.Textures[3] = LogSpiralLibraryMod.BaseTex_Swoosh[8].Value;
        swooshUL.CurrentTechnique.Passes[7].Apply();

        Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.AnisotropicWrap;
        Main.graphics.GraphicsDevice.Textures[0] = ModAsset.AshTreeTile.Value;
        Main.pixelShader.CurrentTechnique.Passes[0].Apply();
        Main.instance.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertexs.ToArray(), 0, vertexs.Count / 3);

        Main.spriteBatch.ReBegin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

        try
        {
            Main.instance.LoadGore(385);
        }
        catch { }
        if (Projectile.timeLeft > 30 && Projectile.ai[0] > 1 && endNodes != null)
        {
            foreach (var vec4 in endNodes)
            {
                var vec = new Vector2(vec4.X, vec4.Y);

                Main.spriteBatch.Draw(TextureAssets.Gore[385].Value, vec - Main.screenPosition, null, Lighting.GetColor(vec.ToTileCoordinates()).MultiplyRGB(Color.Cyan), vec4.Z, new Vector2(14), (1 - MathF.Cos(MathHelper.TwoPi * MathF.Sqrt(Factor * 2f))) * .5f * vec4.W, SpriteEffects.FlipHorizontally, 0);
            }
        }
        return false;
    }
    public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
    {
        if (vertexs == null) return false;
        int m = vertexs.Count;
        for (int n = 0; n < m / 10; n++)
        {
            if (targetHitbox.Contains(vertexs[Main.rand.Next(m)].Position.ToPoint()))
                return true;
        }
        return false;
    }
}
// 抄自最终分形的木
public class AshTree
{
    public struct TreeGenerateInfo
    {
        public float lengthScaler;
        public float widthScaler;
        public float stdRotation;
        public int maxChildrens;
        public float chanceToDecreaseChildren;

        public float mainBranchOffsetLength;
        public float mainBranchOffsetWidth;
        public float mainBranchOffsetRotationScaler;
        public int mainBranchExtraTier;
        public float mainBranchOffsetChildren;

        /// <summary>
        /// 固定旋转量
        /// </summary>
        public float fixedRotation;
    }
    class Node(float length, float width, float rotation, bool main)
    {
        /// <summary>
        /// 子节点
        /// </summary>
        HashSet<Node> Children;

        /// <summary>
        /// 节点始端到末端的长度
        /// </summary>
        float length = length;

        /// <summary>
        /// 节点末端的宽度
        /// </summary>
        float width = width;

        /// <summary>
        /// 相对于父节点的旋转量
        /// </summary>
        float rotation = rotation;

        /// <summary>
        /// 是否处于主干
        /// </summary>
        bool mainBranch = main;

        float getNormalRandom(UnifiedRandom random, float factor) => (float)random.GaussianRandom(factor, factor * .25 * .33);

        public void Generate(UnifiedRandom random, TreeGenerateInfo info, int maxTier, out int depth)
        {
            depth = 1;
            if (maxTier != 0 && length > 1 && width > 1)
            {
                int r = (int)(random.GaussianRandom(info.maxChildrens * .75f, info.maxChildrens * .25 * .33));
                Children = [];
                for (int n = 0; n < r; n++)
                {
                    Node node = new(
                        length * MathHelper.Lerp(getNormalRandom(random, info.lengthScaler), 1, mainBranch ? info.mainBranchOffsetLength : 0),
                        width * MathHelper.Lerp(getNormalRandom(random, info.widthScaler), 1, mainBranch ? info.mainBranchOffsetWidth : 0),
                        (float)random.GaussianRandom(0, info.stdRotation * .33f) + info.fixedRotation,
                        n == 0);

                    Children.Add(node);
                    if (random.NextDouble() < info.chanceToDecreaseChildren * (mainBranch ? info.mainBranchOffsetChildren : 1))
                        info = info with { maxChildrens = info.maxChildrens - 1 };
                    node.Generate(random, info, maxTier - 1, out int curDepth);
                    if (curDepth > depth)
                        depth = curDepth;
                }
                depth++;
            }
        }

        public void AddToVertex(IList<CustomVertexInfo> vertexInfos, IList<Vector4> EndNodes, float parentWidth, float parentRotation, Vector2 nodePoint, Color color, float factor = 1f)
        {
            if (parentWidth == 0) parentWidth = width;
            float realRotation = rotation + parentRotation;

            Vector2 unit = realRotation.ToRotationVector2();
            Vector2 normalUnit = new Vector2(-unit.Y, unit.X);
            CustomVertexInfo[] results = new CustomVertexInfo[4];

            if (length * width > 64f)
                color = Lighting.GetColor(nodePoint.ToTileCoordinates());

            if (MathF.Abs(rotation) < MathHelper.PiOver2)
                nodePoint += MathF.Sign(rotation) * (normalUnit - (parentRotation + MathHelper.PiOver2).ToRotationVector2()) * parentWidth * .5f;



            float u = MathHelper.Clamp(factor, 0, 1);
            float realWidth = width * u * u;
            normalUnit *= .5f;
            results[0] = new CustomVertexInfo(nodePoint + normalUnit * parentWidth, color, new Vector3(0, 0, 1));
            results[1] = new CustomVertexInfo(nodePoint - normalUnit * parentWidth, color, new Vector3(1, 0, 1));
            nodePoint += length * unit * u * (2 - u);
            results[2] = new CustomVertexInfo(nodePoint + normalUnit * realWidth, color, new Vector3(0, 1, 1));
            results[3] = new CustomVertexInfo(nodePoint - normalUnit * realWidth, color, new Vector3(1, 1, 1));

            vertexInfos.Add(results[0]);
            vertexInfos.Add(results[1]);
            vertexInfos.Add(results[2]);

            vertexInfos.Add(results[1]);
            vertexInfos.Add(results[2]);
            vertexInfos.Add(results[3]);

            if (Children != null)
                foreach (var c in Children)
                    c.AddToVertex(vertexInfos, EndNodes, realWidth, realRotation, nodePoint, color, factor - .5f);
            else
                EndNodes.Add(new Vector4(nodePoint, realRotation, (float)Main.rand.GaussianRandom(1, 0.16f)));
        }

    }

    Node mainNode;
    TreeGenerateInfo genInfo;
    int depth;

    public AshTree(float length, float width, float rotation, TreeGenerateInfo info)
    {
        genInfo = info;
        mainNode = new(length, width, rotation, false);
    }

    public void BuildTree(UnifiedRandom random, int maxTier)
    {
        mainNode.Generate(random, genInfo, maxTier, out depth);
    }

    public List<CustomVertexInfo> GetTreeVertex(float rotation, Vector2 start, out List<Vector4> EndNodes, float factor = 1f)
    {
        List<CustomVertexInfo> vertexInfos = [];
        EndNodes = [];
        mainNode.AddToVertex(vertexInfos, EndNodes, 0, rotation, start, Lighting.GetColor(start.ToTileCoordinates()), factor * depth * .5f);
        return vertexInfos;
    }
}
public class WoodFireLeaf : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.width = Projectile.height = 8;
        Projectile.timeLeft = 180;
        Projectile.DamageType = DamageClass.Magic;
        Projectile.friendly = true;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.penetrate = 3;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 5;
        Projectile.aiStyle = -1;
    }
    public override bool PreDraw(ref Color lightColor)
    {
        float alpha = (Projectile.timeLeft / 180f).SmoothSymmetricFactor(1 / 12f);

        for (int n = 9; n > -1; n--)
            for (int m = 0; m < 5; m++)
                Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.oldPos[n] - Main.screenPosition + (m == 0 ? default : Main.rand.NextVector2Unit() * 4), new Rectangle((int)(16 * Projectile.ai[0]), 16, 16, 16), Color.Lerp(lightColor, Color.White, .5f) with { A = 127 } * alpha * ((10 - n) * .1f) * (m == 0 ? 1 : Main.rand.NextFloat(0.25f, 0.5f)), Projectile.oldRot[n], new Vector2(8), 2f * ((10 - n) * .1f) * new Vector2(1.5f, 1f), 0, 0);
        return false;
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        for (int n = 0; n < 4 - Projectile.penetrate; n++)
        {
            for (int k = 0; k < 15; k++)
            {
                Dust.NewDustPerfect(target.Center, DustID.Torch, Main.rand.NextVector2Unit() * Main.rand.NextFloat(4) + Projectile.velocity, 0, default, Main.rand.NextFloat(0.5f, 1.5f));
            }
        }
        target.AddBuff(BuffID.OnFire, 120);
        if (!target.friendly && target.active && target.CanBeChasedBy())
        {
            Main.player[Projectile.owner].GetModPlayer<ElementSkillPlayer>().ElementChargeValue[2] += damageDone / ElementSkillPlayer.Devider;
        }
    }
    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        for (int k = 0; k < 15; k++)
        {
            Dust.NewDustPerfect(Projectile.Center + oldVelocity, DustID.Torch, Main.rand.NextVector2Unit() * Main.rand.NextFloat(4) + oldVelocity, 0, default, Main.rand.NextFloat(0.5f, 1.5f));
        }
        return base.OnTileCollide(oldVelocity);
    }
    public override void AI()
    {
        if (Projectile.timeLeft >= 170) goto Label;
        NPC target = null;
        float maxDistance = 784;
        float maxDistanceCopy = maxDistance;
        foreach (var npc in Main.npc)
        {
            if (!npc.CanBeChasedBy() || npc.friendly) continue;
            var currentDistance = Vector2.Distance(npc.Center, Projectile.Center);
            if (currentDistance < maxDistance)
            {
                maxDistance = currentDistance;
                target = npc;
            }
        }
        if (target != null)
        {
            //var fac = MathF.Cos(Main.GameUpdateCount * MathHelper.Pi / 7.5f) * .5f + .5f;
            float factor = Projectile.frameCounter == 2 ? Utils.GetLerpValue(180, 150, Projectile.timeLeft, true) : 1;
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, (target.Center - Projectile.Center).SafeNormalize(default) * Projectile.ai[1] * factor, Utils.GetLerpValue(maxDistanceCopy / 8, maxDistanceCopy, maxDistance, true) * 0.25f * factor);
            //Projectile.velocity = Projectile.velocity.SafeNormalize(default) * Projectile.ai[1];
            if (Main.GameUpdateCount % 3 == 0)
            {
                if (Projectile.timeLeft > 165) Projectile.timeLeft--;
                if (Projectile.timeLeft < 15) Projectile.timeLeft++;
            }


        }
    Label:
        Projectile.rotation = Projectile.velocity.ToRotation();
        for (int n = 9; n > 0; n--)
        {
            Projectile.oldPos[n] = Projectile.oldPos[n - 1];
            Projectile.oldRot[n] = Projectile.oldRot[n - 1];
        }
        Projectile.oldPos[0] = Projectile.Center;
        Projectile.oldRot[0] = Projectile.rotation;
    }
}
