using Bannerlord.UIExtenderEx;
using HarmonyLib;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.FamilyTree
{
    internal class SubModule : MBSubModuleBase
    {
        private static readonly string Namespace = typeof(SubModule).Namespace;

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            var extender = new UIExtender(Namespace);
            extender.Register(typeof(SubModule).Assembly);
            extender.Enable();

            new Harmony(Namespace).PatchAll();
        }
    }
}