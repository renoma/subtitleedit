﻿using Nikse.SubtitleEdit.Core.Common;
using Nikse.SubtitleEdit.Logic;
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Windows.Forms;

namespace Nikse.SubtitleEdit.Forms.Ocr
{
    public sealed partial class DownloadTesseract302 : Form
    {
        public DownloadTesseract302()
        {
            UiUtil.PreInitialize(this);
            InitializeComponent();
            UiUtil.FixFonts(this);
            Text = LanguageSettings.Current.GetTesseractDictionaries.Download + " Tesseract 3.02";
            labelPleaseWait.Text = LanguageSettings.Current.General.PleaseWait;
            labelDescription1.Text = LanguageSettings.Current.GetTesseractDictionaries.Download + " Tesseract OCR";

            var wc = new WebClient { Proxy = Utilities.GetProxy() };
            wc.DownloadDataAsync(new Uri("https://github.com/SubtitleEdit/support-files/raw/master/Tesseract302.tar.gz"));
            wc.DownloadDataCompleted += wc_DownloadDataCompleted;
            wc.DownloadProgressChanged += (o, args) =>
            {
                labelPleaseWait.Text = LanguageSettings.Current.General.PleaseWait + "  " + args.ProgressPercentage + "%";
            };
        }

        private void wc_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(LanguageSettings.Current.GetTesseractDictionaries.DownloadFailed);
                DialogResult = DialogResult.Cancel;
                return;
            }

            string dictionaryFolder = Configuration.Tesseract302Directory;
            if (!Directory.Exists(dictionaryFolder))
            {
                Directory.CreateDirectory(dictionaryFolder);
            }

            var tempFileName = FileUtil.GetTempFileName(".tar");
            using (var ms = new MemoryStream(e.Result))
            using (var fs = new FileStream(tempFileName, FileMode.Create))
            using (var zip = new GZipStream(ms, CompressionMode.Decompress))
            {
                byte[] buffer = new byte[1024];
                int nRead;
                while ((nRead = zip.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fs.Write(buffer, 0, nRead);
                }
            }

            using (var tr = new TarReader(tempFileName))
            {
                foreach (var th in tr.Files)
                {
                    string fn = Path.Combine(dictionaryFolder, th.FileName.Replace('/', Path.DirectorySeparatorChar));
                    if (th.IsFolder)
                    {
                        Directory.CreateDirectory(Path.Combine(dictionaryFolder, th.FileName.Replace('/', Path.DirectorySeparatorChar)));
                    }
                    else if (th.FileSizeInBytes > 0)
                    {
                        th.WriteData(fn);
                    }
                }
            }
            File.Delete(tempFileName);
            Cursor = Cursors.Default;
            DialogResult = DialogResult.OK;
        }
    }
}
