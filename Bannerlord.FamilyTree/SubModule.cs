using Bannerlord.FamilyTree.Compatibility;
using Bannerlord.UIExtenderEx;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.FamilyTree
{
    internal class SubModule : MBSubModuleBase
    {
        private static readonly string Namespace = typeof(SubModule).Namespace;

        protected override void OnSubModuleLoad()
        {
            var extender = new UIExtender(Namespace);
            extender.Register(typeof(SubModule).Assembly);
            extender.Enable();

            base.OnSubModuleLoad();
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

            ModCompatibility.Initialize();
        }
    }
}