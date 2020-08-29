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
using TwinsaityEditor.Properties;
using WK.Libraries.BetterFolderBrowserNS;

namespace TwinsaityEditor.Workers
{
    public partial class ImageMaker : Form
    {
        public ImageMaker()
        {
            InitializeComponent();
            Show();
        }

        private void ImageMaker_Load(object sender, EventArgs e)
        {
            tspbGenerationProgress.Maximum = 100;
            if (Directory.Exists(Settings.Default.TwinsUnpackedPath))
            {
                tbTwinsanityPath.Text = Settings.Default.TwinsUnpackedPath;
            }
            tbImageName.Text = Settings.Default.ImageName;
            if (Directory.Exists(Settings.Default.ImageOutputPath))
            {
                tbOutputPath.Text = Settings.Default.ImageOutputPath;
            }
        }

        private void btnSelectTwinsPath_Click(object sender, EventArgs e)
        {
            using (BetterFolderBrowser bfb = new BetterFolderBrowser
            {
                RootFolder = Settings.Default.TwinsUnpackedPath,
                Title = "Select Twinsanity folder"
            })
            {
                if (bfb.ShowDialog() == DialogResult.OK)
                {
                    Settings.Default.TwinsUnpackedPath = bfb.SelectedFolder;
                    tbTwinsanityPath.Text = bfb.SelectedFolder;
                }
            }
        }

        private void btnOutputPath_Click(object sender, EventArgs e)
        {
            using (BetterFolderBrowser bfb = new BetterFolderBrowser
            {
                RootFolder = Settings.Default.TwinsUnpackedPath,
                Title = "Select Twinsanity folder"
            })
            {
                if (bfb.ShowDialog() == DialogResult.OK)
                {
                    Settings.Default.ImageOutputPath = bfb.SelectedFolder;
                    tbOutputPath.Text = bfb.SelectedFolder;
                }
            }
        }

        private void tbImageName_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.ImageName = tbImageName.Text;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            var progress = Externals.PS2ImageMaker.PS2ImageMaker.StartPacking(tbTwinsanityPath.Text, tbOutputPath.Text + "\\" + tbImageName.Text + ".iso");
            tsslblCurrentFile.Text = "Current file: " + progress.File;
            tspbGenerationProgress.Value = (int)(progress.ProgressPercentage * 100);
            timer1.Enabled = true;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var progress = Externals.PS2ImageMaker.PS2ImageMaker.PollProgress();
            if (progress.ProgressS == Externals.PS2ImageMaker.PS2ImageMaker.ProgressState.FAILED)
            {
                timer1.Stop();
                timer1.Enabled = false;
                tsslblCurrentFile.Text = "Failed";
                tspbGenerationProgress.Value = 0;
                return;
            }
            tsslblCurrentFile.Text = "Current file: " + progress.File;
            tspbGenerationProgress.Value = (int)(progress.ProgressPercentage * 100);
            if (progress.Finished)
            {
                timer1.Stop();
            }
        }
    }
}
