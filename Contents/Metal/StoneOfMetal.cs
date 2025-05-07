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
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace StoneOfThePhilosophers.Contents.Metal;

public class StoneOfMetal : MagicStone
{
    public override StoneElements Elements => StoneElements.Metal;
    public override void AddOtherIngredients(Recipe recipe)
    {
        recipe.AddIngredient(ItemID.LargeTopaz);
        recipe.AddIngredient(ItemID.MeteoriteBar, 20);
        recipe.AddIngredient(ItemID.Blinkroot, 5);
        recipe.AddRecipeGroup(ExtraRecipeGroupsSystem.IronLeadOres, 20);
        recipe.AddRecipeGroup(ExtraRecipeGroupsSystem.GoldPlatinumOres, 10);
        base.AddOtherIngredients(recipe);
    }
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.damage = 30;
        Item.shoot = ModContent.ProjectileType<StoneOfMetalProj>();
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
    public override bool Extra => true;
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.damage = 70;
    }
}

