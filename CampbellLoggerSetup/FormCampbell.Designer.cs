
using System.ComponentModel;
using System.Windows.Forms;

namespace CampbellLoggerSetup
{
    partial class FormCampbell
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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

        private Label IPAddressLabel;
        private Button addButton;
        private Label pbAddressLabel;
        private Label securityCodeLabel;
        private Label loggerTypeLabel;
        private Label tableNumLabel;
        private Label portLabel;
        private TextBox ipAddressTextBox;
        private TextBox pbAddressTextBox;
        private TextBox securityCodeTextBox;
        private TextBox tableNumTextBox;
        private TextBox portTextBox;
        private ComboBox loggerTypeComboBox;
        private Label loggerNameLabel;
        private TextBox loggerNameTextBox;
        private ListBox loggerListBox;
        private Button deleteButton;
        private Button pollButton;
        private Label dataIntervalTextLabel;
        private ComboBox dataIntervalComboBox;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem cSVSavePathToolStripMenuItem;
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.IPAddressLabel = new System.Windows.Forms.Label();
            this.addButton = new System.Windows.Forms.Button();
            this.pbAddressLabel = new System.Windows.Forms.Label();
            this.securityCodeLabel = new System.Windows.Forms.Label();
            this.loggerTypeLabel = new System.Windows.Forms.Label();
            this.tableNumLabel = new System.Windows.Forms.Label();
            this.portLabel = new System.Windows.Forms.Label();
            this.ipAddressTextBox = new System.Windows.Forms.TextBox();
            this.pbAddressTextBox = new System.Windows.Forms.TextBox();
            this.securityCodeTextBox = new System.Windows.Forms.TextBox();
            this.tableNumTextBox = new System.Windows.Forms.TextBox();
            this.portTextBox = new System.Windows.Forms.TextBox();
            this.loggerTypeComboBox = new System.Windows.Forms.ComboBox();
            this.loggerNameLabel = new System.Windows.Forms.Label();
            this.loggerNameTextBox = new System.Windows.Forms.TextBox();
            this.loggerListBox = new System.Windows.Forms.ListBox();
            this.deleteButton = new System.Windows.Forms.Button();
            this.pollButton = new System.Windows.Forms.Button();
            this.dataIntervalTextLabel = new System.Windows.Forms.Label();
            this.dataIntervalComboBox = new System.Windows.Forms.ComboBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cSVSavePathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.PollAllLoggerButton = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // IPAddressLabel
            // 
            this.IPAddressLabel.AutoSize = true;
            this.IPAddressLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IPAddressLabel.Location = new System.Drawing.Point(94, 156);
            this.IPAddressLabel.Name = "IPAddressLabel";
            this.IPAddressLabel.Size = new System.Drawing.Size(269, 58);
            this.IPAddressLabel.TabIndex = 0;
            this.IPAddressLabel.Text = "IP Address";
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(83, 778);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(280, 110);
            this.addButton.TabIndex = 1;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.AddButtonClick);
            // 
            // pbAddressLabel
            // 
            this.pbAddressLabel.AutoSize = true;
            this.pbAddressLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pbAddressLabel.Location = new System.Drawing.Point(46, 238);
            this.pbAddressLabel.Name = "pbAddressLabel";
            this.pbAddressLabel.Size = new System.Drawing.Size(389, 58);
            this.pbAddressLabel.TabIndex = 2;
            this.pbAddressLabel.Text = "Pakbus Address";
            // 
            // securityCodeLabel
            // 
            this.securityCodeLabel.AutoSize = true;
            this.securityCodeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.securityCodeLabel.Location = new System.Drawing.Point(66, 321);
            this.securityCodeLabel.Name = "securityCodeLabel";
            this.securityCodeLabel.Size = new System.Drawing.Size(339, 58);
            this.securityCodeLabel.TabIndex = 3;
            this.securityCodeLabel.Text = "Security Code";
            // 
            // loggerTypeLabel
            // 
            this.loggerTypeLabel.AutoSize = true;
            this.loggerTypeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loggerTypeLabel.Location = new System.Drawing.Point(75, 404);
            this.loggerTypeLabel.Name = "loggerTypeLabel";
            this.loggerTypeLabel.Size = new System.Drawing.Size(307, 58);
            this.loggerTypeLabel.TabIndex = 4;
            this.loggerTypeLabel.Text = "Logger Type";
            // 
            // tableNumLabel
            // 
            this.tableNumLabel.AutoSize = true;
            this.tableNumLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableNumLabel.Location = new System.Drawing.Point(75, 488);
            this.tableNumLabel.Name = "tableNumLabel";
            this.tableNumLabel.Size = new System.Drawing.Size(343, 58);
            this.tableNumLabel.TabIndex = 5;
            this.tableNumLabel.Text = "Table Number";
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.portLabel.Location = new System.Drawing.Point(166, 570);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(117, 58);
            this.portLabel.TabIndex = 6;
            this.portLabel.Text = "Port";
            // 
            // ipAddressTextBox
            // 
            this.ipAddressTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.ipAddressTextBox.Location = new System.Drawing.Point(514, 153);
            this.ipAddressTextBox.Name = "ipAddressTextBox";
            this.ipAddressTextBox.Size = new System.Drawing.Size(354, 53);
            this.ipAddressTextBox.TabIndex = 7;
            this.ipAddressTextBox.Text = "0.0.0.0";
            // 
            // pbAddressTextBox
            // 
            this.pbAddressTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.pbAddressTextBox.Location = new System.Drawing.Point(514, 235);
            this.pbAddressTextBox.Name = "pbAddressTextBox";
            this.pbAddressTextBox.Size = new System.Drawing.Size(354, 53);
            this.pbAddressTextBox.TabIndex = 8;
            this.pbAddressTextBox.Text = "1";
            // 
            // securityCodeTextBox
            // 
            this.securityCodeTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.securityCodeTextBox.Location = new System.Drawing.Point(514, 318);
            this.securityCodeTextBox.Name = "securityCodeTextBox";
            this.securityCodeTextBox.Size = new System.Drawing.Size(354, 53);
            this.securityCodeTextBox.TabIndex = 9;
            this.securityCodeTextBox.Text = "0000";
            // 
            // tableNumTextBox
            // 
            this.tableNumTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.tableNumTextBox.Location = new System.Drawing.Point(514, 485);
            this.tableNumTextBox.Name = "tableNumTextBox";
            this.tableNumTextBox.Size = new System.Drawing.Size(354, 53);
            this.tableNumTextBox.TabIndex = 10;
            this.tableNumTextBox.Text = "1";
            // 
            // portTextBox
            // 
            this.portTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.portTextBox.Location = new System.Drawing.Point(514, 567);
            this.portTextBox.Name = "portTextBox";
            this.portTextBox.Size = new System.Drawing.Size(354, 53);
            this.portTextBox.TabIndex = 11;
            this.portTextBox.Text = "6785";
            // 
            // loggerTypeComboBox
            // 
            this.loggerTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.loggerTypeComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.loggerTypeComboBox.FormattingEnabled = true;
            this.loggerTypeComboBox.Items.AddRange(new object[] {
            "CR200",
            "CR10XPB",
            "CR1000",
            "CR3000",
            "CR800",
            "CR6",
            "CR300",
            "CR1000X"});
            this.loggerTypeComboBox.Location = new System.Drawing.Point(514, 401);
            this.loggerTypeComboBox.Name = "loggerTypeComboBox";
            this.loggerTypeComboBox.Size = new System.Drawing.Size(353, 54);
            this.loggerTypeComboBox.TabIndex = 12;
            // 
            // loggerNameLabel
            // 
            this.loggerNameLabel.AutoSize = true;
            this.loggerNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loggerNameLabel.Location = new System.Drawing.Point(76, 75);
            this.loggerNameLabel.Name = "loggerNameLabel";
            this.loggerNameLabel.Size = new System.Drawing.Size(329, 58);
            this.loggerNameLabel.TabIndex = 13;
            this.loggerNameLabel.Text = "Logger Name";
            // 
            // loggerNameTextBox
            // 
            this.loggerNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.loggerNameTextBox.Location = new System.Drawing.Point(513, 72);
            this.loggerNameTextBox.Name = "loggerNameTextBox";
            this.loggerNameTextBox.Size = new System.Drawing.Size(354, 53);
            this.loggerNameTextBox.TabIndex = 14;
            // 
            // loggerListBox
            // 
            this.loggerListBox.FormattingEnabled = true;
            this.loggerListBox.ItemHeight = 31;
            this.loggerListBox.Location = new System.Drawing.Point(1032, 72);
            this.loggerListBox.Name = "loggerListBox";
            this.loggerListBox.Size = new System.Drawing.Size(413, 562);
            this.loggerListBox.TabIndex = 15;
            this.loggerListBox.DoubleClick += new System.EventHandler(this.ListBox1DoubleClick);
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(1138, 778);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(413, 110);
            this.deleteButton.TabIndex = 16;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButtonClick);
            // 
            // pollButton
            // 
            this.pollButton.Location = new System.Drawing.Point(439, 778);
            this.pollButton.Name = "pollButton";
            this.pollButton.Size = new System.Drawing.Size(285, 110);
            this.pollButton.TabIndex = 17;
            this.pollButton.Text = "Poll Logger";
            this.pollButton.UseVisualStyleBackColor = true;
            this.pollButton.Click += new System.EventHandler(this.PollClick);
            // 
            // dataIntervalTextLabel
            // 
            this.dataIntervalTextLabel.AutoSize = true;
            this.dataIntervalTextLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataIntervalTextLabel.Location = new System.Drawing.Point(76, 659);
            this.dataIntervalTextLabel.Name = "dataIntervalTextLabel";
            this.dataIntervalTextLabel.Size = new System.Drawing.Size(309, 58);
            this.dataIntervalTextLabel.TabIndex = 19;
            this.dataIntervalTextLabel.Text = "Data Interval";
            // 
            // dataIntervalComboBox
            // 
            this.dataIntervalComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dataIntervalComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dataIntervalComboBox.FormattingEnabled = true;
            this.dataIntervalComboBox.Items.AddRange(new object[] {
            "1 hour intervals",
            "1 minute intervals"});
            this.dataIntervalComboBox.Location = new System.Drawing.Point(513, 659);
            this.dataIntervalComboBox.Name = "dataIntervalComboBox";
            this.dataIntervalComboBox.Size = new System.Drawing.Size(354, 54);
            this.dataIntervalComboBox.TabIndex = 20;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1650, 49);
            this.menuStrip1.TabIndex = 21;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cSVSavePathToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(75, 45);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // cSVSavePathToolStripMenuItem
            // 
            this.cSVSavePathToolStripMenuItem.Name = "cSVSavePathToolStripMenuItem";
            this.cSVSavePathToolStripMenuItem.Size = new System.Drawing.Size(370, 46);
            this.cSVSavePathToolStripMenuItem.Text = "Set CSV save path";
            this.cSVSavePathToolStripMenuItem.Click += new System.EventHandler(this.CsvSavePathToolStripMenuItemClick);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(1032, 649);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(413, 49);
            this.progressBar1.TabIndex = 22;
            // 
            // PollAllLoggerButton
            // 
            this.PollAllLoggerButton.Location = new System.Drawing.Point(795, 778);
            this.PollAllLoggerButton.Name = "PollAllLoggerButton";
            this.PollAllLoggerButton.Size = new System.Drawing.Size(285, 110);
            this.PollAllLoggerButton.TabIndex = 23;
            this.PollAllLoggerButton.Text = "Poll All Loggers";
            this.PollAllLoggerButton.UseVisualStyleBackColor = true;
            this.PollAllLoggerButton.Click += new System.EventHandler(this.PollAllLoggersButtonClick);
            // 
            // FormCampbell
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1650, 984);
            this.Controls.Add(this.PollAllLoggerButton);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.dataIntervalComboBox);
            this.Controls.Add(this.dataIntervalTextLabel);
            this.Controls.Add(this.pollButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.loggerListBox);
            this.Controls.Add(this.loggerNameTextBox);
            this.Controls.Add(this.loggerNameLabel);
            this.Controls.Add(this.loggerTypeComboBox);
            this.Controls.Add(this.portTextBox);
            this.Controls.Add(this.tableNumTextBox);
            this.Controls.Add(this.securityCodeTextBox);
            this.Controls.Add(this.pbAddressTextBox);
            this.Controls.Add(this.ipAddressTextBox);
            this.Controls.Add(this.portLabel);
            this.Controls.Add(this.tableNumLabel);
            this.Controls.Add(this.loggerTypeLabel);
            this.Controls.Add(this.securityCodeLabel);
            this.Controls.Add(this.pbAddressLabel);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.IPAddressLabel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormCampbell";
            this.Text = "Campbell Logger Configuring Tool";
            this.Load += new System.EventHandler(this.FormCampbellLoad);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ProgressBar progressBar1;
        private Button PollAllLoggerButton;
    }
}