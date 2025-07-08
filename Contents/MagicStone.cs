using StoneOfThePhilosophers.UI;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using StoneOfThePhilosophers.Contents.Philosopher;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;

namespace StoneOfThePhilosophers.Contents;

public abstract class MagicStone : ModItem
{
    int uiTimer;
    public virtual bool Extra => false;
    public virtual StoneElements Elements => StoneElements.Empty;
    public override void SetDefaults()
    {
        var item = base.Item;
        item.DamageType = DamageClass.Magic;
        item.width = 34;
        item.noUseGraphic = true;
        item.noMelee = true;
        item.height = 40;
        item.rare = ItemRarityID.Cyan;
        item.autoReuse = true;
        item.useAnimation = 12;
        item.useTime = 12;
        item.useStyle = ItemUseStyleID.Shoot;
        item.channel = true;
        item.value = 150;
        item.knockBack = 4f;
        item.shootSpeed = 10;
        item.damage = 30;
        item.mana = 15;
    }
    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        var tooltip = new TooltipLine(Mod, "OpenUI", Language.GetTextValue("Mods.StoneOfThePhilosophers.Items.MagicStones.MiddleClickTip"));
        tooltips.Add(tooltip);
        base.ModifyTooltips(tooltips);
    }
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        var mplr = player.GetModPlayer<ElementSkillPlayer>();
        var proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
        if (proj.ModProjectile is MagicArea area)
        {
            area.Extra = Extra;
            if (player.altFunctionUse == 2 && Elements != 0 && Extra)
            {
                var index = (int)Elements - 1;
                area.SpecialAttackIndex = mplr.skillIndex[index];
                mplr.ElementChargeValue[index] -= mplr.GetElementCost(index);
            }
        }
        else if (proj.ModProjectile is StoneOfThePhilosopherProj philosopherProj)
        {
            philosopherProj.Extra = Extra;
            var cplr = player.GetModPlayer<ElementCombinePlayer>();
            var combination = Extra ? cplr.CombinationEX : cplr.Combination;
            if (combination.Mode == 1 && player.altFunctionUse == 2)
            {
                var index = (int)combination.MainElements - 1;
                philosopherProj.SpecialAttackIndex = mplr.skillIndex[index];
                mplr.ElementChargeValue[index] -= mplr.GetElementCost(index);
            }
        }
        return false;
    }
    public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] < 1;
    public override void HoldStyle(Player player, Rectangle heldItemFrame)
    {
        uiTimer--;
        if (PlayerInput.MouseInfo.MiddleButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && uiTimer <= 0)
        {
            if (Extra && this is not StoneOfThePhilosopher)
            {
                if (ElementSkillUI.Instance.Enabled)
                    ElementSkillUI.Close();
                else
                    ElementSkillUI.Open(Type);
            }
            else
            {
                var mplr = player.GetModPlayer<ElementCombinePlayer>();
                var combination = Extra ? mplr.CombinationEX : mplr.Combination;
                bool controlSkill = combination.Mode == 1;


                if (ElementCombineUI.Instance.Enabled)
                {

                    ElementCombineUI.Close();
                    if (controlSkill)
                        ElementSkillUI.Close();
                }
                else
                {
                    ElementCombineUI.IsExtra = Extra;
                    ElementCombineUI.Open(Type);
                    if (controlSkill)
                        ElementSkillUI.Open(Type, Main.MouseScreen + Vector2.UnitX * 180);
                }


            }
            uiTimer = 15;
        }
        base.HoldStyle(player, heldItemFrame);
    }
    public override bool AltFunctionUse(Player player)
    {
        if (Elements == 0 || !Extra) return false;
        var mplr = player.GetModPlayer<ElementSkillPlayer>();
        int index = (int)Elements - 1;
        return mplr.ElementChargeValue[index] >= mplr.GetElementCost(index);
    }
    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ItemID.ManaCrystal, 3);
        recipe.AddTile(TileID.DemonAltar);
        AddOtherIngredients(recipe);
        recipe.Register();
    }
    public static void AddEXRequire<T>(Recipe recipe, bool metalStone = false) where T : MagicStone
    {
        recipe.AddIngredient<T>();
        recipe.AddRecipeGroup(ExtraRecipeGroupsSystem.CobaltPalladiumBars, metalStone ? 20 : 10);
        recipe.AddRecipeGroup(ExtraRecipeGroupsSystem.MythrilOrichalcumBars, metalStone ? 16 : 8);
        recipe.AddRecipeGroup(ExtraRecipeGroupsSystem.AdamantiteTitaniumBars, metalStone ? 10 : 5);
        recipe.AddIngredient(ItemID.SoulofLight, 15);
        recipe.AddIngredient(ItemID.SoulofNight, 15);
        recipe.AddTile(TileID.CrystalBall);
    }
    public virtual void AddOtherIngredients(Recipe recipe)
    {

    }

}
