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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void GetMonsterDataFromHtml(string http)
        {
            HttpClient client = new HttpClient();
            string responseBody = "";
            if (!http.Contains("dandwiki"))
            {
                http = http.Trim().Replace(' ', '_');
                http = $"https://www.dandwiki.com/wiki/5e_SRD:{http}";
            }


            try { responseBody = await client.GetStringAsync(http); }
            catch (HttpRequestException ex) { Console.WriteLine(ex.Message); }

            // Start of monster stats
            int statBlockStart = responseBody.IndexOf("mw-parser-output");
            int StatBlockEnd = responseBody.IndexOf("printfooter");
            responseBody = responseBody.Substring(statBlockStart, StatBlockEnd - statBlockStart);
            responseBody = responseBody.Remove(0, responseBody.IndexOf('>') + 1);
            responseBody = responseBody.Remove(responseBody.LastIndexOf('<'));

            Creature.GetMonsterDataToFile(responseBody);

            htmlTextBox.Text = "";
            MessageBox.Show("Done !");
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DoANoteButtonClick(object sender, RoutedEventArgs e)
        {
            if (htmlTextBox.Text != "" && htmlTextBox.Text != null)
                GetMonsterDataFromHtml(htmlTextBox.Text);
        }
    }
}
