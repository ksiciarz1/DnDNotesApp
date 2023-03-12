using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Documents;

namespace DnDNotesApp
{
    internal class Creature
    {
        // Stats
        private string name;
        private string meta;
        private string armorClass;
        private string hitPoints;
        private string speed;
        private string[] stats = new string[6];
        private string skills;
        private string senses;
        private string languages;
        private string challenge;
        private List<string> Abilities = new List<string>();
        private List<string> Actions = new List<string>();

        public Creature() { }

        public static void GetMonsterDataToFile(string html) => new Creature(html).SaveToFile();


        public Creature(string html)
        {
            while (html.Contains('<'))
            {
                int start = html.IndexOf('<');
                int stop = html.IndexOf('>');

                html = html.Remove(start, stop - start + 1);
            }
            GetData(html);
        }

        private void GetData(string data)
        {
            string[] asLines = data.Split('\n');

            name = asLines[4];
            meta = asLines[8];

            bool statBlock = false;
            bool afterChallange = false;
            bool afterActions = false;

            foreach (var line in asLines) // Order matters
            {
                if (line.Contains("Back to Main"))
                {
                    afterChallange = false;
                    afterActions = false;
                }

                if (afterActions && line != "" && line != "\n")
                {
                    Actions.Add(line);
                }


                if (line.Contains("ACTIONS"))
                    afterActions = true;

                if (!afterActions && afterChallange && line != "" && line != "\n")
                {
                    Abilities.Add(line);
                }

                else if (line.Contains("Skills"))
                {
                    statBlock = false;
                    skills = line.Replace("&#32;", " ");
                }
                else if (line.Contains("Senses"))
                    senses = line.Replace("&#8212;", "---");
                else if (line.Contains("Languages"))
                    languages = line.Replace("&#8212;", "---");
                else if (line.Contains("Challenge"))
                {
                    challenge = line;
                    afterChallange = true;
                }


                if (statBlock && line != "")
                    setStatUp(line);
                if (line.Contains("CHA"))
                    statBlock = true;


                if (line.Contains("Armor"))
                    armorClass = line.Substring("Armor Class".Length);
                else if (line.Contains("Hit Points"))
                    hitPoints = line.Substring("Hit Points".Length);
                else if (line.Contains("Speed"))
                    speed = line.Substring("Speed".Length);

            }
        }

        private void setStatUp(string line)
        {
            for (int i = 0; i < stats.Length; i++)
            {
                if (stats[i] == null)
                {
                    stats[i] = line;
                    break;
                }
            }
        }

        public void SaveToFile()
        {

        }
    }
}
