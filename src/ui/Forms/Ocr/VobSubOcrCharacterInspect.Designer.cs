namespace Nikse.SubtitleEdit.Forms.Ocr
{
    partial class VobSubOcrCharacterInspect
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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBoxCurrentCompareImage = new System.Windows.Forms.GroupBox();
            this.buttonAddBetterMatch = new System.Windows.Forms.Button();
            this.checkBoxItalic = new System.Windows.Forms.CheckBox();
            this.labelDoubleSize = new System.Windows.Forms.Label();
            this.pictureBoxCompareBitmapDouble = new System.Windows.Forms.PictureBox();
            this.labelTextAssociatedWithImage = new System.Windows.Forms.Label();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.labelImageInfo = new System.Windows.Forms.Label();
            this.textBoxText = new System.Windows.Forms.TextBox();
            this.contextMenuStripLetters = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pictureBoxCompareBitmap = new System.Windows.Forms.PictureBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.listBoxInspectItems = new System.Windows.Forms.ListBox();
            this.contextMenuStripAddBetterMultiMatch = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addBetterMultiMatchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBoxInspectItems = new System.Windows.Forms.GroupBox();
            this.buttonDetectFont = new System.Windows.Forms.Button();
            this.labelImageSize = new System.Windows.Forms.Label();
            this.labelExpandCount = new System.Windows.Forms.Label();
            this.pictureBoxInspectItem = new System.Windows.Forms.PictureBox();
            this.labelCount = new System.Windows.Forms.Label();
            this.groupBoxCurrentCompareImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCompareBitmapDouble)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCompareBitmap)).BeginInit();
            this.contextMenuStripAddBetterMultiMatch.SuspendLayout();
            this.groupBoxInspectItems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInspectItem)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonCancel.Location = new System.Drawing.Point(859, 695);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(100, 27);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "C&ancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // groupBoxCurrentCompareImage
            // 
            this.groupBoxCurrentCompareImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxCurrentCompareImage.Controls.Add(this.buttonAddBetterMatch);
            this.groupBoxCurrentCompareImage.Controls.Add(this.checkBoxItalic);
            this.groupBoxCurrentCompareImage.Controls.Add(this.labelDoubleSize);
            this.groupBoxCurrentCompareImage.Controls.Add(this.pictureBoxCompareBitmapDouble);
            this.groupBoxCurrentCompareImage.Controls.Add(this.labelTextAssociatedWithImage);
            this.groupBoxCurrentCompareImage.Controls.Add(this.buttonDelete);
            this.groupBoxCurrentCompareImage.Controls.Add(this.buttonUpdate);
            this.groupBoxCurrentCompareImage.Controls.Add(this.labelImageInfo);
            this.groupBoxCurrentCompareImage.Controls.Add(this.textBoxText);
            this.groupBoxCurrentCompareImage.Controls.Add(this.pictureBoxCompareBitmap);
            this.groupBoxCurrentCompareImage.Location = new System.Drawing.Point(591, 14);
            this.groupBoxCurrentCompareImage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxCurrentCompareImage.Name = "groupBoxCurrentCompareImage";
            this.groupBoxCurrentCompareImage.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxCurrentCompareImage.Size = new System.Drawing.Size(368, 674);
            this.groupBoxCurrentCompareImage.TabIndex = 1;
            this.groupBoxCurrentCompareImage.TabStop = false;
            this.groupBoxCurrentCompareImage.Text = "Current compare image";
            // 
            // buttonAddBetterMatch
            // 
            this.buttonAddBetterMatch.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonAddBetterMatch.Location = new System.Drawing.Point(160, 107);
            this.buttonAddBetterMatch.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonAddBetterMatch.Name = "buttonAddBetterMatch";
            this.buttonAddBetterMatch.Size = new System.Drawing.Size(187, 27);
            this.buttonAddBetterMatch.TabIndex = 999;
            this.buttonAddBetterMatch.Text = "Add better match";
            this.buttonAddBetterMatch.UseVisualStyleBackColor = true;
            this.buttonAddBetterMatch.Click += new System.EventHandler(this.buttonAddBetterMatch_Click);
            // 
            // checkBoxItalic
            // 
            this.checkBoxItalic.AutoSize = true;
            this.checkBoxItalic.Location = new System.Drawing.Point(20, 73);
            this.checkBoxItalic.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.checkBoxItalic.Name = "checkBoxItalic";
            this.checkBoxItalic.Size = new System.Drawing.Size(74, 19);
            this.checkBoxItalic.TabIndex = 1;
            this.checkBoxItalic.Text = "Is &italic";
            this.checkBoxItalic.UseVisualStyleBackColor = true;
            this.checkBoxItalic.CheckedChanged += new System.EventHandler(this.checkBoxItalic_CheckedChanged);
            // 
            // labelDoubleSize
            // 
            this.labelDoubleSize.AutoSize = true;
            this.labelDoubleSize.Location = new System.Drawing.Point(19, 201);
            this.labelDoubleSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDoubleSize.Name = "labelDoubleSize";
            this.labelDoubleSize.Size = new System.Drawing.Size(126, 15);
            this.labelDoubleSize.TabIndex = 999;
            this.labelDoubleSize.Text = "Image double size";
            // 
            // pictureBoxCompareBitmapDouble
            // 
            this.pictureBoxCompareBitmapDouble.BackColor = System.Drawing.Color.Red;
            this.pictureBoxCompareBitmapDouble.Location = new System.Drawing.Point(23, 219);
            this.pictureBoxCompareBitmapDouble.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBoxCompareBitmapDouble.Name = "pictureBoxCompareBitmapDouble";
            this.pictureBoxCompareBitmapDouble.Size = new System.Drawing.Size(88, 84);
            this.pictureBoxCompareBitmapDouble.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxCompareBitmapDouble.TabIndex = 27;
            this.pictureBoxCompareBitmapDouble.TabStop = false;
            // 
            // labelTextAssociatedWithImage
            // 
            this.labelTextAssociatedWithImage.AutoSize = true;
            this.labelTextAssociatedWithImage.Location = new System.Drawing.Point(19, 22);
            this.labelTextAssociatedWithImage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTextAssociatedWithImage.Name = "labelTextAssociatedWithImage";
            this.labelTextAssociatedWithImage.Size = new System.Drawing.Size(186, 15);
            this.labelTextAssociatedWithImage.TabIndex = 999;
            this.labelTextAssociatedWithImage.Text = "Text associated with image";
            // 
            // buttonDelete
            // 
            this.buttonDelete.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonDelete.Location = new System.Drawing.Point(160, 74);
            this.buttonDelete.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(187, 27);
            this.buttonDelete.TabIndex = 999;
            this.buttonDelete.Text = "Delete ";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonUpdate.Location = new System.Drawing.Point(160, 40);
            this.buttonUpdate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(187, 27);
            this.buttonUpdate.TabIndex = 999;
            this.buttonUpdate.Text = "Update";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // labelImageInfo
            // 
            this.labelImageInfo.AutoSize = true;
            this.labelImageInfo.Location = new System.Drawing.Point(16, 103);
            this.labelImageInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelImageInfo.Name = "labelImageInfo";
            this.labelImageInfo.Size = new System.Drawing.Size(99, 15);
            this.labelImageInfo.TabIndex = 999;
            this.labelImageInfo.Text = "labelImageInfo";
            // 
            // textBoxText
            // 
            this.textBoxText.ContextMenuStrip = this.contextMenuStripLetters;
            this.textBoxText.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxText.Location = new System.Drawing.Point(19, 40);
            this.textBoxText.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBoxText.Name = "textBoxText";
            this.textBoxText.Size = new System.Drawing.Size(132, 27);
            this.textBoxText.TabIndex = 0;
            this.textBoxText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxText_KeyDown);
            // 
            // contextMenuStripLetters
            // 
            this.contextMenuStripLetters.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripLetters.Name = "contextMenuStripLetters";
            this.contextMenuStripLetters.Size = new System.Drawing.Size(61, 4);
            // 
            // pictureBoxCompareBitmap
            // 
            this.pictureBoxCompareBitmap.BackColor = System.Drawing.Color.Red;
            this.pictureBoxCompareBitmap.Location = new System.Drawing.Point(23, 122);
            this.pictureBoxCompareBitmap.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBoxCompareBitmap.Name = "pictureBoxCompareBitmap";
            this.pictureBoxCompareBitmap.Size = new System.Drawing.Size(69, 60);
            this.pictureBoxCompareBitmap.TabIndex = 22;
            this.pictureBoxCompareBitmap.TabStop = false;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonOK.Location = new System.Drawing.Point(751, 695);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(100, 27);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "&OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // listBoxInspectItems
            // 
            this.listBoxInspectItems.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxInspectItems.ContextMenuStrip = this.contextMenuStripAddBetterMultiMatch;
            this.listBoxInspectItems.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxInspectItems.FormattingEnabled = true;
            this.listBoxInspectItems.ItemHeight = 17;
            this.listBoxInspectItems.Location = new System.Drawing.Point(8, 22);
            this.listBoxInspectItems.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.listBoxInspectItems.Name = "listBoxInspectItems";
            this.listBoxInspectItems.Size = new System.Drawing.Size(319, 616);
            this.listBoxInspectItems.TabIndex = 0;
            this.listBoxInspectItems.SelectedIndexChanged += new System.EventHandler(this.listBoxInspectItems_SelectedIndexChanged);
            this.listBoxInspectItems.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBoxInspectItems_KeyDown);
            // 
            // contextMenuStripAddBetterMultiMatch
            // 
            this.contextMenuStripAddBetterMultiMatch.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripAddBetterMultiMatch.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addBetterMultiMatchToolStripMenuItem});
            this.contextMenuStripAddBetterMultiMatch.Name = "contextMenuStripAddBetterMultiMatch";
            this.contextMenuStripAddBetterMultiMatch.Size = new System.Drawing.Size(239, 28);
            this.contextMenuStripAddBetterMultiMatch.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripAddBetterMultiMatch_Opening);
            // 
            // addBetterMultiMatchToolStripMenuItem
            // 
            this.addBetterMultiMatchToolStripMenuItem.Name = "addBetterMultiMatchToolStripMenuItem";
            this.addBetterMultiMatchToolStripMenuItem.Size = new System.Drawing.Size(238, 24);
            this.addBetterMultiMatchToolStripMenuItem.Text = "Add better multi match";
            this.addBetterMultiMatchToolStripMenuItem.Click += new System.EventHandler(this.addBetterMultiMatchToolStripMenuItem_Click);
            // 
            // groupBoxInspectItems
            // 
            this.groupBoxInspectItems.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxInspectItems.Controls.Add(this.buttonDetectFont);
            this.groupBoxInspectItems.Controls.Add(this.labelImageSize);
            this.groupBoxInspectItems.Controls.Add(this.labelExpandCount);
            this.groupBoxInspectItems.Controls.Add(this.pictureBoxInspectItem);
            this.groupBoxInspectItems.Controls.Add(this.listBoxInspectItems);
            this.groupBoxInspectItems.Location = new System.Drawing.Point(16, 14);
            this.groupBoxInspectItems.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxInspectItems.Name = "groupBoxInspectItems";
            this.groupBoxInspectItems.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxInspectItems.Size = new System.Drawing.Size(567, 674);
            this.groupBoxInspectItems.TabIndex = 0;
            this.groupBoxInspectItems.TabStop = false;
            this.groupBoxInspectItems.Text = "Inspect items";
            // 
            // buttonDetectFont
            // 
            this.buttonDetectFont.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDetectFont.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonDetectFont.Location = new System.Drawing.Point(340, 630);
            this.buttonDetectFont.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonDetectFont.Name = "buttonDetectFont";
            this.buttonDetectFont.Size = new System.Drawing.Size(219, 27);
            this.buttonDetectFont.TabIndex = 999;
            this.buttonDetectFont.Text = "Detect font";
            this.buttonDetectFont.UseVisualStyleBackColor = true;
            this.buttonDetectFont.Click += new System.EventHandler(this.buttonDetectFont_Click);
            // 
            // labelImageSize
            // 
            this.labelImageSize.AutoSize = true;
            this.labelImageSize.Location = new System.Drawing.Point(336, 201);
            this.labelImageSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelImageSize.Name = "labelImageSize";
            this.labelImageSize.Size = new System.Drawing.Size(66, 15);
            this.labelImageSize.TabIndex = 999;
            this.labelImageSize.Text = "labelSize";
            // 
            // labelExpandCount
            // 
            this.labelExpandCount.AutoSize = true;
            this.labelExpandCount.Location = new System.Drawing.Point(336, 113);
            this.labelExpandCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelExpandCount.Name = "labelExpandCount";
            this.labelExpandCount.Size = new System.Drawing.Size(126, 15);
            this.labelExpandCount.TabIndex = 999;
            this.labelExpandCount.Text = "labelExpandCount";
            // 
            // pictureBoxInspectItem
            // 
            this.pictureBoxInspectItem.BackColor = System.Drawing.Color.Red;
            this.pictureBoxInspectItem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxInspectItem.Location = new System.Drawing.Point(336, 132);
            this.pictureBoxInspectItem.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBoxInspectItem.Name = "pictureBoxInspectItem";
            this.pictureBoxInspectItem.Size = new System.Drawing.Size(69, 60);
            this.pictureBoxInspectItem.TabIndex = 23;
            this.pictureBoxInspectItem.TabStop = false;
            // 
            // labelCount
            // 
            this.labelCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCount.AutoSize = true;
            this.labelCount.Location = new System.Drawing.Point(16, 691);
            this.labelCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCount.Name = "labelCount";
            this.labelCount.Size = new System.Drawing.Size(76, 15);
            this.labelCount.TabIndex = 999;
            this.labelCount.Text = "labelCount";
            // 
            // VobSubOcrCharacterInspect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(976, 737);
            this.Controls.Add(this.labelCount);
            this.Controls.Add(this.groupBoxInspectItems);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.groupBoxCurrentCompareImage);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VobSubOcrCharacterInspect";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " ";
            this.Load += new System.EventHandler(this.VobSubOcrCharacterInspect_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.VobSubOcrCharacterInspect_KeyDown);
            this.groupBoxCurrentCompareImage.ResumeLayout(false);
            this.groupBoxCurrentCompareImage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCompareBitmapDouble)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCompareBitmap)).EndInit();
            this.contextMenuStripAddBetterMultiMatch.ResumeLayout(false);
            this.groupBoxInspectItems.ResumeLayout(false);
            this.groupBoxInspectItems.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInspectItem)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.GroupBox groupBoxCurrentCompareImage;
        private System.Windows.Forms.CheckBox checkBoxItalic;
        private System.Windows.Forms.Label labelDoubleSize;
        private System.Windows.Forms.PictureBox pictureBoxCompareBitmapDouble;
        private System.Windows.Forms.Label labelTextAssociatedWithImage;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.Label labelImageInfo;
        private System.Windows.Forms.TextBox textBoxText;
        private System.Windows.Forms.PictureBox pictureBoxCompareBitmap;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.ListBox listBoxInspectItems;
        private System.Windows.Forms.GroupBox groupBoxInspectItems;
        private System.Windows.Forms.PictureBox pictureBoxInspectItem;
        private System.Windows.Forms.Button buttonAddBetterMatch;
        private System.Windows.Forms.Label labelExpandCount;
        private System.Windows.Forms.Label labelCount;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripAddBetterMultiMatch;
        private System.Windows.Forms.ToolStripMenuItem addBetterMultiMatchToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripLetters;
        private System.Windows.Forms.Label labelImageSize;
        private System.Windows.Forms.Button buttonDetectFont;
    }
}