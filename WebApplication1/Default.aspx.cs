using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

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
            rssUrl.Add("http://rss.vivanews.com/get/all");
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
                                     containsKey = false
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
                
                theRss.DataSource = feeds;
                theRss.DataBind();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}