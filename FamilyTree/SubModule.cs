using Bannerlord.UIExtenderEx;
using HarmonyLib;
using TaleWorlds.MountAndBlade;

namespace FamilyTree
{
    internal class SubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            var extender = new UIExtender("FamilyTree");
            extender.Register(typeof(SubModule).Assembly);
            extender.Enable();

            new Harmony("mod.bannerlord.tree.family").PatchAll();
        }
    }
}