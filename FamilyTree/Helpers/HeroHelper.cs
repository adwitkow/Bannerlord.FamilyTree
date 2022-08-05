using TaleWorlds.CampaignSystem;

namespace FamilyTree.Helpers
{
    public static class HeroHelper
    {
        public static Hero FindAncestorOf(Hero hero)
        {
            if (hero.Father?.IsFactionLeader ?? false)
            {
                return FindAncestorOf(hero.Father);
            }
            if (hero.Mother?.IsFactionLeader ?? false)
            {
                return FindAncestorOf(hero.Mother);
            }
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