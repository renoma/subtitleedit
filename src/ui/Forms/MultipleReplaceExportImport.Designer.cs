namespace Nikse.SubtitleEdit.Forms
{
    sealed partial class MultipleReplaceExportImport
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
            this.labelGroups = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.listViewExportStyles = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.saveFileDialogStyle = new System.Windows.Forms.SaveFileDialog();
            this.contextMenuStripSelect = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inverseSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripSelect.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelGroups
            // 
            this.labelGroups.AutoSize = true;
            this.labelGroups.Location = new System.Drawing.Point(13, 14);
            this.labelGroups.Name = "labelGroups";
            this.labelGroups.Size = new System.Drawing.Size(41, 13);
            this.labelGroups.TabIndex = 10;
            this.labelGroups.Text = "Groups";
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonOK.Location = new System.Drawing.Point(138, 267);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 8;
            this.buttonOK.Text = "&OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonCancel.Location = new System.Drawing.Point(219, 267);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 9;
            this.buttonCancel.Text = "C&ancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // listViewExportStyles
            // 
            this.listViewExportStyles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewExportStyles.CheckBoxes = true;
            this.listViewExportStyles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listViewExportStyles.ContextMenuStrip = this.contextMenuStripSelect;
            this.listViewExportStyles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewExportStyles.HideSelection = false;
            this.listViewExportStyles.Location = new System.Drawing.Point(13, 32);
            this.listViewExportStyles.Name = "listViewExportStyles";
            this.listViewExportStyles.Size = new System.Drawing.Size(281, 229);
            this.listViewExportStyles.TabIndex = 7;
            this.listViewExportStyles.UseCompatibleStateImageBehavior = false;
            this.listViewExportStyles.View = System.Windows.Forms.View.Details;
            // 
            // saveFileDialogStyle
            // 
            this.saveFileDialogStyle.OverwritePrompt = false;
            // 
            // contextMenuStripSelect
            // 
            this.contextMenuStripSelect.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllToolStripMenuItem,
            this.inverseSelectionToolStripMenuItem});
            this.contextMenuStripSelect.Name = "contextMenuStripSelect";
            this.contextMenuStripSelect.Size = new System.Drawing.Size(181, 70);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.selectAllToolStripMenuItem.Text = "Select all";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // inverseSelectionToolStripMenuItem
            // 
            this.inverseSelectionToolStripMenuItem.Name = "inverseSelectionToolStripMenuItem";
            this.inverseSelectionToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.inverseSelectionToolStripMenuItem.Text = "Inverse selection";
            this.inverseSelectionToolStripMenuItem.Click += new System.EventHandler(this.inverseSelectionToolStripMenuItem_Click);
            // 
            // MultipleReplaceExportImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 300);
            this.Controls.Add(this.labelGroups);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.listViewExportStyles);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MultipleReplaceExportImport";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MultipleReplaceExportImport";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MultipleReplaceExportImport_KeyDown);
            this.contextMenuStripSelect.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelGroups;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ListView listViewExportStyles;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.SaveFileDialog saveFileDialogStyle;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripSelect;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inverseSelectionToolStripMenuItem;
    }
}