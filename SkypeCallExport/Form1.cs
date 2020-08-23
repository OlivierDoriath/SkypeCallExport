using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SkypeCallExport
{

    public partial class Form1 : Form
    {
        Container data;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = ".\\";
                openFileDialog.Filter = "json files (*.json)|*.json";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                    data = JsonSerializer.Deserialize<Container>(fileContent);
                    label1.Text = "Export date: " + data.exportDate.ToString();
                    checkedListBox1.Enabled = true;
                    checkedListBox1.Items.Clear();
                    foreach (var c in data.conversations)
                    {
                        if (c.id.StartsWith("8:"))
                            checkedListBox1.Items.Add(c.id.Substring(2));
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var finalMessages = new List<Message>();
            var finalFile = new List<string>();
            foreach (var c in data.conversations)
            {
                if (checkedListBox1.CheckedItems.Contains(c.id.Substring(2)))
                    foreach (var m in c.MessageList)
                    {
                        if (m.messagetype == @"Event/Call")
                        {
                            finalMessages.Add(m);
                        }
                    }
            }
            for (int i = finalMessages.Count - 1; i >= 0; i--)
            {

                if (finalMessages[i].content.Contains("duration"))
                {
                    Regex regex = new Regex("<duration>(.*?)</duration>");
                    var group = regex.Match(finalMessages[i].content);
                    var stringTime = group.Groups[1].ToString();
                    var z = double.Parse(stringTime);
                    finalFile.Add(finalMessages[i].originalarrivaltime.Date + "," + z / 60.0 / 60.0);
                }
            }
            File.WriteAllText(@".\out.csv", string.Join("\r\n", finalFile));
            MessageBox.Show("Done!");
        }
    }
}
