using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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