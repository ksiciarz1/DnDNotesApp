using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

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

        // Actions   
        public string? Actions { get; set; }

        public Monster() { }

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
            stringBuilder.AppendLine($"**Actions:**");              // Actions
            stringBuilder.AppendLine($"{Actions}");

            System.IO.File.WriteAllText(@"MyNewTextFile.md", stringBuilder.ToString());
            // TODO: Saving this to file in notes directory
        }

    }
}
