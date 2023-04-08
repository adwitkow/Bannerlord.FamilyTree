using Bannerlord.UIExtenderEx.Attributes;
using Bannerlord.UIExtenderEx.Prefabs2;

namespace Bannerlord.FamilyTree.ViewModels
{
    /* Modules\SandBox\GUI\Prefabs\Encyclopedia\EncyclopediaSubPages */
    [PrefabExtension("EncyclopediaHeroPage", "descendant::NavigationScopeTargeter[@ScopeID='EncyclopediaHeroFamilyContentScope']")]
    internal class EncyclopediaFamilyGridScopeTargeterExtension : PrefabExtensionInsertPatch
    {
        public override InsertType Type => InsertType.Replace;

        [PrefabExtensionFileName]
        public string FamilyGridScopeTargeterReplace => "EncyclopediaFamilyGridScopeTargeter";

    }

    /* Added in reverse order */
    [PrefabExtension("EncyclopediaHeroPage", "descendant::NavigatableGridWidget[@Id='FamilyGrid']")]
    internal class EncyclopediaFamilyTreeWidgetExtension : PrefabExtensionInsertPatch
    {
        public override InsertType Type => InsertType.Append;

        [PrefabExtensionFileName]
        public string FamilyTreeWidgetInject => "EncyclopediaFamilyTreeWidget";

    }

    [PrefabExtension("EncyclopediaHeroPage", "descendant::NavigatableGridWidget[@Id='FamilyGrid']")]
    internal class EncyclopediaFamilyTreeContentScopeTargeterExtension : PrefabExtensionInsertPatch
    {
        public override InsertType Type => InsertType.Append;

        [PrefabExtensionFileName]
        public string FamilyTreeContentScopeTargeterInject => "EncyclopediaFamilyTreeContentScopeTargeter";

    }

    [PrefabExtension("EncyclopediaHeroPage", "descendant::NavigatableGridWidget[@Id='FamilyGrid']")]
    internal class EncyclopediaFamilyTreeDividerExtension : PrefabExtensionInsertPatch
    {
        public override InsertType Type => InsertType.Append;

        [PrefabExtensionFileName]
        public string FamilyTreeDividerInject => "EncyclopediaFamilyTreeDivider";

    }

    [PrefabExtension("EncyclopediaHeroPage", "descendant::NavigatableGridWidget[@Id='FamilyGrid']")]
    internal class EncyclopediaFamilyTreeDividerAutoScrollExtension : PrefabExtensionInsertPatch
    {
        public override InsertType Type => InsertType.Append;

        [PrefabExtensionFileName]
        public string FamilyTreeDividerAutoScrollInject => "EncyclopediaFamilyTreeDividerAutoScroll";

    }

    [PrefabExtension("EncyclopediaHeroPage", "descendant::NavigatableGridWidget[@Id='FamilyGrid']")]
    internal class EncyclopediaFamilyTreeDividerScopeTargeterExtension : PrefabExtensionInsertPatch
    {
        public override InsertType Type => InsertType.Append;

        [PrefabExtensionFileName]
        public string FamilyTreeDividerScopeTargeterInject => "EncyclopediaFamilyTreeDividerScopeTargeter";

    }
}
