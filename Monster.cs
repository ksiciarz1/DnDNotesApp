using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
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
        public string? Senses { get; set; }
        public string? Languages { get; set; }
        public string? Challange { get; set; }
        public string? Proficency { get; set; }

        public Dictionary<string, string> Attributes = new Dictionary<string, string>();

        // Actions   
        public string? Actions { get; set; }

        private string[] htmlClasses = {    "mon-stat-block__name-link", "mon-stat-block__meta", "mon-stat-block__attribute-data-value",
                                            "mon-stat-block__attribute-data-extra", "ability-block__score",
                                            "ability-block__modifier", "mon-stat-block__tidbit-data", "mon-stat-block__description-block-content" };
        private int nowIndex = 0;

        private string Class_Label = "mon-stat-block__attribute-label";

        public Monster() { }

        public void CreateDoubleValueAttribute(string html, string htmlClass)
        {
            string key = GetDataByClass(html, Class_Label);
            string value = $"{GetDataByClass(html, htmlClasses[2])} {GetDataByClass(html, htmlClasses[3])}";

            Attributes.Add(key, value);
        }

        public Monster(string html)
        {
            Name = GetDataByClass(html, htmlClasses[0]);
            Meta = GetDataByClass(html, htmlClasses[1]);

            CreateDoubleValueAttribute(html, Class_Label);

            ArmorClass = $"{GetDataByClass(html, htmlClasses[2])} {GetDataByClass(html, htmlClasses[3])}";
            HitPoints = $"{GetDataByClass(html, htmlClasses[2])} {GetDataByClass(html, htmlClasses[3])}";
            Speed = GetDataByClass(html, htmlClasses[2]);

            Strength = $"{GetDataByClass(html, htmlClasses[4])} {GetDataByClass(html, htmlClasses[5])}";
            Dexterity = $"{GetDataByClass(html, htmlClasses[4])} {GetDataByClass(html, htmlClasses[5])}";
            Constitution = $"{GetDataByClass(html, htmlClasses[4])} {GetDataByClass(html, htmlClasses[5])}";
            Inteligance = $"{GetDataByClass(html, htmlClasses[4])} {GetDataByClass(html, htmlClasses[5])}";
            Wisdom = $"{GetDataByClass(html, htmlClasses[4])} {GetDataByClass(html, htmlClasses[5])}";
            Charisma = $"{GetDataByClass(html, htmlClasses[4])} {GetDataByClass(html, htmlClasses[5])}";

            Senses = GetDataByClass(html, htmlClasses[6]);
            Languages = GetDataByClass(html, htmlClasses[6]);
            Challange = GetDataByClass(html, htmlClasses[6]);

            Actions = GetElementByClass(html, htmlClasses[7]);

            SaveToFile();
        }

        public void SaveToFile()
        {
            // TODO: convert stats to ints

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"## {Name}");                 // General
            stringBuilder.AppendLine($"{Meta}");
            stringBuilder.AppendLine($"");
            stringBuilder.AppendLine($"---");
            stringBuilder.AppendLine($"");
            stringBuilder.AppendLine($"**HitPoints:** {HitPoints}");
            stringBuilder.AppendLine($"**Armor Class:** {ArmorClass}");
            stringBuilder.AppendLine($"**Speed:** {Speed}");
            stringBuilder.AppendLine($"**Challange:** {Challange}");
            stringBuilder.AppendLine($"");
            stringBuilder.AppendLine($"STR|DEX|CON|INT|WIS|CHA");   // StatBlock
            stringBuilder.AppendLine($"---|---|---|---|---|---");
            stringBuilder.AppendLine($"{Strength}|{Dexterity}|{Constitution}|{Inteligance}|{Wisdom}|{Charisma}");
            stringBuilder.AppendLine($"");
            stringBuilder.AppendLine($"**Senses:** {Senses}");      // Additional
            stringBuilder.AppendLine($"**Languages:** {Languages}");
            stringBuilder.AppendLine($"**Proficency:** {Proficency}");
            stringBuilder.AppendLine($"");
            stringBuilder.AppendLine($"#### **Actions:**");              // Actions
            stringBuilder.AppendLine($"{Actions}");

            System.IO.File.WriteAllText(@"MyNewTextFile.md", stringBuilder.ToString());
            // TODO: Saving this to file in notes directory
        }

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
            int valueStart = element.IndexOf('>') + 1;
            int valueEnd = element.Substring(1).IndexOf('<') + 1;
            string value = element.Substring(valueStart, valueEnd - valueStart);
            value = value.Replace("\n", "");
            value = value.Trim();
            return value;
        }

        private string GetDataByClass(string html, string elementClass)
        {
            return GetElementInnerText(GetElementByClass(html, elementClass));
        }
    }
}
