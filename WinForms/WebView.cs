using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms
{
    public partial class WebView : Form
    {
        private WebView _webView;
        private WebView()
        {
            InitializeComponent();
            
        }

        private WebView(string str):this()
        {
            
        }

        public static WebView Instance = new WebView();

        public static void OpenBaidu(string str)
        {
            if (Instance == null)
            {
                Instance = new WebView();
            }
            Instance.Show();
            Instance.webBrowser.Navigate("https://www.baidu.com/s?wd=" + str);
        }


        private void WebView_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            e.Cancel = true;
        }
    }
}
