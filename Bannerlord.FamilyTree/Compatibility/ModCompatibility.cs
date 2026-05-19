using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;

namespace Bannerlord.FamilyTree.Compatibility;

public static class ModCompatibility
{
    public static void Initialize()
    {
        // BannerlordExpanded.SpousesExpanded
        SpousesExpandedCompatibility.InitializeIfLoaded();
        // Dramalord is supported without any additional hooks because
        // in addition to its own spouse status it also hacks around
        // the vanilla spouse <-> exspouse properties.
        // I only had to work around duplicates appearing in exspouses lists.
        // AFAIK MarryAnyone should work the same way but the original one
        // is discontinued and I couldn't be bothered to find a working version.
    }

    public static MBList<Hero> GetSecondarySpouses(Hero hero)
    {
        return SpousesExpandedCompatibility.GetSecondarySpouses(hero);
    }
}
