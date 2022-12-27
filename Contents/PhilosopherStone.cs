﻿using Terraria.ModLoader;
using VirtualDream.Utils;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace VirtualDream.Contents.TouhouProject.Items.Weapons.PhilosopherStone
{

    public class Gem_0Proj1 : GemProj1
    {
        public override string Texture => $"VirtualDream/icon";//{StarBound.NPCs.Bosses.BigApe.BigApeTools.ApePath}StrawBerryArea
        public override Color MainColor => Color.Blue;
        public override void ShootProj(bool dying = false)
        {
            var vector2 = new Vector2(player.Center.X + Main.rand.Next(201) * -(float)player.direction + (Main.mouseX + Main.screenPosition.X - player.position.X), player.MountedCenter.Y - 600f);
            vector2.X = (vector2.X + player.Center.X) / 2f + Main.rand.Next(-200, 201);
            var vec = Main.MouseWorld - vector2;
            if (vec.Y < 0f)
            {
                vec.Y *= -1f;
            }
            if (vec.Y < 20f)
            {
                vec.Y = 20f;
            }
            vec.Normalize();
            Projectile.NewProjectile(projectile.GetSource_FromThis(), vector2 + (dying ? Main.rand.NextVector2Unit() * Main.rand.NextFloat(0, 128) : default), vec * 5, ModContent.ProjectileType<Gem_0Proj>(), projectile.damage, projectile.knockBack, player.whoAmI, 0f, vec.Y + vector2.Y);
        }
    }
    public class Gem_0 : ModItem
    {
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.LargeAmethyst, 5);
            recipe.AddIngredient(ItemID.Ichor, 50);
            recipe.AddIngredient(ItemID.ManaCrystal, 20);
            recipe.AddIngredient(ItemID.SoulofNight, 30);
            recipe.AddIngredient(ItemID.SoulofLight, 30);
            recipe.AddRecipeGroup(IllusionBoundModSystem.CobaltRG, 30);
            recipe.AddRecipeGroup(IllusionBoundModSystem.MythrilBarRG, 30);
            recipe.AddRecipeGroup(IllusionBoundModSystem.AdamantiteBarRG, 30);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("月之石");
            Tooltip.SetDefault("使用月元素魔法程度的能力\n\"明明如月，何时可掇？\"\n月符「沉静的月神」");
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[item.shoot] < 1;
        }
        Item item => Item;

        public override void SetDefaults()
        {
            item.DamageType = DamageClass.Magic;
            item.width = 34;
            item.noUseGraphic = true;
            item.height = 40;
            item.rare = MyRareID.TouhouProject;
            item.autoReuse = true;
            item.useAnimation = 12;
            item.useTime = 12;
            item.useStyle = 5;
            item.channel = true;
            item.value = 150;
            item.shoot = ModContent.ProjectileType<Gem_0Proj1>();
            item.knockBack = 4f;
            item.shootSpeed = 10;
            item.damage = 50;
            item.mana = 5;
            //item.UseSound = SoundID.Item88;
        }
        //     public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        //     {
        ////int num117 = 3;
        ////for (int num118 = 0; num118 < num117; num118++)
        ////{

        ////}

        //return false;
        //     }
    }
    public class Gem_0Proj : ModProjectile
    {
        Projectile projectile => Projectile;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 5;
            //if (projectile.ai[1] != -1f)
            //{
            //	projectile.ai[0] = 0f;
            //	projectile.ai[1] = -1f;
            //	projectile.netUpdate = true;
            //}
        }
        public override void Kill(int timeLeft)
        {
            bool flag = WorldGen.SolidTile(Framing.GetTileSafely((int)projectile.position.X / 16, (int)projectile.position.Y / 16));
            int num3;
            for (int num59 = 0; num59 < 4; num59 = num3 + 1)
            {
                Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default, 1.5f);
                num3 = num59;
            }
            for (int num60 = 0; num60 < 4; num60 = num3 + 1)
            {
                int num61 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 229, 0f, 0f, 0, default, 2.5f);
                Main.dust[num61].noGravity = true;
                Dust dust = Main.dust[num61];
                dust.velocity *= 3f;
                if (flag)
                {
                    Main.dust[num61].noLight = true;
                }
                num61 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 229, 0f, 0f, 100, default, 1.5f);
                dust = Main.dust[num61];
                dust.velocity *= 2f;
                Main.dust[num61].noGravity = true;
                if (flag)
                {
                    Main.dust[num61].noLight = true;
                }
                num3 = num60;
            }
            for (int num62 = 0; num62 < 1; num62 = num3 + 1)
            {
                int num63 = Gore.NewGore(projectile.GetSource_Death(), projectile.position + new Vector2(projectile.width * Main.rand.Next(100) / 100f, projectile.height * Main.rand.Next(100) / 100f) - Vector2.One * 10f, default, Main.rand.Next(61, 64), 1f);
                Gore gore = Main.gore[num63];
                gore.velocity *= 0.3f;
                Gore gore13 = Main.gore[num63];
                gore13.velocity.X = gore13.velocity.X + Main.rand.Next(-10, 11) * 0.05f;
                Gore gore14 = Main.gore[num63];
                gore14.velocity.Y = gore14.velocity.Y + Main.rand.Next(-10, 11) * 0.05f;
                num3 = num62;
            }
        }
        //     public override bool OnTileCollide(Vector2 oldVelocity)
        //     {
        //projectile.ai[0] = 0f;
        //projectile.ai[1] = -1f;
        //projectile.netUpdate = true;
        //return true;
        //     }
        public override void AI()
        {
            //if (projectile.ai[1] != -1f && projectile.position.Y > projectile.ai[1])
            //{
            //	projectile.tileCollide = true;
            //}
            if (projectile.position.HasNaNs())
            {
                projectile.Kill();
                return;
            }
            bool flag5 = WorldGen.SolidTile(Framing.GetTileSafely((int)projectile.position.X / 16, (int)projectile.position.Y / 16));
            Dust dust36 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 229, 0f, 0f, 0, default, 1.5f)];
            dust36.position = projectile.Center;
            dust36.velocity = Vector2.Zero;
            dust36.noGravity = true;
            if (flag5)
            {
                dust36.noLight = true;
            }
            //if (projectile.ai[1] == -1f)
            //{
            //	projectile.ai[0] += 1f;
            //	projectile.velocity = Vector2.Zero;
            //	projectile.tileCollide = false;
            //	projectile.penetrate = -1;
            //	projectile.position = projectile.Center;
            //	projectile.width = (projectile.height = 140);
            //	projectile.Center = projectile.position;
            //	projectile.alpha -= 10;
            //	if (projectile.alpha < 0)
            //	{
            //		projectile.alpha = 0;
            //	}
            //	int num = projectile.frameCounter + 1;
            //	projectile.frameCounter = num;
            //	if (num >= projectile.MaxUpdates * 3)
            //	{
            //		projectile.frameCounter = 0;
            //		num = projectile.frame;
            //		projectile.frame = num + 1;
            //	}
            //	if (projectile.ai[0] >= (float)(Main.projFrames[projectile.type] * projectile.MaxUpdates * 3))
            //	{
            //		projectile.Kill();
            //	}
            //	return;
            //}
            projectile.alpha = 255;
            if (projectile.numUpdates == 0)
            {
                int num187 = -1;
                float num188 = 60f;
                int num;
                for (int num189 = 0; num189 < 200; num189 = num + 1)
                {
                    NPC npc2 = Main.npc[num189];
                    if (npc2.CanBeChasedBy(projectile, false))
                    {
                        float num190 = projectile.Distance(npc2.Center);
                        if (num190 < num188 && Collision.CanHitLine(projectile.Center, 0, 0, npc2.Center, 0, 0))
                        {
                            num188 = num190;
                            num187 = num189;
                        }
                    }
                    num = num189;
                }
                if (num187 != -1)
                {
                    projectile.ai[0] = 0f;
                    projectile.ai[1] = -1f;
                    projectile.netUpdate = true;
                    return;
                }
            }
        }
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.DamageType = DamageClass.Magic;
            projectile.tileCollide = false;
            projectile.extraUpdates = 5;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
        }
    }
    public class Gem_1 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("金之石");
            Tooltip.SetDefault("控制金元素魔法程度的能力\n\"精诚所至,金石为开。\"\n金符「金属疲劳」");
        }
        Item item => Item;

        public override void SetDefaults()
        {
            item.width = 34;
            item.noUseGraphic = true;
            item.height = 40;
            item.rare = MyRareID.TouhouProject;
            item.autoReuse = true;
            item.useAnimation = 12;
            item.useTime = 12;
            item.useStyle = 5;
            item.value = 150;
            item.shoot = ModContent.ProjectileType<Gem_0Proj>();
            item.knockBack = 4f;
            item.shootSpeed = 10;
            item.damage = 50;
            item.mana = 5;
            //item.UseSound = SoundID.Item88;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {            //int num117 = 3;
            //for (int num118 = 0; num118 < num117; num118++)
            //{

            //}
            var vector2 = new Vector2(player.Center.X + Main.rand.Next(201) * -(float)player.direction + (Main.mouseX + Main.screenPosition.X - player.position.X), player.MountedCenter.Y - 600f);
            vector2.X = (vector2.X + player.Center.X) / 2f + Main.rand.Next(-200, 201);
            //vector2.Y -= (float)(100 * num118);
            var vec = Main.MouseWorld - vector2;
            if (vec.Y < 0f)
            {
                vec.Y *= -1f;
            }
            if (vec.Y < 20f)
            {
                vec.Y = 20f;
            }
            vec.Normalize();
            Projectile.NewProjectile(source, vector2, vec * item.shootSpeed * .5f, type, damage, knockback, player.whoAmI, 0f, vec.Y + vector2.Y);
            return false;
        }
    }
}