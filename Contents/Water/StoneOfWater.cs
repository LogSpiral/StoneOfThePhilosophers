using StoneOfThePhilosophers.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using LogSpiralLibrary.CodeLibrary.Utilties;
using LogSpiralLibrary.CodeLibrary.Utilties.Extensions;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.Audio;

namespace StoneOfThePhilosophers.Contents.Water;

public class StoneOfWater : MagicStone
{
    public override StoneElements Elements => StoneElements.Water;

    public override void AddOtherIngredients(Recipe recipe)
    {
        recipe.AddIngredient(ItemID.LargeSapphire);
        recipe.AddIngredient(ItemID.WaterBolt);
        recipe.AddIngredient(ItemID.ReefBlock, 20);
        recipe.AddIngredient(ItemID.SharkFin, 5);
        recipe.AddIngredient(ItemID.Shiverthorn, 5);
        recipe.AddIngredient(ItemID.WaterBucket);
        base.AddOtherIngredients(recipe);
    }
    public override void SetDefaults()
    {
        Item.shoot = ModContent.ProjectileType<StoneOfWaterProj>();
        base.SetDefaults();
        Item.damage = 15;
        Item.mana = 5;
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
    public override bool Extra => true;
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.damage = 30;
    }
}



