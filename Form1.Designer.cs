namespace DB_Disqus_convert
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.DisqusSourceFilePath = new System.Windows.Forms.TextBox();
            this.buttonDisqusXMLFile = new System.Windows.Forms.Button();
            this.buttonSelectComPageLookup = new System.Windows.Forms.Button();
            this.ConsoleScreen = new System.Windows.Forms.TextBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.openXMLdialog = new System.Windows.Forms.OpenFileDialog();
            this.outputDirDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.CommentPageLookupFilePath = new System.Windows.Forms.TextBox();
            this.checkBoxDeleteComments = new System.Windows.Forms.CheckBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.inputNumOfUsers = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.inputNumberOfComments = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.inputDefaultEmail = new System.Windows.Forms.TextBox();
            this.GroupBoxSettings = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize) (this.inputNumOfUsers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.inputNumberOfComments)).BeginInit();
            this.GroupBoxSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // DisqusSourceFilePath
            // 
            this.DisqusSourceFilePath.Location = new System.Drawing.Point(132, 39);
            this.DisqusSourceFilePath.Name = "DisqusSourceFilePath";
            this.DisqusSourceFilePath.Size = new System.Drawing.Size(296, 20);
            this.DisqusSourceFilePath.TabIndex = 0;
            this.DisqusSourceFilePath.Text = "C:\\Users\\Darth Biomech\\Desktop\\";
            this.DisqusSourceFilePath.TextChanged += new System.EventHandler(this.SourceXMLPath_TextChanged);
            // 
            // buttonDisqusXMLFile
            // 
            this.buttonDisqusXMLFile.Location = new System.Drawing.Point(29, 39);
            this.buttonDisqusXMLFile.Name = "buttonDisqusXMLFile";
            this.buttonDisqusXMLFile.Size = new System.Drawing.Size(87, 20);
            this.buttonDisqusXMLFile.TabIndex = 1;
            this.buttonDisqusXMLFile.Text = "Disqus xml:";
            this.buttonDisqusXMLFile.UseVisualStyleBackColor = true;
            this.buttonDisqusXMLFile.Click += new System.EventHandler(this.buttonClickLoadDisqusXML);
            // 
            // buttonSelectComPageLookup
            // 
            this.buttonSelectComPageLookup.Location = new System.Drawing.Point(29, 116);
            this.buttonSelectComPageLookup.Name = "buttonSelectComPageLookup";
            this.buttonSelectComPageLookup.Size = new System.Drawing.Size(87, 20);
            this.buttonSelectComPageLookup.TabIndex = 3;
            this.buttonSelectComPageLookup.Text = "Pages sql:";
            this.buttonSelectComPageLookup.UseVisualStyleBackColor = true;
            this.buttonSelectComPageLookup.Click += new System.EventHandler(this.buttonClickLoadPagesSQLFile);
            // 
            // ConsoleScreen
            // 
            this.ConsoleScreen.AcceptsReturn = true;
            this.ConsoleScreen.AcceptsTab = true;
            this.ConsoleScreen.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.ConsoleScreen.HideSelection = false;
            this.ConsoleScreen.Location = new System.Drawing.Point(29, 192);
            this.ConsoleScreen.Multiline = true;
            this.ConsoleScreen.Name = "ConsoleScreen";
            this.ConsoleScreen.ReadOnly = true;
            this.ConsoleScreen.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.ConsoleScreen.Size = new System.Drawing.Size(779, 246);
            this.ConsoleScreen.TabIndex = 4;
            this.ConsoleScreen.Text = resources.GetString("ConsoleScreen.Text");
            this.ConsoleScreen.WordWrap = false;
            // 
            // buttonStart
            // 
            this.buttonStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.buttonStart.Location = new System.Drawing.Point(29, 145);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(137, 36);
            this.buttonStart.TabIndex = 5;
            this.buttonStart.Text = "GO";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // openXMLdialog
            // 
            this.openXMLdialog.DefaultExt = "xml";
            this.openXMLdialog.FileName = "openXMLdialog";
            this.openXMLdialog.InitialDirectory = "c:\\";
            // 
            // CommentPageLookupFilePath
            // 
            this.CommentPageLookupFilePath.Location = new System.Drawing.Point(132, 116);
            this.CommentPageLookupFilePath.Name = "CommentPageLookupFilePath";
            this.CommentPageLookupFilePath.Size = new System.Drawing.Size(296, 20);
            this.CommentPageLookupFilePath.TabIndex = 6;
            this.CommentPageLookupFilePath.Text = "C:\\Users\\Darth Biomech\\Desktop\\";
            this.CommentPageLookupFilePath.TextChanged += new System.EventHandler(this.PageLookupFilePath_TextChanged);
            // 
            // checkBoxDeleteComments
            // 
            this.checkBoxDeleteComments.Location = new System.Drawing.Point(149, 9);
            this.checkBoxDeleteComments.Name = "checkBoxDeleteComments";
            this.checkBoxDeleteComments.Size = new System.Drawing.Size(201, 27);
            this.checkBoxDeleteComments.TabIndex = 7;
            this.checkBoxDeleteComments.Text = "Do not carry over deleted comments";
            this.checkBoxDeleteComments.UseVisualStyleBackColor = true;
            this.checkBoxDeleteComments.CheckedChanged += new System.EventHandler(this.deleteComments_CheckedChanged);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(180, 152);
            this.progressBar.MarqueeAnimationSpeed = 10;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(600, 22);
            this.progressBar.TabIndex = 8;
            this.progressBar.Value = 50;
            // 
            // inputNumOfUsers
            // 
            this.inputNumOfUsers.Location = new System.Drawing.Point(120, 40);
            this.inputNumOfUsers.Maximum = new decimal(new int[] {1000, 0, 0, 0});
            this.inputNumOfUsers.Name = "inputNumOfUsers";
            this.inputNumOfUsers.Size = new System.Drawing.Size(41, 20);
            this.inputNumOfUsers.TabIndex = 9;
            this.inputNumOfUsers.Value = new decimal(new int[] {999, 0, 0, 0});
            this.inputNumOfUsers.ValueChanged += new System.EventHandler(this.inputNumOfUsers_ValueChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(29, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(399, 15);
            this.label1.TabIndex = 10;
            this.label1.Text = "Disqus backup XML file goes here:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(29, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(399, 30);
            this.label2.TabIndex = 11;
            this.label2.Text = "Exported SQL of table \'Pages\' from your Commentics DB goes here: \r\n(needed only t" + "o match page URLs, no need to reupload it back to DB)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // inputNumberOfComments
            // 
            this.inputNumberOfComments.Location = new System.Drawing.Point(120, 68);
            this.inputNumberOfComments.Maximum = new decimal(new int[] {1000, 0, 0, 0});
            this.inputNumberOfComments.Name = "inputNumberOfComments";
            this.inputNumberOfComments.Size = new System.Drawing.Size(41, 20);
            this.inputNumberOfComments.TabIndex = 12;
            this.inputNumberOfComments.Value = new decimal(new int[] {999, 0, 0, 0});
            this.inputNumberOfComments.ValueChanged += new System.EventHandler(this.inputNumberOfComments_ValueChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(165, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(207, 15);
            this.label3.TabIndex = 13;
            this.label3.Text = "How many users registered in DB already?";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(165, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(207, 15);
            this.label4.TabIndex = 14;
            this.label4.Text = "How many comments exist in DB already?";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(166, 98);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(161, 15);
            this.label5.TabIndex = 15;
            this.label5.Text = "Plug E-mail for Disqus users";
            // 
            // inputDefaultEmail
            // 
            this.inputDefaultEmail.Location = new System.Drawing.Point(7, 95);
            this.inputDefaultEmail.Name = "inputDefaultEmail";
            this.inputDefaultEmail.Size = new System.Drawing.Size(154, 20);
            this.inputDefaultEmail.TabIndex = 16;
            this.inputDefaultEmail.Text = "someAsshole@gmail.com";
            this.inputDefaultEmail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.inputDefaultEmail.TextChanged += new System.EventHandler(this.inputDefaultEmail_TextChanged);
            // 
            // GroupBoxSettings
            // 
            this.GroupBoxSettings.Controls.Add(this.inputDefaultEmail);
            this.GroupBoxSettings.Controls.Add(this.label5);
            this.GroupBoxSettings.Controls.Add(this.label4);
            this.GroupBoxSettings.Controls.Add(this.checkBoxDeleteComments);
            this.GroupBoxSettings.Controls.Add(this.label3);
            this.GroupBoxSettings.Controls.Add(this.inputNumberOfComments);
            this.GroupBoxSettings.Controls.Add(this.inputNumOfUsers);
            this.GroupBoxSettings.Location = new System.Drawing.Point(434, 12);
            this.GroupBoxSettings.Name = "GroupBoxSettings";
            this.GroupBoxSettings.Size = new System.Drawing.Size(374, 126);
            this.GroupBoxSettings.TabIndex = 17;
            this.GroupBoxSettings.TabStop = false;
            this.GroupBoxSettings.Text = "Settings";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(820, 450);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.CommentPageLookupFilePath);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.ConsoleScreen);
            this.Controls.Add(this.buttonSelectComPageLookup);
            this.Controls.Add(this.buttonDisqusXMLFile);
            this.Controls.Add(this.DisqusSourceFilePath);
            this.Controls.Add(this.GroupBoxSettings);
            this.Location = new System.Drawing.Point(15, 15);
            this.Name = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize) (this.inputNumOfUsers)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.inputNumberOfComments)).EndInit();
            this.GroupBoxSettings.ResumeLayout(false);
            this.GroupBoxSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox inputDefaultEmail;
        private System.Windows.Forms.GroupBox GroupBoxSettings;

        private System.Windows.Forms.NumericUpDown inputNumOfUsers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown inputNumberOfComments;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;

        private System.Windows.Forms.CheckBox checkBoxDeleteComments;
        public System.Windows.Forms.ProgressBar progressBar;

        private System.Windows.Forms.TextBox CommentPageLookupFilePath;

        private System.Windows.Forms.OpenFileDialog openXMLdialog;
        private System.Windows.Forms.FolderBrowserDialog outputDirDialog;

        private System.Windows.Forms.Button buttonStart;

        private System.Windows.Forms.TextBox ConsoleScreen;

        private System.Windows.Forms.Button buttonSelectComPageLookup;

        private System.Windows.Forms.TextBox DisqusSourceFilePath;
        private System.Windows.Forms.Button buttonDisqusXMLFile;

        #endregion
    }
}