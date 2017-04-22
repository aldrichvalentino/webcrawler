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
                parserXML();
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            parserXML();
            
        }

        public void parserXML()
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

                    HtmlNode currentNode = doc.DocumentNode.SelectSingleNode("//div[@class='detail_text'][@id='detikdetailtext']");
                    if (currentNode != null) {
                        art.Content = currentNode.InnerHtml;
                        finalfeeds.Add(html);
                    }
                    /*String text = page.DownloadString(html.Link);
                    Article art = new Article();
                    Match m = Regex.Match(text, "<title>(.*)<\/title>");
                    art.Title = m.Value;
                    Match m2 = Regex.Match(text, "id=\"detikdetailtext\"*<b>*</b>");
                    art.Content = m2.Value;
                    if (m.Success)
                    {
                        html.ContainsKeyword = true;
                        finalfeeds.Add(html);
                    }*/
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