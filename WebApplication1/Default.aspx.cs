using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace WebApplication1
{
    public partial class _Default : Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                parserXML(TextBox.Text);
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            parserXML(TextBox.Text);
        }

        public void parserXML(string keyword)
        {
            List<String> rssUrl = new List<string>();
            rssUrl.Add("http://rss.detik.com/index.php/detikcom");
            rssUrl.Add("http://tempo.co/rss/terkini");
            //rssUrl.Add("http://rss.vivanews.com/get/all");
            rssUrl.Add("http://www.antaranews.com/rss/terkini");
            List<Feeds> feeds = new List<Feeds>();

            try
            {
                foreach (var it in rssUrl)
                {
                    XDocument docs = new XDocument();
                    docs = XDocument.Load(it.ToString());
                    var items = (from x in docs.Descendants("item")
                                 select new
                                 {
                                     title = x.Element("title").Value,
                                     link = x.Element("link").Value,
                                     description = x.Element("description").Value,
                                     containsKey = false,
                                     keyWordSentence = ""
                                 });
                    if (items != null)
                    {
                        foreach (var i in items)
                        {
                            Feeds h = new Feeds
                            {
                                Title = i.title,
                                Link = i.link,
                                Description = i.description,
                                ContainsKeyword = i.containsKey
                            };

                            feeds.Add(h);
                        }
                    }
                }

                List<Feeds> finalfeeds = new List<Feeds>();
                foreach (var html in feeds)
                {
                    Article art = new Article();
                    HtmlWeb page = new HtmlWeb();
                    var doc = page.Load(html.Link);

                    HtmlNode currentNodeTitle = doc.DocumentNode.SelectSingleNode("//title");
                    if (currentNodeTitle != null){
                        art.Title = currentNodeTitle.InnerHtml;
                    }
                    HtmlNode currentNodeDetik = doc.DocumentNode.SelectSingleNode("//div[@class='detail_text'][@id='detikdetailtext']");
                    HtmlNode currentNodeTempo = doc.DocumentNode.SelectSingleNode("//p");
                    //HtmlNode currentNodeViva = doc.DocumentNode.SelectSingleNode("//div[@class='detail_text'][@id='detikdetailtext']");
                    HtmlNode currentNodeAntara = doc.DocumentNode.SelectSingleNode("//div[@class='content_news'][@itemprop='articleBody']");
                    if (currentNodeDetik != null) {
                        art.Content = currentNodeDetik.InnerHtml;                        
                    }else if (currentNodeTempo != null){
                        art.Content = currentNodeTempo.InnerHtml;                        
                    }else if (currentNodeDetik != null){
                        art.Content = currentNodeAntara.InnerHtml;
                    }

                    //Algoritma pencarian
                    //Regex
                    Match m = Regex.Match(art.Content,keyword);
                    if (m.Success) {
                      finalfeeds.Add(html);
                    }
                    //KMP
                    //BM
                }

                theRss.DataSource = finalfeeds;
                theRss.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write("<script language='javascript'>alert('" + Server.HtmlEncode(ex.Message) + "')</script>");
            }
        }
    }
}