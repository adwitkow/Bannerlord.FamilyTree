using Bannerlord.UIExtenderEx.Attributes;
using Bannerlord.UIExtenderEx.ViewModels;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages;
using TaleWorlds.Core;
using TaleWorlds.Library;
using FamilyTree.Helpers;

namespace FamilyTree.ViewModels
{
    /* Reference EncyclopediaUnitPageVM and EncyclopediaHeroPageVM */
    [ViewModelMixin]
    internal class EncyclopediaHeroPageVMMixin : BaseViewModelMixin<EncyclopediaHeroPageVM>
    {
        private EncyclopediaFamilyTreeNodeVM _familyTree;

        private string _familyTreeText;

        private readonly Hero _hero;

        public EncyclopediaHeroPageVMMixin(EncyclopediaHeroPageVM vm) : base(vm)
        {
            _hero = vm.Obj as Hero;
            /* TaleWorlds refers to the "root" as the top of the upgrade tree AKA oldest ancestor in our case */
            Hero rootHero = HeroHelper.FindAncestorOf(_hero);
            FamilyTree = new EncyclopediaFamilyTreeNodeVM(rootHero, _hero);
            vm.RefreshValues();
            FamilyTreeText = GameTexts.FindText("str_familytreegroup", null).ToString();
        }

        [DataSourceProperty]
        public EncyclopediaFamilyTreeNodeVM FamilyTree
        {
            get
            {
                return _familyTree;
            }
            set
            {
                if (value != _familyTree)
                {
                    _familyTree = value;
                    ViewModel?.OnPropertyChangedWithValue(value, "FamilyTree");
                }
            }
        }

        [DataSourceProperty]
        public string FamilyTreeText
        {
            get
            {
                return _familyTreeText;
            }
            set
            {
                if (value != _familyTreeText)
                {
                    _familyTreeText = value;
                    ViewModel?.OnPropertyChangedWithValue(value, "FamilyTreeText");
                }
            }
        }
    }
}
