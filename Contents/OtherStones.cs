using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.Audio;
using Terraria.GameContent;
using ReLogic.Graphics;
using System.Runtime.CompilerServices;
using rail;
using StoneOfThePhilosophers.Contents;
using Steamworks;
using static Terraria.ModLoader.PlayerDrawLayer;
using StoneOfThePhilosophers.UI;
using ReLogic.Utilities;

namespace StoneOfThePhilosophers.Contents
{
    public class ElementChargePlayer : ModPlayer
    {
        /// <summary>
        /// 和那边的枚举类型对应，当然，你得把Empty踢开
        /// 最大值100，最小0
        /// </summary>
        public float[] ElementChargeValue = new float[7];
        public override void ResetEffects()
        {
            for (int n = 0; n < 7; n++)
            {
                ElementChargeValue[n] = MathHelper.Clamp(ElementChargeValue[n], 0, 100);
            }

            base.ResetEffects();
        }
        public static float Devider => 500f;
    }
    public class StoneOfMetal : MagicStone
    {
        public override StoneElements Elements => StoneElements.Metal;
        public override void AddOtherIngredients(Recipe recipe)
        {
            recipe.AddIngredient(ItemID.LargeTopaz);
            recipe.AddIngredient(ItemID.MeteoriteBar, 20);
            recipe.AddIngredient(ItemID.Blinkroot, 5);
            recipe.AddRecipeGroup(StoneOfThePhilosophersSystem.IronLeadOres, 20);
            recipe.AddRecipeGroup(StoneOfThePhilosophersSystem.GoldPlatinumOres, 10);
            base.AddOtherIngredients(recipe);
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("金之石");
            Tooltip.SetDefault("使用金元素魔法程度的能力\n金符「金属疲劳」");
        }
        public override void SetDefaults()
        {
            item.shoot = ModContent.ProjectileType<StoneOfMetalProj>();
            base.SetDefaults();
            item.damage = 30;
        }
    }
    public class StoneOfMetalEX : StoneOfMetal
    {
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            AddEXRequire<StoneOfMetal>(recipe, true);
            recipe.Register();
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("真·金之石");
            Tooltip.SetDefault("控制金元素魔法程度的能力\n金符「银龙」");
        }
        public override bool Extra => true;
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.damage = 70;
        }
    }
    public class StoneOfWood : MagicStone
    {
        public override StoneElements Elements => StoneElements.Wood;

        public override void AddOtherIngredients(Recipe recipe)
        {
            recipe.AddIngredient(ItemID.LargeEmerald);
            recipe.AddIngredient(RecipeGroupID.Wood, 50);
            recipe.AddIngredient(ItemID.JungleGrassSeeds, 5);
            recipe.AddIngredient(ItemID.Vine, 5);
            recipe.AddIngredient(ItemID.JungleSpores, 9);
            recipe.AddIngredient(ItemID.Stinger, 5);
            base.AddOtherIngredients(recipe);
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("木之石");
            Tooltip.SetDefault("使用木元素魔法程度的能力\n木符「风灵的角笛」");
        }
        public override void SetDefaults()
        {
            item.shoot = ModContent.ProjectileType<StoneOfWoodProj>();
            base.SetDefaults();
            item.damage = 20;
        }
    }
    public class StoneOfWoodEX : StoneOfWood
    {
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Pearlwood, 50);
            recipe.AddIngredient(ItemID.ToxicFlask);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 10);
            AddEXRequire<StoneOfWood>(recipe);
            recipe.Register();
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("真·木之石");
            Tooltip.SetDefault("控制木元素魔法程度的能力\n木符「翠绿风暴」");
        }
        public override bool Extra => true;
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.damage = 45;
        }
    }
    public class StoneOfWater : MagicStone
    {
        public override StoneElements Elements => StoneElements.Water;

        public override void AddOtherIngredients(Recipe recipe)
        {
            recipe.AddIngredient(ItemID.LargeSapphire);
            recipe.AddIngredient(ItemID.WaterBolt);
            //recipe.AddIngredient(ItemID.) //TODO 加入珊瑚礁块的配方，等1.4.4
            recipe.AddIngredient(ItemID.SharkFin, 5);
            recipe.AddIngredient(ItemID.Shiverthorn, 5);
            recipe.AddIngredient(ItemID.WaterBucket);
            base.AddOtherIngredients(recipe);
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("水之石");
            Tooltip.SetDefault("使用水元素魔法程度的能力\n水符「水精公主」");
        }
        public override void SetDefaults()
        {
            item.shoot = ModContent.ProjectileType<StoneOfWaterProj>();
            base.SetDefaults();
            item.damage = 15;
            item.mana = 5;
        }
    }
    public class StoneOfWaterEX : StoneOfWater
    {
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BottomlessBucket);
            recipe.AddIngredient(ItemID.NeptunesShell);
            AddEXRequire<StoneOfWater>(recipe);
            recipe.Register();
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("真·水之石");
            Tooltip.SetDefault("控制水元素魔法程度的能力\n水符「湖葬」");
        }
        public override bool Extra => true;
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.damage = 30;
        }
    }
    public class StoneOfFire : MagicStone
    {
        public override StoneElements Elements => StoneElements.Fire;

        public override void AddOtherIngredients(Recipe recipe)
        {
            recipe.AddIngredient(ItemID.LargeRuby);
            recipe.AddIngredient(ItemID.FlowerofFire);
            recipe.AddIngredient(ItemID.HellstoneBar, 8);
            recipe.AddIngredient(ItemID.Fireblossom, 5);
            recipe.AddIngredient(ItemID.LavaBucket);
            base.AddOtherIngredients(recipe);
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("火之石");
            Tooltip.SetDefault("使用火元素魔法程度的能力\n火符「火神之光」");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.damage = 25;
            item.shoot = ModContent.ProjectileType<StoneOfFireProj>();

        }
    }
    public class StoneOfFireEX : StoneOfFire
    {
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup(StoneOfThePhilosophersSystem.CursedIchorFlame, 20);
            recipe.AddIngredient(ItemID.LivingFireBlock, 50);
            recipe.AddIngredient(ItemID.InfernoFork);
            AddEXRequire<StoneOfFire>(recipe);
            recipe.Register();
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("真·火之石");
            Tooltip.SetDefault("控制火元素魔法程度的能力\n火符「火神的辉光」");
        }
        public override bool Extra => true;
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.damage = 75;
        }
    }
    public class StoneOfEarth : MagicStone
    {
        public override StoneElements Elements => StoneElements.Soil;

        public override void AddOtherIngredients(Recipe recipe)
        {
            recipe.AddIngredient(ItemID.LargeAmber);
            recipe.AddIngredient(ItemID.SandstorminaBottle);
            recipe.AddIngredient(ItemID.Sandstone, 20);
            recipe.AddIngredient(ItemID.Cactus, 50);
            recipe.AddIngredient(ItemID.FossilOre, 5);
            recipe.AddIngredient(ItemID.Waterleaf, 5);
            base.AddOtherIngredients(recipe);
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("土之石");
            Tooltip.SetDefault("使用土元素魔法程度的能力\n土符「慵懒三石塔」");
        }
        public override void SetDefaults()
        {
            item.shoot = ModContent.ProjectileType<StoneOfEarthProj>();
            item.damage = 20;

            base.SetDefaults();
        }
    }
    public class StoneOfEarthEX : StoneOfEarth
    {
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DesertFossil, 50);
            recipe.AddIngredient(ItemID.DjinnLamp);
            recipe.AddIngredient(ItemID.AncientBattleArmorMaterial, 4);
            AddEXRequire<StoneOfEarth>(recipe);
            recipe.Register();
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("真·土之石");
            Tooltip.SetDefault("控制土元素魔法程度的能力\n土符「三石塔之震」");
        }
        public override bool Extra => true;
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.damage = 60;
        }
    }
    public class StoneOfMoon : MagicStone
    {
        public override StoneElements Elements => StoneElements.Lunar;

        public override bool Extra => true;

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.ManaCrystal, 5);
            recipe.AddIngredient(ItemID.CrystalShard, 20);
            recipe.AddIngredient(ItemID.SoulofLight, 20);
            recipe.AddIngredient(ItemID.SoulofNight, 20);
            //recipe.AddIngredient(ItemID.moonb, 50);
            recipe.AddIngredient(ItemID.Moonglow, 6);
            recipe.AddIngredient(ItemID.MoonStone);
            recipe.AddIngredient(ItemID.LargeDiamond);
            recipe.AddTile(TileID.CrystalBall);
            recipe.Register();
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("月之石");
            Tooltip.SetDefault("使用月元素魔法程度的能力\n月符「沉静的月神」");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.shoot = ModContent.ProjectileType<StoneOfMoonProj>();
            item.damage = 90;
            item.mana = 5;
        }
    }
    public class StoneOfSun : MagicStone
    {
        public override StoneElements Elements => StoneElements.Solar;

        public override bool Extra => true;

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.ManaCrystal, 5);
            recipe.AddIngredient(ItemID.CrystalShard, 20);
            recipe.AddIngredient(ItemID.SoulofLight, 20);
            recipe.AddIngredient(ItemID.SoulofNight, 20);
            recipe.AddIngredient(ItemID.SunplateBlock, 50);
            recipe.AddIngredient(ItemID.Sunflower, 6);
            recipe.AddIngredient(ItemID.SunStone);
            recipe.AddIngredient(ItemID.LargeDiamond);
            recipe.AddTile(TileID.CrystalBall);
            recipe.Register();
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("日之石");
            Tooltip.SetDefault("使用日元素魔法程度的能力\n日符「皇家圣焰」");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.shoot = ModContent.ProjectileType<StoneOfSunProj>();
            item.damage = 70;
            item.mana = 20;
        }
    }
    public class StoneOfMetalProj : MagicArea
    {
        public override StoneElements Elements => StoneElements.Metal;
        public override Color MainColor => Color.Yellow;
        public override int Cycle => Extra ? 27 : 36;//48
        public override void SpecialAttack(Color dustColor, bool trigger)
        {
            base.SpecialAttack(dustColor, trigger);
        }
        public override void ShootProj(Vector2 unit, bool dying = false)
        {
            attackCounter++;
            bool flag = dying && projectile.timeLeft % 3 != 0;
            SoundEngine.PlaySound(SoundID.Item69);
            for (int n = 0; n < (flag ? 3 : 1); n++)
                Projectile.NewProjectile(projectile.GetSource_FromThis(), player.Center + (dying ? unit.RotatedByRandom(MathHelper.Pi / 3) : unit) * 64, unit * 32 * (flag ? 1 : 1), ModContent.ProjectileType<MetalAttack>(), (int)(projectile.damage * Main.rand.NextFloat(1.25f, 0.95f)), projectile.knockBack * 3, projectile.owner, flag ? (Main.rand.Next(4) + 1) : 0, Extra ? 1 : 0);
        }
    }
    public class StoneOfWoodProj : MagicArea
    {
        public override StoneElements Elements => StoneElements.Wood;
        public override Color MainColor => Color.Green;
        public override int Cycle => Extra ? 18 : 24;//24
        public override void SpecialAttack(Color dustColor, bool trigger)
        {
            switch (specialAttackIndex)
            {
                case 1:
                    {
                        //叶绿射线
                        break;
                    }
                case 2:
                    {
                        //巨木之晶
                        break;
                    }
                case 3:
                    {
                        //愈伤组织
                        if (trigger)
                        {
                            player.AddBuff(ModContent.BuffType<Reincarnation>(), 300);
                            int healValue = player.statLifeMax2 - player.statLife;
                            healValue = (int)MathHelper.Clamp(healValue, 0, 50);
                            player.statLife += healValue;
                            player.HealEffect(healValue);
                        }
                        break;
                    }
            }
            base.SpecialAttack(dustColor, trigger);
        }
        public override void ShootProj(Vector2 unit, bool dying = false)
        {
            if (dying && projectile.timeLeft % 2 == 1) return;

            attackCounter++;
            SoundEngine.PlaySound(SoundID.Item74);
            int m = Main.rand.Next(4, 6) - (dying ? 2 : 0) + (Extra ? Main.rand.Next(3) : 0);
            float randAngle = Main.rand.NextFloat(-MathHelper.Pi / 12, MathHelper.Pi / 12);
            for (int n = 0; n < m; n++)
            {
                var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center + 64 * unit, unit.RotatedBy(randAngle + MathHelper.Pi / 3 * (n / (m - 1f) - 0.5f)) * 32,
    ModContent.ProjectileType<WoodAttack>(), projectile.damage, projectile.knockBack, projectile.owner, Main.rand.Next(Main.rand.Next(5)), Main.rand.NextFloat(24, 48));
                proj.penetrate = Extra ? 3 : 2;
                proj.tileCollide = Extra && Main.rand.NextBool(2);
            }

        }
    }
    public class StoneOfWaterProj : MagicArea
    {
        public override StoneElements Elements => StoneElements.Water;
        public override Color MainColor => Color.Blue;
        public override int Cycle => 6;//6
        public override void SpecialAttack(Color dustColor, bool trigger)
        {
            base.SpecialAttack(dustColor, trigger);
        }
        public override void ShootProj(Vector2 unit, bool dying = false)
        {
            attackCounter++;
            if (dying)
                for (int n = -2; n < 3; n += 2)
                {
                    var rand = Main.rand.NextFloat(-MathHelper.Pi / 12, MathHelper.Pi / 12) * .5f;
                    Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center + 64 * unit, unit.RotatedBy(MathHelper.Pi / 12 * n + rand) * 16, ModContent.ProjectileType<WaterAttack>(), projectile.damage, projectile.knockBack, projectile.owner, Main.rand.Next(5));
                    //for (int k = 0; k < 3; k++)
                    //    Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center + (64 + 16 * k) * projectile.velocity, projectile.velocity.RotatedBy(MathHelper.Pi / 12 * n + rand) * (32 + 8 * k) / 3f, ModContent.ProjectileType<WaterAttack>(), projectile.damage, projectile.knockBack, projectile.owner, Main.rand.Next(5));
                }
            else
            {
                if (Extra)
                {
                    for (int n = -1; n < 2; n++)
                    {
                        Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center + 64 * unit, unit.RotatedBy(MathHelper.Pi / 18 * n) * 16, ModContent.ProjectileType<WaterAttack>(), projectile.damage, projectile.knockBack, projectile.owner, Main.rand.Next(5));
                    }
                }
                else
                {
                    for (int n = -1; n < 2; n += 2)
                    {
                        Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center + 64 * unit, unit.RotatedBy(MathHelper.Pi / 12 * n) * 12, ModContent.ProjectileType<WaterAttack>(), projectile.damage, projectile.knockBack, projectile.owner, Main.rand.Next(5));
                    }
                }

            }

            SoundEngine.PlaySound(SoundID.Item84);

        }
    }
    public class StoneOfFireProj : MagicArea
    {
        public override StoneElements Elements => StoneElements.Fire;
        public override Color MainColor => Color.Red;
        public override int Cycle => 30;//30
        public override void SpecialAttack(Color dustColor, bool trigger)
        {
            if (trigger)
            {
                var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), Main.MouseWorld, default, ModContent.ProjectileType<FireAttack>(), projectile.damage * 2, projectile.knockBack, projectile.owner, 6);
                proj.timeLeft = 300;
                proj.tileCollide = false;
                proj.width = proj.height = 320;
                proj.Center = Main.MouseWorld;
                proj.penetrate = -1;
                proj.usesLocalNPCImmunity = true;
                proj.localNPCHitCooldown = 20;
            }
            base.SpecialAttack(dustColor, trigger);
        }
        public override void ShootProj(Vector2 unit, bool dying = false)
        {
            if (dying && projectile.timeLeft % 2 == 1) return;
            SoundEngine.PlaySound(SoundID.Item74);
            attackCounter++;
            if (attackCounter % 5 == 0 && Extra)
            {
                var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center + 64 * unit, unit.RotatedByRandom(MathHelper.TwoPi / 48f * (dying ? 2 : 0)) * 32, ModContent.ProjectileType<FireAttack>(), (int)(projectile.damage * 1.25f), projectile.knockBack, projectile.owner, 4);
                proj.timeLeft = 45;
                proj.tileCollide = false;
            }
            else
            {
                Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center + 64 * unit, unit.RotatedByRandom(MathHelper.TwoPi / 48f * (dying ? 2 : 1)) * 32,
    ModContent.ProjectileType<FireAttack>(), projectile.damage, projectile.knockBack, projectile.owner, Extra ? 1 : 0);
            }

        }
    }
    public class StoneOfEarthProj : MagicArea
    {
        public override StoneElements Elements => StoneElements.Soil;
        public override Color MainColor => Color.Orange;
        public override int Cycle => Extra ? 24 : 36;
        public override void SpecialAttack(Color dustColor, bool trigger)
        {
            base.SpecialAttack(dustColor, trigger);
        }
        public override void ShootProj(Vector2 unit, bool dying = false)
        {
            if (dying && projectile.timeLeft % 2 == 1) return;
            SoundEngine.PlaySound(SoundID.Item69);
            attackCounter++;
            if (attackCounter % 5 == 0 && Extra)
            {
                for (int n = 0; n < 3; n++)
                {
                    var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center + 64 * unit, unit.RotatedBy((n - 1) * MathHelper.Pi / 12) * 32,
ModContent.ProjectileType<EarthAttack>(), (int)(projectile.damage * 1.25f), projectile.knockBack, projectile.owner, 0, 2);
                    (proj.ModProjectile as EarthAttack).SetDefaultStorm();
                    proj.Center = projectile.Center + 64 * unit;
                }

            }
            else
            {
                for (int n = 0; n < 4; n++)
                {
                    var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center + 64 * unit, unit.RotatedByRandom(MathHelper.TwoPi / 24f * (dying ? 2 : 1)) * 32,
ModContent.ProjectileType<EarthAttack>(), projectile.damage, projectile.knockBack, projectile.owner);
                    proj.frame = Main.rand.Next(1, 5);
                    proj.tileCollide = !Extra;
                }

            }
        }
    }
    public class StoneOfMoonProj : MagicArea
    {
        public override StoneElements Elements => StoneElements.Lunar;
        public override int Cycle => (int)((Main.dayTime ? 18 : 12) * (player.HasBuff<BlessingFromLunarGod>() ? 0.75f : 1f));//12
        public override Color MainColor => Color.Purple;
        public override void SpecialAttack(Color dustColor, bool trigger)
        {
            if (trigger)
            {
                player.AddBuff(ModContent.BuffType<BlessingFromLunarGod>(), 450);

            }
            base.SpecialAttack(dustColor, trigger);
        }
        public override void ShootProj(Vector2 unit, bool dying = false)
        {
            attackCounter++;
            float r = Main.rand.Next(-32, 32) * (dying ? 8f : 1f);
            int randX = Main.rand.Next(-256, 256);//Main.rand.Next(-64, 64);
            var v = new Vector2(randX, -Main.rand.Next(280, 560));
            var flag = player.HasBuff<BlessingFromLunarGod>();
            var center = Main.MouseWorld + new Vector2(r, 0);
            if (flag && (!dying || Main.rand.NextBool(2)) && player.GetZenithTarget(Main.MouseWorld, 1024f, out var index))
            {
                center = Main.npc[index].Center + Main.npc[index].velocity * 15;
            }
            var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), center - v, default, ModContent.ProjectileType<MoonAttack>(), flag ? (int)(projectile.damage * 5 / 4) : projectile.damage, projectile.knockBack, projectile.owner, v.ToRotation(), v.Length() * 2);
            (proj.ModProjectile as MoonAttack).boost = flag;
        }
    }
    public class StoneOfSunProj : MagicArea
    {
        public override StoneElements Elements => StoneElements.Solar;
        public override Color MainColor => Color.White;
        public override int Cycle => 60;
        public override void SpecialAttack(Color dustColor, bool trigger)
        {
            base.SpecialAttack(dustColor, trigger);
        }
        public override void ShootProj(Vector2 unit, bool dying = false)
        {
            if (dying && projectile.timeLeft % 3 != 0) return;
            attackCounter++;
            SoundEngine.PlaySound(SoundID.Item74);
            if (dying)
            {
                unit = unit.RotatedBy((projectile.timeLeft / 3f - 2) * MathHelper.Pi / 8);
                Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center + 64 * unit, unit * 32,
    ModContent.ProjectileType<SunAttack>(), projectile.damage, projectile.knockBack, projectile.owner);
            }
            else
            {
                Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center + 64 * unit, unit.RotatedByRandom(MathHelper.Pi / 48f) * 32,
    ModContent.ProjectileType<SunAttack>(), projectile.damage, projectile.knockBack, projectile.owner);
            }

        }
    }
    public class MetalAttack : ModProjectile
    {
        public bool Extra => Projectile.ai[1] == 1;
        public int TargetIndex = -1;
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 32;
            Projectile.timeLeft = 180;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 4;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
            Projectile.aiStyle = -1;
        }
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            float alpha = (Projectile.timeLeft / 180f).SmoothSymmetricFactor(1 / 12f);
            for (int n = TargetIndex == -1 ? 9 : 0; n > -1; n--)
                Main.EntitySpriteDraw(Extra ? ModContent.Request<Texture2D>(Texture + "EX").Value : TextureAssets.Projectile[Type].Value, Projectile.oldPos[n] - Main.screenPosition, new Rectangle((int)(16 * Projectile.ai[0]), 0, 16, 16), lightColor * ((10 - n) * .1f) * alpha * (n == 0 ? 1 : .25f), Projectile.oldRot[n], new Vector2(8), 3f * ((10 - n) * .1f), 0, 0);
            for (int n = TargetIndex == -1 ? 9 : 0; n > -1; n--)
                Main.EntitySpriteDraw(ModContent.Request<Texture2D>(GlowTexture + (Extra ? "EX" : "")).Value, Projectile.oldPos[n] - Main.screenPosition, new Rectangle((int)(16 * Projectile.ai[0]), 0, 16, 16), Color.White * ((10 - n) * .1f) * alpha * (n == 0 ? 1 : .25f), Projectile.oldRot[n], new Vector2(8), 3f * ((10 - n) * .1f), 0, 0);
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.CanBeChasedBy() && target.type != NPCID.WallofFlesh && target.type != NPCID.WallofFleshEye)
                target.velocity += Projectile.velocity * (Projectile.ai[0] == 0 ? 1f : 0.25f) * (crit ? .6f : .2f);
            for (int n = 0; n < 5 - Projectile.penetrate; n++)
            {
                if (Projectile.ai[0] == 0 && Main.rand.NextBool(2))
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity * .5f + Main.rand.NextVector2Unit() * Main.rand.Next(4, 8), Type, Projectile.damage * 3 / 4, Projectile.knockBack / 2, Projectile.owner, Main.rand.Next(4) + 1, Projectile.ai[1]);
                for (int k = 0; k < 5; k++)
                {
                    Dust.NewDustPerfect(target.Center, Extra ? DustID.Silver : DustID.Copper, Main.rand.NextVector2Unit() * Main.rand.NextFloat(4) + Projectile.velocity, 0, default, Main.rand.NextFloat(1, 2));
                }
            }
            if (!target.friendly && target.active && target.CanBeChasedBy())
            {
                Main.player[Projectile.owner].GetModPlayer<ElementChargePlayer>().ElementChargeValue[3] += damage / ElementChargePlayer.Devider;
            }
            if ((Main.rand.NextBool(3) || Projectile.penetrate == 1) && Projectile.ai[0] != 0)
            {
                Projectile.penetrate = 2;
                TargetIndex = target.whoAmI;
                offset = Main.rand.NextVector2FromRectangle(new Rectangle(0, 0, target.width * 2 / 3, target.height * 2 / 3)) - new Vector2(target.width, target.height) / 3;
            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (Projectile.ai[0] == 0)
                return targetHitbox.Intersects(Utils.CenteredRectangle(projHitbox.Center(), projHitbox.Size() * 3));
            return base.Colliding(projHitbox, targetHitbox);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            var vec = Projectile.velocity;
            if (vec.X == oldVelocity.X) vec.X = -vec.X;
            if (vec.Y == oldVelocity.Y) vec.Y = -vec.Y;

            if (Projectile.ai[0] == 0)
                for (int n = 0; n < Projectile.penetrate; n++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, -vec + Main.rand.NextVector2Unit() * Main.rand.Next(4, 8), Type, Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner, Main.rand.Next(4) + 1, Projectile.ai[1]);
                }
            for (int k = 0; k < 30; k++)
            {
                Dust.NewDustPerfect(Projectile.Center + oldVelocity, Extra ? DustID.Silver : DustID.Copper, Main.rand.NextVector2Unit() * Main.rand.NextFloat(4) + oldVelocity, 0, default, Main.rand.NextFloat(1, 2));
            }
            return base.OnTileCollide(oldVelocity);
        }
        public Vector2 offset;
        public override void AI()
        {

            if (TargetIndex == -1)
            {
                if (Projectile.ai[0] > 0) Projectile.velocity += new Vector2(0, 0.4f);
                Projectile.rotation++;
                for (int n = 9; n > 0; n--)
                {
                    Projectile.oldPos[n] = Projectile.oldPos[n - 1];
                    Projectile.oldRot[n] = Projectile.oldRot[n - 1];
                }
                Projectile.oldPos[0] = Projectile.Center;
                Projectile.oldRot[0] = Projectile.rotation;
            }
            else
            {
                var target = Main.npc[TargetIndex];
                if (target.active)
                {
                    Projectile.velocity = default;
                    Projectile.Center = target.Center + offset;
                    if (Projectile.timeLeft % 30 == 0)
                    {
                        var damage = Extra ? 60 : 20;
                        damage = (int)(damage * Main.rand.NextFloat(0.85f, 0.15f));
                        Main.player[Projectile.owner].ApplyDamageToNPC(target, damage, 0, 0, false);
                        if (!target.friendly && target.active && target.CanBeChasedBy())
                        {
                            Main.player[Projectile.owner].GetModPlayer<ElementChargePlayer>().ElementChargeValue[3] += damage / ElementChargePlayer.Devider;
                        }
                    }
                    Projectile.oldPos[0] = Projectile.Center + offset;

                    Projectile.friendly = false;
                }
                else
                {
                    Projectile.Kill();
                }
            }

            base.AI();
        }
    }
    public class WoodAttack : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 8;
            Projectile.timeLeft = 180;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
            Projectile.aiStyle = -1;
        }
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            float alpha = (Projectile.timeLeft / 180f).SmoothSymmetricFactor(1 / 12f);

            for (int n = 9; n > -1; n--)
                for (int m = 0; m < 5; m++)
                    Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.oldPos[n] - Main.screenPosition + (m == 0 ? default : Main.rand.NextVector2Unit() * 4), new Rectangle((int)(16 * Projectile.ai[0]), 0, 16, 16), Color.Lerp(lightColor, Color.White, .5f) with { A = 0 } * alpha * ((10 - n) * .1f) * (m == 0 ? 1 : Main.rand.NextFloat(0.25f, 0.5f)), Projectile.oldRot[n], new Vector2(8), 2f * ((10 - n) * .1f) * new Vector2(1.5f, 1f), 0, 0);
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int n = 0; n < 4 - Projectile.penetrate; n++)
            {
                for (int k = 0; k < 15; k++)
                {
                    Dust.NewDustPerfect(target.Center, DustID.TerraBlade, Main.rand.NextVector2Unit() * Main.rand.NextFloat(4) + Projectile.velocity, 0, default, Main.rand.NextFloat(0.5f, 1.5f));
                }
            }
            target.AddBuff(BuffID.Poisoned, 360);
            if (!target.friendly && target.active && target.CanBeChasedBy())
            {
                Main.player[Projectile.owner].GetModPlayer<ElementChargePlayer>().ElementChargeValue[2] += damage / ElementChargePlayer.Devider;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int k = 0; k < 15; k++)
            {
                Dust.NewDustPerfect(Projectile.Center + oldVelocity, DustID.TerraBlade, Main.rand.NextVector2Unit() * Main.rand.NextFloat(4) + oldVelocity, 0, default, Main.rand.NextFloat(0.5f, 1.5f));
            }
            return base.OnTileCollide(oldVelocity);
        }
        public override void AI()
        {
            NPC target = null;
            float maxDistance = 256f;
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
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, (target.Center - Projectile.Center).SafeNormalize(default) * Projectile.ai[1], Utils.GetLerpValue(32, 256, maxDistance, true) * 0.25f);
                //Projectile.velocity = Projectile.velocity.SafeNormalize(default) * Projectile.ai[1];
                if (Main.GameUpdateCount % 3 == 0)
                {
                    if (Projectile.timeLeft > 165) Projectile.timeLeft--;
                    if (Projectile.timeLeft < 15) Projectile.timeLeft++;
                }


            }
            //else
            //{
            //    Projectile.alpha = Projectile.timeLeft;
            //}
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
    public class WaterAttack : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 8;
            Projectile.timeLeft = 180;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 5;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
            Projectile.aiStyle = -1;
            Projectile.extraUpdates = 2;
        }
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            float alpha = (Projectile.timeLeft / 180f).SmoothSymmetricFactor(1 / 12f);
            for (int n = 9; n > -1; n--)
            {
                for (int m = 0; m < 4; m++)
                    Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.oldPos[n] - Main.screenPosition - (Projectile.velocity + Main.rand.NextVector2Unit() * 4) * MathF.Sqrt(m * .5f), new Rectangle((int)(16 * Projectile.ai[0]), 0, 16, 16), Color.Lerp(lightColor, Color.White, .5f) with { A = 0 } * ((10 - n) * .1f) * alpha, Projectile.oldRot[n] + MathHelper.PiOver2, new Vector2(8), 1f * ((10 - n) * .1f), 0, 0);

            }
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int k = 0; k < 7; k++)
            {
                Dust.NewDustPerfect(Projectile.Center + oldVelocity * .5f, DustID.BlueTorch, Main.rand.NextVector2Unit() * Main.rand.NextFloat(4) + oldVelocity / 8f, 0, default, Main.rand.NextFloat(0.5f, 1.5f));
            }
            for (int k = 0; k < 7; k++)
            {
                Dust.NewDustPerfect(Projectile.Center + oldVelocity * .5f, DustID.BlueTorch, Main.rand.NextVector2Unit() * Main.rand.NextFloat(4), 0, default, Main.rand.NextFloat(0.5f, 1.5f));
            }
            if (Projectile.alpha == 0 && !Main.rand.NextBool(4))
            {

                Projectile.alpha = 1;
                if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X;
                if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y;
            }
            else
            {
                if (Projectile.timeLeft > 3)
                {
                    Projectile.timeLeft = 3;
                    Projectile.velocity = oldVelocity;
                }

            }

            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 0;
            for (int n = 0; n < 6 - Projectile.penetrate; n++)
            {
                for (int k = 0; k < 7; k++)
                {
                    Dust.NewDustPerfect(target.Center, DustID.BlueTorch, Main.rand.NextVector2Unit() * Main.rand.NextFloat(4) + Projectile.velocity / 8f, 0, default, Main.rand.NextFloat(0.5f, 1.5f));
                }
                for (int k = 0; k < 7; k++)
                {
                    Dust.NewDustPerfect(target.Center, DustID.BlueTorch, Main.rand.NextVector2Unit() * Main.rand.NextFloat(4), 0, default, Main.rand.NextFloat(0.5f, 1.5f));
                }
            }
            if (!target.friendly && target.active && target.CanBeChasedBy())
            {
                Main.player[Projectile.owner].GetModPlayer<ElementChargePlayer>().ElementChargeValue[1] += damage / ElementChargePlayer.Devider;
            }
            base.OnHitNPC(target, damage, knockback, crit);
        }
        public override void AI()
        {
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
    public class FireAttack : ModProjectile
    {
        //BezierCurve<FloatVector2, Vector2> bezierCurve;
        Projectile projectile => Projectile;
        /// <summary>
        /// 0为火球 1真火球 2大爆炸 3小爆炸 4凤凰 5追踪真火 6火之领域
        /// </summary>
        int style => (int)projectile.ai[0];
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 32;
            Projectile.timeLeft = 180;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.aiStyle = -1;
        }
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            switch (style)
            {
                case 0:
                    {
                        float alpha = (Projectile.timeLeft / 180f).SmoothSymmetricFactor(1 / 12f);
                        Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition, new Rectangle(0, (int)Main.GameUpdateCount / 2 % 4, 78, 42), Color.White with { A = 0 } * alpha, Projectile.rotation, new Vector2(66, 21), new Vector2(2.5f, 1.75f) * .5f * new Vector2(MathF.Cos(Main.GlobalTimeWrappedHourly * MathHelper.TwoPi) * .25f + 1.75f, 1f), 0, 0);
                        for (int n = 0; n < 4; n++)
                        {
                            Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition + (MathHelper.PiOver4 * n).ToRotationVector2() * 8 + Main.rand.NextVector2Unit() * Main.rand.NextFloat(2, 6), new Rectangle(0, 42 * Main.rand.Next(4), 78, 42), Color.Lerp(Color.White, Color.Red, Main.rand.NextFloat(0, .5f)) with { A = 0 } * 0.125f * alpha, Projectile.rotation, new Vector2(66, 21), new Vector2(2f, 1.5f), 0, 0);
                        }
                        for (int n = 0; n < 4; n++)
                        {
                            Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition + (MathHelper.PiOver4 * n).ToRotationVector2() * 12 + Main.rand.NextVector2Unit() * Main.rand.NextFloat(6, 12), new Rectangle(0, 42 * Main.rand.Next(4), 78, 42), Color.Lerp(Color.Orange, Color.Red, Main.rand.NextFloat(0, .5f)) with { A = 0 } * 0.0625f * alpha, Projectile.rotation, new Vector2(66, 21), new Vector2(2f, 1.5f), 0, 0);
                        }
                        for (int n = 9; n > -1; n--)
                        {
                            Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.oldPos[n] - Main.screenPosition - (Projectile.velocity + Main.rand.NextVector2Unit() * 4), new Rectangle(0, 42 * Main.rand.Next(4), 78, 42), Color.Lerp(lightColor, Color.White, .5f) with { A = 0 } * ((10 - n) * .1f) * alpha * .25f, Projectile.oldRot[n], new Vector2(66, 21), 1f * ((10 - n) * .1f), 0, 0);
                        }
                        break;
                    }
                case 1:
                case 5:
                    {
                        float alpha = (Projectile.timeLeft / 180f).SmoothSymmetricFactor(1 / 12f);
                        Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition, new Rectangle(0, (int)Main.GameUpdateCount / 2 % 4, 78, 42), Color.White with { A = 0 } * alpha, Projectile.rotation, new Vector2(66, 21), new Vector2(2.5f, 1.75f) * .5f * new Vector2(MathF.Cos(Main.GlobalTimeWrappedHourly * MathHelper.TwoPi) * .25f + 1.75f, 1f), 0, 0);
                        for (int n = 0; n < 8; n++)
                        {
                            Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition + (MathHelper.PiOver4 * n).ToRotationVector2() * 8 + Main.rand.NextVector2Unit() * Main.rand.NextFloat(2, 6), new Rectangle(0, 42 * Main.rand.Next(4), 78, 42), Color.Lerp(Color.White, Color.Red, Main.rand.NextFloat(0, .5f)) with { A = 0 } * 0.125f * alpha, Projectile.rotation, new Vector2(66, 21), new Vector2(2f, 1.5f), 0, 0);
                        }
                        for (int n = 0; n < 8; n++)
                        {
                            Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition + (MathHelper.PiOver4 * n).ToRotationVector2() * 12 + Main.rand.NextVector2Unit() * Main.rand.NextFloat(6, 12), new Rectangle(0, 42 * Main.rand.Next(4), 78, 42), Color.Lerp(Color.Orange, Color.Red, Main.rand.NextFloat(0, .5f)) with { A = 0 } * 0.0625f * alpha, Projectile.rotation, new Vector2(66, 21), new Vector2(2f, 1.5f) * 1.5f, 0, 0);
                        }
                        //for (int n = 9; n > -1; n--)
                        //{
                        //    Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.oldPos[n] - Main.screenPosition - (Projectile.velocity + Main.rand.NextVector2Unit() * 4), new Rectangle(0, 42 * Main.rand.Next(4), 78, 42), Color.Lerp(lightColor, Color.White, .5f) with { A = 0 } * ((10 - n) * .1f) * alpha * .25f, Projectile.oldRot[n], new Vector2(66, 21), 1f * ((10 - n) * .1f), 0, 0);
                        //}
                        StoneOfThePhilosophersHelper.VertexDraw(projectile.TailVertexFromProj(default, 30, .5f, false, Color.Yellow * alpha),
                            ModContent.Request<Texture2D>("StoneOfThePhilosophers/Images/Style_4").Value,
                            ModContent.Request<Texture2D>("StoneOfThePhilosophers/Images/Style_8").Value,
                            ModContent.Request<Texture2D>("StoneOfThePhilosophers/Images/HeatMap_0").Value,
                            new Vector2(-Main.GlobalTimeWrappedHourly * 2, 0), false, null,
                            "HeatMap");
                        break;
                    }
                case 2:
                    {
                        var fac = 1 - projectile.timeLeft / 21f;
                        for (int n = 0; n < 3; n++)
                            Main.EntitySpriteDraw(ModContent.Request<Texture2D>(Texture.Replace("FireAttack", "ExplosionEffect")).Value, projectile.Center - Main.screenPosition + Main.rand.NextVector2Unit() * 4, new Rectangle(0, 588 - projectile.timeLeft / 3 * 98, 98, 98), new Color(255, 255, 255, 0) * fac.HillFactor2(1) * .75f, projectile.rotation, new Vector2(49), 3f * fac, 0, 0);//new Rectangle(0, projectile.timeLeft / 2, 52, 52)

                        break;
                    }
                case 3:
                    {
                        var fac = 1 - projectile.timeLeft / 21f;
                        Main.EntitySpriteDraw(ModContent.Request<Texture2D>(Texture.Replace("FireAttack", "ExplosionEffect")).Value, projectile.Center - Main.screenPosition, new Rectangle(0, 588 - projectile.timeLeft / 3 * 98, 98, 98), new Color(255, 255, 255, 0) * fac.HillFactor2(1), projectile.rotation, new Vector2(49), 2f * fac, 0, 0);//new Rectangle(0, projectile.timeLeft / 2, 52, 52)
                        break;
                    }
                case 4:
                    {
                        float alpha = (Projectile.timeLeft / 180f).SmoothSymmetricFactor(1 / 12f);
                        var tex = ModContent.Request<Texture2D>("StoneOfThePhilosophers/Contents/FirePhoenix").Value;
                        var rect = new Rectangle(0, (int)Main.GameUpdateCount / 2 % 8 * 76, 72, 76);
                        SpriteEffects spriteEffects = projectile.velocity.X > 0 ? 0 : SpriteEffects.FlipVertically;
                        //var origin = new Vector2(projectile.velocity.X > 0 ? 58 : 14, 42);
                        var origin = new Vector2(58, projectile.velocity.X > 0 ? 42 : 34);

                        Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, rect, Color.White with { A = 0 } * alpha, Projectile.rotation, origin, new Vector2(2.5f, 1.75f) * .5f * new Vector2(MathF.Cos(Main.GlobalTimeWrappedHourly * MathHelper.TwoPi) * .25f + 1.75f, 1f), spriteEffects, 0);
                        for (int n = 0; n < 8; n++)
                        {
                            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition + (MathHelper.PiOver4 * n).ToRotationVector2() * 8 + Main.rand.NextVector2Unit() * Main.rand.NextFloat(2, 6)
                                , rect, Color.Lerp(Color.White, Color.Red, Main.rand.NextFloat(0, .5f)) with { A = 0 } * 0.125f * alpha, Projectile.rotation, origin, new Vector2(3f, 1.5f), spriteEffects, 0);
                        }
                        for (int n = 0; n < 8; n++)
                        {
                            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition + (MathHelper.PiOver4 * n).ToRotationVector2() * 12 + Main.rand.NextVector2Unit() * Main.rand.NextFloat(6, 12)
                                , rect, Color.Lerp(Color.Orange, Color.Red, Main.rand.NextFloat(0, .5f)) with { A = 0 } * 0.0625f * alpha, Projectile.rotation, origin, new Vector2(4f, 1.5f) * 1.5f, spriteEffects, 0);
                        }
                        //for (int n = 9; n > -1; n--)
                        //{
                        //    Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.oldPos[n] - Main.screenPosition - (Projectile.velocity + Main.rand.NextVector2Unit() * 4), new Rectangle(0, 42 * Main.rand.Next(4), 78, 42), Color.Lerp(lightColor, Color.White, .5f) with { A = 0 } * ((10 - n) * .1f) * alpha * .25f, Projectile.oldRot[n], new Vector2(66, 21), 1f * ((10 - n) * .1f), 0, 0);
                        //}
                        StoneOfThePhilosophersHelper.VertexDraw(projectile.TailVertexFromProj(default, t => MathHelper.SmoothStep(30, 0, t), t => Color.Yellow * t.WaterDropFactor(), .5f),
                            ModContent.Request<Texture2D>("StoneOfThePhilosophers/Images/Style_4").Value,
                            ModContent.Request<Texture2D>("StoneOfThePhilosophers/Images/Style_8").Value,
                            ModContent.Request<Texture2D>("StoneOfThePhilosophers/Images/HeatMap_0").Value,
                            new Vector2(-Main.GlobalTimeWrappedHourly * 2, 0), false, null,
                            "HeatMap");//default, 30, .5f * alpha, false, Color.Yellow
                        break;
                    }
                case 6:
                    {
                        float alpha = (Projectile.timeLeft / 300f).SmoothSymmetricFactor(1 / 12f);
                        Main.spriteBatch.End();
                        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                        #region Shader
                        float r = Main.GlobalTimeWrappedHourly * 2f;
                        Main.instance.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("StoneOfThePhilosophers/Images/MagicArea_4").Value;
                        Main.instance.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("StoneOfThePhilosophers/Images/HeatMap_0").Value;
                        Main.instance.GraphicsDevice.SamplerStates[1] = SamplerState.AnisotropicWrap;
                        Main.instance.GraphicsDevice.SamplerStates[2] = SamplerState.AnisotropicClamp;
                        StoneOfThePhilosophers.HeatMap.Parameters["uTime"].SetValue(projectile.velocity.SafeNormalize(default).RotatedBy(MathHelper.PiOver2) * Main.GlobalTimeWrappedHourly);
                        StoneOfThePhilosophers.HeatMap.CurrentTechnique.Passes[0].Apply();

                        #endregion
                        Main.EntitySpriteDraw(TextureAssets.Projectile[ModContent.ProjectileType<SunAttack>()].Value, Projectile.Center - Main.screenPosition, null, Color.White * alpha, -2 * r, new Vector2(16), new Vector2(12f) * (-MathF.Cos(Main.GlobalTimeWrappedHourly * MathHelper.Pi) * .25f + 1.25f), 0, 0);//

                        Main.EntitySpriteDraw(TextureAssets.Projectile[ModContent.ProjectileType<SunAttack>()].Value, Projectile.Center - Main.screenPosition, null, Color.White * alpha, r, new Vector2(16), new Vector2(16f) * (MathF.Cos(Main.GlobalTimeWrappedHourly * MathHelper.Pi) * .125f + 1f), 0, 0);//
                        Main.spriteBatch.End();
                        Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                        break;
                    }
            }

            #region BezierCurveTest
            //IVector<FloatVector2, Vector2>[] vectors = new IVector<FloatVector2, Vector2>[8];
            //for (int n = 0; n < 8; n++)
            //{
            //    //vectors[n] = new FloatVector2((MathHelper.TwoPi / 7 * 2 * n).ToRotationVector2() * (1 + n * .125f));
            //    vectors[n] = new FloatVector2(Main.rand.NextVector2Unit() * Main.rand.NextFloat(.5f, 2f));
            //}
            ////for (int n = 0; n < 5; n++)
            ////{
            ////    vectors[n] = new FloatVector2((MathHelper.TwoPi / 5 * n).ToRotationVector2() * (n * n * .1f + 1));
            ////}
            //if (bezierCurve == null)
            //{
            //    bezierCurve = new BezierCurve<FloatVector2, Vector2>(vectors);
            //    bezierCurve.Recalculate(180);
            //}
            //else
            //{
            //    for (int n = 0; n < 8; n++)
            //    {
            //        float t = n / 7f;
            //        vectors[n] = bezierCurve.results[(int)(t * 179f)];
            //    }
            //}
            //var m = BezierCurve<FloatVector2, Vector2>.GetMatrix(2);
            //var str = m.ToString();
            ////var vecs = BezierCurve<FloatVector2, Vector2>.c_Vectors;
            ////if (vecs.Count < 4)
            ////{
            ////    vecs.Add(BezierCurve<FloatVector2, Vector2>.GetVectors(0));
            ////    vecs.Add(BezierCurve<FloatVector2, Vector2>.GetVectors(1));
            ////    vecs.Add(BezierCurve<FloatVector2, Vector2>.GetVectors(2));
            ////    vecs.Add(BezierCurve<FloatVector2, Vector2>.GetVectors(3));
            ////}
            ////var matrix = BezierCurve<FloatVector2, Vector2>.GetMatrix(2);
            ////var str = matrix.ToString();
            ////str += "";
            ////var vec = vecs[1];
            ////var str2 = "(";
            ////for (int i = 0; i < vec.Length; i++)
            ////{
            ////    str2 += $" {vec[i]}";
            ////}
            ////str2 += ")";
            //Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, 0, 1920, 1120), Color.Black);
            //foreach (var point in bezierCurve.results)
            //{
            //    var vec = point.Value;
            //    Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, vec * 128 + new Vector2(960, 560), new Rectangle(0, 0, 1, 1), Color.Red, 0, new Vector2(.5f), 8, 0, 0);
            //}
            ////foreach (var point in vectors)
            ////{

            ////}
            //for (int n = 0; n < vectors.Length; n++)
            //{
            //    var vec = vectors[n].Value;
            //    var t = n / (vectors.Length - 1f);
            //    Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, vec * 128 + new Vector2(960, 560), new Rectangle(0, 0, 1, 1), Main.hslToRgb(t, 1, 0.75f), 0, new Vector2(.5f), 16, 0, 0);
            //    Main.spriteBatch.DrawString(FontAssets.MouseText.Value, n.ToString(), vec * 128 + new Vector2(960, 560), Color.White);
            //}
            ////if (bezierCurve == null)
            ////{
            ////    var wtf = new IVector<FloatVector2, Vector2>[4];
            ////    for (int n = 0; n < 4; n++)
            ////    {
            ////        wtf[n] = new FloatVector2((MathHelper.TwoPi / 3 * n).ToRotationVector2() * 128 + new Vector2(960, 560));
            ////    }
            ////    bezierCurve = new BezierCurve<FloatVector2, Vector2>(wtf);
            ////    //bezierCurve.Recalculate(60);
            ////}
            ////var input = new Vector2[4];
            ////for (int n = 0; n < 4; n++)
            ////{
            ////    //input[n] = (MathHelper.TwoPi / 3 * n).ToRotationVector2();
            ////    input[n] = n switch
            ////    {
            ////        0 or 3 => new Vector2(1, 0),
            ////        1 => new Vector2(0, 1),
            ////        2 or _ => new Vector2(-1, 0)
            ////    };
            ////}
            ////var vectors = new Vector2[4];
            ////vectors[0] = input[0];
            ////vectors[^1] = input[^1];
            ////Vector2[] array = BezierCurve<FloatVector2, Vector2>.c_Matrixes[1].Apply(input[1..^1], input[0], input[^1]);
            ////for (int m = 0; m < 2; m++)
            ////    vectors[m + 1] = array[m];
            ////array = new Vector2[60];


            ////for (int n = 0; n < 4; n++) 
            ////{
            ////    var str = vectors[n].ToString();
            ////    str += "";
            ////}
            ////for (int n = 0; n < 60; n++)
            ////{
            ////    float t = n / (60 - 1f);
            ////    for (int i = 0; i < 4; i++)
            ////    {
            ////        if (i == 0) array[n] = vectors[0] * MathF.Pow(1 - t, 3);
            ////        else array[n] = array[n] + vectors[i] * MathF.Pow(1 - t, 3 - i) * MathF.Pow(t, i) * BezierCurve<FloatVector2, Vector2>.c_Vectors[1][i];
            ////    }
            ////}
            ////#region MyRegion
            //////MatrixEX matrix = new MatrixEX(4, (i, j) => i switch
            //////{
            //////    0 => j switch
            //////    {
            //////        0 => 2,
            //////        1 => 3,
            //////        2 => 3,
            //////        3 or _ => 3
            //////    },
            //////    1 => j switch
            //////    {
            //////        0 => 1,
            //////        1 => 1,
            //////        2 => 4,
            //////        3 or _ => 5
            //////    },
            //////    2 => j switch
            //////    {
            //////        0 => 5,
            //////        1 => 1,
            //////        2 => 4,
            //////        3 or _ => 0
            //////    },
            //////    3 or _ => j switch
            //////    {
            //////        0 => 2,
            //////        1 => 7,
            //////        2 => 1,
            //////        3 or _ => 8
            //////    }
            //////});
            //////float[] values = matrix.Apply(new float[] { 1, 2, 3, 4 });
            //////var str = "(";
            //////for (int n = 0; n < 4; n++) 
            //////{
            //////    str += $" {values[n]},";
            //////}
            //////str += ")";
            //////str += "";
            ////#endregion
            ////BezierCurve<FloatVector2, Vector2>.GetMatrix(1);
            ////foreach (var vec in array)
            ////{
            ////    Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, vec * 128 + new Vector2(960, 560), new Rectangle(0, 0, 1, 1), Color.Red, 0, new Vector2(.5f), 8, 0, 0);
            ////}
            ////foreach (var vec in input)
            ////{
            ////    Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, vec * 128 + new Vector2(960, 560), new Rectangle(0, 0, 1, 1), Color.Cyan, 0, new Vector2(.5f), 16, 0, 0);
            ////}
            #endregion

            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (style == 0 || style == 1 || style == 5)
            {
                Projectile.timeLeft = 15;
                Projectile.friendly = false;
                Projectile.tileCollide = false;
                Projectile.velocity = oldVelocity * .375f;
                for (int n = 0; n < 3; n++)
                {
                    var unit = (MathHelper.TwoPi / 6 * n + projectile.rotation).ToRotationVector2();
                    var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center + unit * 16, unit * 4, projectile.type, projectile.damage, 8, projectile.owner, style == 0 ? 3 : 2);
                    proj.timeLeft = 21;
                    proj.width = proj.height = 160;
                    proj.penetrate = -1;
                    proj.Center = projectile.Center + projectile.velocity;
                    proj.rotation = MathHelper.TwoPi / 6 * n + projectile.rotation;
                    proj.tileCollide = false;
                }
                for (int num431 = 4; num431 < 31; num431++)
                {
                    float num432 = projectile.oldVelocity.X * (30f / (float)num431);
                    float num433 = projectile.oldVelocity.Y * (30f / (float)num431);
                    for (int n = 0; n < 4; n++)
                    {
                        int num434 = Dust.NewDust(new Vector2(projectile.oldPosition.X - num432, projectile.oldPosition.Y - num433) + Main.rand.NextVector2Unit() * Main.rand.NextFloat(4) + projectile.velocity, 8, 8, MyDustId.Fire, projectile.oldVelocity.X, projectile.oldVelocity.Y, 100, Color.Orange, 1.2f);
                        Main.dust[num434].noGravity = true;
                        Dust dust = Main.dust[num434];
                        dust.velocity = projectile.velocity;
                        dust.velocity *= 0.5f;
                    }
                }
                SoundEngine.PlaySound(SoundID.Item62);
            }
            return false;
        }
        public override void Kill(int timeLeft)
        {
            //switch (style)
            //{
            //    case 0:
            //        {
            //            for (int n = 0; n < 3; n++)
            //            {
            //                var unit = (MathHelper.TwoPi / 6 * n + projectile.rotation).ToRotationVector2();
            //                var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center + unit * 16, unit * 4, projectile.type, projectile.damage, 8, projectile.owner, 1);
            //                proj.timeLeft = 21;
            //                proj.width = proj.height = 160;
            //                proj.penetrate = -1;
            //                proj.Center = projectile.Center + projectile.velocity;
            //                proj.rotation = MathHelper.TwoPi / 6 * n + projectile.rotation;
            //                proj.tileCollide = false;
            //            }
            //            for (int num431 = 4; num431 < 31; num431++)
            //            {
            //                float num432 = projectile.oldVelocity.X * (30f / (float)num431);
            //                float num433 = projectile.oldVelocity.Y * (30f / (float)num431);
            //                for (int n = 0; n < 4; n++)
            //                {
            //                    int num434 = Dust.NewDust(new Vector2(projectile.oldPosition.X - num432, projectile.oldPosition.Y - num433) + Main.rand.NextVector2Unit() * Main.rand.NextFloat(4) + projectile.velocity, 8, 8, MyDustId.Fire, projectile.oldVelocity.X, projectile.oldVelocity.Y, 100, Color.Orange, 1.2f);
            //                    Main.dust[num434].noGravity = true;
            //                    Dust dust = Main.dust[num434];
            //                    dust.velocity = projectile.velocity;
            //                    dust.velocity *= 0.5f;
            //                }

            //            }
            //            SoundEngine.PlaySound(SoundID.Item62);
            //            break;
            //        }
            //}
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(24, 300);
            //target.immune[Projectile.owner] = 0;
            if (style == 0 || style == 1 || style == 5)
            {
                Projectile.timeLeft = 15;
                Projectile.friendly = false;
                for (int n = 0; n < 3; n++)
                {
                    var unit = (MathHelper.TwoPi / 6 * n + projectile.rotation).ToRotationVector2();
                    var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center + unit * 16, unit * 4, projectile.type, projectile.damage * 3 / 4, 8, projectile.owner, style == 0 ? 3 : 2);
                    proj.timeLeft = 21;
                    proj.width = proj.height = 160;
                    proj.penetrate = -1;
                    proj.Center = projectile.Center + projectile.velocity;
                    proj.rotation = MathHelper.TwoPi / 6 * n + projectile.rotation;
                    proj.tileCollide = false;
                }
                for (int num431 = 4; num431 < 31; num431++)
                {
                    float num432 = projectile.oldVelocity.X * (30f / (float)num431);
                    float num433 = projectile.oldVelocity.Y * (30f / (float)num431);
                    for (int n = 0; n < 4; n++)
                    {
                        int num434 = Dust.NewDust(new Vector2(projectile.oldPosition.X - num432, projectile.oldPosition.Y - num433) + Main.rand.NextVector2Unit() * Main.rand.NextFloat(4) + projectile.velocity, 8, 8, MyDustId.Fire, projectile.oldVelocity.X, projectile.oldVelocity.Y, 100, Color.Orange, 1.2f);
                        Main.dust[num434].noGravity = true;
                        Dust dust = Main.dust[num434];
                        dust.velocity = projectile.velocity;
                        dust.velocity *= 0.5f;
                    }

                }
                SoundEngine.PlaySound(SoundID.Item62);
                target.immune[projectile.owner] = 2;
            }
            else if (style == 4)
            {
                for (int n = 0; n < 2; n++)
                {
                    if (!Main.rand.NextBool(4)) continue;
                    var unit = (MathHelper.TwoPi / 6 * n + projectile.rotation).ToRotationVector2();
                    var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center + unit * 16, unit * 4, projectile.type, projectile.damage / 6, 8, projectile.owner, 3);
                    proj.timeLeft = 21;
                    proj.width = proj.height = 120;
                    proj.penetrate = -1;
                    proj.Center = projectile.Center + projectile.velocity;
                    proj.rotation = MathHelper.TwoPi / 6 * n + projectile.rotation;
                    proj.tileCollide = false;

                }
                for (int num431 = 4; num431 < 31; num431++)
                {
                    float num432 = projectile.oldVelocity.X * (30f / (float)num431);
                    float num433 = projectile.oldVelocity.Y * (30f / (float)num431);
                    for (int n = 0; n < 4; n++)
                    {
                        int num434 = Dust.NewDust(new Vector2(projectile.oldPosition.X - num432, projectile.oldPosition.Y - num433) + Main.rand.NextVector2Unit() * Main.rand.NextFloat(4) + projectile.velocity, 8, 8, MyDustId.Fire, projectile.oldVelocity.X, projectile.oldVelocity.Y, 100, Color.Orange, 1.2f);
                        Main.dust[num434].noGravity = true;
                        Dust dust = Main.dust[num434];
                        dust.velocity = projectile.velocity;
                        dust.velocity *= 0.5f;
                    }

                }
                var soundEff = SoundID.Item62;
                soundEff.Volume *= .5f;
                SoundEngine.PlaySound(soundEff);
                target.immune[projectile.owner] = 2;
            }
            else if (style == 6)
            {
                target.immune[projectile.owner] = 0;
                target.AddBuff(BuffID.Daybreak, 300);
            }
            else
            {
                projectile.frameCounter++;
                target.immune[projectile.owner] = 0;
            }
            if (!target.friendly && target.active && target.CanBeChasedBy())
            {
                Main.player[Projectile.owner].GetModPlayer<ElementChargePlayer>().ElementChargeValue[0] += damage / ElementChargePlayer.Devider;
            }


        }
        public override void AI()
        {
            switch (style)
            {
                case 0:
                case 1:
                case 4:
                case 5:
                    {
                        Projectile.rotation = Projectile.velocity.ToRotation();
                        Dust.NewDustPerfect(projectile.Center, MyDustId.Fire, new Vector2(0, 0), 0, Color.White, 1f).noGravity = true;
                        Dust.NewDustPerfect(projectile.Center + Main.rand.NextVector2Unit() * 4, MyDustId.Fire, Main.rand.NextVector2Unit() + projectile.velocity * .25f, 0, Color.White, Main.rand.NextFloat(1.5f, 3f)).noGravity = true;

                        for (int n = 9; n > 0; n--)
                        {
                            Projectile.oldPos[n] = Projectile.oldPos[n - 1];
                            Projectile.oldRot[n] = Projectile.oldRot[n - 1];
                        }
                        Projectile.oldPos[0] = Projectile.Center;
                        Projectile.oldRot[0] = Projectile.rotation;
                        if (style == 4)
                        {
                            if (projectile.timeLeft == 15)
                            {
                                Projectile.friendly = false;
                                for (int n = 0; n < 5; n++)
                                {
                                    var unit = (MathHelper.TwoPi / 6 * n + projectile.rotation + Main.rand.NextFloat()).ToRotationVector2();
                                    var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center, unit * 32, projectile.type, projectile.damage / 2, 8, projectile.owner, 5);
                                    proj.rotation = MathHelper.TwoPi / 6 * n + projectile.rotation;
                                }
                                for (int num431 = 4; num431 < 31; num431++)
                                {
                                    float num432 = projectile.oldVelocity.X * (30f / (float)num431);
                                    float num433 = projectile.oldVelocity.Y * (30f / (float)num431);
                                    for (int n = 0; n < 4; n++)
                                    {
                                        int num434 = Dust.NewDust(new Vector2(projectile.oldPosition.X - num432, projectile.oldPosition.Y - num433) + Main.rand.NextVector2Unit() * Main.rand.NextFloat(4) + projectile.velocity, 8, 8, MyDustId.Fire, projectile.oldVelocity.X, projectile.oldVelocity.Y, 100, Color.Orange, 1.2f);
                                        Main.dust[num434].noGravity = true;
                                        Dust dust = Main.dust[num434];
                                        dust.velocity = projectile.velocity;
                                        dust.velocity *= 0.5f;
                                    }

                                }
                                SoundEngine.PlaySound(SoundID.Item62);
                            }
                        }
                        if (style == 5)
                        {
                            NPC target = null;
                            float maxDistance = 256f;
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
                                Projectile.velocity = Vector2.Lerp(Projectile.velocity, (target.Center - Projectile.Center).SafeNormalize(default) * 32, Utils.GetLerpValue(32, 256, maxDistance, true) * 0.25f);
                            }
                        }
                        break;
                    }
                case 2:
                case 3:
                    {
                        projectile.friendly = projectile.frameCounter == 0;
                        break;
                    }
                case 6:
                    {
                        if (projectile.timeLeft % 15 == 0)
                        {
                            foreach (var npc in Main.npc)
                            {
                                if (npc.active && !npc.friendly)
                                {
                                    var distance = Vector2.Distance(npc.Center, projectile.Center);
                                    if (distance < 320)
                                    {
                                        var fac = Utils.GetLerpValue(320, 0, distance);
                                        var damage = (int)(MathF.Pow(fac, 0.5f) * projectile.damage / 4);
                                        int count = (int)(fac * 30);
                                        for (int n = 0; n < count; n++)
                                        {
                                            var unit = (n * MathHelper.TwoPi / count).ToRotationVector2();
                                            Dust.NewDustPerfect(npc.Center, MyDustId.Fire, unit * Main.rand.NextFloat(1, 3), 0, default, Main.rand.NextFloat(0.5f, 1f));
                                        }
                                        if (npc.CanBeChasedBy() || npc.type == NPCID.TargetDummy)
                                        {
                                            Main.player[projectile.owner].ApplyDamageToNPC(npc, damage, 0, projectile.direction, false);
                                            Main.player[Projectile.owner].GetModPlayer<ElementChargePlayer>().ElementChargeValue[6] += damage / ElementChargePlayer.Devider;
                                        }
                                        npc.AddBuff(BuffID.Daybreak, 30);
                                        //OnHitNPC(npc, damage, 0, false);
                                    }
                                }
                            }
                            for (int n = 0; n < 30; n++)
                            {
                                var unit = (n / 30f * MathHelper.TwoPi).ToRotationVector2();
                                Dust.NewDustPerfect(projectile.Center + unit * 96, MyDustId.Fire, unit * Main.rand.NextFloat(2, 8), 0, default, Main.rand.NextFloat(1f, 1.5f));
                            }
                        }
                        break;
                    }
            }
            if (style == 2 && projectile.timeLeft == 10)
            {
                for (int n = 0; n < 3; n++)
                {
                    var unit = Main.rand.NextVector2Unit();
                    var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center + unit * Main.rand.NextFloat(16, 32f) * 16, unit * Main.rand.NextFloat(2, 8), projectile.type, projectile.damage / 3, 8, projectile.owner, 3);
                    proj.timeLeft = 21;
                    proj.width = proj.height = 80;
                    proj.penetrate = -1;
                    proj.Center = projectile.Center + projectile.velocity;
                    proj.rotation = MathHelper.TwoPi / 6 * n + projectile.rotation;
                    proj.tileCollide = false;
                    for (int m = 0; m < 8; m++)
                    {
                        Dust.NewDustPerfect(proj.Center, MyDustId.Fire, Main.rand.NextVector2Unit() * Main.rand.NextFloat(2, 8), 0, default, Main.rand.NextFloat(0.5f, 2f));
                    }
                }
                SoundEngine.PlaySound(SoundID.Item62);
            }
            base.AI();
        }
    }
    public class EarthAttack : ModProjectile
    {
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return base.OnTileCollide(oldVelocity);
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.magic = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
            Projectile.ignoreWater = true;
        }
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            switch ((int)Projectile.ai[1])
            {
                case 0:
                    {
                        float alpha = (Projectile.timeLeft / 60f).SmoothSymmetricFactor(1 / 12f);
                        for (int n = 9; n > -1; n--)
                            Main.EntitySpriteDraw(ModContent.Request<Texture2D>(Texture + "Piece").Value, Projectile.oldPos[n] - Main.screenPosition,
                                new Rectangle((int)(16 * Projectile.frame), 0, 16, 16), lightColor * ((10 - n) * .1f) * alpha * (n == 0 ? 1 : .25f), Projectile.oldRot[n], new Vector2(8), 3f * ((10 - n) * .1f), 0, 0);
                        break;
                    }
                case 1:
                    {
                        break;
                    }
                case 2:
                    {
                        float num287 = 300f;
                        float num288 = Projectile.ai[0];
                        float num289 = MathHelper.Clamp(num288 / 30f, 0f, 1f);
                        if (num288 > num287 - 60f)
                            num289 = MathHelper.Lerp(1f, 0f, (num288 - (num287 - 60f)) / 60f);

                        float num290 = 0.2f;
                        Vector2 top2 = Projectile.Top;
                        Vector2 bottom = Projectile.Bottom;
                        Vector2.Lerp(top2, bottom, 0.5f);
                        Vector2 vector44 = new Vector2(0f, bottom.Y - top2.Y);
                        vector44.X = vector44.Y * num290;
                        new Vector2(top2.X - vector44.X / 2f, top2.Y);
                        Texture2D value108 = TextureAssets.Projectile[Projectile.type].Value;
                        Rectangle rectangle19 = value108.Frame();
                        Vector2 origin19 = rectangle19.Size() / 2f;
                        float num291 = -(float)Math.PI / 20f * num288 * (float)((!(Projectile.velocity.X > 0f)) ? 1 : (-1));
                        SpriteEffects effects3 = (Projectile.velocity.X > 0f) ? SpriteEffects.FlipVertically : SpriteEffects.None;
                        bool flag31 = Projectile.velocity.X > 0f;
                        Vector2 spinningpoint5 = Vector2.UnitY.RotatedBy(num288 * 0.14f);
                        float num292 = 0f;
                        float num293 = 5.01f + num288 / 150f * -0.9f;
                        if (num293 < 4.11f)
                            num293 = 4.11f;

                        Color value109 = new Color(160, 140, 100, 127);
                        Color color69 = new Color(140, 160, 255, 127);
                        float num294 = num288 % 60f;
                        if (num294 < 30f)
                            color69 *= Utils.GetLerpValue(22f, 30f, num294, clamped: true);
                        else
                            color69 *= Utils.GetLerpValue(38f, 30f, num294, clamped: true);

                        bool flag32 = color69 != Color.Transparent;
                        for (float num295 = (int)bottom.Y; num295 > (float)(int)top2.Y; num295 -= num293)
                        {
                            num292 += num293;
                            float num296 = num292 / vector44.Y;
                            float num297 = num292 * ((float)Math.PI * 2f) / -20f;
                            if (flag31)
                                num297 *= -1f;

                            float num298 = num296 - 0.35f;
                            Vector2 position19 = spinningpoint5.RotatedBy(num297);
                            Vector2 value110 = new Vector2(0f, num296 + 1f);
                            value110.X = value110.Y * num290;
                            Color color70 = Color.Lerp(Color.Transparent, value109, num296 * 2f);
                            if (num296 > 0.5f)
                                color70 = Color.Lerp(Color.Transparent, value109, 2f - num296 * 2f);

                            color70.A = (byte)((float)(int)color70.A * 0.5f);
                            color70 *= num289;
                            position19 *= value110 * 100f;
                            position19.Y = 0f;
                            position19.X = 0f;
                            position19 += new Vector2(bottom.X, num295) - Main.screenPosition;
                            if (flag32)
                            {
                                Color color71 = Color.Lerp(Color.Transparent, color69, num296 * 2f);
                                if (num296 > 0.5f)
                                    color71 = Color.Lerp(Color.Transparent, color69, 2f - num296 * 2f);

                                color71.A = (byte)((float)(int)color71.A * 0.5f);
                                color71 *= num289;
                                Main.EntitySpriteDraw(value108, position19, rectangle19, color71, num291 + num297, origin19, (1f + num298) * 0.8f, effects3, 0);
                            }

                            Main.EntitySpriteDraw(value108, position19, rectangle19, color70, num291 + num297, origin19, 1f + num298, effects3, 0);
                        }
                        break;
                    }
            }
            return false;
        }
        public void SetDefaultStorm()
        {
            Projectile.width = 60;
            Projectile.height = 210;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.magic = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.timeLeft = 60;
            Projectile.localNPCHitCooldown = -1;
            Projectile.ignoreWater = true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!target.friendly && target.active && target.CanBeChasedBy())
            {
                Main.player[Projectile.owner].GetModPlayer<ElementChargePlayer>().ElementChargeValue[4] += damage / ElementChargePlayer.Devider;
            }
            base.OnHitNPC(target, damage, knockback, crit);
        }
        public override void AI()
        {
            switch ((int)Projectile.ai[1])
            {
                case 0:
                    {
                        Projectile.velocity += new Vector2(0, 0.2f);
                        Projectile.rotation++;
                        for (int n = 9; n > 0; n--)
                        {
                            Projectile.oldPos[n] = Projectile.oldPos[n - 1];
                            Projectile.oldRot[n] = Projectile.oldRot[n - 1];
                        }
                        Projectile.oldPos[0] = Projectile.Center;
                        Projectile.oldRot[0] = Projectile.rotation;
                        //Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Unit() * 4, MyDustId.BrownDirt, Main.rand.NextVector2Unit() + Projectile.velocity * .25f, 0, Color.White, Main.rand.NextFloat(1.5f, 3f));
                        if (Projectile.timeLeft % 2 == 0 && Main.rand.NextBool(3))
                        {
                            var unit = Projectile.velocity.RotatedByRandom(0.1f);
                            Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), Projectile.Center, unit, Type, Projectile.damage / 3, Projectile.knockBack, Projectile.owner, 0, 1).tileCollide = false;
                            for (int n = 0; n < 5; n++)
                            {
                                var dust = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Unit() * 4, MyDustId.BrownDirt, unit * .125f + Main.rand.NextVector2Unit() * 4, 0, Color.White with { A = (byte)Main.rand.Next(0, 255) } * Main.rand.NextFloat(0, 1), Main.rand.NextFloat(1.5f, 3f) * .5f);
                                dust.noGravity = true;
                                dust.fadeIn = -1f;
                            }
                        }
                        break;
                    }
                case 1:
                    {
                        Projectile.velocity *= 0.85f;
                        break;
                    }
                case 2:
                    {
                        float num = 300f;
                        if (Projectile.soundDelay == 0)
                        {
                            Projectile.soundDelay = -1;
                            Projectile.localAI[1] = SoundEngine.PlayTrackedSound(SoundID.DD2_BookStaffTwisterLoop, Projectile.Center).ToFloat();
                        }

                        ActiveSound activeSound = SoundEngine.GetActiveSound(SlotId.FromFloat(Projectile.localAI[1]));
                        if (activeSound != null)
                        {
                            activeSound.Position = Projectile.Center;
                            activeSound.Volume = 1f - Math.Max(Projectile.ai[0] - (num - 15f), 0f) / 15f;
                        }
                        else
                        {
                            Projectile.localAI[1] = SlotId.Invalid.ToFloat();
                        }

                        if (Projectile.localAI[0] >= 16f && Projectile.ai[0] < num - 15f)
                            Projectile.ai[0] = num - 15f;

                        Projectile.ai[0] += 1f;
                        if (Projectile.ai[0] >= num)
                            Projectile.Kill();

                        Vector2 top = Projectile.Top;
                        Vector2 bottom = Projectile.Bottom;
                        Vector2 value = Vector2.Lerp(top, bottom, 0.5f);
                        Vector2 value2 = new Vector2(0f, bottom.Y - top.Y);
                        value2.X = value2.Y * 0.2f;

                        if (Projectile.ai[0] < num - 30f)
                        {
                            for (int j = 0; j < 1; j++)
                            {
                                float value3 = -1f;
                                float value4 = 0.9f;
                                float amount = Main.rand.NextFloat();
                                Vector2 value5 = new Vector2(MathHelper.Lerp(0.1f, 1f, Main.rand.NextFloat()), MathHelper.Lerp(value3, value4, amount));
                                value5.X *= MathHelper.Lerp(2.2f, 0.6f, amount);
                                value5.X *= -1f;
                                Vector2 value6 = new Vector2(6f, 10f);
                                Vector2 position2 = value + value2 * value5 * 0.5f + value6;
                                Dust dust = Main.dust[Dust.NewDust(position2, 0, 0, DustID.ApprenticeStorm)];
                                dust.position = position2;
                                dust.fadeIn = 1.3f;
                                dust.scale = 0.87f;
                                dust.alpha = 211;
                                if (value5.X > -1.2f)
                                    dust.velocity.X = 1f + Main.rand.NextFloat();

                                dust.noGravity = true;
                                dust.velocity.Y = Main.rand.NextFloat() * -0.5f - 1.3f;
                                dust.velocity.X += Projectile.velocity.X * 2.1f;
                                dust.noLight = true;
                            }
                        }

                        Vector2 position3 = Projectile.Bottom + new Vector2(-25f, -25f);
                        for (int k = 0; k < 4; k++)
                        {
                            Dust dust2 = Dust.NewDustDirect(position3, 50, 25, DustID.Smoke, Projectile.velocity.X, -2f, 100);
                            dust2.fadeIn = 1.1f;
                            dust2.noGravity = true;
                        }

                        if (Main.rand.NextBool(3))
                        {
                            var unit = Projectile.velocity.RotatedByRandom(0.1f);
                            var proj = Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), Projectile.Center, unit, Type, Projectile.damage * 2 / 3, Projectile.knockBack, Projectile.owner, 0, 1);
                            proj.tileCollide = false;
                            proj.timeLeft = 180;
                            for (int n = 0; n < 5; n++)
                            {
                                var dust = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Unit() * 4, MyDustId.BrownDirt, unit * .125f + Main.rand.NextVector2Unit() * 8, 0, Color.White with { A = (byte)Main.rand.Next(0, 255) } * Main.rand.NextFloat(0, 1), Main.rand.NextFloat(1.5f, 3f) * .5f);
                                dust.noGravity = true;
                                dust.fadeIn = -2f;
                            }
                        }

                        //for (int l = 0; l < 1; l++)
                        //{
                        //    if (Main.rand.NextBool(5))
                        //    {
                        //        Gore gore = Gore.NewGoreDirect(Projectile.TopLeft + Main.rand.NextVector2Square(0f, 1f) * Projectile.Size, new Vector2(Projectile.velocity.X * 1.5f, (0f - Main.rand.NextFloat()) * 16f), Utils.SelectRandom(Main.rand, 1007, 1008, 1008));
                        //        gore.timeLeft = 60;
                        //        gore.alpha = 50;
                        //        gore.velocity.X += Projectile.velocity.X;
                        //    }
                        //}

                        //for (int m = 0; m < 1; m++)
                        //{
                        //    if (Main.rand.NextBool(7))
                        //    {
                        //        Gore gore2 = Gore.NewGoreDirect(Projectile.TopLeft + Main.rand.NextVector2Square(0f, 1f) * Projectile.Size, new Vector2(Projectile.velocity.X * 1.5f, (0f - Main.rand.NextFloat()) * 16f), Utils.SelectRandom(Main.rand, 1007, 1008, 1008));
                        //        gore2.timeLeft = 0;
                        //        gore2.alpha = 80;
                        //    }
                        //}

                        //for (int n = 0; n < 1; n++)
                        //{
                        //    if (Main.rand.NextBool(7))
                        //    {
                        //        Gore gore3 = Gore.NewGoreDirect(Projectile.TopLeft + Main.rand.NextVector2Square(0f, 1f) * Projectile.Size, new Vector2(Projectile.velocity.X * 1.5f, (0f - Main.rand.NextFloat()) * 16f), Utils.SelectRandom(Main.rand, 1007, 1008, 1008));
                        //        gore3.timeLeft = 0;
                        //        gore3.alpha = 80;
                        //    }
                        //}
                        break;
                    }
            }
        }
    }
    public class MoonAttack : ModProjectile
    {
        public bool boost = false;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int n = 0; n < 15; n++)
            {
                Dust.NewDustPerfect(target.Center, DustID.PinkTorch, dirVec.SafeNormalize(default).RotatedByRandom(MathHelper.Pi / 6) * Main.rand.NextFloat(-6, 6), 0, default, Main.rand.NextFloat(1, 2)).noGravity = true;
            }
            if (!target.friendly && target.active && target.CanBeChasedBy())
            {
                Main.player[Projectile.owner].GetModPlayer<ElementChargePlayer>().ElementChargeValue[5] += damage / ElementChargePlayer.Devider;
            }
        }
        public Projectile projectile => Projectile;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (projectile.timeLeft > 30)
            {
                return false;
            }
            float point = 0f;
            float factor = projectile.timeLeft / 15f;
            factor = MathHelper.Clamp(factor * factor, 0, 1);
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, projectile.Center + dirVec * factor, (boost ? 2 : 1) * 32 * (float)Math.Sin(MathHelper.Pi * Math.Sqrt(1 - projectile.timeLeft / 30f)), ref point))
            {
                return true;
            }

            return false;
        }

        public Vector2 dirVec;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            var factor = 0.125f * MathHelper.SmoothStep(0, 1, (45 - projectile.timeLeft) / 15f);
            var origin = boost ? new Vector2(128) : new Vector2(16, 48);
            var scaler = boost ? new Vector2(0.125f, 0.375f * 1.25f) : new Vector2(1, 0.75f);
            var texture = boost ? ModContent.Request<Texture2D>(Texture + "_Ultra").Value : TextureAssets.Projectile[Type].Value;
            float a = Main.rand.NextFloat(0, 1);
            float b = Main.rand.NextFloat(0, 1);
            var color = boost ? (Color.Lerp(Color.Lerp(Color.Blue, Color.Cyan, a), Color.Purple, b)) : Color.Purple;
            spriteBatch.Draw(texture, projectile.Center + dirVec * 0.5f - Main.screenPosition, null, color with { A = 0 } * factor, projectile.ai[0], origin, scaler * new Vector2(projectile.ai[1] / 32f, factor * 1.25f), 0, 0);
            spriteBatch.Draw(texture, projectile.Center + dirVec * 0.5f - Main.screenPosition, null, Color.White with { A = 0 } * factor, projectile.ai[0], origin, scaler * new Vector2(projectile.ai[1] / 32f, factor * .625f), 0, 0);

            if (projectile.timeLeft < 30)
            {
                float fac = (30 - projectile.timeLeft) / 15f;
                fac = MathHelper.Clamp(fac * fac, 0, 1);
                factor = (float)Math.Sin(MathHelper.Pi * Math.Sqrt(1 - projectile.timeLeft / 30f));
                spriteBatch.Draw(texture, projectile.Center + dirVec * 0.5f * fac - Main.screenPosition + Main.rand.NextVector2Unit() * factor * 12f, null, color with { A = 0 } * factor, projectile.ai[0], origin, scaler * new Vector2(projectile.ai[1] / 32f * fac, factor * 1.25f), 0, 0);
                spriteBatch.Draw(texture, projectile.Center + dirVec * 0.5f * fac - Main.screenPosition + Main.rand.NextVector2Unit() * factor * 8f, null, Color.White with { A = 0 } * factor, projectile.ai[0], origin, scaler * new Vector2(projectile.ai[1] / 32f * fac, factor * .625f), 0, 0);
            }
            var randSize = factor * Main.rand.NextFloat(0.5f);
            #region 法阵
            Matrix transform =
                Matrix.CreateScale(2) *
                Matrix.CreateTranslation(-1, -1, -1) *
                new Matrix(projectile.ai[1] * .5f, 0, 0, 0,
                                                0, (boost ? 64 : 48) * (1 + factor + randSize), 0, 0,
                                                0, 0, (boost ? 64 : 48) * (1 + factor + randSize), 0,
                                                0, 0, 0, 1) *
                Matrix.CreateRotationX(Main.GlobalTimeWrappedHourly * MathHelper.TwoPi / 2f) *
                Matrix.CreateRotationZ(projectile.ai[0]);
            CustomVertexInfo[] vertexInfos = new CustomVertexInfo[16];
            for (int n = 0; n < 16; n++)
            {
                var vec = new Vector3(n % 2, n / 4 % 2, n / 2 % 2);
                vertexInfos[n].TexCoord = new Vector3(vec.Y, vec.Z, 1);

                vertexInfos[n].Color = color * factor;
                vec = Vector3.Transform(vec, transform);
                if (n == 7)
                {
                    randSize = factor * Main.rand.NextFloat(0.75f);

                    transform =
                    Matrix.CreateScale(2) *
                    Matrix.CreateTranslation(-1, -1, -1) *
                    new Matrix(projectile.ai[1] * .5f * 1.05f, 0, 0, 0,
                                            0, 48 * (3 - factor - randSize) * .8f, 0, 0,
                                            0, 0, 48 * (3 - factor - randSize) * .8f, 0,
                                            0, 0, 0, 1) *
                    Matrix.CreateRotationX(-Main.GlobalTimeWrappedHourly * MathHelper.TwoPi) *
                    Matrix.CreateRotationZ(projectile.ai[0]);
                }
                vec += new Vector3(projectile.Center + dirVec * 0.5f - Main.screenPosition - new Vector2(Main.screenWidth, Main.screenHeight) * .5f, 0);
                vec.Z = (2000 - vec.Z) / 2000f;
                vec /= vec.Z;
                vertexInfos[n].Position = new Vector2(vec.X, vec.Y) + Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) * .5f;
            }
            CustomVertexInfo[] vertexs = new CustomVertexInfo[24];
            for (int n = 0; n < 24; n++)
            {
                int index = (n % 12) switch
                {
                    0 => 0,
                    1 => 4,
                    2 => 6,
                    3 => 0,
                    4 => 6,
                    5 => 2,
                    6 => 1,
                    7 => 5,
                    8 => 7,
                    9 => 1,
                    10 => 7,
                    11 or _ => 3,
                };
                if (n > 11) index += 8;
                vertexs[n] = vertexInfos[index];
            }
            StoneOfThePhilosophersHelper.VertexDraw(vertexs, ModContent.Request<Texture2D>($"StoneOfThePhilosophers/Images/MagicArea_{(boost ? 5 : 2)}").Value, TextureAssets.MagicPixel.Value);
            #endregion

            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("沉静的月神");
        }
        public override void SetDefaults()
        {
            projectile.tileCollide = false;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.DamageType = DamageClass.Magic;
            projectile.aiStyle = -1;
            projectile.width = 1;
            projectile.height = 1;
            projectile.timeLeft = 45;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
        }
        public override void AI()
        {
            projectile.localNPCHitCooldown = boost ? 5 : 10;
            if (projectile.timeLeft == 45)
            {
                dirVec = projectile.ai[0].ToRotationVector2() * projectile.ai[1];
            }

            if (projectile.timeLeft == 30)
            {
                SoundEngine.PlaySound(SoundID.Item12, projectile.Center);
            }
            //if (owner.type != ModContent.NPCType<ErchiusHorror>() || !owner.active || owner.ai[3] != 2) projectile.Kill();
        }
    }
    public class SunAttack : ModProjectile
    {
        Projectile projectile => Projectile;
        /// <summary>
        /// 0为火球 1大爆炸 2小爆炸
        /// </summary>
        int style => (int)projectile.ai[0];
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 32;
            Projectile.timeLeft = 180;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.aiStyle = -1;
        }
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            switch (style)
            {
                case 0:
                    {
                        float alpha = (Projectile.timeLeft / 180f).SmoothSymmetricFactor(1 / 12f);
                        //BlendState state = new BlendState();
                        //state.ColorBlendFunction = BlendFunction.Add;
                        //state.ColorDestinationBlend = Blend.InverseSourceAlpha;
                        //state.AlphaDestinationBlend = Blend.Zero;
                        //state.ColorSourceBlend = Blend.InverseDestinationColor;
                        //state.AlphaSourceBlend = Blend.One;
                        Main.spriteBatch.End();
                        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                        //alpha = 1f;
                        //Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, 0, 960, 1120), Color.White);
                        #region Shader
                        float r = Main.rand.NextFloat();
                        Main.instance.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("StoneOfThePhilosophers/Images/Style_6").Value;
                        Main.instance.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("StoneOfThePhilosophers/Images/HeatMap_0").Value;
                        Main.instance.GraphicsDevice.SamplerStates[1] = SamplerState.AnisotropicWrap;
                        Main.instance.GraphicsDevice.SamplerStates[2] = SamplerState.AnisotropicClamp;
                        StoneOfThePhilosophers.HeatMap.Parameters["uTime"].SetValue(projectile.velocity.SafeNormalize(default).RotatedBy(MathHelper.PiOver2) * Main.GlobalTimeWrappedHourly);
                        StoneOfThePhilosophers.HeatMap.CurrentTechnique.Passes[0].Apply();

                        #endregion
                        Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition, null, Color.White * alpha, r, new Vector2(16), new Vector2(8f) * (MathF.Cos(Main.GlobalTimeWrappedHourly * MathHelper.Pi) * .125f + 1f), 0, 0);//
                        //for (int n = 0; n < 8; n++)
                        //{
                        //    Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition + (MathHelper.PiOver4 * n).ToRotationVector2() * 8 + Main.rand.NextVector2Unit() * Main.rand.NextFloat(2, 6), null, Color.Lerp(Color.White, Color.Red, Main.rand.NextFloat(0, .5f)) * 0.125f * alpha, Projectile.rotation, new Vector2(16), new Vector2(2f, 1.5f), 0, 0);
                        //}
                        //for (int n = 0; n < 8; n++)
                        //{
                        //    Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition + (MathHelper.PiOver4 * n).ToRotationVector2() * 12 + Main.rand.NextVector2Unit() * Main.rand.NextFloat(6, 12), null, Color.Lerp(Color.Orange, Color.Red, Main.rand.NextFloat(0, .5f)) * 0.0625f * alpha, Projectile.rotation, new Vector2(16), new Vector2(2f, 1.5f) * 1.5f, 0, 0);
                        //}
                        //for (int n = 9; n > -1; n--)
                        //{
                        //    Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.oldPos[n] - Main.screenPosition - (Projectile.velocity + Main.rand.NextVector2Unit() * 4), null, Color.Lerp(lightColor, Color.White, .5f) * ((10 - n) * .1f) * alpha * .25f * 10, Projectile.oldRot[n], new Vector2(16), 1f * ((10 - n) * .1f), 0, 0);
                        //}
                        Main.spriteBatch.End();
                        //state.Dispose();
                        Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                        break;
                    }
                case 1:
                    {
                        var fac = 1 - projectile.timeLeft / 21f;
                        var fac1 = fac.HillFactor2(1);

                        for (int n = 0; n < 3; n++)
                            Main.EntitySpriteDraw(ModContent.Request<Texture2D>(Texture.Replace("SunAttack", "ExplosionEffect")).Value, projectile.Center - Main.screenPosition + Main.rand.NextVector2Unit() * 4, new Rectangle(0, 588 - projectile.timeLeft / 3 * 98, 98, 98), new Color(255, 255, 255, 0) * fac1 * .75f, projectile.rotation, new Vector2(49), 6f * fac, 0, 0);//new Rectangle(0, projectile.timeLeft / 2, 52, 52)
                        Main.EntitySpriteDraw(ModContent.Request<Texture2D>(Texture.Replace("SunAttack", "FlameOfNuclear")).Value, projectile.Center - Main.screenPosition, null, Color.Lerp(Color.Orange, Color.Yellow, fac1) with { A = 0 } * fac1, projectile.rotation, new Vector2(128), 3f * fac, 0, 0);
                        Main.EntitySpriteDraw(ModContent.Request<Texture2D>(Texture.Replace("SunAttack", "FlameOfNuclear")).Value, projectile.Center - Main.screenPosition, null, Color.Lerp(Color.Yellow, Color.White, fac1) * fac1, projectile.rotation, new Vector2(128), 2f * fac, 0, 0);

                        break;
                    }
                case 2:
                    {
                        float alpha = (Projectile.timeLeft / 180f).SmoothSymmetricFactor(1 / 12f);

                        //StoneOfThePhilosophersHelper.VertexDraw(projectile.TailVertexFromProj(default, t => 30f, t => Color.Yellow, .5f),
                        //    ModContent.Request<Texture2D>("StoneOfThePhilosophers/Images/Style_4").Value,
                        //    ModContent.Request<Texture2D>("StoneOfThePhilosophers/Images/Style_8").Value,
                        //    ModContent.Request<Texture2D>("StoneOfThePhilosophers/Images/HeatMap_0").Value,
                        //    new Vector2(-Main.GlobalTimeWrappedHourly * 2, 0), false, null,
                        //    "HeatMap");
                        StoneOfThePhilosophersHelper.VertexDraw(projectile.TailVertexFromProj(default, t => MathF.Pow(t, 1.5f).WaterDropFactor() * 16, t => Color.White * MathF.Pow(t, 2f).WaterDropFactor() * 2 * alpha, .5f),
                            ModContent.Request<Texture2D>("StoneOfThePhilosophers/Images/Style_4").Value,
                            ModContent.Request<Texture2D>("StoneOfThePhilosophers/Images/Style_8").Value,
                            ModContent.Request<Texture2D>("StoneOfThePhilosophers/Images/HeatMap_4").Value,
                            new Vector2(-Main.GlobalTimeWrappedHourly * 2, 0), false, null,
                            "HeatMap", true, true);//default, 30, .5f, true, Color.Yellow
                        var spriteBatch = Main.spriteBatch;
                        var starLight = ModContent.Request<Texture2D>("StoneOfThePhilosophers/Images/StarLight").Value;
                        spriteBatch.End();
                        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                        spriteBatch.Draw(starLight, projectile.Center - projectile.velocity * 2 - Main.screenPosition, null, Color.Orange * alpha, projectile.rotation + MathHelper.PiOver2, new Vector2(36), new Vector2(1, 3) * .5f, 0, 0);
                        spriteBatch.Draw(starLight, projectile.Center - projectile.velocity * 2 - Main.screenPosition, null, Color.Orange * .5f * alpha, projectile.rotation, new Vector2(36), new Vector2(1, 1) * .5f, 0, 0);
                        spriteBatch.Draw(starLight, projectile.Center - projectile.velocity * 2 - Main.screenPosition, null, Color.White * alpha, projectile.rotation + MathHelper.PiOver2, new Vector2(36), new Vector2(1, 3) * .25f, 0, 0);
                        spriteBatch.Draw(starLight, projectile.Center - projectile.velocity * 2 - Main.screenPosition, null, Color.White * .5f * alpha, projectile.rotation, new Vector2(36), new Vector2(1, 1) * .25f, 0, 0);
                        spriteBatch.End();
                        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                        break;
                    }
            }

            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return base.OnTileCollide(oldVelocity);
        }
        public override void Kill(int timeLeft)
        {
            switch (style)
            {
                case 0:
                    {
                        for (int n = 0; n < 3; n++)
                        {
                            var unit = (MathHelper.TwoPi / 6 * n + projectile.rotation).ToRotationVector2();
                            var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center + unit * 16, unit * 4, projectile.type, projectile.damage, 8, projectile.owner, 1);
                            proj.timeLeft = 21;
                            proj.width = proj.height = 640;
                            proj.penetrate = -1;
                            proj.Center = projectile.Center + projectile.velocity;
                            proj.rotation = MathHelper.TwoPi / 6 * n + projectile.rotation;
                            proj.tileCollide = false;
                        }
                        for (int num431 = 4; num431 < 31; num431++)
                        {
                            float num432 = projectile.oldVelocity.X * (30f / (float)num431);
                            float num433 = projectile.oldVelocity.Y * (30f / (float)num431);
                            for (int n = 0; n < 4; n++)
                            {
                                int num434 = Dust.NewDust(new Vector2(projectile.oldPosition.X - num432, projectile.oldPosition.Y - num433) + Main.rand.NextVector2Unit() * Main.rand.NextFloat(4) + projectile.velocity, 8, 8, MyDustId.Fire, projectile.oldVelocity.X, projectile.oldVelocity.Y, 100, Color.Orange, 1.2f);
                                Main.dust[num434].noGravity = true;
                                Dust dust = Main.dust[num434];
                                dust.velocity = projectile.velocity;
                                dust.velocity *= 0.5f;
                            }

                        }
                        SoundEngine.PlaySound(SoundID.Item62);
                        break;
                    }
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Daybreak, 120);
            //target.immune[Projectile.owner] = 0;
            if (style == 0)
            {
                if (Projectile.active)
                {
                    var rand = Main.rand.NextFloat();
                    for (int n = 0; n < 8; n++)
                    {
                        var unit = (n / 8f * MathHelper.TwoPi + rand).ToRotationVector2();
                        var proj = Projectile.NewProjectileDirect(projectile.GetProjectileSource_FromThis(), projectile.Center + unit * 192, unit * 16, Type, Projectile.damage / 4, projectile.knockBack * .5f, projectile.owner, 2);
                        proj.penetrate = 3;

                    }
                }

                Projectile.Kill();
            }
            else
            {
                target.immune[projectile.owner] = 0;
                projectile.frameCounter++;
                if (style == 2)
                {
                    if (projectile.penetrate == 1)
                    {
                        projectile.timeLeft = 15;
                        projectile.penetrate = 2;
                        projectile.friendly = false;
                    }
                    for (int n = 0; n < 4 - Projectile.penetrate; n++)
                    {
                        for (int k = 0; k < 15; k++)
                        {
                            Dust.NewDustPerfect(target.Center, DustID.SolarFlare, Main.rand.NextVector2Unit() * Main.rand.NextFloat(4) + Projectile.velocity, 0, default, Main.rand.NextFloat(0.5f, 1.5f));
                        }
                    }
                }
            }
            if (!target.friendly && target.active && target.CanBeChasedBy())
            {
                Main.player[Projectile.owner].GetModPlayer<ElementChargePlayer>().ElementChargeValue[6] += damage / ElementChargePlayer.Devider;
            }
        }
        public override void AI()
        {
            switch (style)
            {
                case 0:
                    {
                        Projectile.rotation = Projectile.velocity.ToRotation();
                        Dust.NewDustPerfect(projectile.Center, MyDustId.Fire, new Vector2(0, 0), 0, Color.White, 1f).noGravity = true;
                        for (int n = 9; n > 0; n--)
                        {
                            Projectile.oldPos[n] = Projectile.oldPos[n - 1];
                            Projectile.oldRot[n] = Projectile.oldRot[n - 1];
                        }
                        Projectile.oldPos[0] = Projectile.Center;
                        Projectile.oldRot[0] = Projectile.rotation;
                        projectile.velocity *= .96f;
                        if (projectile.timeLeft % 15 == 0)
                        {
                            foreach (var npc in Main.npc)
                            {
                                if (npc.active && !npc.friendly)
                                {
                                    var distance = Vector2.Distance(npc.Center, projectile.Center);
                                    if (distance < 432)
                                    {
                                        var fac = Utils.GetLerpValue(432, 0, distance);
                                        var damage = (int)(MathF.Pow(fac, 0.5f) * projectile.damage / 4);
                                        int count = (int)(fac * 30);
                                        for (int n = 0; n < count; n++)
                                        {
                                            var unit = (n * MathHelper.TwoPi / count).ToRotationVector2();
                                            Dust.NewDustPerfect(npc.Center, MyDustId.Fire, unit * Main.rand.NextFloat(1, 3), 0, default, Main.rand.NextFloat(0.5f, 1f));
                                        }
                                        if (npc.CanBeChasedBy() || npc.type == NPCID.TargetDummy)
                                        {
                                            Main.player[projectile.owner].ApplyDamageToNPC(npc, damage, 0, projectile.direction, false);
                                            Main.player[Projectile.owner].GetModPlayer<ElementChargePlayer>().ElementChargeValue[6] += damage / ElementChargePlayer.Devider;
                                        }
                                        npc.AddBuff(BuffID.Daybreak, 30);
                                        //OnHitNPC(npc, damage, 0, false);
                                    }
                                }
                            }
                            for (int n = 0; n < 30; n++)
                            {
                                var unit = (n / 30f * MathHelper.TwoPi).ToRotationVector2();
                                Dust.NewDustPerfect(projectile.Center + unit * 96, MyDustId.Fire, unit * Main.rand.NextFloat(2, 8), 0, default, Main.rand.NextFloat(1f, 1.5f));
                            }
                        }
                        if (projectile.timeLeft <= 120)
                        {
                            if (projectile.timeLeft % 15 == 0 && projectile.timeLeft != 0)
                            {
                                var unit = (projectile.timeLeft / 120f * MathHelper.TwoPi).ToRotationVector2();
                                var proj = Projectile.NewProjectileDirect(projectile.GetProjectileSource_FromThis(), projectile.Center + unit * 96, unit * 16, Type, Projectile.damage / 4, projectile.knockBack * .5f, projectile.owner, 2);
                                proj.penetrate = 3;
                            }
                        }
                        break;
                    }
                case 1:
                    {
                        projectile.friendly = projectile.frameCounter == 0;
                        break;
                    }
                case 2:
                    {
                        for (int n = 9; n > 0; n--)
                        {
                            Projectile.oldPos[n] = Projectile.oldPos[n - 1];
                            Projectile.oldRot[n] = Projectile.oldRot[n - 1];
                        }
                        Projectile.oldPos[0] = Projectile.Center;
                        Projectile.oldRot[0] = Projectile.rotation;
                        if (projectile.timeLeft > 165)
                        {
                            projectile.velocity = projectile.velocity.RotatedBy(Utils.GetLerpValue(165, 180, projectile.timeLeft, true) * MathHelper.Pi / 6);
                        }
                        else if (projectile.timeLeft < 150 && projectile.timeLeft > 15)
                        {
                            NPC target = null;
                            float MaxDistance = 512f;
                            foreach (var npc in Main.npc)
                            {
                                if (!npc.friendly && npc.active && npc.CanBeChasedBy())
                                {
                                    float distance = Vector2.Distance(npc.Center, projectile.Center);
                                    if (distance < MaxDistance)
                                    {
                                        target = npc;
                                        MaxDistance = distance;
                                    }
                                }
                            }
                            if (target != null)
                            {
                                Vector2 targetVec = (target.Center - projectile.Center);
                                projectile.velocity = Vector2.Lerp(projectile.velocity, targetVec, 0.005f);
                                projectile.timeLeft = 30;
                            }
                        }
                        else if (projectile.timeLeft <= 15)
                        {
                            projectile.friendly = false;
                        }
                        projectile.rotation = projectile.velocity.ToRotation();
                        break;
                    }
            }
            //if (style == 1 && projectile.timeLeft == 10)
            //{
            //    for (int n = 0; n < 3; n++)
            //    {
            //        var unit = Main.rand.NextVector2Unit();
            //        var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center + unit * Main.rand.NextFloat(16, 32f) * 16, unit * Main.rand.NextFloat(2, 8), projectile.type, projectile.damage / 2, 8, projectile.owner, 2);
            //        proj.timeLeft = 21;
            //        proj.width = proj.height = 80;
            //        proj.penetrate = -1;
            //        proj.Center = projectile.Center + projectile.velocity;
            //        proj.rotation = MathHelper.TwoPi / 6 * n + projectile.rotation;
            //        proj.tileCollide = false;
            //        for (int m = 0; m < 8; m++)
            //        {
            //            Dust.NewDustPerfect(proj.Center, MyDustId.Fire, Main.rand.NextVector2Unit() * Main.rand.NextFloat(2, 8), 0, default, Main.rand.NextFloat(0.5f, 2f));
            //        }
            //    }
            //    SoundEngine.PlaySound(SoundID.Item62);
            //}
            base.AI();
        }
    }
    public class TidalErosion : ModBuff
    {
        public override void Update(NPC npc, ref int buffIndex)
        {
        }
        public override void Update(Player player, ref int buffIndex)
        {
            Main.NewText("好湿好湿(x\n你知道吗，原版实现npc防御修改的方式是在最终造成伤害的地方改");
            player.statDefense -= 5;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("潮蚀");
            Description.SetDefault("欸，这个debuff明明是给npc的，你是怎么得到的呢\n不要问为什么是恋恋，因为恋恋无处不在");
        }
    }
    public class TidalErosionGBNPC : GlobalNPC
    {
        public override bool StrikeNPC(NPC npc, ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            if (npc.HasBuff<TidalErosion>()) damage += 10;
            return base.StrikeNPC(npc, ref damage, defense, ref knockback, hitDirection, ref crit);
        }
    }
    public class BlessingFromLunarGod : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            for (int l = 0; l < Player.MaxBuffs; l++)
            {
                int num24 = player.buffType[l];
                if (Main.debuff[num24] && player.buffTime[l] > 0 && (num24 < 0 || !BuffID.Sets.NurseCannotRemoveDebuff[num24]))
                {
                    player.DelBuff(l);
                    l = -1;
                }
            }
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("月神的祝福");
            Description.SetDefault("免疫几乎所有Debuff");
        }
    }
    public class Reincarnation : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen += 40;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("复苏");
            Description.SetDefault("生命超快速恢复！！");
        }
    }
}