using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DnDNotesApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GetHttpRequest();
        }

        private async void GetHttpRequest(string http = "https://www.dndbeyond.com/monsters/16798-bandit")
        {
            string[] htmlClasses = {    "mon-stat-block__name-link", "mon-stat-block__meta","mon-stat-block__attribute-data-value",
                                        "mon-stat-block__attribute-data-extra", "mon-stat-block__tidbit-data", "mon-stat-block__description-block-content" };
            HttpClient client = new HttpClient();

            string responseBody = "";

            try { responseBody = await client.GetStringAsync(http); }
            catch (HttpRequestException ex) { Console.WriteLine(ex.Message); }

            string[] monster = new string[10];

            // Start of monster stats
            int statBlockStart = responseBody.IndexOf("<div class=\"mon-stat-block\">");
            int StatBlockEnd = responseBody.IndexOf("<div class=\"more-info-content\">");
            responseBody = responseBody.Substring(statBlockStart, StatBlockEnd - statBlockStart);

            Monster monster1 = new Monster();

            string element = GetElementByClass(responseBody, htmlClasses[0]);
            monster1.Name = GetElementInnerText(element);
            element = GetElementByClass(responseBody, htmlClasses[1]);
            monster1.Meta = GetElementInnerText(element);

            // Attributes
            element = GetElementByClass(responseBody, htmlClasses[2]);
            string armorClass = GetElementInnerText(element);
            responseBody = responseBody.Replace(element, "");
            element = GetElementByClass(responseBody, htmlClasses[3]);
            responseBody = responseBody.Replace(element, "");
            armorClass += " " + GetElementInnerText(element);
            monster1.ArmorClass = armorClass;

            element = GetElementByClass(responseBody, htmlClasses[2]);
            string hitpoints = GetElementInnerText(element);
            responseBody = responseBody.Replace(element, "");
            element = GetElementByClass(responseBody, htmlClasses[3]);
            responseBody = responseBody.Replace(element, "");
            hitpoints += " " + GetElementInnerText(element);
            monster1.HitPoints = hitpoints;

            element = GetElementByClass(responseBody, htmlClasses[2]);
            string speed = GetElementInnerText(element);
            responseBody = responseBody.Replace(element, "");
            monster1.Speed = speed;

            // Stat Block
            // Other
            element = GetElementByClass(responseBody, htmlClasses[4]);
            responseBody = responseBody.Replace(element, "");
            monster1.Senses = GetElementInnerText(element);

            element = GetElementByClass(responseBody, htmlClasses[4]);
            responseBody = responseBody.Replace(element, "");
            monster1.Languages = GetElementInnerText(element);

            element = GetElementByClass(responseBody, htmlClasses[4]);
            responseBody = responseBody.Replace(element, "");
            monster1.Challange = GetElementInnerText(element);

            // Actions
            element = GetElementByClass(responseBody, htmlClasses[5]);
            monster1.Actions = GetElementInnerText(element);

            monster1.SaveToFile();
            Close();
        }


        private string GetElementByClass(string html, string elementClass)
        {
            // TODO: Make so that inner elements are included
            if (!html.Contains(elementClass)) return "";

            int indexOfClass = html.IndexOf(elementClass);
            int elementStart = html.Substring(0, indexOfClass).LastIndexOf('<');
            string v = html.Substring(elementStart);
            int elementEnd = v.IndexOf('>', v.IndexOf('>') + 1) + 1;


            string finalElement = html.Substring(elementStart, elementEnd);
            return finalElement;
        }

        private string GetElementInnerText(string element)
        {
            int valueStart = element.IndexOf('>') + 1;
            int valueEnd = element.Substring(1).IndexOf('<');
            string value = element.Substring(valueStart, valueEnd - valueStart);
            value = value.Replace("\n", "");
            value = value.Trim();
            return value;
        }
    }
}
