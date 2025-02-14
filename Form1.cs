using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DB_Disqus_convert
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            buttonStart.Enabled = false;
            Program.thisProgram.gui = this;
            numericUpDown1.Value = Program.thisProgram.siteID;
            CommentFilePath.Text = Program.thisProgram.commentFile;
            PageFilePath.Text = Program.thisProgram.pagefile;
        }

        public void Log(string text)
        {
            ConsoleScreen.AppendText(text);
            ConsoleScreen.AppendText(Environment.NewLine);
        }
        private void SourceXMLPath_TextChanged(object sender, EventArgs e)
        {
            if (CommentFilePath.Text == "" || CommentFilePath.Text == @"C:\")
            {
              //  CommentFilePath.Text = PageFilePath.Text;
            }
        }

        private void selectButton_Click(object sender, EventArgs e)
        {
            openXMLdialog.Filter = @"txt files (*.txt)|*.txt";
            openXMLdialog.FilterIndex = 1;
            openXMLdialog.RestoreDirectory = true;
            if (PageFilePath.Text != string.Empty)
            {
                openXMLdialog.InitialDirectory = PageFilePath.Text;
            }
            else
            {
                openXMLdialog.InitialDirectory = @"c:\";
            }
            if (openXMLdialog.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                PageFilePath.Text = openXMLdialog.FileName;
                
                Program.LoadPageFile(PageFilePath.Text);
                buttonStart.Enabled = true;
                Log("Selected Page file.");
            }
        }

        private void buttonOutput_Click(object sender, EventArgs e)
        {
            openXMLdialog.Filter = @"txt files (*.txt)|*.txt";
            openXMLdialog.FilterIndex = 1;
            openXMLdialog.RestoreDirectory = true;
            if (CommentFilePath.Text != string.Empty)
            {
                openXMLdialog.InitialDirectory = CommentFilePath.Text;
            }
            else
            {
                openXMLdialog.InitialDirectory = @"c:\";
            }
            if (openXMLdialog.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                CommentFilePath.Text = openXMLdialog.FileName;
                
                Program.LoadCommentFile(CommentFilePath.Text);
                buttonStart.Enabled = true;
                Log("Selected Comment File.");
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            Program.thisProgram.SaveSettings();
            Program.Begin();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Program.thisProgram.siteID = (int) numericUpDown1.Value;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.thisProgram.SaveSettings();
        }
    }
}