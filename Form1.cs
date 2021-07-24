using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace Notepad
{
    public partial class NotePad : Form
    {
        string CurrentFile;
        string ProjectName;
        bool modified = false;
        public NotePad()
        {
            InitializeComponent();
            Timer tmr = new Timer();
            tmr.Interval = 150;
            tmr.Tick += Update;
            tmr.Start();
            CurrentFile = null;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        //Font Button
        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Font = new Font(fontDialog.Font.Name, fontDialog.Font.Size, fontDialog.Font.Style);
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        private void Update(object sender, EventArgs e)
        {
            Console.WriteLine("currentfile: " + CurrentFile + "\n" + "projectname: " + ProjectName + "\n" + "Name: " + Text);
            if (Clipboard.ContainsText())
            {
                PasteButton.Enabled = true;
            }
            else
            {
                PasteButton.Enabled = false;
            }

            if (CurrentFile == "" || CurrentFile == null)
            {
                ProjectName = "Untitled";
            }
            else
            {
                ProjectName = CurrentFile;
            }

            if (textBox1.Text == "")
            {
                modified = false;
            }
            else
            {
                modified = true;
            }
            if (modified)
            {
                Text = ProjectName + "*" + " - " + "Notepad";
            }
            else
            {
                Text = ProjectName + " - " + "Notepad";
            }
        }

        private void PasteButton_Click(object sender, EventArgs e)
        {
            textBox1.Text += Clipboard.GetText();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (modified)
            {
                DialogResult dialog = SaveDialog("Notepad", "Do you want to save changes to " + ProjectName + "?");
                if (dialog == DialogResult.Yes)
                {
                    WriteToFile(CurrentFile, ReadLines());
                    NewFile();
                }
                if (dialog == DialogResult.No)
                {
                    NewFile();
                }
            }
            else
            {
                CurrentFile = null;
            }
        }
        private void OpenButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Text Files|*.txt";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                CurrentFile = fileDialog.FileName;
                OpenFile(CurrentFile);
            }
        }

        public void SetFile(string[] textData)
        {
            textBox1.Text = "";
            foreach (string line in textData)
            {
                textBox1.Text += line + "\r\n";
            }
        }

        public void WriteToFile(string path, string[] data)
        {
            File.WriteAllLines(path, data);
        }
        public string[] ReadLinesFromFile(string filePath)
        {
            List<string> lines = new List<string>();
            foreach (string line in File.ReadLines(filePath))
            {
                lines.Add(line);
            }
            return lines.ToArray();
        }

        public string[] ReadLines()
        {
            string[] lines = Regex.Split(textBox1.Text, "\r\n|\r|\n");
            return lines;
        }
        public void NewFile()
        {
            textBox1.Text = "";
            CurrentFile = null;
        }
        public void OpenFile(string filePath)
        {
            string[] lines = ReadLinesFromFile(filePath);
            SetFile(lines);
        }
        public void SaveFile()
        {
            if (CurrentFile != null)
            {
                WriteToFile(CurrentFile, ReadLines());
            }
            else
            {
                SaveFileAs();
            }
        }
        public void SaveFileDirect()
        {
            SaveFileAs();
        }
        public void SaveFileAs()
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "Text Files|*.txt";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                WriteToFile(fileDialog.FileName, ReadLines());
                CurrentFile = fileDialog.FileName;
            }
        }

        private DialogResult SaveDialog(string dialogTitle, string dialogMessage)
        {
            DialogResult messageBox = MessageBox.Show(null, dialogMessage, dialogTitle, MessageBoxButtons.YesNoCancel, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
            return messageBox;
        }
        private void ShowDialog(string dialogTitle, string dialogMessage)
        {
            MessageBox.Show(dialogMessage, dialogTitle, MessageBoxButtons.OK, MessageBoxIcon.None);
        }
        private void ShowWarning(string dialogTitle, string dialogMessage)
        {
            MessageBox.Show(dialogMessage, dialogTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void ShowError(string dialogTitle, string dialogMessage)
        {
            MessageBox.Show(dialogMessage, dialogTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (CurrentFile != null)
            {
                SaveFile();
            }
            else
            {
                SaveFileAs();
                OpenFile(CurrentFile);
            }
        }

        private void SaveAsButton_Click(object sender, EventArgs e)
        {
            SaveFile();
            OpenFile(CurrentFile);
        }

        private void NotePad_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void NotePad_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (modified)
            {
                DialogResult dialog = SaveDialog("Notepad", "Do you want to save changes to " + ProjectName + "?");
                if (dialog == DialogResult.Yes)
                {
                    SaveFile();
                }
                if (dialog == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
