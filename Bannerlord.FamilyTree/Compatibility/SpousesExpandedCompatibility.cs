using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.Library;

namespace Bannerlord.FamilyTree.Compatibility;

public static class SpousesExpandedCompatibility
{
    private static readonly object[] NoArguments = new object[] { };

    private static FieldInfo CampaignBehaviorsField = default!;
    private static MethodInfo SettingsInstanceGetter = default!;
    private static MethodInfo PolygamyEnabledGetter = default!;
    private static MethodInfo GetPlayerSpousesMethod = default!;
    private static Type PlayerPolygamyBehaviorType = default!;

    public static bool Initialized { get; private set; }

    public static void InitializeIfLoaded()
    {
        Initialized = TryInitialize();
    }

    public static bool TryInitialize()
    {
        CampaignBehaviorsField = AccessTools.DeclaredField(typeof(CampaignBehaviorManager), "_campaignBehaviors");
        if (CampaignBehaviorsField is null)
        {
            return false;
        }
        var settingsType = AccessTools.TypeByName("BannerlordExpanded.SpousesExpanded.Settings.MCMSettings");
        if (settingsType is null)
        {
            return false;
        }

        SettingsInstanceGetter = AccessTools.PropertyGetter(settingsType, "Instance");
        if (SettingsInstanceGetter is null)
        {
            return false;
        }

        PolygamyEnabledGetter = AccessTools.PropertyGetter(settingsType, "PolygamyEnabled");
        if (PolygamyEnabledGetter is null)
        {
            return false;
        }

        PlayerPolygamyBehaviorType = AccessTools.TypeByName("BannerlordExpanded.SpousesExpanded.Polygamy.Behaviors.PlayerPolygamyBehavior");
        if (PlayerPolygamyBehaviorType is null)
        {
            return false;
        }

        GetPlayerSpousesMethod = AccessTools.DeclaredMethod(PlayerPolygamyBehaviorType, "GetPlayerSpouses");
        if (GetPlayerSpousesMethod is null)
        {
            return false;
        }

        return true;
    }

    public static MBList<Hero> GetSecondarySpouses(Hero hero)
    {
        if (!Initialized
            || hero != Hero.MainHero
            || !IsPolygamyEnabled()
            || !TryGetPlayerPolygamyBehavior(out CampaignBehaviorBase? playerPolygamyBehavior))
        {
            return new MBList<Hero>();
        }

        return (MBList<Hero>)GetPlayerSpousesMethod.Invoke(playerPolygamyBehavior, NoArguments);
    }

    private static bool IsPolygamyEnabled()
    {
        var settingsInstance = SettingsInstanceGetter.Invoke(null, NoArguments);
        var polygamyEnabled = (bool)PolygamyEnabledGetter.Invoke(settingsInstance, NoArguments);

        return polygamyEnabled;
    }

    private static bool TryGetPlayerPolygamyBehavior([NotNullWhen(true)]out CampaignBehaviorBase? behavior)
    {
        var campaignBehaviors = (List<CampaignBehaviorBase>)CampaignBehaviorsField.GetValue(Campaign.Current.CampaignBehaviorManager);
        
        behavior = campaignBehaviors.FirstOrDefault(campaignBehavior => campaignBehavior.GetType() == PlayerPolygamyBehaviorType);

        return behavior is not null;
    }
}
