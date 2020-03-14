using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HTML_IDE_GUI
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            openFD.FileName = "";
            openFD.InitialDirectory = "C:";
            openFD.Filter = "HTML|*.html|ALL|*.*";

            if (openFD.ShowDialog() != DialogResult.Cancel)
            {
                webBrowser1.Navigate(new Uri(openFD.FileName));
            } else { }
        }
    }
}
