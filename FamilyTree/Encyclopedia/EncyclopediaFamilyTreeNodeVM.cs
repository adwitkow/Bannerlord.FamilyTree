using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items;
using TaleWorlds.Library;

namespace FamilyTree.Encyclopedia
{
    /* Reference EncyclopediaTroopTreeNodeVM */
    public class EncyclopediaFamilyTreeNodeVM : ViewModel 
    {
        private MBBindingList<EncyclopediaFamilyTreeNodeVM> _branch;

        private EncyclopediaFamilyMemberVM _familyMember;

        public EncyclopediaFamilyTreeNodeVM(Hero rootHero, Hero activeHero)
        {
            Branch = new MBBindingList<EncyclopediaFamilyTreeNodeVM>();
            FamilyMember = new EncyclopediaFamilyMemberVM(rootHero, activeHero);
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
            FamilyMember.RefreshValues();
        }

        [DataSourceProperty]
        public EncyclopediaFamilyMemberVM FamilyMember
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
                    OnPropertyChangedWithValue(value, "FamilyMember");
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
