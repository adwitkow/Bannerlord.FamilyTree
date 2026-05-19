using Bannerlord.FamilyTree.Compatibility;
using Bannerlord.ModuleManager;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items;
using TaleWorlds.Library;

namespace Bannerlord.FamilyTree.ViewModels
{
    /* Reference EncyclopediaTroopTreeNodeVM */
    public class EncyclopediaFamilyTreeNodeVM : ViewModel
    {
        private MBBindingList<EncyclopediaFamilyTreeNodeVM> _branch;

        private MBBindingList<EncyclopediaFamilyMemberVM> _familyMember;

        public EncyclopediaFamilyTreeNodeVM(Hero currentHero, Hero selectedHero)
        {
            Branch = new();
            FamilyMember = new()
            {
                new EncyclopediaFamilyMemberVM(currentHero, selectedHero)
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
                //var relation = ConversationHelper.GetHeroRelationToHeroTextShort(spouse, selectedHero, true);
                FamilyMember.Add(new EncyclopediaFamilyMemberVM(spouse, selectedHero));
            }

            foreach (Hero child in currentHero.Children.DistinctBy(child => child.StringId))
            {
                Branch.Add(new EncyclopediaFamilyTreeNodeVM(child, selectedHero));
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
