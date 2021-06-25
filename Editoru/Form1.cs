using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Editoru
{
    public partial class Form1 : Form
    {

        private Boolean[] isSaved = new Boolean[100];
        private String []fisiereSalvate = new String[100];

        public Form1()
        {
            for(int i = 0; i < 100; i++)
            {
                isSaved[i] = false;
            }
            InitializeComponent();
        }

        private void iesireToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.WordWrap = wordWrapToolStripMenuItem.Checked;
            for(int i = 2; i <= n; i++)
            {
                v[i].WordWrap = wordWrapToolStripMenuItem.Checked;
            }
        }

        private void despreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Editoru' de texte - v.1", "Despre", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool saveFile(string fName)
        {
            bool res = false;

            try
            {
                if (tabActiv == 1)
                {
                    v[1] = textBox1;
                    vPage[1] = tabPage1;
                    StreamWriter sw = new StreamWriter(fName);
                    sw.Write(textBox1.Text);
                    sw.Close();
                }
                else
                {
                    StreamWriter sw = new StreamWriter(fName);
                    sw.Write(v[tabActiv].Text);
                    sw.Close();
                }

                fisiereSalvate[tabActiv] = fName;

                isSaved[tabActiv] = true;

                updateInterface();

                res = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la salvare!\n" + ex.Message, "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return res;
        }

        private void salveazaCaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveAsDialog.ShowDialog() == DialogResult.OK)
            {
                if (saveFile(saveAsDialog.FileName))
                {
                    fileNameInEditor = saveAsDialog.FileName;
                    updateInterface();
                }
            }
        }

        private string fileNameInEditor = null;

        private void updateInterface()
        {
            if (!isSaved[tabActiv])
            {
                string s = this.Text;
                if (s[0]!='*')
                {
                    this.Text = "*" + s;
                }
            }
            else
            {
                string s = this.Text;
                if (s[0] == '*')
                {
                    this.Text = s.Replace("*","");
                }
            }

            labelFName.Text = (fisiereSalvate[tabActiv] == null) ? "Fisier fara nume" : fisiereSalvate[tabActiv];
        }

        private void salveazaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fisiereSalvate[tabActiv] == null)
            {
                salveazaCaToolStripMenuItem_Click(sender, null);
            }
            else
            {
                saveFile(fisiereSalvate[tabActiv]);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool confirmIesire = true;

            if (!isSaved[tabActiv])
            {
                String mesaj = "Aveti modificari nesalvate.\n";
                for(int i = 1; i <= nPage; i++)
                {
                    if(isSaved[i] == false)
                    {
                        mesaj = mesaj + "Tabul " + i.ToString() + "\n";
                    }
                }
                mesaj = mesaj + "Sunteti sigur ca vreti sa parasiti aplicatia fara sa salvati?";
                DialogResult resDiag = MessageBox.Show(mesaj, "Confirmare", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (resDiag == DialogResult.Yes)
                {
                    confirmIesire = true;
                }

                if (resDiag == DialogResult.No)
                {
                    salveazaToolStripMenuItem_Click(sender, null);
                    confirmIesire = true;
                }

                if(resDiag == DialogResult.Cancel)
                {
                    confirmIesire = false;
                }

            }

            if (confirmIesire == false)
            {
                e.Cancel = true;
            }
        }

        private void nouToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (!isSaved[tabActiv])
            {
                    String mesaj = "";
                    if (isSaved[tabActiv] != true)
                    {
                    mesaj = mesaj + "Aveti modificari nesalvate in tabul curent\n";
                    }
                mesaj = mesaj + "Sunteti sigur ca vreti stergeti continutul tabului fara sa salvati?";
                DialogResult resDiag = MessageBox.Show(mesaj, "Confirmare", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (resDiag == DialogResult.No)
                {
                    salveazaCaToolStripMenuItem_Click(sender, null);
                    v[tabActiv].Text = "";
                    isSaved[tabActiv] = false;
                    fisiereSalvate[tabActiv] = null;
                }

                if (resDiag == DialogResult.Yes)
                {
                    v[tabActiv].Text = "";
                    isSaved[tabActiv] = false;
                    fisiereSalvate[tabActiv] = null;
                }


            }


            
        }

        private void deschideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool isDoOpen = true;

            if (!isSaved[tabActiv])
            {
                String mesaj = "";
                if (isSaved[tabActiv] != true)
                {
                    mesaj = mesaj + "Aveti modificari nesalvate in tabul curent\n";
                }
                mesaj = mesaj + "Sunteti sigur ca vreti stergeti continutul tabului fara sa salvati?";
                DialogResult resDiag = MessageBox.Show(mesaj, "Confirmare", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (resDiag == DialogResult.No)
                {
                    salveazaCaToolStripMenuItem_Click(sender, null);
                    isDoOpen = true;
                }

                if(resDiag == DialogResult.Yes)
                {
                    isDoOpen = true;
                }

                if(resDiag == DialogResult.Cancel)
                {
                    isDoOpen = false;
                }

            }

            if (isDoOpen && openDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamReader sr = new StreamReader(openDialog.FileName);
                    v[tabActiv].Text = sr.ReadToEnd();
                    sr.Close();

                    fisiereSalvate[tabActiv] = openDialog.FileName;
                    isSaved[tabActiv] = true;

                    updateInterface();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Eroare la deschidere!\n" + ex.Message, "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}
