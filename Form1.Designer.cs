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
            this.PageFilePath = new System.Windows.Forms.TextBox();
            this.buttonPages = new System.Windows.Forms.Button();
            this.CommentFilePath = new System.Windows.Forms.TextBox();
            this.buttonComments = new System.Windows.Forms.Button();
            this.ConsoleScreen = new System.Windows.Forms.TextBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.openXMLdialog = new System.Windows.Forms.OpenFileDialog();
            this.outputDirDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize) (this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // PageFilePath
            // 
            this.PageFilePath.Location = new System.Drawing.Point(176, 34);
            this.PageFilePath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PageFilePath.Name = "PageFilePath";
            this.PageFilePath.Size = new System.Drawing.Size(681, 22);
            this.PageFilePath.TabIndex = 0;
            this.PageFilePath.Text = "C:\\Users\\Darth Biomech\\Desktop\\";
            this.PageFilePath.TextChanged += new System.EventHandler(this.SourceXMLPath_TextChanged);
            // 
            // buttonPages
            // 
            this.buttonPages.Location = new System.Drawing.Point(39, 36);
            this.buttonPages.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonPages.Name = "buttonPages";
            this.buttonPages.Size = new System.Drawing.Size(116, 23);
            this.buttonPages.TabIndex = 1;
            this.buttonPages.Text = "PageList";
            this.buttonPages.UseVisualStyleBackColor = true;
            this.buttonPages.Click += new System.EventHandler(this.selectButton_Click);
            // 
            // CommentFilePath
            // 
            this.CommentFilePath.Location = new System.Drawing.Point(176, 90);
            this.CommentFilePath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CommentFilePath.Name = "CommentFilePath";
            this.CommentFilePath.Size = new System.Drawing.Size(681, 22);
            this.CommentFilePath.TabIndex = 2;
            this.CommentFilePath.Text = "C:\\Users\\Darth Biomech\\Desktop\\";
            // 
            // buttonComments
            // 
            this.buttonComments.Location = new System.Drawing.Point(39, 91);
            this.buttonComments.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonComments.Name = "buttonComments";
            this.buttonComments.Size = new System.Drawing.Size(116, 23);
            this.buttonComments.TabIndex = 3;
            this.buttonComments.Text = "CommentsFile";
            this.buttonComments.UseVisualStyleBackColor = true;
            this.buttonComments.Click += new System.EventHandler(this.buttonOutput_Click);
            // 
            // ConsoleScreen
            // 
            this.ConsoleScreen.AcceptsReturn = true;
            this.ConsoleScreen.AcceptsTab = true;
            this.ConsoleScreen.HideSelection = false;
            this.ConsoleScreen.Location = new System.Drawing.Point(39, 230);
            this.ConsoleScreen.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ConsoleScreen.Multiline = true;
            this.ConsoleScreen.Name = "ConsoleScreen";
            this.ConsoleScreen.ReadOnly = true;
            this.ConsoleScreen.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.ConsoleScreen.Size = new System.Drawing.Size(1221, 308);
            this.ConsoleScreen.TabIndex = 4;
            this.ConsoleScreen.WordWrap = false;
            // 
            // buttonStart
            // 
            this.buttonStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.buttonStart.Location = new System.Drawing.Point(41, 153);
            this.buttonStart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(183, 44);
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
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(428, 153);
            this.numericUpDown1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(65, 22);
            this.numericUpDown1.TabIndex = 6;
            this.numericUpDown1.Value = new decimal(new int[] {2, 0, 0, 0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(313, 155);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 22);
            this.label1.TabIndex = 7;
            this.label1.Text = "Site ID";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1293, 554);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.ConsoleScreen);
            this.Controls.Add(this.buttonComments);
            this.Controls.Add(this.CommentFilePath);
            this.Controls.Add(this.buttonPages);
            this.Controls.Add(this.PageFilePath);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize) (this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label1;

        private System.Windows.Forms.OpenFileDialog openXMLdialog;
        private System.Windows.Forms.FolderBrowserDialog outputDirDialog;

        private System.Windows.Forms.Button buttonStart;

        private System.Windows.Forms.TextBox ConsoleScreen;

        private System.Windows.Forms.TextBox CommentFilePath;
        private System.Windows.Forms.Button buttonComments;

        private System.Windows.Forms.TextBox PageFilePath;
        private System.Windows.Forms.Button buttonPages;

        #endregion
    }
}