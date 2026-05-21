using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Library;

namespace Bannerlord.FamilyTree.ViewModels;

public class FamilyTreeEncyclopediaFamilyMemberVM : HeroVM
{
    private string _role;

    public FamilyTreeEncyclopediaFamilyMemberVM(Hero hero, Hero baseHero)
        : base(hero, useCivilian: false)
    {
        BaseHero = baseHero;
        Role = string.Empty;

        RefreshValues();
    }

    public Hero BaseHero { get; }

    [DataSourceProperty]
    public string Role
    {
        get
        {
            return this._role;
        }
        set
        {
            if (value != this._role)
            {
                this._role = value;
                base.OnPropertyChangedWithValue<string>(value, nameof(Role));
            }
        }
    }

    public override void RefreshValues()
    {
        base.RefreshValues();

        if (BaseHero is not null && Hero != BaseHero)
        {
            this.Role = ConversationHelper.GetHeroRelationToHeroTextShort(Hero, BaseHero, true);
        }
    }
}
