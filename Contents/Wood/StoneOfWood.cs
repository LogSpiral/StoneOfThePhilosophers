﻿using StoneOfThePhilosophers.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using LogSpiralLibrary.CodeLibrary.Utilties.Extensions;
using LogSpiralLibrary.CodeLibrary.Utilties;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace StoneOfThePhilosophers.Contents.Wood;

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
    public override void SetDefaults()
    {
        Item.shoot = ModContent.ProjectileType<StoneOfWoodProj>();
        base.SetDefaults();
        Item.damage = 20;
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
    public override bool Extra => true;
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.damage = 45;
    }
}




