using StoneOfThePhilosophers.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace StoneOfThePhilosophers.Contents.Moon;

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
        recipe.AddIngredient(ItemID.Moonglow, 6);
        recipe.AddIngredient(ItemID.MoonStone);
        recipe.AddIngredient(ItemID.LargeAmethyst);
        recipe.AddTile(TileID.CrystalBall);
        recipe.Register();
    }
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.shoot = ModContent.ProjectileType<StoneOfMoonProj>();
        Item.damage = 90;
        Item.mana = 5;
    }
}



