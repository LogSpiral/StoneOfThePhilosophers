using LogSpiralLibrary.CodeLibrary.Utilties;
using LogSpiralLibrary.CodeLibrary.Utilties.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StoneOfThePhilosophers.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace StoneOfThePhilosophers.Contents.Sun;
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
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.shoot = ModContent.ProjectileType<StoneOfSunProj>();
        Item.damage = 70;
        Item.mana = 20;
    }
}














