using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DnDNotesApp
{
    internal class Monster
    {
        // General
        public string? Name { get; set; }
        public string? Meta { get; set; }
        public string? ArmorClass { get; set; }
        public string? HitPoints { get; set; }
        public string? Speed { get; set; }

        // StatBlock 
        public string? Strength { get; set; }
        public string? Dexterity { get; set; }
        public string? Constitution { get; set; }
        public string? Inteligance { get; set; }
        public string? Wisdom { get; set; }
        public string? Charisma { get; set; }

        // Additional
        public string? Skills { get; set; }
        public string? Resistances { get; set; }
        public string? Immunities { get; set; }
        public string? ConditionImunities { get; set; }
        public string? Senses { get; set; }
        public string? Languages { get; set; }
        public string? Challange { get; set; }
        public string? Proficency { get; set; }

        // Actions   
        public string? Actions { get; set; }


        // html classes
        private const string nameClass = "mon-stat-block__name-link";
        private const string metaClass = "mon-stat-block__meta";

        private const string attributeLabelClass = "mon-stat-block__attribute-label";
        private const string attributeValueClass = "mon-stat-block__attribute-data-value";
        private const string attributeExtraClass = "mon-stat-block__attribute-data-extra";

        private const string abilityScoreClass = "ability-block__score";
        private const string abilityModifierClass = "ability-block__modifier";

        private const string additionalLabelClass = "mon-stat-block__tidbit-label";
        private const string additionalDataClass = "mon-stat-block__tidbit-data";

        private const string actionsClass = "mon-stat-block__description-block-content";

        private int nowIndex = 0;
        private Dictionary<string, string> stats = new Dictionary<string, string>();

        public Monster() { }

        public void CreateDoubleValueAttribute(string html)
        {
            string key = GetDataByClass(html, attributeLabelClass);
            string value = $"{GetDataByClass(html, attributeValueClass)} {GetDataByClass(html, attributeExtraClass)}";

            stats.Add(key, value);
        }

        public void CreateDoubleValueAdditional(string html)
        {
            string key = GetDataByClass(html, additionalLabelClass);
            string value = $"{GetDataByClass(html, additionalDataClass)}";

            stats.Add(key, value);
        }

        public static void GetMonsterDataToFile(string html) => new Monster(html).SaveToFile();


        public Monster(string html)
        {
            Name = GetDataByClass(html, nameClass);
            Meta = GetDataByClass(html, metaClass);

            while (html.IndexOf(attributeLabelClass, nowIndex) != -1)
            {
                CreateDoubleValueAttribute(html);
            }
            ArmorClass = stats["Armor Class"];
            HitPoints = stats["Hit Points"];
            Speed = stats["Speed"];

            Strength = $"{GetDataByClass(html, abilityScoreClass)} {GetDataByClass(html, abilityModifierClass)}";
            Dexterity = $"{GetDataByClass(html, abilityScoreClass)} {GetDataByClass(html, abilityModifierClass)}";
            Constitution = $"{GetDataByClass(html, abilityScoreClass)} {GetDataByClass(html, abilityModifierClass)}";
            Inteligance = $"{GetDataByClass(html, abilityScoreClass)} {GetDataByClass(html, abilityModifierClass)}";
            Wisdom = $"{GetDataByClass(html, abilityScoreClass)} {GetDataByClass(html, abilityModifierClass)}";
            Charisma = $"{GetDataByClass(html, abilityScoreClass)} {GetDataByClass(html, abilityModifierClass)}";

            while (html.IndexOf(additionalLabelClass, nowIndex) != -1)
            {
                CreateDoubleValueAdditional(html);
            }
            Skills = stats.ContainsKey("Skills") ? stats["Skills"] : null;
            Resistances = stats.ContainsKey("Damage Resistances") ? stats["Damage Resistances"] : null;
            Immunities = stats.ContainsKey("Damage Immunities") ? stats["Damage Immunities"] : null;
            ConditionImunities = stats.ContainsKey("Condition Immunities") ? stats["Condition Immunities"] : null;
            Senses = stats.ContainsKey("Senses") ? stats["Senses"] : null;
            Languages = stats.ContainsKey("Languages") ? stats["Languages"] : null;
            Challange = stats.ContainsKey("Challenge") ? stats["Challenge"] : null;
            Proficency = stats.ContainsKey("Proficency") ? stats["Proficency"] : null;

            Actions = GetElementByClass(html, actionsClass);

            SaveToFile();
        }

        public void SaveToFile()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"## {Name}");                     // General
            stringBuilder.AppendLine($"{Meta}");
            stringBuilder.AppendLine($"");
            stringBuilder.AppendLine($"---");
            stringBuilder.AppendLine($"");
            stringBuilder.AppendLine($"**HitPoints:** {HitPoints}");
            stringBuilder.AppendLine($"**Armor Class:** {ArmorClass}");
            stringBuilder.AppendLine($"**Speed:** {Speed}");
            stringBuilder.AppendLine($"**Challange:** {Challange}");
            stringBuilder.AppendLine($"");
            stringBuilder.AppendLine($"STR|DEX|CON|INT|WIS|CHA");       // StatBlock
            stringBuilder.AppendLine($"---|---|---|---|---|---");
            stringBuilder.AppendLine($"{Strength}|{Dexterity}|{Constitution}|{Inteligance}|{Wisdom}|{Charisma}");
            stringBuilder.AppendLine($"");
            stringBuilder.AppendLine($"---");
            stringBuilder.AppendLine($"");
            stringBuilder.AppendLine($"#### Additional");               // Additional
            if (Skills != null)
                stringBuilder.AppendLine($"**Skills:** {Skills}");
            if (Resistances != null)
                stringBuilder.AppendLine($"**Damage Resistances:** {Resistances}");
            if (Immunities != null)
                stringBuilder.AppendLine($"**Damage Immunities:** {Immunities}");
            if (ConditionImunities != null)
                stringBuilder.AppendLine($"**Conditional Immunities:** {ConditionImunities}");
            if (Senses != null)
                stringBuilder.AppendLine($"**Senses:** {Senses}");
            if (Languages != null)
                stringBuilder.AppendLine($"**Languages:** {Languages}");
            if (Proficency != null)
                stringBuilder.AppendLine($"**Proficency:** {Proficency}");
            stringBuilder.AppendLine($"");
            stringBuilder.AppendLine($"---");
            stringBuilder.AppendLine($"");
            stringBuilder.AppendLine($"#### **Actions:**");              // Actions
            stringBuilder.AppendLine($"{Actions}");

            if (Name != null)
                System.IO.File.WriteAllText($"{Name}.md", stringBuilder.ToString());
            else
                System.IO.File.WriteAllText($"MyMonsterTextFile.md", stringBuilder.ToString());
        }


        private string GetDataByClass(string html, string elementClass) => GetElementInnerText(GetElementByClass(html, elementClass));

        private string GetElementByClass(string html, string elementClass)
        {
            if (!html.Contains(elementClass)) return "";

            string finalElement = "";
            int indexOfClass = html.IndexOf(elementClass, nowIndex);
            if (indexOfClass == -1) return "";
            int elementStart = html.Substring(0, indexOfClass).LastIndexOf('<');
            if (html.Substring(elementStart + 1).IndexOf('<') == html.Substring(elementStart + 1).IndexOf("</"))
            {
                string v = html.Substring(elementStart);
                int elementEnd = v.IndexOf('>', v.IndexOf('>') + 1) + 1;
                finalElement = html.Substring(elementStart, elementEnd);
                nowIndex = elementStart + elementEnd;
            }
            else
            {
                int subElementCount = 0;
                string htmlElement = html.Substring(elementStart);
                int elementEnd = 0;
                bool locked = true;
                htmlElement = htmlElement.Replace("</", "<|"); // "/" Can exist in text making counting html markers unreliable
                for (int i = 0; i < htmlElement.Length; i++)
                {
                    if (htmlElement[i] == '<')
                    {
                        subElementCount++;
                    }
                    if (htmlElement[i] == '|')
                    {
                        subElementCount -= 2;
                    }

                    if (subElementCount == 0 && !locked)
                    {
                        elementEnd += i - 1;
                        break;
                    }
                    else if (locked) locked = false;
                }
                finalElement = html.Substring(elementStart, elementEnd);
                nowIndex = elementStart + elementEnd;

                finalElement = finalElement.Replace("<strong>", "**");
                finalElement = finalElement.Replace("</strong>", "**");
                finalElement = finalElement.Replace("|", "");
                finalElement = finalElement.Replace("\n", "");
                while (finalElement.Contains("<"))
                {
                    string firstString = finalElement.Substring(0, finalElement.IndexOf('<'));
                    string secoundString = finalElement.Substring(finalElement.IndexOf('>') + 1);
                    finalElement = firstString + secoundString;
                }
            }
            finalElement = finalElement.Trim();
            finalElement = finalElement.Replace(".**", ".**\n");
            return finalElement;
        }

        private string GetElementInnerText(string element)
        {
            if (element.Length == 0) return "";
            if (!element.Contains('>')) return element;
            int valueStart = element.IndexOf('>') + 1;
            int valueEnd = element.Substring(1).IndexOf('<') + 1;
            string value = element.Substring(valueStart, valueEnd - valueStart);
            value = value.Replace("\n", "");
            value = value.Trim();
            return value;
        }
    }
}
