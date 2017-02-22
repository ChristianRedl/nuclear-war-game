using SimpleNuclearWarGameplayDemo.Game;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleNuclearWarGameplayDemo
{
    public partial class NewsForm : Form, NewsFeed
    {
        public NewsForm()
        {
            InitializeComponent();
        }

        private void NewsForm_Load(object sender, EventArgs e)
        {

        }

        string body = "";
        public void DisplayHtml(string html)
        {
            if(String.IsNullOrEmpty(html))
            {
                return;
            }
            lock (webBrowser) {
                try
                { 
                    body = html + "\n<hr/>" + body;                
                    webBrowser.Navigate("about:blank");
                    if (webBrowser.Document != null)
                    {
                        webBrowser.Document.Write(string.Empty);
                    }
                    textBoxUrl.Text = "http://www.tomorrowsnewstoday.com/";
                }
                catch (Exception e)
                {
                    Console.Out.Write(e.Message);
                } // do nothing with this                
                webBrowser.DocumentText = body;
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            if(webBrowser.CanGoBack)
            {
                webBrowser.GoBack();
            }
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            try
            {
                webBrowser.Navigate(new Uri(textBoxUrl.Text));
            }
            catch(Exception ex)
            {
                Console.Out.Write(ex.Message);
            }
            
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try 
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Title = "Save HTML";
                save.Filter = "Web Site|*.html";
                save.AddExtension = true;
                save.DefaultExt = ".html";
                if (save.ShowDialog() == DialogResult.OK && save.FileName != "")
                {
                    System.IO.File.WriteAllText(save.FileName, body);
                }
            }
            catch(Exception ex)
            {
                Console.Out.Write(ex.Message);
            }
        }
    }
}
