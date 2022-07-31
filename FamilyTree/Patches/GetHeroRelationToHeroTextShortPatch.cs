using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace FamilyTree.Patches
{
    /* Repurposed from Extended Family to add in more relevant roles */
    [HarmonyPatch(typeof(ConversationHelper), "GetHeroRelationToHeroTextShort")]
    internal class GetHeroRelationToHeroTextShortPatch
    {
        private static List<string> _list;

        private static bool Prefix(ref string __result, Hero queriedHero, Hero baseHero, bool uppercaseFirst)
        {
            _list = new List<string>();

            // Siblings
            if (baseHero.Siblings.Contains(queriedHero))
            {
                if (baseHero.Father == queriedHero.Father && baseHero.Mother == queriedHero.Mother)
                {
                    if (baseHero.Age == queriedHero.Age)
                    {
                        AddList("str_twin");
                    }
                    else
                    {
                        AddList(queriedHero.IsFemale ? "str_sister" : "str_brother");
                    }
                }
                else if (baseHero.Father == queriedHero.Father != (baseHero.Mother == queriedHero.Mother))
                {
                    AddList(queriedHero.IsFemale ? "str_halfsister" : "str_halfbrother");
                }
            }
            // Children
            else if (baseHero.Siblings.Any((Hero sibling) => sibling.Children.Contains(queriedHero)))
            {
                AddList(queriedHero.IsFemale ? "str_niece" : "str_nephew");
            }
            else if (!baseHero.Children.IsEmpty())
            {
                foreach (Hero child in baseHero.Children)
                {
                    if (child == queriedHero)
                    {
                        AddList(queriedHero.IsFemale ? "str_daughter" : "str_son");
                    }
                    else
                    {
                        GetGrandchildren(child, queriedHero);
                    }
                }
            }
            // Parents
            if (baseHero.Father == queriedHero)
            {
                AddList("str_father");
            }
            else if (baseHero.Mother == queriedHero)
            {
                AddList("str_mother");
            }
            else
            { 
                RelatedToParent(baseHero.Father, queriedHero);
                RelatedToParent(baseHero.Mother, queriedHero);
            }
            // Spouse
            if (baseHero.Spouse == queriedHero)
            {
                AddList(queriedHero.IsFemale ? "str_wife" : "str_husband");
            }
            // You
            if (queriedHero == baseHero)
            {
                AddList("str_you");
            }
            // Companion
            if (_list.Count == 0)
            {
                if (queriedHero.CompanionOf == baseHero.Clan)
                {
                    AddList("str_companion");
                }
            }

            // Filter out dupes
            _list = _list.Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList();

            // Join titles together
            string result = string.Join(", ", _list);
            TextObject textObject = new(result, null);

            // Adapted from original method
            if (textObject.Length == 0)
            {
                Debug.FailedAssert("GENERIC - UNSPECIFIED RELATION in clan", "C:\\Develop\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Conversation\\ConversationHelper.cs", "GetHeroRelationToHeroTextShort", 275);
                textObject = GameTexts.FindText("str_relative_of_player", null);
            }
            else if (queriedHero != null)
            {
                textObject.SetCharacterProperties("NPC", queriedHero.CharacterObject, false);
            }
            string text = textObject.ToString();
            if (!char.IsLower(text[0]) != uppercaseFirst)
            {
                char[] array = text.ToCharArray();
                text = (uppercaseFirst ? array[0].ToString().ToUpper() : array[0].ToString().ToLower());
                for (int i = 1; i < array.Count(); i++)
                {
                    text += array[i].ToString();
                }
            }
            __result = text;

            // Skipping this method
            return false;
        }

        private static void RelatedToParent(Hero parent, Hero queriedHero)
        {
            if (parent is null)
            {
                return;
            }
            else if (parent.Siblings.Contains(queriedHero))
            {
                AddList(queriedHero.IsFemale ? "str_aunt" : "str_uncle");
            }
            else if (parent.Siblings.Any((Hero auntUncle) => auntUncle.Children.Contains(queriedHero)))
            {
                AddList("str_cousin");
            }
            else
            {
                GetGrandparents(parent, queriedHero);
            }
        }

        private static void GetGrandparents(Hero parent, Hero queriedHero, bool first = true)
        {
            if (parent.Father == queriedHero)
            {
                AddList(first ? "str_grandfather" : "str_greatgrandfather");
            }
            else if (parent.Mother == queriedHero)
            {
                AddList(first ? "str_grandmother" : "str_greatgrandmother");
            }
            else
            {
                if (parent.Father is not null)
                {
                    GetGrandparents(parent.Father, queriedHero, false);
                }
                if (parent.Mother is not null)
                {
                    GetGrandparents(parent.Mother, queriedHero, false);
                }
            }
        }

        private static void GetGrandchildren(Hero child, Hero queriedHero, bool first = true)
        {
            if (child.Children.Any((Hero grandChild) => grandChild == queriedHero))
            {
                if (first)
                {
                    AddList(queriedHero.IsFemale ? "str_granddaughter" : "str_grandson");
                }
                else
                {
                    AddList(queriedHero.IsFemale ? "str_greatgranddaughter" : "str_greatgrandson");
                }
            }
            else
            {
                foreach(Hero grandChild in child.Children)
                {
                    GetGrandchildren(grandChild, queriedHero, false);
                }
            }
        }

        private static bool AddList(string str)
        {
            _list.Add(FindText(str).ToString());
            return true;
        }

        private static TextObject FindText(string id)
        {
            return GameTexts.FindText(id, null);
        }
    }
}
