using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using fileDownloader;
using System.Xml;
using System.Net;

namespace fileDownloader2

{
    public partial class Form2 : Form
    {

        public string linkURL = "";
        public string searchedAnime = "";
        public string selectedAnime = "";

        public Form2()
        {
            InitializeComponent();
            lstAnime.DoubleClick += new EventHandler(lstAnime_DoubleClick);
            lblEpisode.Visible = false;
            txtEpisode.Visible = false;
            btnEpisode.Visible = false;
            lblAnimeList.Visible = false;
            lstAnime.Visible = false;
            //text input: link to episode page (https://www1.gogoanime.ai/dragon-ball-z-episode-21)
            //download the content from https://www1.gogoanime.ai/dragon-ball-z-episode-21
            //$('li.dowloads > a').href
            //once you've scraped this url using xpath(or something similiar): "https://streamani.net/download?id=MTQ5NTk=&typesub=Gogoanime-SUB&title=Dragon+Ball+Z+Episode+21"
            //$('div.dowload > a').href
            //same thing: scrape for "https://storage.googleapis.com/pure-meridian-315002/T8BKBPU8OIPC/23a_1623237790_14959.mp4" and voila!
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(Form2_Closed);
        }

        public void Form2_Load(object sender, EventArgs e)
        {
            this.Width = 281;
        }

        public void Form2_Closed(object sender, EventArgs e)
        {
            Form1.ActiveForm.Show();
            Form1.ActiveForm.WindowState = FormWindowState.Normal;
            Form1.episodeReady = true;
            Form1.animeTab = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.Width = 495;
            lblEpisode.Visible = false;
            txtEpisode.Visible = false;
            btnEpisode.Visible = false;
            lstAnime.Items.Clear();
            lstLinks.Items.Clear();
            string xpath = "//p[@class=\"name\"]/a";
            searchedAnime = "https://www1.gogoanime.ai/search.html?keyword=" + txtAnime.Text;
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            WebClient webClient = new WebClient();
            string htmlContent = webClient.DownloadString(searchedAnime);
            doc.LoadHtml(htmlContent);
            var nodes = doc.DocumentNode.SelectNodes(xpath);
            foreach (var node in nodes)
            {
                string animeName = node.Attributes["title"].Value;
                string animeLink = node.Attributes["href"].Value;
                lstAnime.Items.Add(animeName);
                lstLinks.Items.Add(animeLink.Remove(0,9));
            }
            lblAnimeList.Visible = true;
            lstAnime.Visible = true;
        }
        private void lstAnime_DoubleClick(object sender, EventArgs e)
        {
            if (lstAnime.SelectedItem != null)
            {
                this.Width = 705;
                int selectionNumber = lstAnime.SelectedIndex;
                lstLinks.SelectedIndex = selectionNumber;
                selectedAnime = lstLinks.SelectedItem.ToString();
                lblEpisode.Visible = true;
                txtEpisode.Visible = true;
                btnEpisode.Visible = true;
            }
        }

        private void btnEpisode_Click(object sender, EventArgs e)
        {  
            string episodeLink = "https://www1.gogoanime.ai" + selectedAnime + "-episode-" + txtEpisode.Text;
            string episodeXpath = "//li[@class=\"dowloads\"]/a";
            string downloadLinkXpath = "//div[@class=\"mirror_link\"]/div[@class=\"dowload\"]/a";

            HtmlAgilityPack.HtmlDocument episodeDoc = new HtmlAgilityPack.HtmlDocument();
            WebClient episodeWebClient = new WebClient();
            string episodeHtmlContent = episodeWebClient.DownloadString(episodeLink);
            episodeDoc.LoadHtml(episodeHtmlContent);
            var node = episodeDoc.DocumentNode.SelectSingleNode(episodeXpath);
            var link = node.Attributes["href"].Value;
            var downloadLinkDoc = new HtmlAgilityPack.HtmlDocument();
            string downloadLinkHtmlContent = episodeWebClient.DownloadString(link);
            downloadLinkDoc.LoadHtml(downloadLinkHtmlContent);
            var downloadLinkNode = downloadLinkDoc.DocumentNode.SelectSingleNode(downloadLinkXpath);
            var downloadLink = downloadLinkNode.Attributes["href"].Value;

            linkURL = downloadLink;
            Form1.scrapedURL = linkURL;
            MessageBox.Show(Form1.scrapedURL);
            lblReady.Visible = true;
            btnCloseAnime.Visible = true;
        }

        private void btnCloseAnime_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}


//https://www1.gogoanime.ai/one-piece-episode-
//https://www1.gogoanime.ai/dragon-ball-z-episode-
//selectedEpisode = txtEpisode.Text;
//HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
//string URL = "" + selectedEpisode;
//WebClient webClient = new WebClient();
//string htmlContent = webClient.DownloadString(URL);
//doc.LoadHtml(htmlContent);
//string xpath = "//li[@class=\"dowloads\"]/a";
//var node = doc.DocumentNode.SelectSingleNode(xpath);
//var link = node.Attributes["href"].Value;
//var episodeDoc = new HtmlAgilityPack.HtmlDocument();
//string episodeHtmlContent = webClient.DownloadString(link);
//episodeDoc.LoadHtml(episodeHtmlContent);
//string episodeXpath = "//div[@class=\"mirror_link\"]/div[@class=\"dowload\"]/a";
//var episodeNode = episodeDoc.DocumentNode.SelectSingleNode(episodeXpath);
//var episodeLink = episodeNode.Attributes["href"].Value;

//linkURL = episodeLink;
//Form1.scrapedURL = linkURL;
//lblReady.Visible = true;