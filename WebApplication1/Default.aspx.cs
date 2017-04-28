/**
 * File : Default.aspx.cs
 * Author : AAR
 * Aldrich Valentino H. - 13515081
 * Roland Hartanto - 13515107
 * M. Akmal Pratama - 13515135
 * 
 */
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
using Subgurim.Controles;

namespace WebApplication1
{
    public partial class _Default : Page
    {
        //Me-load halaman
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                search(TextBox.Text);
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            search(TextBox.Text);
        }

        /**
         * I.S. : keyword terdefinisi, rss dan html terdefinisi 
         * F.S. : List dengan berita yang sesuai dengan kata kunci
         */
        public void search(string keyword)
        {
            //menyimpan rss ke dalam list
            List<String> rssUrl = new List<string>();
            rssUrl.Add("http://rss.detik.com/index.php/detikcom");
            //rssUrl.Add("http://tempo.co/rss/terkini");
            //rssUrl.Add("http://rss.viva.co.id");
            //rssUrl.Add("http://www.antaranews.com/rss/terkini");
            List<Feeds> feeds = new List<Feeds>();

            try
            {
                //membaca (parsing) rss
                foreach (var it in rssUrl)
                {
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

                //membaca html untuk setiap feed yang diperoleh dari rss
                List<Feeds> finalfeeds = new List<Feeds>();
                foreach (var html in feeds)
                {
                    Article art = new Article();
                    HtmlWeb page = new HtmlWeb();
                    try
                    {
                        var doc = page.Load(html.Link);
                        //memfilter konten judul
                        HtmlNode currentNodeTitle = doc.DocumentNode.SelectSingleNode("//title");
                        if (currentNodeTitle != null)
                        {
                            art.Title = currentNodeTitle.InnerHtml;
                        }
                        //memfilter konten artikel
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

                        //pencarian kata kunci dalam konten artikel dan judul
                        if (KMPRadioButton.Checked == true)
                        {//KMP
                            KMP K1 = new KMP(keyword, art.Content);
                            KMP K2 = new KMP(keyword, art.Title);
                            String result1 = K1.getKMPResult();
                            String result2 = K2.getKMPResult();
                            if (!((result1.Equals("not found")) && (result2.Equals("not found"))))
                            {
                                if (!result2.Equals("not found"))
                                {
                                    html.Description = html.Description + "<br> <br>" + result2;
                                }
                                else
                                {
                                    html.Description = html.Description +"<br> <br>"+ result1;
                                }

                                finalfeeds.Add(html);
                            }
                        }
                        else if (BMRadioButton.Checked == true)
                        {//Boyer Moore
                            BoyerMoore B1 = new BoyerMoore(keyword, art.Content);
                            BoyerMoore B2 = new BoyerMoore(keyword, art.Title);
                            String result1 = B1.getBoyerMooreResult();
                            String result2 = B2.getBoyerMooreResult();
                            if (!((result1.Equals("not found")) && (result2.Equals("not found"))))
                            {
                                if (!result2.Equals("not found", StringComparison.Ordinal))
                                {
                                    html.Description = html.Description + "<br> <br>" + result2;
                                }
                                else
                                {
                                    html.Description = html.Description + "<br> <br>" + result1;
                                }

                                finalfeeds.Add(html);
                            }
                        }
                        else if (RegexRadioButton.Checked == true)
                        {//Regex
                            string newKeyWord = keyword.Replace(" ","(.*)");
                            Match mj = Regex.Match(art.Title, newKeyWord, RegexOptions.IgnoreCase);
                            Match m = Regex.Match(art.Content, newKeyWord, RegexOptions.IgnoreCase);
                            if ((m.Success) || (mj.Success))
                            {
                                string desc;
                                if (m.Success) {
                                    desc = "...";
                                    if (m.Index - 50 >= 0) {
                                        desc = desc + art.Content.Substring(m.Index-50,50);
                                    }
                                    desc = desc + "<b>" + art.Content.Substring(m.Index,m.Length) + "</b>";
                                    if (art.Content.Length - 1 - (m.Index + m.Length - 1) >= 50) {
                                        desc = desc + art.Content.Substring(m.Index + m.Length, 50);
                                    }
                                    desc = desc + "...";
                                    html.Description = html.Description + "<br> <br>" + desc;
                                }
                                finalfeeds.Add(html);
                            }
                        }
                        else
                        {
                            //do nothing bila tidak memilih mode pencarian/saat awal halaman di-load
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