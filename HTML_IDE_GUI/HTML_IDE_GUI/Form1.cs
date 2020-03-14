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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 secondForm = new Form2();
            secondForm.Show();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textToSaveRTextbox.CanUndo == true)
            {
                textToSaveRTextbox.Undo();
            } else { }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textToSaveRTextbox.SelectedText != "")
            {
                textToSaveRTextbox.Cut();
            } else { }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFD.Filter = "HTML|*.html|ALL|*.*";
            openFD.FileName = "";
            openFD.InitialDirectory = "C:";

            if (openFD.ShowDialog() != DialogResult.Cancel)
            {
                textToSaveRTextbox.LoadFile(openFD.FileName, RichTextBoxStreamType.PlainText);
            } else { }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFD.InitialDirectory = "C:";
            saveFD.FileName = "";
            saveFD.Filter = "HTML|*.html|ALL|*.*";

            if (saveFD.ShowDialog() != DialogResult.Cancel)
            {
                textToSaveRTextbox.SaveFile(saveFD.FileName, RichTextBoxStreamType.PlainText);
            } else { }
        }
    }
}
