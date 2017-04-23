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
            //rssUrl.Add("http://tempo.co/rss/terkini");
            //rssUrl.Add("http://rss.viva.co.id");
            //rssUrl.Add("http://www.antaranews.com/rss/terkini");
            List<Feeds> feeds = new List<Feeds>();

            try
            {
                foreach (var it in rssUrl)
                {
                    //HttpWebRequest webreq = (HttpWebRequest) WebRequest.Create(it.ToString());
                    //webreq.KeepAlive = true;
                    //webreq.ProtocolVersion = HttpVersion.Version10;
                    XDocument docs = new XDocument();
                    try
                    {
                        docs = XDocument.Load(it.ToString());
                    }
                    catch (Exception e){

                    }
                    
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
                    try
                    {
                        var doc = page.Load(html.Link);

                        HtmlNode currentNodeTitle = doc.DocumentNode.SelectSingleNode("//title");
                        if (currentNodeTitle != null)
                        {
                            art.Title = currentNodeTitle.InnerHtml;
                        }
                        HtmlNode currentNodeDetik = doc.DocumentNode.SelectSingleNode("//div[@class='detail_text'][@id='detikdetailtext']");
                        HtmlNode currentNodeTempo = doc.DocumentNode.SelectSingleNode("//p");
                        HtmlNode currentNodeViva = doc.DocumentNode.SelectSingleNode("//span[@itemprop='description']");
                        HtmlNode currentNodeAntara = doc.DocumentNode.SelectSingleNode("//div[@class='content_news'][@itemprop='articleBody']");
                        if (currentNodeDetik != null)
                        {
                            art.Content = currentNodeDetik.InnerHtml;
                        }
                        else if (currentNodeTempo != null)
                        {
                            art.Content = currentNodeTempo.InnerHtml;
                        }
                        else if (currentNodeDetik != null)
                        {
                            art.Content = currentNodeAntara.InnerHtml;
                        } else if (currentNodeViva != null)
                        {
                            art.Content = currentNodeViva.InnerHtml;
                        }

                        //Algoritma pencarian
                        if (KMPRadioButton.Checked == true)
                        {
                            KMP K1 = new KMP(keyword, art.Content);
                            KMP K2 = new KMP(keyword, art.Title);
                            String result1 = K1.getKMPResult();
                            String result2 = K2.getKMPResult();
                            if (!((result1.Equals("not found")) && (result2.Equals("not found"))))
                            {
                                if (!result2.Equals("not found"))
                                {
                                    html.Description = result2;
                                }
                                else
                                {
                                    html.Description = result1;
                                }

                                finalfeeds.Add(html);
                            }
                        }
                        else if (BMRadioButton.Checked == true)
                        {
                            BoyerMoore B1 = new BoyerMoore(keyword, art.Content);//blm diganti
                            BoyerMoore B2 = new BoyerMoore(keyword, art.Title);
                            String result1 = B1.getBoyerMooreResult();
                            String result2 = B2.getBoyerMooreResult();
                            if (!((result1.Equals("not found")) && (result2.Equals("not found"))))
                            {
                                if (!result2.Equals("not found", StringComparison.Ordinal))
                                {
                                    html.Description = result2;
                                }
                                else
                                {
                                    html.Description = result1;
                                }

                                finalfeeds.Add(html);
                            }
                        }
                        else if (RegexRadioButton.Checked == true)
                        {//Regex
                         /* String[] keyelements = Regex.Split(keyword, "[ ,]");
                          //String str = new String(" ");
                          int i;
                          for (i = 0;i < keyelements.Length;i++) {
                              char temp;
                              if (keyelements[i][0] >= 97) {
                                  temp = (char)(keyelements[i][0] - 32);
                              }else if (keyelements[i][0] <= 90){
                                  temp = (char)(keyelements[i][0] + 32);
                              }
                             // str = "[" + temp + keyelements[i][0] + "]" +;
                          }*/
                            Match mj = Regex.Match(art.Title, keyword);
                            Match m = Regex.Match(art.Content, keyword);
                            if ((m.Success) || (mj.Success))
                            {
                                finalfeeds.Add(html);
                            }
                        }
                        else
                        {
                            //do nothing
                        }
                    }
                    catch (Exception e) { }
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