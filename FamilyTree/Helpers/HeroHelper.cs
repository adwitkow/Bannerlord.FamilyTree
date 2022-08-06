using System.Collections.Generic;
using TaleWorlds.CampaignSystem;

namespace FamilyTree.Helpers
{
    public static class HeroHelper
    {
        // Patrilineal as well as by leader ranking and clan ranking
        public static Hero FindAncestorOf(Hero hero)
        {
            List<Hero> parents = new();

            // Add parents to list if not null
            if (hero.Father is not null)
            {
                parents.Add(hero.Father);
            }
            if (hero.Mother is not null)
            {
                parents.Add(hero.Mother);
            }

            // Kingdom Ruling Clan Leader
            foreach (Hero parent in parents)
            {
                if (parent.Clan.Kingdom?.Leader == parent)
                {
                    return FindAncestorOf(parent);
                }
            }
            // Kingdom Ruling Clan
            foreach (Hero parent in parents)
            {
                if (parent.Clan.Kingdom?.RulingClan == parent.Clan)
                {
                    return FindAncestorOf(parent);
                }
            }

            // Kingdom Clan Leader
            foreach (Hero parent in parents)
            {
                if (parent.Clan.IsKingdomFaction && parent.IsFactionLeader)
                {
                    return FindAncestorOf(parent);
                }
            }
            // Kingdom Clan
            foreach (Hero parent in parents)
            {
                if (parent.Clan.IsKingdomFaction)
                {
                    return FindAncestorOf(parent);
                }
            }

            // Minor Faction Leader
            foreach (Hero parent in parents)
            {
                if (parent.Clan.IsMinorFaction && parent.IsFactionLeader)
                {
                    return FindAncestorOf(parent);
                }
            }
            // Minor Faction Clan
            foreach (Hero parent in parents)
            {
                if (parent.Clan.IsMinorFaction)
                {
                    return FindAncestorOf(parent);
                }
            }

            // Clan Leader
            foreach (Hero parent in parents)
            {
                if (parent.Clan.Leader == parent)
                {
                    return FindAncestorOf(parent);
                }
            }

            // Other
            foreach (Hero parent in parents)
            {
                return FindAncestorOf(parent);
            }

            return hero;
        }
    }
}