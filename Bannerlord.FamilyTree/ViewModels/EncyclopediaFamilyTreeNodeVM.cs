using Bannerlord.FamilyTree.Compatibility;
using Bannerlord.ModuleManager;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;

namespace Bannerlord.FamilyTree.ViewModels
{
    public class EncyclopediaFamilyTreeNodeVM : ViewModel
    {
        private MBBindingList<EncyclopediaFamilyTreeNodeVM> _branches;

        private MBBindingList<FamilyTreeEncyclopediaFamilyMemberVM> _familyMembers;

        public EncyclopediaFamilyTreeNodeVM(Hero currentHero, Hero selectedHero)
        {
            Branches = new();
            FamilyMembers = new()
            {
                new FamilyTreeEncyclopediaFamilyMemberVM(currentHero, selectedHero)
            };

            var spouses = new MBList<Hero>();
            if (currentHero.Spouse is not null)
            {
                spouses.Add(currentHero.Spouse);
            }

            foreach (Hero secondarySpouse in ModCompatibility.GetSecondarySpouses(currentHero))
            {
                spouses.Add(secondarySpouse);
            }

            foreach (Hero exSpouse in currentHero.ExSpouses)
            {
                spouses.Add(exSpouse);
            }

            foreach (Hero spouse in spouses.DistinctBy(spouse => spouse.StringId))
            {
                FamilyMembers.Add(new FamilyTreeEncyclopediaFamilyMemberVM(spouse, selectedHero));
            }

            foreach (Hero child in currentHero.Children.DistinctBy(child => child.StringId))
            {
                Branches.Add(new EncyclopediaFamilyTreeNodeVM(child, selectedHero));
            }
        }

        public override void RefreshValues()
        {
            base.RefreshValues();

            Branches.ApplyActionOnAllItems(x => x.RefreshValues());
            FamilyMembers.ApplyActionOnAllItems(x => x.RefreshValues());
        }

        [DataSourceProperty]
        public MBBindingList<FamilyTreeEncyclopediaFamilyMemberVM> FamilyMembers
        {
            get
            {
                return _familyMembers;
            }
            set
            {
                if (value != _familyMembers)
                {
                    _familyMembers = value;
                    OnPropertyChanged("FamilyMember");
                }
            }
        }

        [DataSourceProperty]
        public MBBindingList<EncyclopediaFamilyTreeNodeVM> Branches
        {
            get
            {
                return _branches;
            }
            set
            {
                if (value != _branches)
                {
                    _branches = value;
                    OnPropertyChanged("Branch");
                }
            }
        }
    }
}
