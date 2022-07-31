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
            bool skip = false;

            // Siblings
            if (baseHero.Siblings.Contains(queriedHero))
            {
                if (baseHero.Father == queriedHero.Father && baseHero.Mother == queriedHero.Mother)
                {
                    if (baseHero.Age == queriedHero.Age)
                    {
                        skip = AddList("str_twin");
                    }
                    else
                    {
                        skip = AddList(queriedHero.IsFemale ? "str_sister" : "str_brother");
                    }
                }
                // != acts as an XOR (exclusive OR)
                else if (baseHero.Father == queriedHero.Father != (baseHero.Mother == queriedHero.Mother))
                {
                    skip = AddList(queriedHero.IsFemale ? "str_halfsister" : "str_halfbrother");
                }
            }
            else 
            {
                // Nieces/nephews
                foreach (Hero sibling in baseHero.Siblings)
                {
                   skip = GetNiecesNephews(sibling, queriedHero);
                }
            }
            // Children
            if (!skip)
            {
                skip = false;
                if (baseHero.Children.Contains(queriedHero))
                {
                    skip = AddList(queriedHero.IsFemale ? "str_daughter" : "str_son");
                }
                else
                {
                    // Grandchildren
                    foreach (Hero child in baseHero.Children)
                    {
                        skip = GetGrandChildren(child, queriedHero);
                    }
                }
            }
            // Parents
            if (!skip)
            {
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
            }
            // Spouse
            if (baseHero.Spouse == queriedHero)
            {
                AddList(queriedHero.IsFemale ? "str_wife" : "str_husband");
            }
            else if (baseHero.ExSpouses.Contains(queriedHero))
            {
                AddList(queriedHero.IsFemale ? "str_exwife" : "str_exhusband");
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
            if (GetCousins(parent, queriedHero))
            {
                return;
            }
            if (GetAuntsUncles(parent, queriedHero))
            {
                return;
            }
            GetGrandParents(parent, queriedHero);
        }

        private static bool GetNiecesNephews(Hero sibling, Hero queriedHero, bool first = true)
        {
            foreach (Hero nieceNephew in sibling.Children)
            {
                if (nieceNephew == queriedHero)
                {
                    if (first)
                    {
                        return AddList(queriedHero.IsFemale ? "str_niece" : "str_nephew");
                    }
                    return AddList(queriedHero.IsFemale ? "str_grandniece" : "str_grandnephew");
                }
                GetNiecesNephews(nieceNephew, queriedHero, false);
            }
            return false;
        }

        private static bool GetCousins(Hero parent, Hero queriedHero)
        {
            foreach (Hero auntUncle in parent.Siblings)
            {
                if (auntUncle.Children.Contains(queriedHero))
                {
                    return AddList("str_cousin");
                }
                GetDistantCousins(auntUncle, queriedHero);
            }
            if (parent.Father is not null)
            {
                GetCousins(parent.Father, queriedHero);
            }
            if (parent.Mother is not null)
            {
                GetCousins(parent.Mother, queriedHero);
            }
            return false;
        }

        private static bool GetDistantCousins(Hero auntUncle, Hero queriedHero)
        {
            if (auntUncle.Children.Any((Hero auntUncleChild) => auntUncleChild == queriedHero)) 
            { 
                return AddList("str_distantcousin");
            }
            foreach(Hero auntUncleChild in auntUncle.Children)
            {
                GetDistantCousins(auntUncleChild, queriedHero);
            }
            return false;
        }

        private static bool GetAuntsUncles(Hero parent, Hero queriedHero, bool first = true)
        {
            if (parent.Siblings.Contains(queriedHero))
            {
                if (first)
                {
                    return AddList(queriedHero.IsFemale ? "str_aunt" : "str_uncle");
                }
                return AddList(queriedHero.IsFemale ? "str_grandaunt" : "str_granduncle");
            }
            if (parent.Father is not null)
            {
                GetAuntsUncles(parent.Father, queriedHero, false);
            }
            if (parent.Mother is not null)
            {
                GetAuntsUncles(parent.Mother, queriedHero, false);
            }
            return false;
        }

        private static bool GetGrandParents(Hero parent, Hero queriedHero, bool first = true)
        {
            if (parent.Father == queriedHero)
            {
                return AddList(first ? "str_grandfather" : "str_greatgrandfather");
            }
            else if (parent.Mother == queriedHero)
            {
                return AddList(first ? "str_grandmother" : "str_greatgrandmother");
            }
            if (parent.Father is not null)
            {
                GetGrandParents(parent.Father, queriedHero, false);
            }
            if (parent.Mother is not null)
            {
                GetGrandParents(parent.Mother, queriedHero, false);
            }
            return false;
        }

        private static bool GetGrandChildren(Hero child, Hero queriedHero, bool first = true)
        {
            if (child.Children.Any((Hero grandChild) => grandChild == queriedHero))
            {
                if (first)
                {
                    return AddList(queriedHero.IsFemale ? "str_granddaughter" : "str_grandson");
                }
                return AddList(queriedHero.IsFemale ? "str_greatgranddaughter" : "str_greatgrandson");
            }
            foreach(Hero grandChild in child.Children)
            {
                GetGrandChildren(grandChild, queriedHero, false);
            }
            return false;
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
