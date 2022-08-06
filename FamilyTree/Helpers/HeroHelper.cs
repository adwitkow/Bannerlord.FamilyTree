using TaleWorlds.CampaignSystem;

namespace FamilyTree.Helpers
{
    public static class HeroHelper
    {
        // Patrilineal as well as by leader ranking and clan ranking
        public static Hero FindAncestorOf(Hero hero)
        {
            Clan fatherClan = null;
            Clan motherClan = null;

            if (hero.Father is not null)
            {
                fatherClan = hero.Father.Clan;
            }
            if (hero.Mother is not null)
            {
                motherClan = hero.Mother.Clan;
            }

            // Kingdom Leader
            if (fatherClan is not null && (hero.Father?.IsFactionLeader ?? false) && fatherClan?.Kingdom?.RulingClan == fatherClan)
            {
                return FindAncestorOf(hero.Father);
            }
            if (motherClan is not null && (hero.Mother?.IsFactionLeader ?? false) && motherClan?.Kingdom?.RulingClan == motherClan)
            {
                return FindAncestorOf(hero.Mother);
            }

            // Kingdom Clan
            if (fatherClan is not null && fatherClan?.Kingdom?.RulingClan == fatherClan)
            {
                return FindAncestorOf(hero.Father);
            }
            if (motherClan is not null && motherClan?.Kingdom?.RulingClan == motherClan)
            {
                return FindAncestorOf(hero.Mother);
            }

            // Minor Faction Leader
            if ((fatherClan?.IsMinorFaction ?? false) && (hero.Father?.IsFactionLeader ?? false))
            {
                return FindAncestorOf(hero.Father);
            }
            if ((motherClan?.IsMinorFaction ?? false) && (hero.Mother?.IsFactionLeader ?? false))
            {
                return FindAncestorOf(hero.Mother);
            }

            // Minor Faction Clan
            if (fatherClan?.IsMinorFaction ?? false)
            {
                return FindAncestorOf(hero.Father);
            }
            if (motherClan?.IsMinorFaction ?? false)
            {
                return FindAncestorOf(hero.Mother);
            }

            // Clan Leader
            if (fatherClan is not null && fatherClan?.Leader == hero.Father)
            {
                return FindAncestorOf(hero.Father);
            }
            if (motherClan is not null && motherClan?.Leader == hero.Mother)
            {
                return FindAncestorOf(hero.Mother);
            }

            // Other
            if (hero.Father is not null)
            {
                return FindAncestorOf(hero.Father);
            }
            if (hero.Mother is not null)
            {
                return FindAncestorOf(hero.Mother);
            }

            return hero;
        }
    }
}