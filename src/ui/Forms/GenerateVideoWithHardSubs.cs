﻿using Nikse.SubtitleEdit.Core.Common;
using Nikse.SubtitleEdit.Core.SubtitleFormats;
using Nikse.SubtitleEdit.Logic;
using System;
using System.IO;
using System.Windows.Forms;

namespace Nikse.SubtitleEdit.Forms
{
    public sealed partial class GenerateVideoWithHardSubs : Form
    {
        private bool _abort;
        private Subtitle _assaSubtitle;
        private string _inputVideoFileName;
        public string VideoFileName { get; private set; }

        public GenerateVideoWithHardSubs(Subtitle assaSubtitle, string inputVideoFileName, int? fontSize)
        {
            UiUtil.PreInitialize(this);
            InitializeComponent();
            UiUtil.FixFonts(this);

            Text = LanguageSettings.Current.GenerateVideoWithBurnedInSubs.Title;
            labelInfo.Text = LanguageSettings.Current.GenerateVideoWithBurnedInSubs.Info;
            _assaSubtitle = assaSubtitle;
            _inputVideoFileName = inputVideoFileName;
            buttonOK.Text = LanguageSettings.Current.Watermark.Generate;
            labelPleaseWait.Text = LanguageSettings.Current.General.PleaseWait;
            labelFontSize.Text = LanguageSettings.Current.ExportPngXml.FontSize;
            buttonCancel.Text = LanguageSettings.Current.General.Cancel;
            progressBar1.Visible = false;
            labelPleaseWait.Visible = false;

            if (fontSize.HasValue)
            {
                numericUpDownFontSize.Value = fontSize.Value;
            }
            else
            {
                labelFontSize.Visible = false;
                numericUpDownFontSize.Visible = false;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            _abort = true;
            if (buttonOK.Enabled)
            {
                DialogResult = DialogResult.Cancel;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            buttonOK.Enabled = false;
            numericUpDownFontSize.Enabled = false;
            using (var saveDialog = new SaveFileDialog { FileName = string.Empty, Filter = "MP4|*.mp4|Matroska|*.mkv|WebM|*.webm" })
            {
                if (saveDialog.ShowDialog(this) != DialogResult.OK)
                {
                    buttonOK.Enabled = true;
                    numericUpDownFontSize.Enabled = true;
                    return;
                }

                VideoFileName = saveDialog.FileName;
            }

            if (File.Exists(VideoFileName))
            {
                File.Delete(VideoFileName);
            }

            if (numericUpDownFontSize.Visible)
            {
                var fontSize = (int)numericUpDownFontSize.Value;
                var style = AdvancedSubStationAlpha.GetSsaStyle("Default", _assaSubtitle.Header);
                style.FontSize = fontSize;
                var styleLine = style.ToRawAss();
                _assaSubtitle.Header = AdvancedSubStationAlpha.AddTagToHeader("Style", styleLine, "[V4+ Styles]", _assaSubtitle.Header);
            }

            if (Configuration.Settings.General.RightToLeftMode && LanguageAutoDetect.CouldBeRightToLeftLanguage(_assaSubtitle))
            {
                for (var index = 0; index < _assaSubtitle.Paragraphs.Count; index++)
                {
                    var paragraph = _assaSubtitle.Paragraphs[index];
                    if (LanguageAutoDetect.ContainsRightToLeftLetter(paragraph.Text))
                    {
                        paragraph.Text = Utilities.FixRtlViaUnicodeChars(paragraph.Text);
                    }
                }
            }

            SubtitleFormat format = new AdvancedSubStationAlpha();
            var assaTempFileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".ass");
            File.WriteAllText(assaTempFileName, format.ToText(_assaSubtitle, null));

            progressBar1.Style = ProgressBarStyle.Marquee;
            progressBar1.Visible = true;
            labelPleaseWait.Visible = true;
            var process = VideoPreviewGenerator.GenerateHardcodedVideoFile(
                _inputVideoFileName,
                assaTempFileName,
                VideoFileName);

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

            try
            {
                File.Delete(assaTempFileName);
            }
            catch
            {
                // ignore
            }

            DialogResult = _abort ? DialogResult.Cancel : DialogResult.OK;
        }
    }
}
