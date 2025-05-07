using StoneOfThePhilosophers.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using StoneOfThePhilosophers.Contents.Philosopher;

namespace StoneOfThePhilosophers.Contents;

public abstract class MagicStone : ModItem
{
    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        var tooltip = new TooltipLine(Mod, "OpenUI", "使用鼠标中键以开启配置ui");
        tooltips.Add(tooltip);
        base.ModifyTooltips(tooltips);
    }
    public override void HoldStyle(Player player, Rectangle heldItemFrame)
    {
        uiTimer--;
        if (PlayerInput.MouseInfo.MiddleButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && uiTimer <= 0)
        {
            if (Extra && Type != ModContent.ItemType<StoneOfThePhilosopher>() && Type != ModContent.ItemType<StoneOfThePhilosopherEX>())
            {
                if (ElementSkillUI.Visible)
                    ElementSkillSystem.Instance.skillUI.Close();
                else
                {
                    ElementSkillSystem.Instance.skillUI.Open();
                    ElementSkillSystem.Instance.skillUI.itemType = Type;
                }
            }
            else
            {
                if (ElementUI.Visible)
                    ElementSystem.Instance.elementUI.Close();
                else
                {
                    ElementUI.IsExtra = Extra;
                    ElementSystem.Instance.elementUI.Open();
                }
            }
            uiTimer = 15;
        }
        base.HoldStyle(player, heldItemFrame);
    }
    public int uiTimer;
    public virtual bool Extra => false;
    public virtual StoneElements Elements => StoneElements.Empty;
    public override bool AltFunctionUse(Player player)
    {
        if (Elements == 0 || !Extra) return false;
        var mplr = player.GetModPlayer<ElementSkillPlayer>();
        int index = (int)Elements - 1;
        return mplr.ElementChargeValue[index] >= mplr.GetElementCost(index);
    }
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        var mplr = player.GetModPlayer<ElementSkillPlayer>();
        var proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
        if (proj.ModProjectile is not MagicArea area) return false;
        area.Extra = Extra;
        if (player.altFunctionUse == 2 && Elements != 0 && Extra)
        {
            var index = (int)Elements - 1;
            area.specialAttackIndex = mplr.skillIndex[index] + 1;
            mplr.ElementChargeValue[index] -= mplr.GetElementCost(index);
        }
        return false;
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
    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ItemID.ManaCrystal, 3);
        recipe.AddTile(TileID.DemonAltar);
        AddOtherIngredients(recipe);
        recipe.Register();
    }
    public virtual void AddOtherIngredients(Recipe recipe)
    {

    }
    public override bool CanUseItem(Player player) => player.ownedProjectileCounts[base.Item.shoot] < 1;
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
}
