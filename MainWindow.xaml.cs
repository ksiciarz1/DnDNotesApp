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
using System.Xml.Linq;

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

        private async void GetHttpRequest(string http = "https://www.dndbeyond.com/monsters/17011-shambling-mound")
        {
            string[] htmlClasses = {    "mon-stat-block__name-link", "mon-stat-block__meta","mon-stat-block__attribute-data-value",
                                        "mon-stat-block__attribute-data-extra", "mon-stat-block__tidbit-data", "mon-stat-block__description-block-content",
                                        "ability-block__score", "ability-block__modifier" };

            HttpClient client = new HttpClient();

            string responseBody = "";

            try { responseBody = await client.GetStringAsync(http); }
            catch (HttpRequestException ex) { Console.WriteLine(ex.Message); }

            // Start of monster stats
            int statBlockStart = responseBody.IndexOf("<div class=\"mon-stat-block\">");
            int StatBlockEnd = responseBody.IndexOf("<div class=\"more-info-content\">");
            responseBody = responseBody.Substring(statBlockStart, StatBlockEnd - statBlockStart);

            Monster monster = new Monster(responseBody);
            Close();
        }


        private string GetElementByClass(string html, string elementClass)
        {
            if (!html.Contains(elementClass)) return "";

            string finalElement = "";
            int indexOfClass = html.IndexOf(elementClass);
            int elementStart = html.Substring(0, indexOfClass).LastIndexOf('<');
            if (html.Substring(elementStart + 1).IndexOf('<') == html.Substring(elementStart + 1).IndexOf("</"))
            {
                string v = html.Substring(elementStart);
                int elementEnd = v.IndexOf('>', v.IndexOf('>') + 1) + 1;
                finalElement = html.Substring(elementStart, elementEnd);
            }
            else
            {
                int subElementCount = 0;
                string htmlElement = html.Substring(elementStart);
                int elementEnd = 0;
                bool locked = true;
                htmlElement = htmlElement.Replace("</", "<|");
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
            int valueStart = element.IndexOf('>') + 1;
            int valueEnd = element.Substring(1).IndexOf('<') + 1;
            string value = element.Substring(valueStart, valueEnd - valueStart);
            value = value.Replace("\n", "");
            value = value.Trim();
            return value;
        }
    }
}
