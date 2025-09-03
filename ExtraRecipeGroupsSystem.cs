using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace StoneOfThePhilosophers;

public class ExtraRecipeGroupsSystem : ModSystem
{
    public const string IronLeadOres = "StoneOfThePhilosophers:IronLeadOres";
    public const string GoldPlatinumOres = "StoneOfThePhilosophers:GoldPlatinumOres";
    public const string CobaltPalladiumBars = "StoneOfThePhilosophers:CobaltPalladiumBars";
    public const string MythrilOrichalcumBars = "StoneOfThePhilosophers:MythrilOrichalcumBars";
    public const string AdamantiteTitaniumBars = "StoneOfThePhilosophers:AdamantiteTitaniumBars";
    public const string CursedIchorFlame = "StoneOfThePhilosophers:CursedIchorFlame";

    public override void AddRecipeGroups()
    {
        RecipeGroup group = new(() => Language.GetTextValue("LegacyMisc.37") + Language.GetTextValue("Mods.StoneOfThePhilosphers.IronLeadOres"),
        [
            ItemID.IronOre,
            ItemID.LeadOre
        ]);
        RecipeGroup.RegisterGroup(IronLeadOres, group);
        group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + Language.GetTextValue("Mods.StoneOfThePhilosphers.GoldPlatinumOres"),
        [
            ItemID.GoldOre,
            ItemID.PlatinumOre
        ]);
        RecipeGroup.RegisterGroup(GoldPlatinumOres, group);
        group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + Language.GetTextValue("Mods.StoneOfThePhilosphers.CobaltPalladiumBars"),
        [
            ItemID.CobaltBar,
            ItemID.PalladiumBar
        ]);
        RecipeGroup.RegisterGroup(CobaltPalladiumBars, group);
        group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + Language.GetTextValue("Mods.StoneOfThePhilosphers.MythrilOrichalcumBars"),
        [
            ItemID.MythrilBar,
            ItemID.OrichalcumBar
        ]);
        RecipeGroup.RegisterGroup(MythrilOrichalcumBars, group);
        group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + Language.GetTextValue("Mods.StoneOfThePhilosphers.AdamantiteTitaniumBars"),
        [
            ItemID.AdamantiteBar,
            ItemID.TitaniumBar
        ]);
        RecipeGroup.RegisterGroup(AdamantiteTitaniumBars, group);
        group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + Language.GetTextValue("Mods.StoneOfThePhilosphers.CursedIchorFlame"),
        [
            ItemID.CursedFlame,
            ItemID.Ichor
        ]);
        RecipeGroup.RegisterGroup(CursedIchorFlame, group);
    }
}