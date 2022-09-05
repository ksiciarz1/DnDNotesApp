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

            try { responseBody = await client.GetStringAsync(http); }
            catch (HttpRequestException ex) { Console.WriteLine(ex.Message); }

            // Start of monster stats
            int statBlockStart = responseBody.IndexOf("<div class=\"mon-stat-block\">");
            int StatBlockEnd = responseBody.IndexOf("<div class=\"more-info-content\">");
            responseBody = responseBody.Substring(statBlockStart, StatBlockEnd - statBlockStart);

            Monster.GetMonsterDataToFile(responseBody);

            htmlTextBox.Text = "";
            MessageBox.Show("Done !");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (htmlTextBox.Text != "" && htmlTextBox.Text != null)
                GetMonsterDataFromHtml(htmlTextBox.Text);
        }
    }
}
