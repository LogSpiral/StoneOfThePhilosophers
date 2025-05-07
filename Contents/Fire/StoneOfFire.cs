using StoneOfThePhilosophers.UI;
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
using LogSpiralLibrary;

namespace StoneOfThePhilosophers.Contents.Fire;
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
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.damage = 25;
        Item.shoot = ModContent.ProjectileType<StoneOfFireProj>();

    }
}
public class StoneOfFireEX : StoneOfFire
{
    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddRecipeGroup(ExtraRecipeGroupsSystem.CursedIchorFlame, 20);
        recipe.AddIngredient(ItemID.LivingFireBlock, 50);
        recipe.AddIngredient(ItemID.InfernoFork);
        AddEXRequire<StoneOfFire>(recipe);
        recipe.Register();
    }
    public override bool Extra => true;
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.damage = 75;
    }
}


