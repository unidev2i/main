using System;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{

    public partial class AjoutEleve : Form
    {

        public AjoutEleve()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //Bouton s'inscrire appuyer
            string nom = textBox2.Text;
            string prenom = textBox1.Text;
            string classe = textBox3.Text;    


            ("INSERT INTO eleve (idClasse,Nom,Prenom) VALUES ('"+classe+"','"+nom+"','"+prenom+"')").SimpleRequest();

        }

        private void AjoutEleve_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
