using System;
using System.IO;
using AltoHttp;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fileDownloader2;

namespace fileDownloader
{
    public partial class Form1 : Form
    {
        public static bool animeTab = false;
        public static bool episodeReady = false;
        public static string scrapedURL = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        HttpDownloader httpDownloader;
        bool downloading = false;
        bool paused = false;
        bool started = false;

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (paused)
            {
                started = false;
                lblStatus.Text = ("Stopping...");
                httpDownloader.Pause();
                //File.Delete($"{Application.StartupPath}\\Downloads\\{Path.GetFileName(txtURL.Text)}");
                MessageBox.Show("No files has been deleted!");
                MessageBox.Show("Your download has been stopped.");
                txtURL.Text = "";
                txtURL.ReadOnly = false;
                progressBar.Value = 0;
                lblSpeed.Text = ("0 MB/s");
                lblDownloaded.Text = ("0 MB");
                lblPercent.Text = ("0 %");
                btnStart.Text = "&Start";
                downloading = false;
                paused = false;
                lblStatus.Text = ("Inactive");
            }
            else if (downloading == false)
            {
                if (txtURL.TextLength < 10)
                {
                    MessageBox.Show("Enter a valid URL.");
                }
                else
                {
                    started = true;
                    httpDownloader = new HttpDownloader(txtURL.Text, $"{Application.StartupPath}\\Downloads\\{Path.GetFileName(txtURL.Text)}");
                    httpDownloader.DownloadCompleted += HttpDownloader_DownloadCompleted;
                    httpDownloader.ProgressChanged += HttpDownloader_ProgressChanged;
                    httpDownloader.Start();
                    MessageBox.Show("Your download has been successfully initiated.");
                }
            }
            else if (downloading)
            {
                started = false;
                lblStatus.Text = ("Stopping...");
                httpDownloader.Pause();
                //File.Delete($"{Application.StartupPath}\\Downloads\\{Path.GetFileName(txtURL.Text)}");
                MessageBox.Show("No files has been deleted!");
                MessageBox.Show("Your download has been stopped.");
                txtURL.Text = "";
                txtURL.ReadOnly = false;
                progressBar.Value = 0;
                lblSpeed.Text = ("0 MB/s");
                lblDownloaded.Text = ("0 MB");
                lblPercent.Text = ("0 %");
                btnStart.Text = "&Start";
                downloading = false;
                paused = false;
                lblStatus.Text = ("Inactive");
            }
        }

        private void HttpDownloader_ProgressChanged(object sender, AltoHttp.ProgressChangedEventArgs e)
        {
            progressBar.Value = (int)e.Progress;
            lblPercent.Text = $"{e.Progress.ToString("0.00")} %";
            lblSpeed.Text = string.Format("{0} MB/s", (e.SpeedInBytes / 1024d / 1024d).ToString("0.00"));
            lblDownloaded.Text = string.Format("{0} MB", (httpDownloader.TotalBytesReceived / 1024d / 1024d).ToString("0.00"));
            lblStatus.Text = "Downloading...";
            downloading = true;
            txtURL.ReadOnly = true;
            btnStart.Text = ("&Stop");
        }

        private void HttpDownloader_DownloadCompleted(object sender, EventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                lblStatus.Text = "Completed";
                lblPercent.Text = "100%";
                MessageBox.Show("Your download has been completed.");
                txtURL.Text = "";
                txtURL.ReadOnly = false;
                progressBar.Value = 0;
                lblSpeed.Text = ("0 MB/s");
                lblDownloaded.Text = ("0 MB");
                lblPercent.Text = ("0 %");
                btnStart.Text = "&Start";
                downloading = false;
                paused = false;
                lblStatus.Text = ("Inactive");
            });
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (started == true && paused == false)
            {
                if (downloading)
                {
                    if (httpDownloader != null)
                        httpDownloader.Pause();
                    lblStatus.Text = "Paused";
                    paused = true;
                    MessageBox.Show("Your download has been paused.");
                }
            }
            else if (paused == false)
            {
                //MessageBox.Show("Your download is already paused.");
            }
            else
            {
                //MessageBox.Show("Start a download.");
            }
        }

        private void btnResume_Click(object sender, EventArgs e)
        {
            if (started)
            {
                if (paused == true)
                {

                    if (httpDownloader != null)
                        httpDownloader.Resume();
                    lblStatus.Text = "Resuming...";
                    System.Threading.Thread.Sleep(3000);
                    paused = false;
                    downloading = true;
                    MessageBox.Show("Your download has been resumed.");
                }
                else
                {
                    //MessageBox.Show("Your file is already downloading.");
                }
            }
            else
            {
                //MessageBox.Show("Start a download.");
            }
        }

        private void btnDirectory_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", ($"{Application.StartupPath}\\Downloads"));
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnAnime_Click(object sender, EventArgs e)
        {
            if (downloading)
            {
                MessageBox.Show("Wait for current download to complete.");
            }
            else
            {
                if (animeTab == false)
                {
                    var animeForm = new Form2();
                    animeForm.Show();
                    animeTab = true;
                }
                else if (animeTab == true)
                {
                }
            }
        }

        private void btnAttach_Click(object sender, EventArgs e)
        {
            if (downloading)
            {
                MessageBox.Show("Wait for current download to complete.");
            }
            else
            {
                if (episodeReady == true)
                {
                    txtURL.Text = scrapedURL;
                    episodeReady = false;
                }
                else if (episodeReady == false)
                {
                    MessageBox.Show("First scrape a link.");
                }
            }
        }
    }
}