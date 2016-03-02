using ANDREICSLIB.ClassReplacements;

namespace Crossword_Puzzle_Solver
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.solvedWordsCanOnlyBeUsedOnceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.createbutton = new System.Windows.Forms.Button();
            this.createheightTB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.createwidthTB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.clearbutton = new System.Windows.Forms.Button();
            this.savebutton = new System.Windows.Forms.Button();
            this.loadwordgrid = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grid = new ANDREICSLIB.ClassReplacements.PanelReplacement();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.solveB = new System.Windows.Forms.Button();
            this.GridLetterContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toggleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.randomiseB = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.GridLetterContext.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Silver;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(440, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.solvedWordsCanOnlyBeUsedOnceToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // solvedWordsCanOnlyBeUsedOnceToolStripMenuItem
            // 
            this.solvedWordsCanOnlyBeUsedOnceToolStripMenuItem.Checked = true;
            this.solvedWordsCanOnlyBeUsedOnceToolStripMenuItem.CheckOnClick = true;
            this.solvedWordsCanOnlyBeUsedOnceToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.solvedWordsCanOnlyBeUsedOnceToolStripMenuItem.Name = "solvedWordsCanOnlyBeUsedOnceToolStripMenuItem";
            this.solvedWordsCanOnlyBeUsedOnceToolStripMenuItem.Size = new System.Drawing.Size(302, 22);
            this.solvedWordsCanOnlyBeUsedOnceToolStripMenuItem.Text = "Words can only be used once when solving";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.randomiseB);
            this.groupBox1.Controls.Add(this.createbutton);
            this.groupBox1.Controls.Add(this.createheightTB);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.createwidthTB);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.clearbutton);
            this.groupBox1.Controls.Add(this.savebutton);
            this.groupBox1.Controls.Add(this.loadwordgrid);
            this.groupBox1.Location = new System.Drawing.Point(12, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(422, 81);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Step 1: Load or create word grid";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // createbutton
            // 
            this.createbutton.Location = new System.Drawing.Point(262, 15);
            this.createbutton.Name = "createbutton";
            this.createbutton.Size = new System.Drawing.Size(122, 23);
            this.createbutton.TabIndex = 8;
            this.createbutton.Text = "Create New Grid";
            this.createbutton.UseVisualStyleBackColor = true;
            this.createbutton.Click += new System.EventHandler(this.createbutton_Click);
            // 
            // createheightTB
            // 
            this.createheightTB.Location = new System.Drawing.Point(212, 17);
            this.createheightTB.Name = "createheightTB";
            this.createheightTB.Size = new System.Drawing.Size(44, 20);
            this.createheightTB.TabIndex = 7;
            this.createheightTB.Text = "7";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(134, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Create Height:";
            // 
            // createwidthTB
            // 
            this.createwidthTB.Location = new System.Drawing.Point(84, 17);
            this.createwidthTB.Name = "createwidthTB";
            this.createwidthTB.Size = new System.Drawing.Size(44, 20);
            this.createwidthTB.TabIndex = 5;
            this.createwidthTB.Text = "7";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Create Width:";
            // 
            // clearbutton
            // 
            this.clearbutton.Location = new System.Drawing.Point(185, 48);
            this.clearbutton.Name = "clearbutton";
            this.clearbutton.Size = new System.Drawing.Size(75, 23);
            this.clearbutton.TabIndex = 3;
            this.clearbutton.Text = "Clear";
            this.clearbutton.UseVisualStyleBackColor = true;
            this.clearbutton.Click += new System.EventHandler(this.clearbutton_Click);
            // 
            // savebutton
            // 
            this.savebutton.Location = new System.Drawing.Point(99, 48);
            this.savebutton.Name = "savebutton";
            this.savebutton.Size = new System.Drawing.Size(80, 23);
            this.savebutton.TabIndex = 2;
            this.savebutton.Text = "Save To File";
            this.savebutton.UseVisualStyleBackColor = true;
            this.savebutton.Click += new System.EventHandler(this.savebutton_Click);
            // 
            // loadwordgrid
            // 
            this.loadwordgrid.Location = new System.Drawing.Point(6, 48);
            this.loadwordgrid.Name = "loadwordgrid";
            this.loadwordgrid.Size = new System.Drawing.Size(87, 23);
            this.loadwordgrid.TabIndex = 1;
            this.loadwordgrid.Text = "Load From File";
            this.loadwordgrid.UseVisualStyleBackColor = true;
            this.loadwordgrid.Click += new System.EventHandler(this.loadwordgrid_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.grid);
            this.panel1.Location = new System.Drawing.Point(12, 168);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(422, 335);
            this.panel1.TabIndex = 16;
            // 
            // grid
            // 
            this.grid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grid.BorderColour = System.Drawing.Color.Black;
            this.grid.BorderWidth = 0;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.Location = new System.Drawing.Point(0, 0);
            this.grid.Name = "grid";
            this.grid.Size = new System.Drawing.Size(422, 335);
            this.grid.TabIndex = 15;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.solveB);
            this.groupBox2.Location = new System.Drawing.Point(12, 114);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(422, 48);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Step 2:Solve";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // solveB
            // 
            this.solveB.Location = new System.Drawing.Point(6, 19);
            this.solveB.Name = "solveB";
            this.solveB.Size = new System.Drawing.Size(75, 23);
            this.solveB.TabIndex = 0;
            this.solveB.Text = "Solve";
            this.solveB.UseVisualStyleBackColor = true;
            this.solveB.Click += new System.EventHandler(this.button1_Click);
            // 
            // GridLetterContext
            // 
            this.GridLetterContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toggleToolStripMenuItem});
            this.GridLetterContext.Name = "GridLetterContext";
            this.GridLetterContext.Size = new System.Drawing.Size(112, 26);
            // 
            // toggleToolStripMenuItem
            // 
            this.toggleToolStripMenuItem.Name = "toggleToolStripMenuItem";
            this.toggleToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.toggleToolStripMenuItem.Text = "Toggle";
            this.toggleToolStripMenuItem.Click += new System.EventHandler(this.toggleToolStripMenuItem_Click);
            // 
            // randomiseB
            // 
            this.randomiseB.Location = new System.Drawing.Point(266, 48);
            this.randomiseB.Name = "randomiseB";
            this.randomiseB.Size = new System.Drawing.Size(75, 23);
            this.randomiseB.TabIndex = 9;
            this.randomiseB.Text = "Randomise";
            this.randomiseB.UseVisualStyleBackColor = true;
            this.randomiseB.Click += new System.EventHandler(this.randomiseB_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 515);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.GridLetterContext.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button createbutton;
        private System.Windows.Forms.TextBox createheightTB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox createwidthTB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button clearbutton;
        private System.Windows.Forms.Button savebutton;
        private System.Windows.Forms.Button loadwordgrid;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button solveB;
        private PanelReplacement grid;
        public System.Windows.Forms.ContextMenuStrip GridLetterContext;
        private System.Windows.Forms.ToolStripMenuItem toggleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem solvedWordsCanOnlyBeUsedOnceToolStripMenuItem;
        private System.Windows.Forms.Button randomiseB;
    }
}

