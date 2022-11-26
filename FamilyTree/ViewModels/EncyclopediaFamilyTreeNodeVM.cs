using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items;
using TaleWorlds.Library;

namespace FamilyTree.ViewModels
{
    /* Reference EncyclopediaTroopTreeNodeVM */
    public class EncyclopediaFamilyTreeNodeVM : ViewModel 
    {
        private MBBindingList<EncyclopediaFamilyTreeNodeVM> _branch;

        private MBBindingList<EncyclopediaFamilyMemberVM> _familyMember;

        // Reminding myself what the params are:
        // rootHero runs through each member in the family
        // activeHero is always the currently selected hero
        public EncyclopediaFamilyTreeNodeVM(Hero rootHero, Hero activeHero)
        {
            Branch = new();
            FamilyMember = new()
            {
                new EncyclopediaFamilyMemberVM(rootHero, activeHero)
            };
            if (rootHero.Spouse is not null)
            {
                FamilyMember.Add(new EncyclopediaFamilyMemberVM(rootHero.Spouse, activeHero));
            }
            // Almost forgot to add exspouses
            // We'll see how crazy this gets!
            foreach (Hero exSpouses in rootHero.ExSpouses)
            {
                FamilyMember.Add(new EncyclopediaFamilyMemberVM(exSpouses, activeHero));
            }
            foreach (Hero child in rootHero.Children)
            {
                Branch.Add(new EncyclopediaFamilyTreeNodeVM(child, activeHero));
            }
        }

        public override void RefreshValues()
        {
            base.RefreshValues();
            Branch.ApplyActionOnAllItems(delegate (EncyclopediaFamilyTreeNodeVM x)
            {
                x.RefreshValues();
            });
            FamilyMember.ApplyActionOnAllItems(delegate (EncyclopediaFamilyMemberVM x)
            {
                x.RefreshValues();
            });
        }

        [DataSourceProperty]
        public MBBindingList<EncyclopediaFamilyMemberVM> FamilyMember
        {
            get
            {
                return _familyMember;
            }
            set
            {
                if (value != _familyMember)
                {
                    _familyMember = value;
                    OnPropertyChanged("FamilyMember");
                }
            }
        }

        [DataSourceProperty]
        public MBBindingList<EncyclopediaFamilyTreeNodeVM> Branch
        {
            get
            {
                return _branch;
            }
            set
            {
                if (value != _branch)
                {
                    _branch = value;
                    OnPropertyChanged("Branch");
                }
            }
        }
    }
}
