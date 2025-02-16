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
            ConsoleScreen.Text = "";
            progressBar.Value = 0;
            buttonStart.Enabled = false;
            Program.thisProgram.gui = this;
            DisqusSourceFilePath.Text = Program.thisProgram.inputFile;
            CommentPageLookupFilePath.Text = Program.thisProgram.pagesLookupFile;
            checkBoxDeleteComments.Checked = Program.thisProgram.skipDeletedPosts;
            inputNumOfUsers.Value = Program.thisProgram.numOfUsers;
            inputNumberOfComments.Value = Program.thisProgram.numOfComments;
            inputDefaultEmail.Text = Program.thisProgram.defaultEmail;
            TryEnableStartButton();
            Log("--------------------------------------------------------------------------",false);
            Log("--------------------------------------------------------------------------",false);
            Log("---------DO NOT FORGET TO MAKE FULL BACKUPS BEFORE DOING ANYTHING---------",false);
            Log("--------------------------------------------------------------------------",false);
            Log("--------------------------------------------------------------------------");
            Log("Ready to start converting.");
        }

        private void TryEnableStartButton()
        {
            if (DisqusSourceFilePath.Text.Contains(".txt") || DisqusSourceFilePath.Text.Contains(".xml"))
            {
                if (CommentPageLookupFilePath.Text.Contains(".txt") || CommentPageLookupFilePath.Text.Contains(".sql"))
                {
                    buttonStart.Enabled = true;
                }
            }
        }


        private void deleteComments_CheckedChanged(object sender, EventArgs e)
        {
            Program.thisProgram.skipDeletedPosts = checkBoxDeleteComments.Checked;
        }

        private void inputNumOfUsers_ValueChanged(object sender, EventArgs e)
        {
            Program.thisProgram.numOfUsers = (int)Math.Round(inputNumOfUsers.Value);
        }

        private void inputNumberOfComments_ValueChanged(object sender, EventArgs e)
        {
            Program.thisProgram.numOfComments = (int)Math.Round(inputNumberOfComments.Value);
        }

        private void inputDefaultEmail_TextChanged(object sender, EventArgs e)
        {
            Program.thisProgram.defaultEmail = inputDefaultEmail.Text;
        }
        public string GetLog()=>ConsoleScreen.Text;
        public void Log(string text, bool newline = true)
        {
            ConsoleScreen.AppendText(text);
            ConsoleScreen.AppendText(Environment.NewLine);
            if(newline)
                ConsoleScreen.AppendText(Environment.NewLine);
        }
        private void SourceXMLPath_TextChanged(object sender, EventArgs e)
        {
            if (!DisqusSourceFilePath.Text.Contains(".txt") && !DisqusSourceFilePath.Text.Contains(".xml"))
                return;
            Program.SetInputFile(DisqusSourceFilePath.Text);
        }
        private void buttonClickLoadDisqusXML(object sender, EventArgs e)
        {
            openXMLdialog.Filter = @"discus backup (*.xml)|*.xml|discus dump (*.txt)|*.txt";
            openXMLdialog.FilterIndex = 1;
            openXMLdialog.RestoreDirectory = true;
            if (DisqusSourceFilePath.Text != string.Empty)
            {
                openXMLdialog.InitialDirectory = DisqusSourceFilePath.Text;
            }
            else
            {
                openXMLdialog.InitialDirectory = Application.StartupPath;
            }
            if (openXMLdialog.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                DisqusSourceFilePath.Text = openXMLdialog.FileName;
                Program.SetInputFile(DisqusSourceFilePath.Text);
                
                TryEnableStartButton();
                Log("Selected Disqus backup file.");
            }
        }

        private void PageLookupFilePath_TextChanged(object sender, EventArgs e)
        {
            if (!CommentPageLookupFilePath.Text.Contains(".txt") && !CommentPageLookupFilePath.Text.Contains(".sql"))
                return;
            Program.SetOutputFile(CommentPageLookupFilePath.Text);
        }
        private void buttonClickLoadPagesSQLFile(object sender, EventArgs e)
        {
            openXMLdialog.Filter = @"mysql file (*.sql)|*.sql|(*.txt)|*.txt";
            openXMLdialog.FilterIndex = 1;
            openXMLdialog.RestoreDirectory = true;
            if (CommentPageLookupFilePath.Text != string.Empty)
            {
                openXMLdialog.InitialDirectory = CommentPageLookupFilePath.Text;
            }
            else
            {
                openXMLdialog.InitialDirectory = Application.StartupPath;
            }
            if (openXMLdialog.ShowDialog() == DialogResult.OK)
            {
                CommentPageLookupFilePath.Text = openXMLdialog.FileName;
                Program.SetOutputFile(CommentPageLookupFilePath.Text);
                TryEnableStartButton();
                Log("Selected Commentics Page SQL File.");
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            Program.thisProgram.SaveSettings();
            Program.Begin();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.thisProgram.SaveSettings();
        }

    }
}