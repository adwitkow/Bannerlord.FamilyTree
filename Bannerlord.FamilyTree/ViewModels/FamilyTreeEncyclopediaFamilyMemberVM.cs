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

        ResetRoleValue();
    }

    public Hero BaseHero { get; }

    [DataSourceProperty]
    public string Role
    {
        get
        {
            return _role;
        }
        set
        {
            if (value != _role)
            {
                _role = value;
                base.OnPropertyChangedWithValue(value, nameof(Role));
            }
        }
    }

    public override void RefreshValues()
    {
        base.RefreshValues();

        ResetRoleValue();
    }

    private void ResetRoleValue()
    {
        if (BaseHero is not null && Hero != BaseHero)
        {
            Role = ConversationHelper.GetHeroRelationToHeroTextShort(Hero, BaseHero, true);
        }
    }
}
