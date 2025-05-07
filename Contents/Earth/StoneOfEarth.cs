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
using ReLogic.Utilities;
using Terraria.GameContent;

namespace StoneOfThePhilosophers.Contents.Earth;

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
    public override void SetDefaults()
    {
        Item.shoot = ModContent.ProjectileType<StoneOfEarthProj>();
        Item.damage = 30;

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
    public override bool Extra => true;
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.damage = 80;
    }
}


