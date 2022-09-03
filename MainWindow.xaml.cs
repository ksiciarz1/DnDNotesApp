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
            string[] htmlClasses = { "mon-stat-block__name-link", "mon-stat-block__meta", "mon-stat-block__attributes" };
            HttpClient client = new HttpClient();

            string responseBody = "";

            try { responseBody = await client.GetStringAsync(http); }
            catch (HttpRequestException ex) { Console.WriteLine(ex.Message); }

            string[] monster = new string[10];


            int statBlockStart = responseBody.IndexOf("<div class=\"mon-stat-block\">");
            int StatBlockEnd = responseBody.IndexOf("<div class=\"more-info-content\">");
            responseBody = responseBody.Substring(statBlockStart, StatBlockEnd - statBlockStart);

            for (int i = 0; i < htmlClasses.Length; i++)
            {
                int startIndex = responseBody.IndexOf($"class=\"{htmlClasses[i]}\"");

                int dataStart = responseBody.IndexOf(">", startIndex) + 1;
                int dataLength = responseBody.IndexOf("<", startIndex) - responseBody.IndexOf(">", startIndex) - 1;

                string monsterData = responseBody.Substring(dataStart, dataLength);

                //counting html elements inside
                int elementStartCount = monsterData.Count((value) =>
                {
                    return value == '<';
                });
                int elementEndCount = monsterData.Count((value) =>
                {
                    return value == '>';
                });

                monsterData = GetDataFromHTML(monsterData);
                monster[i] = monsterData;
            }
        }

        private string GetDataFromHTML(string cutHTML)
        {
            string returnString = cutHTML;
            returnString = returnString.Replace("\n", " ");
            returnString = returnString.Trim();
            return returnString;
        }

        private string GetElementByClass(string html, string elementClass)
        {
            if (!html.Contains(elementClass)) return "";

            int indexOfClass = html.IndexOf(elementClass);
            int elementStart = html.Substring(0, indexOfClass).LastIndexOf('<');
            int subElementCount = html.Substring(elementStart).Count((value) => { return value == '<'; });

            int elementEnd = elementStart;
            for (int i = 0; i < subElementCount; i++)
            {
                elementEnd = html.Substring(elementEnd).IndexOf('<');
            }

            string finalElement = html.Substring(elementStart, elementEnd - elementStart);
            return finalElement;
        }

        private string GetElementValue(string element)
        {
            int valueStart = element.IndexOf('>') + 1;
            int valueEnd = element.IndexOf('<');
            string value = element.Substring(valueStart, valueEnd - valueStart);
            value = value.Replace("\n", "");
            value = value.Trim();
            return value;
        }
    }
}
