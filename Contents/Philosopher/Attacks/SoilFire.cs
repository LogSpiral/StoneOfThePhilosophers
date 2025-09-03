using LogSpiralLibrary;
using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing;
using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing.RenderDrawingContents;
using LogSpiralLibrary.CodeLibrary.DataStructures.Drawing.RenderDrawingEffects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace StoneOfThePhilosophers.Contents.Philosopher.Attacks;

public class SoilFireZoneVisual : VertexDrawInfo
{
    private CustomVertexInfo[] _vertexInfos = new CustomVertexInfo[40];
    public override CustomVertexInfo[] VertexInfos => _vertexInfos;
    public float rotation;

    public override void Update()
    {
        if (!autoUpdate)
        {
            autoUpdate = true;
            return;
        }
        timeLeft--;
        const int Counts = 20;
        for (int i = 0; i < Counts; i++)
        {
            var f = i / (Counts - 1f);
            var lerp = f;
            float theta2 = MathHelper.Lerp(-5 / 12f, 5 / 12f, lerp) * MathHelper.Pi;
            Vector2 offsetVec = (theta2 + rotation).ToRotationVector2() * scaler;
            VertexInfos[2 * i] = new CustomVertexInfo(center + offsetVec, new Vector3(f, 1, 1));
            VertexInfos[2 * i + 1] = new CustomVertexInfo(center, new Vector3(f, 0, 1));
        }
    }

    public override void PreDraw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
    {
        base.PreDraw(spriteBatch, graphicsDevice);
        Effect effect = LogSpiralLibraryMod.ShaderSwooshUL;
        effect.Parameters["uTransform"].SetValue(RenderCanvasSystem.uTransform);
        effect.Parameters["uTime"].SetValue((float)GlobalTimeSystem.GlobalTime * 0.003f);
        effect.Parameters["checkAir"].SetValue(false);
        effect.Parameters["airFactor"].SetValue(2);
        effect.Parameters["lightShift"].SetValue(0f);
        effect.Parameters["heatMapAlpha"].SetValue(true);
        effect.Parameters["stab"].SetValue(false);
        effect.Parameters["alphaOffset"].SetValue(0f);
        var sampler = SamplerState.AnisotropicWrap;
        Main.graphics.GraphicsDevice.SamplerStates[0] = sampler;
        Main.graphics.GraphicsDevice.SamplerStates[1] = sampler;
        Main.graphics.GraphicsDevice.SamplerStates[2] = sampler;
        Main.graphics.GraphicsDevice.SamplerStates[3] = SamplerState.AnisotropicWrap;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        Main.graphics.GraphicsDevice.Textures[0] = LogSpiralLibraryMod.BaseTex[21].Value;
        Main.graphics.GraphicsDevice.Textures[1] = LogSpiralLibraryMod.BaseTex[9].Value;

        Effect effect = LogSpiralLibraryMod.ShaderSwooshUL;
        effect.Parameters["gather"].SetValue(false);
        effect.Parameters["AlphaVector"].SetValue(new Vector3(0, 0, 1));
        effect.Parameters["normalize"].SetValue(false);
        effect.Parameters["heatRotation"].SetValue(Matrix.Identity);
        effect.Parameters["alphaFactor"].SetValue(2f);

        var fac = 1 - Factor;
        fac = fac < .1f ? MathHelper.SmoothStep(0, 1, fac * 10f) :
            MathHelper.SmoothStep(1, 0, (fac - .1f) / .9f);

        effect.Parameters["lightShift"].SetValue(fac * 1.1f - 1);

        base.Draw(spriteBatch);
    }

    public static SoilFireZoneVisual NewZone(string canvasName, int timeLeft, float scaler, float rotation, Vector2 center)
    {
        var result = new SoilFireZoneVisual();
        result.timeLeft = result.timeLeftMax = timeLeft;
        result.scaler = scaler;
        result.rotation = rotation;
        result.center = center;
        result.heatMap = LogSpiralLibraryMod.HeatMap[0].Value;
        result.weaponTex = LogSpiralLibraryMod.BaseTex[8].Value;
        RenderCanvasSystem.AddRenderDrawingContent(canvasName, result);
        return result;
    }
}

public class SoilFireZone : ModProjectile
{
    public const string CanvasName = $"{nameof(StoneOfThePhilosophers)}:{nameof(SoilFireZone)}";
    private static readonly MaskEffect maskEffect = Main.dedServ ? new() : new(LogSpiralLibraryMod.Mask[3].Value, Color.MediumPurple, 0.05f, 0.07f, default, false, false);
    private static readonly BloomEffect bloomEffect = new(0, 1f, 2, 3, true, 0, true);
    private static readonly IRenderEffect[][] renderEffects = [[maskEffect, bloomEffect]];

    public override void Load()
    {
        if (Main.dedServ) return;
        RenderCanvasSystem.RegisterCanvasFactory(CanvasName, () => new(renderEffects));

        base.Load();
    }

    public override void OnSpawn(IEntitySource source)
    {
        if (Main.dedServ) return;
        if (Projectile.velocity != default)
            Projectile.rotation = Projectile.velocity.ToRotation();
        maskEffect.FillTex = LogSpiralLibraryMod.Mask[6].Value;
        maskEffect.GlowColor = Color.Red;
        SoilFireZoneVisual.NewZone(CanvasName, Projectile.timeLeft, 300, Projectile.rotation, Projectile.Center);
    }

    public override string Texture => $"Terraria/Images/Item_{ItemID.Dirt1Echo}";

    public override void SetDefaults()
    {
        Projectile.width = Projectile.height = 1;
        Projectile.timeLeft = 240;
        Projectile.DamageType = DamageClass.Magic;
        Projectile.friendly = true;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.penetrate = -1;
        Projectile.aiStyle = -1;
    }

    public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
    {
        Vector2 center = Projectile.Center;
        for (int n = 4; n < 5; n++)
        {
            Vector2 checkTargetPosition = n == 4 ?
                targetHitbox.Center() :
                targetHitbox.TopLeft() + new Vector2(targetHitbox.Width * (n % 2), targetHitbox.Height * (n / 2));

            Vector2 vec = checkTargetPosition - center;
            float length = vec.Length();
            float dot = Vector2.Dot(vec, Projectile.velocity);
            float target = length * MathF.Cos(MathF.PI / 12 * 5);
            if (length < 300 && dot >= target)
                return true;
        }
        return false;
    }

    public override void AI()
    {
        if (Projectile.timeLeft == 232)
            SoundEngine.PlaySound(SoundID.Item74);
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.AddBuff(ModContent.BuffType<SoilFireBuff>(), 30);
        target.AddBuff(BuffID.OnFire, 600);
    }

    public override bool? CanCutTiles() => false;

    public override bool PreDraw(ref Color lightColor) => false;

    public override bool ShouldUpdatePosition() => false;
}

public class SoilFireBuff : ModBuff
{
    public override string Texture => StoneOfThePhilosopher.DebuffTexturePath;
}

public class SoilFireGlobalNPC : GlobalNPC
{
    public override void AI(NPC npc)
    {
        if (!npc.boss && npc.HasBuff<SoilFireBuff>())
            npc.velocity *= .95f;
        base.AI(npc);
    }
}