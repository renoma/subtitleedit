﻿using Nikse.SubtitleEdit.Core.Common;
using Nikse.SubtitleEdit.Logic;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace Nikse.SubtitleEdit.Forms
{
    public sealed partial class GenerateVideo : Form
    {

        public string VideoFileName { get; private set; }
        private bool _abort;

        public GenerateVideo(Subtitle subtitle)
        {
            UiUtil.PreInitialize(this);
            InitializeComponent();
            UiUtil.FixFonts(this);

            numericUpDownDurationMinutes.Value = Configuration.Settings.Tools.BlankVideoMinutes;
            panelColor.BackColor = Configuration.Settings.Tools.BlankVideoColor;
            if (Configuration.Settings.Tools.BlankVideoUseCheckeredImage)
            {
                radioButtonCheckeredImage.Checked = true;
            }
            else
            {
                radioButtonColor.Checked = true;
            }

            Text = LanguageSettings.Current.GenerateBlankVideo.Title;
            radioButtonCheckeredImage.Text = LanguageSettings.Current.GenerateBlankVideo.CheckeredImage;
            radioButtonColor.Text = LanguageSettings.Current.GenerateBlankVideo.SolidColor;
            labelDuration.Text = LanguageSettings.Current.GenerateBlankVideo.DurationInMinutes;
            labelFrameRate.Text = LanguageSettings.Current.General.FrameRate;
            buttonColor.Text = LanguageSettings.Current.Settings.ChooseColor;
            buttonOK.Text = LanguageSettings.Current.Watermark.Generate;
            labelResolution.Text = LanguageSettings.Current.ExportPngXml.VideoResolution;
            labelPleaseWait.Text = LanguageSettings.Current.General.PleaseWait;
            buttonCancel.Text = LanguageSettings.Current.General.Cancel;
            progressBar1.Visible = false;
            labelPleaseWait.Visible = false;

            var left = Math.Max(labelResolution.Left + labelResolution.Width, labelDuration.Left + labelDuration.Width) + 5;
            numericUpDownDurationMinutes.Left = left;
            numericUpDownWidth.Left = left;
            labelX.Left = numericUpDownWidth.Left + numericUpDownWidth.Width + 3;
            numericUpDownHeight.Left = labelX.Left + labelX.Width + 3;
            comboBoxFrameRate.Left = left;

            comboBoxFrameRate.Items.Clear();
            comboBoxFrameRate.Items.Add(23.976.ToString(CultureInfo.CurrentCulture));
            comboBoxFrameRate.Items.Add(24.0.ToString(CultureInfo.CurrentCulture));
            comboBoxFrameRate.Items.Add(25.0.ToString(CultureInfo.CurrentCulture));
            comboBoxFrameRate.Items.Add(29.97.ToString(CultureInfo.CurrentCulture));
            comboBoxFrameRate.Items.Add(30.00.ToString(CultureInfo.CurrentCulture));
            comboBoxFrameRate.Items.Add(50.00.ToString(CultureInfo.CurrentCulture));
            comboBoxFrameRate.Items.Add(59.94.ToString(CultureInfo.CurrentCulture));
            comboBoxFrameRate.Items.Add(60.00.ToString(CultureInfo.CurrentCulture));
            comboBoxFrameRate.SelectedIndex = 0;
            for (var index = 0; index < comboBoxFrameRate.Items.Count; index++)
            {
                var item = comboBoxFrameRate.Items[index];
                var v = Convert.ToDecimal(item.ToString(), CultureInfo.CurrentCulture);
                if (Math.Abs(v - Configuration.Settings.Tools.BlankVideoFrameRate) < 0.01m)
                {
                    comboBoxFrameRate.SelectedIndex = index;
                    break;
                }
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            EnableDisableControls(false);
            var fileName = radioButtonColor.Checked ? "blank_video_solid" : "blank_video_checkered";
            using (var saveDialog = new SaveFileDialog { FileName = fileName, Filter = "MP4|*.mp4|Matroska|*.mkv|WebM|*.webm" })
            {
                if (saveDialog.ShowDialog(this) != DialogResult.OK)
                {
                    EnableDisableControls(true);
                    return;
                }

                VideoFileName = saveDialog.FileName;
            }

            if (File.Exists(VideoFileName))
            {
                File.Delete(VideoFileName);
            }

            progressBar1.Style = ProgressBarStyle.Marquee;
            progressBar1.Visible = true;
            labelPleaseWait.Visible = true;
            var process = VideoPreviewGenerator.GenerateVideoFile(
                VideoFileName,
                (int)Math.Round(numericUpDownDurationMinutes.Value * 60),
                (int)numericUpDownWidth.Value,
                (int)numericUpDownHeight.Value,
                panelColor.BackColor,
                radioButtonCheckeredImage.Checked,
                decimal.Parse(comboBoxFrameRate.Text)
                );

            process.Start();
            while (!process.HasExited)
            {
                System.Threading.Thread.Sleep(100);
                Application.DoEvents();
                if (_abort)
                {
                    process.Kill();
                }
            }

            progressBar1.Visible = false;
            labelPleaseWait.Visible = false;
            DialogResult = _abort ? DialogResult.Cancel : DialogResult.OK;
        }

        private void EnableDisableControls(bool enable)
        {
            buttonOK.Enabled = enable;
            numericUpDownDurationMinutes.Enabled = enable;
            numericUpDownWidth.Enabled = enable;
            numericUpDownHeight.Enabled = enable;
            comboBoxFrameRate.Enabled = enable;
            groupBoxBackground.Enabled = enable;
        }

        private void buttonColor_Click(object sender, EventArgs e)
        {
            using (var colorChooser = new ColorChooser { Color = panelColor.BackColor, ShowAlpha = false })
            {
                if (colorChooser.ShowDialog() == DialogResult.OK)
                {
                    panelColor.BackColor = colorChooser.Color;
                }
            }
        }

        private void panelColor_MouseClick(object sender, MouseEventArgs e)
        {
            buttonColor_Click(null, null);
        }

        private void GenerateVideo_FormClosing(object sender, FormClosingEventArgs e)
        {
            Configuration.Settings.Tools.BlankVideoColor = panelColor.BackColor;
            Configuration.Settings.Tools.BlankVideoUseCheckeredImage = radioButtonCheckeredImage.Checked;
            Configuration.Settings.Tools.BlankVideoMinutes = (int)numericUpDownDurationMinutes.Value;
            Configuration.Settings.Tools.BlankVideoFrameRate = Convert.ToDecimal(comboBoxFrameRate.Text, CultureInfo.CurrentCulture);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            _abort = true;
            if (buttonOK.Enabled)
            {
                DialogResult = DialogResult.Cancel;
            }
        }

        private void numericUpDownWidth_ValueChanged(object sender, EventArgs e)
        {
            var v = (int)numericUpDownWidth.Value;
            if (v % 2 == 1)
            {
                numericUpDownWidth.Value++;
            }
        }

        private void numericUpDownHeight_ValueChanged(object sender, EventArgs e)
        {
            var v = (int)numericUpDownHeight.Value;
            if (v % 2 == 1)
            {
                numericUpDownHeight.Value++;
            }
        }
    }
}
