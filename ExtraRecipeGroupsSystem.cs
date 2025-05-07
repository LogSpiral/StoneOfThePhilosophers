using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
namespace StoneOfThePhilosophers;

public class ExtraRecipeGroupsSystem : ModSystem
{
    // TODO 合成组本地化添加
    public const string IronLeadOres = "StoneOfThePhilosophers:IronLeadOres";
    public const string GoldPlatinumOres = "StoneOfThePhilosophers:GoldPlatinumOres";
    public const string CobaltPalladiumBars = "StoneOfThePhilosophers:CobaltPalladiumBars";

    public const string MythrilOrichalcumBars = "StoneOfThePhilosophers:MythrilOrichalcumBars";
    public const string AdamantiteTitaniumBars = "StoneOfThePhilosophers:AdamantiteTitaniumBars";
    public const string CursedIchorFlame = "StoneOfThePhilosophers:CursedIchorFlame";
    public override void AddRecipeGroups()
    {
        RecipeGroup group = new(() => Language.GetTextValue("LegacyMisc.37") + " 铁铅矿",
        [
            ItemID.IronOre,
            ItemID.LeadOre
        ]);
        RecipeGroup.RegisterGroup(IronLeadOres, group);
        group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " 金铂矿",
        [
            ItemID.GoldOre,
            ItemID.PlatinumOre
        ]);
        RecipeGroup.RegisterGroup(GoldPlatinumOres, group);
        group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " 钴钯金锭",
        [
            ItemID.CobaltBar,
            ItemID.PalladiumBar
        ]);
        RecipeGroup.RegisterGroup(CobaltPalladiumBars, group);
        group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " 秘山铜锭",
        [
            ItemID.MythrilBar,
            ItemID.OrichalcumBar
        ]);
        RecipeGroup.RegisterGroup(MythrilOrichalcumBars, group);
        group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " 精钛金锭",
        [
            ItemID.AdamantiteBar,
            ItemID.TitaniumBar
        ]);
        RecipeGroup.RegisterGroup(AdamantiteTitaniumBars, group);
        group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " 咒火灵液",
        [
            ItemID.CursedFlame,
            ItemID.Ichor
        ]);
        RecipeGroup.RegisterGroup(CursedIchorFlame, group);
    }
}
